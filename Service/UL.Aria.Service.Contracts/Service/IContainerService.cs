using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Service interface contract for containers.
    /// </summary>
    [ServiceContract]
    public interface IContainerService
    {
        /// <summary>
        ///     Gets the container by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Container Dto</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ContainerDto FetchById(string id);

        /// <summary>
        ///     Updates the specified container.
        /// </summary>
        /// <param name="container">The container.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(ContainerDto container);

        /// <summary>
        ///     Deletes the Container by id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string id);
    }
}