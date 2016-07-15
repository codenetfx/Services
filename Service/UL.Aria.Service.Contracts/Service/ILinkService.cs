using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Provides an interface for managing link dto objects.
    /// </summary>
    [ServiceContract]
    public interface ILinkService
    {
        /// <summary>
        /// Fetches the link matching the specified id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        LinkDto FetchById(string id);

        /// <summary>
        /// Fetches all active links associated with the specified entityId.
        /// </summary>
        /// <param name="entityId">The owner entity identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Entity/{entityId}", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<LinkDto> FetchLinksByEntity(string entityId);

        /// <summary>
        /// Deletes a link with the specified linkId.
        /// </summary>
        /// <param name="linkId">The link identifier.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{linkId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Delete(string linkId);

        /// <summary>
        /// Creates the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        LinkDto Create(LinkDto link);

        /// <summary>
        /// Updates the specified link.
        /// </summary>
        /// <param name="linkId">The link identifier.</param>
        /// <param name="link">The link.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{linkId}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(string linkId, LinkDto link);
        
        /// <summary>
        /// Searches the specified search criteria dto.
        /// </summary>
        /// <param name="searchCriteriaDto">The search criteria dto.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        LinkSearchDto Search(SearchCriteriaDto searchCriteriaDto);


        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/lookup", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<LookupDto> GetLookups();

    }
}
