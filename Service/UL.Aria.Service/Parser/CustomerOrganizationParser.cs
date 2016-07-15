using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;
//using UL.Aria.Common.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Parser
{
    /// <summary>
    /// Parser for customer organization objects <see cref="CustomerOrganization"/>
    /// </summary>
    [Entity(EntityTypeEnumDto.CustomerOrganization)]
    public sealed class CustomerOrganizationParser : XmlParserBase, IXmlParser
    {
        /// <summary>
        /// Parses the specified incoming order message.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>
        /// NewProjectDto.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object Parse(string xml)
        {
            Guard.IsNotNullOrEmpty(xml, "xml");

            var customerOrganization = new CustomerOrganization() ;

            StringReader stringReader = null;

            try
            {
                stringReader = new StringReader(xml);

                using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings { IgnoreWhitespace = true }))
                {
                    stringReader = null;
                    ProcessRoot(customerOrganization, xmlReader, ProcessSubTreeRoot);
                }
            }
            finally
            {
                if (stringReader != null)
                    stringReader.Dispose();
            }

            return customerOrganization;
        }

        private void ProcessSubTreeRoot(CustomerOrganization organization, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "DataArea":
                    ProcessSubTree(organization, xmlReader, ProcessSubTreeDataArea);
                    break;
            }
        }

        private void ProcessSubTreeDataArea(CustomerOrganization organization, XmlReader xmlReader)
        {
            // this level varies, move to next
            if (xmlReader.LocalName != "DataArea")
            {
                ProcessSubTree(organization, xmlReader, ProcessSubTreeDataAreaChild);
            }
        }

        private void ProcessSubTreeDataAreaChild(CustomerOrganization organization, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Identification":
                    ProcessSubTree(organization, xmlReader, ProcessSubTreeProcessDataAreaIdentification);
                    break;
                case "PartyLocation":
                    var location = new IncomingOrderContact();
                    ProcessSubTree(location, xmlReader, ProcessSubTreeProcessPartyLocation);
                    organization.Locations.Add(location);
                    break;
                case "PartyContact":
                    var contact = new IncomingOrderContact();
                    ProcessSubTree(contact, xmlReader, ProcessSubTreeProcessPartyContact);
                    organization.Contacts.Add(contact);
                    break;
                case "Organization":
                    ProcessSubTree(organization, xmlReader, ProcessSubTreeProcessOrganization);
                    break;
                case "PartyRelatedParty":
                    ProcessSubTree(organization, xmlReader, ProcessSubTreeProcessPartyRelatedParty);
                    break;
                case "CustomerPartyAccount":
                    ProcessSubTree(organization, xmlReader, ProcessSubTreeProcessCustomerPartyAccount);
                    break;
                    
            }
            
        }

        private void ProcessSubTreeProcessCustomerPartyAccount(CustomerOrganization organization, XmlReader xmlReader)
        {
            organization.Customer = organization.Customer ?? new IncomingOrderCustomer();
            switch (xmlReader.LocalName)
            {
                case "Identification":
                    ProcessSubTree(organization, xmlReader, ProcessSubTreeProcessPartyCustomerPartyIdentification);
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyCustomerPartyIdentification(CustomerOrganization organization, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "AlternateObjectKey":
                    ProcessSubTree(organization, xmlReader, ProcessSubTreeProcessPartyCustomerPartyIdentificationAlternateObjectKey);
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyCustomerPartyIdentificationAlternateObjectKey(CustomerOrganization organization, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    organization.Customer.ExternalId = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyRelatedParty(CustomerOrganization organization, XmlReader xmlReader)
        {
        }

        private void ProcessSubTreeProcessOrganization(CustomerOrganization organization, XmlReader xmlReader)
        {
            organization.Customer = organization.Customer ?? new IncomingOrderCustomer();
            switch (xmlReader.LocalName)
            {
                case  "Name":
                    organization.Customer.Name = xmlReader.ReadString();
                    break;
                case "DUNSInquiryIdentifier":
                    organization.Customer.DUNS = xmlReader.ReadString();
                    break;
            }
        }

        private interface IPreferredItem
        {
            bool IsPreferred { get; set; }
        }
        private class PhoneNumber : IPreferredItem
        {
            public string AreaCode { get; set; }
            public string AccessCode { get; set; }
            public string LocalNumber { get; set; }
            public string CountryCode { get; set; }
            public string Extension { get; set; }
            public bool IsPreferred { get; set; }
            public string CompleteNumber { get; set; }

        }

        private class Email : IPreferredItem
        {
            public bool IsPreferred { get; set; }
            public string EmailAddress { get; set; }
        }

        private void ProcessSubTreeProcessPartyContact(IncomingOrderContact contact, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Contact":
                    ProcessSubTree(contact, xmlReader, ProcessSubTreeProcessPartyContactContact);
                    break;
            }

        }

        private void ProcessSubTreeProcessPartyContactContact(IncomingOrderContact contact, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Identification":
                    ProcessSubTree(contact, xmlReader, ProcessSubTreeProcessPartyLocationOrContactIdentification);
                    break;
                case "OrganizationName":
                    if (string.IsNullOrWhiteSpace(contact.FullName))
                        contact.FullName = xmlReader.ReadString();
                    break;
                case "PersonName":
                    ProcessSubTree(contact, xmlReader, ProcessSubTreeProcessPartyContactPersonName);
                    break;
                case "ContactPhoneCommunication":
                    var phoneNumber = new PhoneNumber();
                    ProcessSubTree(phoneNumber, xmlReader, ProcessSubTreeProcessPartyContactContactPhoneCommunication);
                    
                    if (!phoneNumber.IsPreferred)
                        break;

                    if (string.IsNullOrWhiteSpace(phoneNumber.CompleteNumber))
                    {
                        contact.Phone = string.Join(" ", phoneNumber.AccessCode, phoneNumber.CountryCode,
                            phoneNumber.AreaCode, phoneNumber.LocalNumber, phoneNumber.Extension).Trim();
                    }
                    else
                    {
                        contact.Phone = phoneNumber.CompleteNumber;
                    }
                    break;
                case "JobTitle":
                    contact.Title = xmlReader.ReadString();
                    break;
                case "ContactEmailCommunication":
                    var email = new Email();
                    ProcessSubTree(email, xmlReader, ProcessSubTreeProcessPartyContactContactEmailCommunication);
                    if (email.IsPreferred)
                    {
                        contact.Email = email.EmailAddress;
                    }
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyContactPersonName(IncomingOrderContact contact, XmlReader xmlReader)   
        {
            switch (xmlReader.LocalName)
            {
                case "FullName":
                    contact.FullName = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyContactContactEmailCommunication(Email email,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "EmailCommunication":
                    ProcessSubTree(email, xmlReader, ProcessSubTreeProcessPartyContactContactEmailCommunicationEmailCommunication);
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyContactContactEmailCommunicationEmailCommunication(Email email,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "URI":
                    email.EmailAddress = xmlReader.ReadString();
                    break;
                case "Preference":
                    ProcessSubTree(email, xmlReader, ProcessSubTreeProcessPreference);
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyContactContactPhoneCommunication(PhoneNumber phoneNumber,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "PhoneCommunication":
                    ProcessSubTree(phoneNumber, xmlReader, ProcessSubTreeProcessPartyContactContactPhoneCommunicationPhoneCommunication);
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyContactContactPhoneCommunicationPhoneCommunication(PhoneNumber phoneNumber, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "CompleteNumber":
                        phoneNumber.CompleteNumber= xmlReader.ReadString();
                    break;
                case "CountryCode":
                    phoneNumber.CountryCode = xmlReader.ReadString();
                    break;
                case "AreaCode":
                    phoneNumber.AreaCode = xmlReader.ReadString();
                    break;
                case "LocalNumber":
                    phoneNumber.LocalNumber = xmlReader.ReadString();
                    break;
                case "ExtensionNumber":
                    phoneNumber.Extension = xmlReader.ReadString();
                    break;
                case "Preference":
                    ProcessSubTree(phoneNumber, xmlReader, ProcessSubTreeProcessPreference);
                    break;
                case "AccessCode":
                    phoneNumber.AccessCode = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeProcessPreference(IPreferredItem isPreferred,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "PreferredIndicator":
                    isPreferred.IsPreferred = (xmlReader.ReadString() ?? "false").ToLower() == "true";
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyLocation(IncomingOrderContact location, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Identification":
                    ProcessSubTree(location, xmlReader, ProcessSubTreeProcessPartyLocationOrContactIdentification);
                    break;
                case "LocationReference":
                     ProcessSubTree(location, xmlReader, ProcessSubTreeProcessPartyLocationLocationReference);
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyLocationLocationReference(IncomingOrderContact location, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Address":
                    var addressArray = new string[9];
                    ProcessSubTree(location, xmlReader, addressArray, ProcessSubTreeProcessPartyLocationAddress);
                    location.Address = string.Join(Environment.NewLine, addressArray.Where(x => !string.IsNullOrEmpty(x)));
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyLocationAddress(IncomingOrderContact location, XmlReader xmlReader, string[] addressArray)
        {
            switch (xmlReader.LocalName)
            {
                case "LineOne":
                    addressArray[0] = xmlReader.ReadString();
                    break;
                case "LineTwo":
                    addressArray[1] = xmlReader.ReadString();
                    break;
                case "LineThree":
                    addressArray[2] = xmlReader.ReadString();
                    break;
                case "LineFour":
                    addressArray[3] = xmlReader.ReadString();
                    break;
                case "LineFive":
                    addressArray[4] = xmlReader.ReadString();
                    break;
                case "LineSix":
                    addressArray[5] = xmlReader.ReadString();
                    break;
                case "LineSeven":
                    addressArray[6] = xmlReader.ReadString();
                    break;
                case "LineEight":
                    addressArray[7] = xmlReader.ReadString();
                    break;
                case "LineNine":
                    addressArray[8] = xmlReader.ReadString();
                    break;
                case "CityName":
                    location.City = xmlReader.ReadString();
                    break;
                case "StateName":
                    location.State = xmlReader.ReadString();
                    break;
                case "ProvinceName":
                    location.Province = xmlReader.ReadString();
                    break;
                case "Country":
                    location.Country = xmlReader.ReadString();
                    break;
                case "PostalCode":
                    location.PostalCode = xmlReader.ReadString();
                    break; 
            }
        }
        
        private void ProcessSubTree<T>(T dto, XmlReader xmlReader,  string[] addressArray, Action<T, XmlReader, string[]> processSubTreeProcessor)
        {
            var xmlReaderSubTree = xmlReader.ReadSubtree();
            xmlReaderSubTree.Read(); // reads value of current element
            xmlReaderSubTree.Read(); // Reads first child element

            while (!xmlReaderSubTree.EOF)
            {
                processSubTreeProcessor(dto, xmlReaderSubTree, addressArray);
                xmlReaderSubTree.Skip();
            }
        }

        private void ProcessSubTreeProcessPartyLocationOrContactIdentification(IncomingOrderContact contact, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ApplicationObjectKey":
                    ProcessSubTree(contact, xmlReader, ProcessSubTreeProcessPartyLocationIdentificationApplicationObjectKey);
                    break;
            }
        }

        private void ProcessSubTreeProcessPartyLocationIdentificationApplicationObjectKey(IncomingOrderContact contact, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    contact.ExternalId = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeProcessDataAreaIdentification(CustomerOrganization organization, XmlReader xmlReader)
        {
        } 
    }
}