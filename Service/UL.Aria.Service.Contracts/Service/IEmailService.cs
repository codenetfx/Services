using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    
    /// <summary>
    /// contract for facilitating the sending of e-mail
    /// </summary>
    [ServiceContract]
    public interface IEmailService
    {
	    /// <summary>
		/// Sends the contact us email.
		/// </summary>
		/// <param name="emailRequest">The email request.</param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(UriTemplate = "/ContactUs", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		EmailResponseDto ContactUs(EmailContactUsDto emailRequest);

		/// <summary>
		/// Portals the access request.
		/// </summary>
		/// <param name="loginId">The login unique identifier.</param>
		/// <returns></returns>
        [OperationContract]
		[WebInvoke(UriTemplate = "/PortalAccessRequest", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        EmailResponseDto PortalAccessRequest(string loginId);

		/// <summary>
		/// Portals the access granted.
		/// </summary>
		/// <param name="loginId">The login unique identifier.</param>
		/// <returns></returns>
        [OperationContract]
		[WebInvoke(UriTemplate = "/PortalAccessGranted", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
		EmailResponseDto PortalAccessGranted(string loginId);

    }
}