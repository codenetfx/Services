using System;
using System.IO;
using System.Xml;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Parser
{
    /// <summary>
    ///     Class IncomingOrderXmlParser. This class cannot be inherited.
    /// </summary>
    [Entity(EntityTypeEnumDto.IncomingOrder)]
    [Entity(EntityTypeEnumDto.Project)]
    public sealed class IncomingOrderXmlParser : XmlParserBase, IXmlParser
    {
        /// <summary>
        ///     Parses the specified incoming order message.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>NewProjectDto.</returns>
        public object Parse(string xml)
        {
            Guard.IsNotNullOrEmpty(xml, "xml");

            var newProject = new IncomingOrderDto {OriginalXmlParsed = xml};

            StringReader stringReader = null;

            try
            {
                stringReader = new StringReader(xml);

                using (var xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings {IgnoreWhitespace = true}))
                {
                    stringReader = null;
                    ProcessRoot(newProject, xmlReader, ProcessSubTreeRoot);
                }
            }
            finally
            {
                if (stringReader != null)
                    stringReader.Dispose();
            }

            return newProject;
        }

        /// <summary>
        ///     Processes the sub tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dto">The dto.</param>
        /// <param name="xmlReader">The XML reader.</param>
        /// <param name="processSubTreeProcessor">The process sub tree processor.</param>
        /// <param name="contactRole">The contact role.</param>
        private void ProcessSubTree<T>(T dto, XmlReader xmlReader,
            Action<T, XmlReader, ContactRoleEnum> processSubTreeProcessor,
            ContactRoleEnum contactRole)
        {
            var xmlReaderSubTree = xmlReader.ReadSubtree();
            xmlReaderSubTree.Read(); // reads value of current element
            xmlReaderSubTree.Read(); // Reads first child element

            while (!xmlReaderSubTree.EOF)
            {
                processSubTreeProcessor(dto, xmlReaderSubTree, contactRole);
                xmlReaderSubTree.Skip();
            }
        }

        private void ProcessSubTreeRoot(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "DataArea":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeDataArea);
                    break;
            }
        }

        private void ProcessSubTreeDataArea(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ProcessSalesOrderFulfillment":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeProcessSalesOrderFulfillment);
                    break;
            }
        }

        private void ProcessSubTreeProcessSalesOrderFulfillment(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "SalesOrderLine":
                    var serviceLine = new IncomingOrderServiceLineDto();
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeSalesOrderLine);

                    if (!string.IsNullOrEmpty(serviceLine.Description))
                        incomingOrder.ServiceLines.Add(serviceLine);

                    break;

                case "Custom":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeCustom);
                    break;

                case "SalesOrderCustomerParty":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeSalesOrderCustomerParty);
                    break;

                case "Identification":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeIdentification);
                    break;

                case "EarliestShipDateTime":
                    incomingOrder.DateBooked = ReadDateTime(xmlReader);
                    break;

                case "OrderDateTime":
                    incomingOrder.DateOrdered = ReadDateTime(xmlReader);
                    break;

                case "RequestedShipDateTime":
                    incomingOrder.CustomerRequestedDate = ReadDateTimeNullable(xmlReader);
                    break;

                case "TypeCode":
                    incomingOrder.OrderType = xmlReader.ReadString();
                    break;

                case "Status":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeStatus);
                    break;

                case "BusinessUnitReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeBusinessUnitReference);
                    break;

                case "CustomerPartyReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeCustomerPartyReference);
                    break;

                case "CustomerPurchaseOrderReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeCustomerPurchaseOrderReference);
                    break;

                case "OriginalSalesOrderReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeOriginalSalesOrderReference);
                    break;

                case "ProjectReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeProjectReference);
                    break;

                case "BillToPartyReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeBillToPartyReference);
                    break;

                case "ShipToPartyReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePartyReferenceShipToLocationReference);
                    break;

				case "TotalAmount":
		            var totalAmount = xmlReader.ReadString();
		            if (!string.IsNullOrWhiteSpace(totalAmount))
		            {
			            incomingOrder.TotalOrderPrice = Convert.ToDecimal(totalAmount);
		            }
		            break;
                case "CurrencyCode":
                    var currency = xmlReader.ReadString();
                    if (!string.IsNullOrWhiteSpace(currency))
                    {
                        incomingOrder.Currency = currency;
                    }
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceShipToLocationReference(IncomingOrderDto incomingOrder,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "LocationReference":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePartyReferenceContactAddress,
                        ContactRoleEnum.ShipTo);
                    break;
                case "Contact":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePartyReferenceContact, ContactRoleEnum.ShipTo);
                    break;
                case "PartyIdentification":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePartyPartyIdentification, ContactRoleEnum.ShipTo);
                    break;
                case "CustomerPartyAccountIdentification":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreePartyReferenceContactIdentification, ContactRoleEnum.ShipTo);
                    break;
            }
        }

        private void ProcessSubTreePartyPartyIdentification(IncomingOrderDto incomingOrder, XmlReader xmlReader, ContactRoleEnum contactRole)
        {
            switch (xmlReader.LocalName)
            {
                case "BusinessComponentID":
                    if (contactRole == ContactRoleEnum.ShipTo)
                    {
                        var readString = xmlReader.ReadString();
                        if (!string.IsNullOrWhiteSpace(readString))
                        {
                            incomingOrder.ShipToContact.ExternalId = readString;
                            incomingOrder.ShipToContact.SubscriberNumber = readString;
                        }
                    }
                    if (contactRole == ContactRoleEnum.Customer)
                    {
                        var readString = xmlReader.ReadString();
                        if (!string.IsNullOrWhiteSpace(readString))
                        {
                            incomingOrder.IncomingOrderContact.ExternalId = readString;
                            incomingOrder.IncomingOrderContact.SubscriberNumber = readString;
                            incomingOrder.IncomingOrderContact.PartySiteNumber = readString;
                        }
                    }
                    break;
            }
        }

        private void ProcessSubTreeBillToPartyReference(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Contact":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePartyReferenceContact, ContactRoleEnum.BillTo);
                    break;

                case "CustomerPartyAccountIdentification":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreePartyReferenceContactIdentification, ContactRoleEnum.BillTo);
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceContact(IncomingOrderDto incomingOrder, XmlReader xmlReader,
            ContactRoleEnum contactRole)
        {
            switch (xmlReader.LocalName)
            {
                case "ContactAddressCommunication":
                    if (contactRole == ContactRoleEnum.BillTo)
                        ProcessSubTree(incomingOrder, xmlReader,
                            ProcessSubTreeBillToPartyReferenceContactContactAddressCommunication);
                    break;
                case "ContactPhoneCommunication":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreePartyReferenceContactContactPhoneCommunication, contactRole);
                    break;
            }
        }
        private void ProcessSubTreePartyReferenceContactIdentification(IncomingOrderDto incomingOrder,
            XmlReader xmlReader, ContactRoleEnum contactRole)
        {
            switch (xmlReader.LocalName)
            {
                case "ApplicationObjectKey":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreePartyReferenceContactApplicationObjectKey, contactRole);
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceContactApplicationObjectKey(IncomingOrderDto incomingOrder, XmlReader xmlReader, ContactRoleEnum contactRole)
        {

            switch (xmlReader.LocalName)
            {
                case "ID":
                    switch (contactRole)
                    {
                        case ContactRoleEnum.BillTo:
                             incomingOrder.BillToContact.ExternalId = xmlReader.ReadString();
                            break;
                        case ContactRoleEnum.ShipTo:
                            if (string.IsNullOrWhiteSpace(incomingOrder.ShipToContact.ExternalId))
                            {
                                var readString = xmlReader.ReadString();
                                if (!string.IsNullOrWhiteSpace(readString))
                                {
                                    incomingOrder.ShipToContact.ExternalId = readString;
                                    incomingOrder.ShipToContact.PartySiteNumber = readString;
                                }
                            }
                            break;
                    }
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceContactContactPhoneCommunication(IncomingOrderDto incomingOrder,
            XmlReader xmlReader, ContactRoleEnum contactRole)
        {
            switch (xmlReader.LocalName)
            {
                case "PhoneCommunication":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreePartyReferenceContactContactPhoneCommunicationPhoneCommunication, contactRole);
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceContactContactPhoneCommunicationPhoneCommunication(
            IncomingOrderDto incomingOrder, XmlReader xmlReader, ContactRoleEnum contactRole)
        {
            switch (contactRole)
            {
                case ContactRoleEnum.BillTo:
                    ProcessSubTreePartyReferenceContactContactPhoneCommunicationPhoneCommunicationPhone
                        (incomingOrder.BillToContact, xmlReader);
                    break;
                case ContactRoleEnum.ShipTo:
                    ProcessSubTreePartyReferenceContactContactPhoneCommunicationPhoneCommunicationPhone
                        (incomingOrder.ShipToContact, xmlReader);
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceContactContactPhoneCommunicationPhoneCommunicationPhone(
            IncomingOrderContactDto incomingOrderContact, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "CompleteNumber":
                    incomingOrderContact.Phone = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeBillToPartyReferenceContactContactAddressCommunication(
            IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "AddressCommunication":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePartyReferenceContactAddress,
                        ContactRoleEnum.BillTo);
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceContactAddress(IncomingOrderDto incomingOrder, XmlReader xmlReader,
            ContactRoleEnum contactRole)
        {
            switch (xmlReader.LocalName)
            {
                case "Address":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreePartyReferenceContactContactAddressCommunicationAddressCommunicationAddress,
                        contactRole);
                    break;
            }
        }

        private void ProcessSubTreePartyReferenceContactContactAddressCommunicationAddressCommunicationAddress(
            IncomingOrderDto incomingOrder, XmlReader xmlReader, ContactRoleEnum contactRole)
        {
            switch (contactRole)
            {
                case ContactRoleEnum.BillTo:
                    ProcessSubTreePartyReferenceContactContactAddressCommunicationAddressCommunicationAddressIncomingOrderContact
                        (incomingOrder.BillToContact, xmlReader);
                    break;
                case ContactRoleEnum.ShipTo:
                    ProcessSubTreePartyReferenceContactContactAddressCommunicationAddressCommunicationAddressIncomingOrderContact
                        (incomingOrder.ShipToContact, xmlReader);
                    break;
            }
        }

        private void
            ProcessSubTreePartyReferenceContactContactAddressCommunicationAddressCommunicationAddressIncomingOrderContact
            (IncomingOrderContactDto incomingOrderContact, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName.ToLowerInvariant())
            {
                case "lineone":
                    incomingOrderContact.Address = xmlReader.ReadString();
                    break;
                case "linetwo":
                    AppendAddressLine(incomingOrderContact, xmlReader.ReadString());
                    break;
                case "linethree":
                    AppendAddressLine(incomingOrderContact, xmlReader.ReadString());
                    break;
                case "linefour":
                    AppendAddressLine(incomingOrderContact, xmlReader.ReadString());
                    break;
                case "linefive":
                    AppendAddressLine(incomingOrderContact, xmlReader.ReadString());
                    break;
                case "cityname":
                    incomingOrderContact.City = xmlReader.ReadString();
                    break;
                case "statename":
                    incomingOrderContact.State = xmlReader.ReadString();
                    break;
                case "provincename":
                    incomingOrderContact.Province = xmlReader.ReadString();
                    break;
                case "countrycode":
                    incomingOrderContact.Country = xmlReader.ReadString();
                    break;
                case "postalcode":
                    incomingOrderContact.PostalCode = xmlReader.ReadString();
                    break;
            }
        }

        private void AppendAddressLine(IncomingOrderContactDto incomingOrderContact, string addressLine)
        {
            if (!string.IsNullOrWhiteSpace(addressLine))
            {
                if (!string.IsNullOrWhiteSpace(incomingOrderContact.Address))
                {
                    incomingOrderContact.Address += Environment.NewLine;
                }
                incomingOrderContact.Address += addressLine;
            }
        }

        private void ProcessSubTreeProjectReference(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ProjectIdentfication":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeProjectReferenceProjectIdentfication);
                    break;
            }
        }

        private void ProcessSubTreeProjectReferenceProjectIdentfication(IncomingOrderDto incomingOrder,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Description":
                    incomingOrder.IncomingOrderCustomer.ProjectName = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeOriginalSalesOrderReference(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "SalesOrderIdentification":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreeOriginalSalesOrderReferenceSalesOrderIdentification);
                    break;
            }
        }

        private void ProcessSubTreeOriginalSalesOrderReferenceSalesOrderIdentification(IncomingOrderDto incomingOrder,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    incomingOrder.OrderNumber = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeCustomerPurchaseOrderReference(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Identification":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeCustomerPurchaseOrderReferenceIdentification);
                    break;
            }
        }

        private void ProcessSubTreeCustomerPurchaseOrderReferenceIdentification(IncomingOrderDto incomingOrder,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    incomingOrder.CustomerPo = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeCustomerPartyReference(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Contact":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeCustomerPartyReferenceContact);
                    break;
                case "Custom":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeCustomerPartyReferenceCustom);
                    break;
                case "CustomerPartyAccountContactIdentification":
                    ProcessSubTree(incomingOrder, xmlReader,
                        ProcessSubTreeCustomerPartyReferenceCustomerPartyAccountContactIdentification);
                    break;
                case "PartyIdentification":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePartyPartyIdentification, ContactRoleEnum.Customer);
                    break;
            }
        }

        private void ProcessSubTreeCustomerPartyReferenceCustomerPartyAccountContactIdentification(
            IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "BusinessComponentID":
                    incomingOrder.IncomingOrderCustomer.ExternalId = xmlReader.ReadString();
                    //incomingOrder.IncomingOrderContact.PartySiteNumber = incomingOrder.IncomingOrderCustomer.ExternalId;
                    break;
            }
        }

        private void ProcessSubTreeCustomerPartyReferenceCustom(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "DUNS_Number":
                    incomingOrder.IncomingOrderCustomer.DUNS = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeCustomerPartyReferenceContact(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "PersonName":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeCustomerPartyReferenceContactPersonName);
                    break;
            }
        }

        private void ProcessSubTreeCustomerPartyReferenceContactPersonName(IncomingOrderDto incomingOrder,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "FullName":
                    incomingOrder.ShipToContact.FullName = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeBusinessUnitReference(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Custom":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeBusinessUnitReferenceCustom);
                    break;
            }
        }

        private void ProcessSubTreeBusinessUnitReferenceCustom(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "TaskOrganization":
                    incomingOrder.BusinessUnit = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderCustomerParty(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "CustomerPartyReference":          
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeSalesOrderCustomerPartyReference);
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderCustomerPartyReference(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "PersonName":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreePersonName);
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderLine(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "OrderQuantity":
                    serviceLine.Quantity = ReadInt32(xmlReader);
                    break;
                case "SalesOrderSchedule":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeSalesOrderSchedule);
                    break;
                case "ItemReference":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeItemReference);
                    break;
                case "FulfillmentModeCode":
                    serviceLine.FulfillmentMethodCode = xmlReader.ReadString();
                    break;
                case "Custom":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeSalesOrderLineCustom);
                    break;
                case "ParentSalesOrderLineReference":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeParentSalesOrderLineReference);
                    break;
                case "Status":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeParentSalesOrderLineStatus);
                    break;
                case "Identification":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeSalesOrderLineIdentification);
                    break;
				case "Hold":
		            serviceLine.Hold = xmlReader.ReadString();
					break;
				case "TotalAmount":
		            var totalAmount = xmlReader.ReadString();
		            if (!string.IsNullOrWhiteSpace(totalAmount))
		            {
			            serviceLine.Price = Convert.ToDecimal(totalAmount);
		            }
		            break;
                case "CurrencyCode":
                    var currency = xmlReader.ReadString();
                    if (!string.IsNullOrWhiteSpace(currency))
                    {
                        serviceLine.Currency = currency;
                    }
                    break;
			}
        }

        private void ProcessSubTreeSalesOrderLineIdentification(IncomingOrderServiceLineDto serviceLine,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "BusinessComponentID":
                    serviceLine.WorkOrderLineBusinessComponentId = xmlReader.ReadString();
                    break;
                case "ApplicationObjectKey":
                    ProcessSubTree(serviceLine, xmlReader,
                        ProcessSubTreeSalesOrderLineIdentificationApplicationObjectKey);
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderLineIdentificationApplicationObjectKey(
            IncomingOrderServiceLineDto serviceLine,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    serviceLine.ApplicationObjectKeyId = xmlReader.ReadString();
                    break;
                case "ContextID":
                    serviceLine.WorkOrderLineId = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeParentSalesOrderLineStatus(IncomingOrderServiceLineDto serviceLine,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Code":
                    serviceLine.Status = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeParentSalesOrderLineReference(IncomingOrderServiceLineDto serviceLine,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "SalesOrderIdentification":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeSalesOrderIdentification);
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderIdentification(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    serviceLine.ParentExternalId = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderLineCustom(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "FulfillmentSet":
                    serviceLine.FulfillmentSet = xmlReader.ReadString();
                    break;
                case "LongTaskName":
                    serviceLine.Description = xmlReader.ReadString();
                    break;
                case "TaskNumber":
                    serviceLine.LineNumber = xmlReader.ReadString();
                    break;
                case "TaskId":
                    serviceLine.ExternalId = xmlReader.ReadString();
                    break;
                case "CustomerModelNumber":
                    serviceLine.CustomerModelNumber = xmlReader.ReadString();
                    break;
                case "AdditionalChargesAllowed":
                    serviceLine.BillableExpenses = xmlReader.ReadString();
                    break;

                case "PreferredFLSLocCode":
                    serviceLine.LocationCode = xmlReader.ReadString();
                    break;
                case "PreferredFLSLocName":
                    serviceLine.LocationName = xmlReader.ReadString();
                    break;
				case "LocationDescription":
					serviceLine.LocationCodeLabel = xmlReader.ReadString();
					break;
				case "ServiceLineDescription":
					serviceLine.ServiceCodeLabel = xmlReader.ReadString();
					break;
				case "IndustryDescription":
					serviceLine.IndustryCodeLabel = xmlReader.ReadString();
					break;
                case "Hold":
                    serviceLine.Hold = xmlReader.ReadString();
					break;
			}
        }

        private void ProcessSubTreeItemReference(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ItemIdentification":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeItemIdentification);
                    break;
                case "ClassificationCode":
                    serviceLine.ItemCategories = xmlReader.ReadString();
                    break;
                case "TypeCode":
                    serviceLine.TypeCode = xmlReader.ReadString();
                    break;
                case "Description":
                    serviceLine.Name = xmlReader.ReadString();
                    break;
                case "ServiceIndicator":
                    serviceLine.ServiceCode = xmlReader.ReadString();
                    break;
				case "Custom":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeItemReferenceCustom);
                    break;
			}
        }

		private void ProcessSubTreeItemReferenceCustom(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
		{
			switch (xmlReader.LocalName)
			{
				case "ItemCatalogInfo":
					ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeItemReferenceCustomItemCatalogInfo);
					break;
			}
		}

		private void ProcessSubTreeItemReferenceCustomItemCatalogInfo(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
		{
			switch (xmlReader.LocalName)
			{
				case "DetailedService":
					serviceLine.DetailedService = xmlReader.ReadString();
					break;
				case "DetailedServiceDescription":
					serviceLine.DetailedServiceDescription = xmlReader.ReadString();
					break;
				case "ServiceProgram":
					serviceLine.ServiceProgram = xmlReader.ReadString();
					break;
				case "ServiceProgramDescription":
					serviceLine.ServiceProgramDescription = xmlReader.ReadString();
					break;
				case "ServiceSub-category":
					serviceLine.ServiceSubCategory = xmlReader.ReadString();
					break;
				case "ServiceSub-categoryDescription":
					serviceLine.ServiceSubCategoryDescription = xmlReader.ReadString();
					break;
				case "ServiceCategory":
					serviceLine.ServiceCategory = xmlReader.ReadString();
					break;
				case "ServiceCategoryDescription":
					serviceLine.ServiceCategoryDescription = xmlReader.ReadString();
					break;
				case "ServiceSegment":
					serviceLine.ServiceSegment = xmlReader.ReadString();
					break;
				case "ServiceSegmentDescription":
					serviceLine.ServiceSegmentDescription = xmlReader.ReadString();
					break;
				case "ProductType":
					serviceLine.ProductType = xmlReader.ReadString();
					break;
				case "ProductTypeDescription":
					serviceLine.ProductTypeDescription = xmlReader.ReadString();
					break;
				case "ProductGroup":
					serviceLine.ProductGroup = xmlReader.ReadString();
					break;
				case "ProductGroupDescription":
					serviceLine.ProductGroupDescription = xmlReader.ReadString();
					break;
				case "IndustrySub-Category":
					serviceLine.IndustrySubCategory = xmlReader.ReadString();
					break;
				case "IndustrySub-CategoryDescription":
					serviceLine.IndustrySubCategoryDescription = xmlReader.ReadString();
					break;
				case "IndustryCategory":
					serviceLine.IndustryCategory = xmlReader.ReadString();
					break;
				case "IndustryCategoryDescription":
					serviceLine.IndustryCategoryDescription = xmlReader.ReadString();
					break;
				case "Industry":
					serviceLine.Industry = xmlReader.ReadString();
					break;
				case "IndustryDescription":
					serviceLine.IndustryDescription = xmlReader.ReadString();
					break;
			}
		}

        private void ProcessSubTreeItemIdentification(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ApplicationObjectKey":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeApplicationObjectKey);
                    break;
                case "AlternateObjectKey":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeAlternateObjectKey);
                    break;
            }
        }

        private void ProcessSubTreeAlternateObjectKey(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    serviceLine.IndustryCode = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeApplicationObjectKey(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    serviceLine.ConfigurationId = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderSchedule(IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "PromisedShipDateTime":
                    serviceLine.PromiseDate = ReadDateTime(xmlReader);
                    break;
                case "RequestedShipDateTime":
                    serviceLine.RequestDate = ReadDateTime(xmlReader);
                    break;
                case "PurchaseDate":
                    serviceLine.StartDate = ReadDateTime(xmlReader);
                    break;
                case "SalesOrderScheduleCharge":
                    ProcessSubTree(serviceLine, xmlReader,
                        ProcessSubTreeSalesOrderLineSalesOrderScheduleSalesOrderScheduleCharge);
                    break;
                case "Custom":
                    ProcessSubTree(serviceLine, xmlReader, ProcessSubTreeSalesOrderLineSalesOrderScheduleCustom);
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderLineSalesOrderScheduleCustom(IncomingOrderServiceLineDto serviceLine,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "BillableFlag":
                    serviceLine.Billable = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderLineSalesOrderScheduleSalesOrderScheduleCharge(
            IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Charge":
                    ProcessSubTree(serviceLine, xmlReader,
                        ProcessSubTreeSalesOrderLineSalesOrderScheduleSalesOrderScheduleChargeCharge);
                    break;
            }
        }

        private void ProcessSubTreeSalesOrderLineSalesOrderScheduleSalesOrderScheduleChargeCharge(
            IncomingOrderServiceLineDto serviceLine, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "TypeCode":
                    serviceLine.AllowChargesFromOtherOperatingUnits = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreePersonName(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "FullName":
                    incomingOrder.IncomingOrderCustomer.Name = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeCustom(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ProjectNumber":
                    incomingOrder.ProjectNumber = xmlReader.ReadString();
                    break;
                case "ProjectName":
                    incomingOrder.ProjectName = xmlReader.ReadString();
                    break;
                case "ProjectID":
                    incomingOrder.ExternalProjectId = xmlReader.ReadString();
                    break;
                case "CreationDate":
                    incomingOrder.CreationDate = ReadDateTime(xmlReader);
                    break;
                case "ProjectHeaderStatus":
                    incomingOrder.ProjectHeaderStatus = xmlReader.ReadString();
                    break;
				case "QuoteNumber":
					incomingOrder.QuoteNo = xmlReader.ReadString();
					break;
			}
        }

        private void ProcessSubTreeIdentification(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    incomingOrder.WorkOrderBusinessComponentId = xmlReader.ReadString();
                    break;
                case "Revision":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeRevision);
                    break;
                case "ApplicationObjectKey":
                    ProcessSubTree(incomingOrder, xmlReader, ProcessSubTreeIdentificationApplicationObjectKey);
                    break;
            }
        }

        private void ProcessSubTreeIdentificationApplicationObjectKey(IncomingOrderDto incomingOrder,
            XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "ID":
                    incomingOrder.WorkOrderId = xmlReader.ReadString();
                    break;
            }
        }

        private void ProcessSubTreeRevision(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "EffectiveDate":
                    incomingOrder.LastUpdateDate = ReadDateTimeNullable(xmlReader);
                    break;
            }
        }

        private void ProcessSubTreeStatus(IncomingOrderDto incomingOrder, XmlReader xmlReader)
        {
            switch (xmlReader.LocalName)
            {
                case "Code":
                    incomingOrder.Status = xmlReader.ReadString();
                    break;
            }
        }
    }
}