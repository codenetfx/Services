using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
	/// <summary>
	/// Interface IDocumentTemplateService
	/// </summary>
	[ServiceContract]
	public interface IDocumentTemplateService
	{
		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns>DocumentTemplateSearchResultSetDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DocumentTemplateSearchResultSetDto Search(SearchCriteriaDto searchCriteria);

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentTemplateDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DocumentTemplateDto FetchById(string id);

		/// <summary>
		/// Creates the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>DocumentTemplateDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DocumentTemplateDto Create(DocumentTemplateDto entity);

		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="entity">The entity.</param>
		/// <returns>DocumentTemplateDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		DocumentTemplateDto Update(string id, DocumentTemplateDto entity);

		/// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		[OperationContract]
		[FaultContract(typeof (InvalidOperationException))]
		[WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		void Delete(string id);

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <returns></returns>
	    [OperationContract]
	    [FaultContract(typeof (InvalidOperationException))]
	    [WebInvoke(UriTemplate = "/FetchAll", Method = "GET", RequestFormat = WebMessageFormat.Json,
	        ResponseFormat = WebMessageFormat.Json)]
	    IEnumerable<DocumentTemplateDto> FetchAll();


        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Validate", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<ValidationViolationDto> Validate(DocumentTemplateDto entity);

	}
}