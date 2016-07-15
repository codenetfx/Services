using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     defines contract for manipulating projects
    /// </summary>
    [ServiceContract]
    public interface IProjectService
    {
        /// <summary>
        ///     Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     ProfileDto
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ProjectDto Fetch(string id);

        /// <summary>
        ///     Gets the project download by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}/Download",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetProjectDownloadById(string id);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="project">The project.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}",
            Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(string id, ProjectDto project);

        /// <summary>
        /// Gets the project download by id.
        /// </summary>
        /// <param name="ids">The ids, pipe delimited.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Download",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetMultipleProjectDownload(string ids);


        /// <summary>
        /// Validates the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "{id}/Validate", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<ProjectValidationEnumDto> Validate(string id, ProjectDto project);
    }
}