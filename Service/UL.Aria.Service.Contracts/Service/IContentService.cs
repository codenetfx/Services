using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IContentService
    /// </summary>
    [ServiceContract]
    public interface IContentService
    {
        /// <summary>
        ///     Creates the content with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="contentStream">The content stream.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Create(string id, Stream contentStream);

        /// <summary>
        ///     Updates the content specified by the id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="contentStream">The content stream.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, Stream contentStream);

        /// <summary>
        ///     Fetches the content specified by the id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Stream.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Stream Fetch(string id);
    }
}