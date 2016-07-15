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
    /// Defines operations for publishing <see cref="CertificationRequestDto"/> entities to their intended destination.
    /// </summary>
    [ServiceContract]
    public interface ICertificationRequestService
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
        string PublishCertificationRequest(CertificationRequestDto request);
    }
}
