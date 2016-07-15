using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Contract that defines the operations for the scratch storage space whereby temporary files can be stored
    /// </summary>
    [ServiceContract]
    public interface IScratchSpaceService
    {
        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List/{userId}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IList<ScratchFileDescriptorDto> FetchAll(string userId);


        /// <summary>
        /// Fetches the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{userId}/{fileId}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Stream FetchContent(string userId, string fileId);


        /// <summary>
        /// Publishes the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileName"></param>
        /// <param name="contentStream">The content stream.</param>
        /// <returns>
        /// return the Id for that file
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{userId}/{fileName}", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string PublishContent(string userId, string fileName, Stream contentStream);


        /// <summary>
        /// Deletes the specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{userId}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string userId);
    }
}
