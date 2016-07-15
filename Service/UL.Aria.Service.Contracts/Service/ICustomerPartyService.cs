using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Service contract for customer information.
    /// </summary>
    [ServiceContract]
    public interface ICustomerPartyService
    {
        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{externalId}",
            Method = "POST",
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        CustomerOrganizationDto Fetch(string externalId);

        /// <summary>
        /// Fetches the agent.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{orderNumber}/agent",
            Method = "POST",
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IncomingOrderPartiesDto FetchParties(string orderNumber);

        /// <summary>
        /// Fills the contact.
        /// </summary>
        /// <param name="recordToFill">The record to fill.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{orderNumber}/fillcontact",
            Method = "POST",
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IncomingOrderPartyDto FillContact(IncomingOrderPartyDto recordToFill, string orderNumber);

        /// <summary>
        /// Fills the party.
        /// </summary>
        /// <param name="recordToFill">The record to fill.</param>
        /// <param name="orderNumber">The order number.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{orderNumber}/fillparty",
            Method = "POST",
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IncomingOrderPartyDto FillParty(IncomingOrderPartyDto recordToFill, string orderNumber);
    }
}