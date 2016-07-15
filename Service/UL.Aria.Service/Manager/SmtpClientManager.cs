using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Common.Authorization;
using UL.Aria.Common.BusinessMessage;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.View;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Templating;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Manages Smtp Client Provider activities
    /// </summary>
    public class SmtpClientManager : ISmtpClientManager
    {
        private readonly IBusinessMessageProvider _businessMessageProvider;        
        private readonly IServiceConfiguration _serviceConfiguration;
        private readonly ISmtpClientProvider _smtpClientProvider;
        private readonly IAriaTemplateService _templateService;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProfileManager _profileManager;
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="SmtpClientManager" /> class.
        /// </summary>
        /// <param name="smtpClientProvider">The SMTP client provider.</param>
        /// <param name="businessMessageProvider">The business message provider.</param>
        /// <param name="templateService">The template service.</param>
        /// <param name="serviceConfiguration">The service configuration.</param>
        /// <param name="profileManager">The profile manager.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public SmtpClientManager(ISmtpClientProvider smtpClientProvider,
            IBusinessMessageProvider businessMessageProvider, IAriaTemplateService templateService,
            IServiceConfiguration serviceConfiguration, IProfileManager profileManager, IPrincipalResolver principalResolver)
        {
            _smtpClientProvider = smtpClientProvider;
            _businessMessageProvider = businessMessageProvider;
            _templateService = templateService;
            _serviceConfiguration = serviceConfiguration;
            _profileManager = profileManager;
            _principalResolver = principalResolver;
        }      

        /// <summary>
        ///     Builds and sends the Contact Us email
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <exception cref="System.Net.Mail.SmtpException"></exception>
        public void ContactUs(ContactUs contact)
        {
            Guard.IsNotNullOrEmpty(contact.ContactEmail, "ContactEmail");
            Guard.IsNotNullOrEmpty(contact.IndustryName, "IndustryName");
            //Guard.IsNotNullOrEmpty(contact.Subject, "Subject");
            Guard.IsNotNullOrEmpty(contact.Message, "Message");
            if (string.IsNullOrEmpty(contact.Subject))
            {
                contact.Subject = "UL Customer Portal Feedback";
            }

            var messageBody = _templateService.RenderTemplate("Email/ContactUs.cshtml", contact);

            var maildAddressTo = CreateMailAddress(_serviceConfiguration.CustomerSupportEmail);
            var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = contact.Subject,
                IsBodyHtml = true,
                Body = messageBody
            };

            message.CC.Add(contact.ContactEmail);

            if (!string.IsNullOrEmpty(contact.ContactCopyEmail))
                message.CC.Add(contact.ContactCopyEmail);
            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.ContactUs);
        }

	    /// <summary>
	    /// Sends the the prooduct status change email.
	    /// </summary>
	    /// <param name="product"></param>
	    public void SendProductStatusChanged(Product product)
		{
			var messageBody = _templateService.RenderTemplate("Email/ProductStatusChange.cshtml", product);
			var maildAddressTo = CreateMailAddress(_serviceConfiguration.ProductSupportEmail);
			var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
			var message = new MailMessage(maildAddressFrom, maildAddressTo)
			{
				Subject = "Product Status Changed",
				Body = messageBody,
				IsBodyHtml = true
			};

			if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
				message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

			EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.UpdateProductStatus);
        }

        /// <summary>
        ///     Sends the portal access request.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        public void SendPortalAccessRequest(string loginId)
        {
            var user = _profileManager.FetchByUserName(loginId);

            var messageBody = _templateService.RenderTemplate("Email/AccessRequest.cshtml", user);
            var maildAddressTo = CreateMailAddress(_serviceConfiguration.CustomerSupportEmail);
			var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = "UL Customer Portal Access Request",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        /// <summary>
        ///     Sends the portal access granted.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        public void SendPortalAccessGranted(string loginId)
        {
            var user = _profileManager.FetchByUserName(loginId);

            var messageBody = _templateService.RenderTemplate("Email/AccessGrant.cshtml", user);
            var maildAddressTo = CreateMailAddress(user.LoginId);
			var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = "Account Access Granted",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }
        
        /// <summary>
        /// This method sends email notification to create a new company.
        /// </summary>        
        /// <param name="model">model</param>
        public void SendNewCompany(NewCompanyEmail model)
        {
            var messageBody = _templateService.RenderTemplate("Email/NewCompanyCreate.cshtml", model);
            var mailAddressTo = CreateMailAddress(_serviceConfiguration.PortalAdminEmail);
            var mailAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(mailAddressFrom, mailAddressTo)
            {
                Subject = "Create New Customer Company Profile",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        /// <summary>
        /// This method sends out an email when a project is created.
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="model">model</param>
        public void SendProjectCreated(string user, ProjectEmail model)
        {
            var currentUser = _profileManager.FetchById(model.ActorId);
            model.ActorName = currentUser.DisplayName;
            model.ActorLoginId = currentUser.LoginId;
            var messageBody = _templateService.RenderTemplate("Email/ProjectCreate.cshtml", model);
            var maildAddressTo = CreateMailAddress(user);
            var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = "Project Created",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        /// <summary>
        /// This method sends out an email when project handler is reassigned.
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="model">model</param>
        public void SendProjectAssigned(string user, ProjectAssignmentEmail model)
        {
            var actor = _principalResolver.UserId;
            var currentUser = _profileManager.FetchById(actor);
            model.ActorName = currentUser.DisplayName;
            model.ActorLoginId = currentUser.LoginId;
            var messageBody = _templateService.RenderTemplate("Email/ProjectAssigned.cshtml", model);
            var mailAddressTo = new MailAddress(user);
            var mailAddressFrom = new MailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(mailAddressFrom, mailAddressTo)
            {
                Subject = "Project Assigned",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        /// <summary>
        /// This method sends out an email to new handler when project handler is reassigned.
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="model">model</param>
        public void SendProjectAssignedNewHandler(string user, ProjectAssignmentEmail model)
        {
            var actor = _principalResolver.UserId;
            var currentUser = _profileManager.FetchById(actor);
            model.ActorName = currentUser.DisplayName;
            model.ActorLoginId = currentUser.LoginId;
            var messageBody = _templateService.RenderTemplate("Email/ProjectAssignedNewHandler.cshtml", model);
            var mailAddressTo = new MailAddress(user);
            var mailAddressFrom = new MailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(mailAddressFrom, mailAddressTo)
            {
                Subject = "Project Assigned",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

		/// <summary>
		/// This method sends out an email to the recipient (project handler and/or order owner) when the project is completed.
		/// </summary>
		/// <param name="user">user</param>
		/// <param name="model">model</param>
		public void SendProjectCompleted(string user, ProjectCompletedEmail model)
		{
			var actor = _principalResolver.UserId;
			var currentUser = _profileManager.FetchById(actor);
			model.ActorName = currentUser.DisplayName;
			model.ActorLoginId = currentUser.LoginId;
			var messageBody = _templateService.RenderTemplate("Email/ProjectCompleted.cshtml", model);
			var mailAddressTo = new MailAddress(user);
			var mailAddressFrom = new MailAddress(_serviceConfiguration.DefaultSenderEmail);
			var message = new MailMessage(mailAddressFrom, mailAddressTo)
			{
				Subject = "Project Completed",
				Body = messageBody,
				IsBodyHtml = true
			};

			if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
				message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

			EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
		}

		/// <summary>
		/// This method sends out an email to the recipient when the project order owner is reassigned.
		/// </summary>
		/// <param name="user">user</param>
		/// <param name="model">model</param>
		public void SendProjectOrderOwnerReassigned(string user, ProjectOrderOwnerReassignedEmail model)
		{
			var actor = _principalResolver.UserId;
			var currentUser = _profileManager.FetchById(actor);
			model.ActorName = currentUser.DisplayName;
			model.ActorLoginId = currentUser.LoginId;
			var messageBody = _templateService.RenderTemplate("Email/ProjectOrderOwnerReassigned.cshtml", model);
			var mailAddressTo = new MailAddress(user);
			var mailAddressFrom = new MailAddress(_serviceConfiguration.DefaultSenderEmail);
			var message = new MailMessage(mailAddressFrom, mailAddressTo)
			{
				Subject = "Project Order Owner Reassigned",
				Body = messageBody,
				IsBodyHtml = true
			};

			if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
				message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

			EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
		}

        /// <summary>
        ///     Sends the account created email.
        /// </summary>
        /// <param name="user">The user.</param>
        public void SendAccountCreated(ProfileBo user)
        {
            var messageBody = _templateService.RenderTemplate("Email/AccountCreate.cshtml", user);
            var maildAddressTo = CreateMailAddress(user.LoginId);
			var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = "Account Created",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        private void EmbedLogoSendAndLog(MailMessage message, string messageBody, AuditMessageIdEnumDto auditMessageId, IEnumerable< Func<LinkedResource>> additionalResources = null)
        {
            var contentType = new ContentType(message.IsBodyHtml ? MediaTypeNames.Text.Html : MediaTypeNames.Text.Plain);
            var alt = AlternateView.CreateAlternateViewFromString(messageBody, contentType);
            List<LinkedResource> resources = new List<LinkedResource>();
           
                resources.Add(GetLogoResource());
                if (null != additionalResources)
                {
                    foreach (var additionalResource in additionalResources)
                    {
                        resources.Add(additionalResource());
                    }
                }
                foreach (var linkedResource in resources)
                {
                    alt.LinkedResources.Add(linkedResource);
                }
                
                message.AlternateViews.Add(alt);

                string errorMessage;
                bool isSuccess = _smtpClientProvider.Send(message, out errorMessage);

                if (!isSuccess)
                    throw new SmtpException(errorMessage);
           
            //
            // log business message
            //
            string businessMessage = string.Format(
                "An email message was sent FROM: {0} TO: {1} SUBJECT: {2} TEXT: {3}", message.From, message.To,
                message.Subject, messageBody);
            _businessMessageProvider.Publish(auditMessageId, businessMessage);
        }

        private LinkedResource GetTaskCompletedResource()
        {
            return GetResource("UL.Aria.Service.Views.Email.TaskCompleted.gif", "taskcompleted", MediaTypeNames.Image.Gif);
        }
        private LinkedResource GetTaskCompletedLabelResource()
        {
            return GetResource("UL.Aria.Service.Views.Email.TaskCompletedLabel.gif", "taskcompletedlabel", MediaTypeNames.Image.Gif);
        }

        private LinkedResource GetLogoResource()
        {
            return GetResource("UL.Aria.Service.Views.Email.logo.gif", "logo", MediaTypeNames.Image.Gif);
        }

        private LinkedResource GetResource(string resourceName, string resourceId, string contentType)
        {
            var ass = Assembly.GetExecutingAssembly();
            var imageStream = ass.GetManifestResourceStream(resourceName);
            var logo = new LinkedResource(imageStream, new ContentType(contentType));

            //this is what the HTML message body will use to reference this image
            logo.ContentId = resourceId;

            return logo;
        }

        /// <summary>
        /// Sends the task completed email.
        /// </summary>
        /// <param name="handler">The login unique identifier.</param>
        /// <param name="taskCompletedEmail">The task completed email.</param>
        public void SendTaskCompleted(string handler, TaskCompletedEmail taskCompletedEmail)
        {
            var actor = _principalResolver.UserId;
            var actorProfile = _profileManager.FetchById(actor);
            var handlerProfile = _profileManager.FetchByUserName(handler);
            if (actorProfile != null)
            {
                taskCompletedEmail.ActorLoginId = actorProfile.LoginId;
                taskCompletedEmail.ActorName = actorProfile.DisplayName;                
            }

            if (handlerProfile != null)
            {
                taskCompletedEmail.RecipientName = handlerProfile.DisplayName;
            }
            else if (!string.IsNullOrWhiteSpace(handler))
            {
                taskCompletedEmail.RecipientName = handler;
            }

            int status = string.Compare(handler, taskCompletedEmail.ActorLoginId, true);

            if (status.Equals(0))
                return;

            var messageBody = _templateService.RenderTemplate("Email/TaskComplete.cshtml", taskCompletedEmail);
            var mailAddressTo = CreateMailAddress(handler);
            var mailAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(mailAddressFrom, mailAddressTo)
            {
                Subject = "Task Completed",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail, new List<Func<LinkedResource>> { GetTaskCompletedResource, GetTaskCompletedLabelResource });
        }
        
        /// <summary>
        /// Sends the portal task assign.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <param name="taskEmail">The task email.</param>
        public void SendTaskAssigned(string loginId, TaskEmail taskEmail)
        {
            int status = string.Compare(loginId, taskEmail.ActorLoginId, true);

            if (status.Equals(0))
                return;

            var messageBody = _templateService.RenderTemplate("Email/TaskAssign.cshtml", taskEmail);
            var maildAddressTo = CreateMailAddress(loginId);
            var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = "Task Assigned",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        /// <summary>
        /// Sends the portal task reassign.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <param name="taskEmail">The task email.</param>
        public void SendTaskReassigned(string loginId, TaskEmail taskEmail)
        {
            int status = string.Compare(loginId, taskEmail.ActorLoginId, true);

            if (status.Equals(0))
                return;
            
            var messageBody = _templateService.RenderTemplate("Email/TaskReassign.cshtml", taskEmail);
            var maildAddressTo = CreateMailAddress(loginId);
			var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = "Task Reassigned",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        /// <summary>
        /// Sends the portal task delete.
        /// </summary>
        /// <param name="loginId">The login unique identifier.</param>
        /// <param name="taskEmail">The task email.</param>
        public void SendTaskDelete(string loginId, TaskEmail taskEmail)
        {
            int status = string.Compare(loginId, taskEmail.ActorLoginId, true);

            if (status.Equals(0))
                return;
            
            var messageBody = _templateService.RenderTemplate("Email/TaskDelete.cshtml", taskEmail);
            var maildAddressTo = CreateMailAddress(loginId);
			var maildAddressFrom = CreateMailAddress(_serviceConfiguration.DefaultSenderEmail);
            var message = new MailMessage(maildAddressFrom, maildAddressTo)
            {
                Subject = "Task Deleted",
                Body = messageBody,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(_serviceConfiguration.GlobalBccEmail))
                message.Bcc.Add(_serviceConfiguration.GlobalBccEmail);

            EmbedLogoSendAndLog(message, messageBody, AuditMessageIdEnumDto.SendEmail);
        }

        private MailAddress CreateMailAddress(string address)
        {
            return new MailAddress(address.SafeTrim());
        }
    }
}