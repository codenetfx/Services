using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Defines service operations for <see cref="BusinessUnitDto"/> entities.
    /// </summary>
    [ServiceContract]
    public interface IBusinessUnitService
    {
        /// <summary>
        /// Searches the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        BusinessUnitSearchResultSetDto Search(SearchCriteriaDto searchCriteria);

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        BusinessUnitDto FetchById(string id);

        /// <summary>
        /// Creates the specified business unit.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        BusinessUnitDto Create(BusinessUnitDto entity);

        /// <summary>
        /// Validates the specified business unit.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Validate", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<ValidationViolationDto> Validate(BusinessUnitDto entity);
        
        /// <summary>
        /// Updates the business unit.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        BusinessUnitDto Update(string id, BusinessUnitDto entity);

        /// <summary>
        /// Deletes the business unit by id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Delete(string id);

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Count", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<BusinessUnitDto> FetchAll();

    }
}
