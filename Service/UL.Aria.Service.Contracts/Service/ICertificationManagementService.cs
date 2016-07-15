using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Defines operations for publishing <see cref="CertificationRequestDto"/> entities to their intended destination.
    /// </summary>
    [ServiceContract]
    public interface ICertificationManagementService
    {
        /// <summary>
        /// Submits a certification request.
        /// </summary>
        /// <param name="request">The request.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/",
            Method = "POST",
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string PublishCertificationRequest(CertificationManagementDto request);

        /// <summary>
        /// Fetches the certification requests.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{taskId}",
            Method = "POST",
            RequestFormat = WebMessageFormat.Xml,
            ResponseFormat = WebMessageFormat.Xml,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<CertificationManagementDto> FetchCertificationRequests(string taskId);
    }
}