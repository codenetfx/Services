using System;
using System.ServiceModel;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.View;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// class the provides email capabilities
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
      InstanceContextMode = InstanceContextMode.PerCall)]
    public class EmailService : IEmailService
    {
        private readonly ISmtpClientManager _smtpClientManager;
	    private readonly IMapperRegistry _mapperRegistry;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="EmailService" /> class.
	    /// </summary>
	    /// <param name="smtpClientManager">The SMTP client manager.</param>
	    /// <param name="mapperRegistry">The mapper registry.</param>
	    public EmailService(ISmtpClientManager smtpClientManager, IMapperRegistry mapperRegistry)
        {
	        _smtpClientManager = smtpClientManager;
	        _mapperRegistry = mapperRegistry;
        }

	    /// <summary>
		/// Sends teh contact us email.
		/// </summary>
		/// <param name="emailRequest">The email request.</param>
		/// <returns></returns>
		public EmailResponseDto ContactUs(EmailContactUsDto emailRequest)
		{
			Guard.IsNotNullOrEmpty(emailRequest.ContactEmail, "ContactEmail");
			Guard.IsNotNullOrEmpty(emailRequest.IndustryName, "IndustryName");
			//Guard.IsNotNullOrEmpty(emailRequest.Subject, "Subject");
			Guard.IsNotNullOrEmpty(emailRequest.Message, "Message");

			var model = _mapperRegistry.Map<ContactUs>(emailRequest);
			var dto = new EmailResponseDto();

			try
			{
				_smtpClientManager.ContactUs(model);
			}
			catch (Exception ex)
			{
				dto.Error = true;
				dto.Message = ex.Message;
			}

			return dto;
		}

		/// <summary>
		/// Portals the access request.
		/// </summary>
		/// <param name="loginId">The login unique identifier.</param>
		/// <returns></returns>
		public EmailResponseDto PortalAccessRequest(string loginId)
		{
			Guard.IsNotNullOrEmpty(loginId, "loginId");

			//
			// send the email
			//
			var dto = new EmailResponseDto();
			try
			{
				_smtpClientManager.SendPortalAccessRequest(loginId);
			}
			catch (Exception ex)
			{
				dto.Error = true;
				dto.Message = ex.Message;
			}

			return dto;
		}

		/// <summary>
		/// Portals the access granted.
		/// </summary>
		/// <param name="loginId">The login unique identifier.</param>
		/// <returns></returns>
		public EmailResponseDto PortalAccessGranted(string loginId)
		{
			Guard.IsNotNullOrEmpty(loginId, "loginId");

			//
			// send the email
			//
			var dto = new EmailResponseDto();
			try
			{
				_smtpClientManager.SendPortalAccessGranted(loginId);
			}
			catch (Exception ex)
			{
				dto.Error = true;
				dto.Message = ex.Message;
			}

			return dto;
		}
    }
}
