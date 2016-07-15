using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading;

using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;
using Microsoft.ServiceBus.Messaging;

using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Aria.Service.InboundOrderProcessing.Manager;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Security;
using UL.Aria.Web.Common.Configuration;
using UL.Aria.Web.Common.Services;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.InboundMessageProcessor.WebJob
{
	/// <summary>
	/// Class Functions.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class Functions
	{
		/// <summary>
		/// The _max dequeue count
		/// </summary>
		// ReSharper disable once InconsistentNaming
		private static readonly int _maxDequeueCount;

		/// <summary>
		/// Initializes static members of the <see cref="Functions"/> class.
		/// </summary>
		static Functions()
		{
			_maxDequeueCount = ConfigurationManager.AppSettings.GetValue("InboundMessage.DequeueCount", 3);
		}

		public static void SetPrincipal(IProfileService profileService)
		{
			var profile = profileService.FetchByIdOrUserName("portalservices@ul.com");

			var claimsIdentity = new ClaimsIdentity(
				profile.Claims.Select(x => new Claim(x.EntityClaim, x.Value ?? x.EntityId.ToString()))
				);
			claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, profile.LoginId));
			claimsIdentity.AddClaim(new Claim(SecuredClaims.UserId, profile.Id.Value.ToString("N")));
			if (
				claimsIdentity.HasClaim(
					x =>
						x.Type == SecuredClaims.CompanyAccess &&
						x.Value.ToUpperInvariant() == "46F65EA8-913D-4F36-9E28-89951E7CE8EF"))
			{
				claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "UL-Employee"));
			}
			Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
		}

		/// <summary>
		/// Processes the order message.
		/// </summary>
		/// <param name="brokeredMessage">The brokered message.</param>
		public static void ProcessOrderMessage(
			[ServiceBusTrigger("%InboundMessage.QueueName%")] BrokeredMessage brokeredMessage)
		{
			using (new NativeMethods.Impersonation(WorkerRole._domain, WorkerRole._user, WorkerRole._pwd))
			{
				string message = null;
				var inboundMessage = brokeredMessage.GetBody<InboundMessageDto>();
				var blobMetadata = new Dictionary<string, string>
				{
					{"MessageId", inboundMessage.MessageId},
					{"ExternalMessageId", inboundMessage.ExternalMessageId},
					{"Receiver", inboundMessage.Receiver},
					{"Originator", inboundMessage.Originator}
				};

				try
				{
					Trace.CorrelationManager.ActivityId = Guid.NewGuid();

					var logMessage1 = new LogMessage((int) AuditMessageIdEnumDto.BuyMessageDequeued, LogPriority.Low,
						TraceEventType.Information, "Buy Message Dequeued", LogCategory.System,
						LogCategory.InboundOrderMessageService);
					logMessage1.Data.Add("MessageId", inboundMessage.MessageId);
					logMessage1.Data.Add("ExternalMessageId", inboundMessage.ExternalMessageId);
					logMessage1.Data.Add("Originator", inboundMessage.Originator);
					logMessage1.Data.Add("Receiver", inboundMessage.Receiver);
					var logManager =
						(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
					logManager.Log(logMessage1);

					var inboundMessageProvider =
						(IInboundMessageProvider) ContainerLocator.Container.Resolve(typeof (IInboundMessageProvider));
					var inboundMessageBlob = inboundMessageProvider.FetchNewMessage(inboundMessage.MessageId);
					message = inboundMessageBlob.Message;

					if (brokeredMessage.DeliveryCount > _maxDequeueCount)
					{
						var logMessage = new LogMessage(MessageIds.InboundOrderProcessingDequeueCountExceeded)
						{
							LogPriority = LogPriority.Critical,
							Severity = TraceEventType.Critical,
							Message = "Dequeue count exceeded"
						};
						logMessage.LogCategories.Add(LogCategory.InboundProcessor);

						logMessage.Data.Add("MessageId", inboundMessage.MessageId);
						logMessage.Data.Add("ExternalMessageId", inboundMessage.ExternalMessageId);
						logMessage.Data.Add("Originator", inboundMessage.Originator);
						logMessage.Data.Add("Receiver", inboundMessage.Receiver);

						logManager.Log(logMessage);
						inboundMessageProvider.SaveFailedMessage(inboundMessage.MessageId, message, blobMetadata);
						inboundMessageProvider.DeleteNewMessage(inboundMessage.MessageId);
						return;
					}

					var messageManager =
						(IInboundMessageWebJobManager) ContainerLocator.Container.Resolve(typeof (IInboundMessageWebJobManager));

					var portalConfig = new PortalConfiguration();
					var proxyConfig = new ProxyConfigurationSource(portalConfig);
					var profileService = new ProfileServiceProxy(proxyConfig);
					SetPrincipal(profileService);
					messageManager.Process(inboundMessage, message);
				}
				catch (Exception ex)
				{
					var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingProcessOrderMessageException,
						LogCategory.InboundProcessor,
						LogPriority.Critical,
						TraceEventType.Critical);

					logMessage.Data.Add("MessageId", inboundMessage.MessageId);
					logMessage.Data.Add("ExternalMessageId", inboundMessage.ExternalMessageId);
					logMessage.Data.Add("Originator", inboundMessage.Originator);
					logMessage.Data.Add("Receiver", inboundMessage.Receiver);

					var logManager =
						(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
					logManager.Log(logMessage);
					var inboundMessageProvider =
						(IInboundMessageProvider) ContainerLocator.Container.Resolve(typeof (IInboundMessageProvider));
					if (message != null)
					{
						inboundMessageProvider.SaveFailedMessage(inboundMessage.MessageId, message, blobMetadata);
					}
					throw;
				}
			}
		}
	}
}