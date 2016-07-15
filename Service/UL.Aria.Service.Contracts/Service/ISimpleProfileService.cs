using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Basic operations for <see cref="ProfileDto"/>
    /// </summary>
    [ServiceContract]
    public interface ISimpleProfileService
    {
        /// <summary>
        /// Gets the profile by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// ProfileDto
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ProfileDto FetchByIdOrUserName(string id);

    }
}