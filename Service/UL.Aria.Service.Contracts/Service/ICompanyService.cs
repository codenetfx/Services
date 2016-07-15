using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Service interface contract for Company entities.
    /// </summary>
    [ServiceContract]
    public interface ICompanyService
    {
        /// <summary>
        /// Gets the ul company identifier.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/ULCompanyId", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Guid GetULCompanyId();


        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <returns>
        /// Company Dto
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<CompanyDto> FetchAll();

        /// <summary>
        ///     Search based on the provided criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CompanySearchModelDto Search(SearchCriteriaDto searchCriteria);

        /// <summary>
        /// Gets the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Company Dto</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat =  WebMessageFormat.Json)]
        CompanyDto FetchById(string id);

        /// <summary>
        /// Gets the company by external id.
        /// </summary>
        /// <param name="externalId">The external id.</param>
        /// <returns>Company.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/ExternalId/{externalId}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CompanyDto FetchByExternalId(string externalId);

        /// <summary>
        /// Publishes the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>The published company</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/CreateCompany", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CompanyDto Create(CompanyDto company);

        /// <summary>
        /// Updates the company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/UpdateCompany", Method = "PUT", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CompanyDto Update(CompanyDto company);

        /// <summary>
        /// Deletes the company by id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void Delete(string id);

        /// <summary>
        /// Fetches all count.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Count", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        
        int FetchAllCount();
    }
}
