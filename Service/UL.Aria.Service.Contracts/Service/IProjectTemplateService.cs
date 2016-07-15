using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IProjectTemplateService
    /// </summary>
    [ServiceContract]
    public interface IProjectTemplateService
    {
        /// <summary>
        ///     Fetches the specified entity type.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<ProjectTemplateDto> FetchAll();
       
        /// <summary>
        ///     Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProjectTemplateDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ProjectTemplateDto FetchById(string id);
       
        /// <summary>
        ///     Fetches editable version by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProjectTemplateDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/Edit/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ProjectTemplateDto FetchEditableById(string id);

        /// <summary>
        ///     Creates the specified project template.
        /// </summary>
        /// <param name="projectTemplate">The project template.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Create(ProjectTemplateDto projectTemplate);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="projectTemplate">The project template.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, ProjectTemplateDto projectTemplate);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string id);

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns>ProjectTemplateSearchResultSetDto.</returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/Search", Method = "POST", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		ProjectTemplateSearchResultSetDto Search(SearchCriteriaDto searchCriteria);
    }
}