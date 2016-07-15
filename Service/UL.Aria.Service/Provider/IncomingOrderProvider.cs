using System;
using System.Diagnostics.CodeAnalysis;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Notifications;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Class IncomingOrderProvider
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class IncomingOrderProvider : IncomingOrderCoreProvider, IIncomingOrderProvider
	{
		private readonly IProjectTemplateTaskCreationManager _projectTemplateTaskCreationManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="IncomingOrderProvider" /> class.
		/// </summary>
		/// <param name="incomingOrderRepository">The incoming order repository.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="projectProvider">The project provider.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="businessMessageProvider">The business message provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="smtpClientManager">SMTP Client Manager</param>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="orderNotificationCheckManager">The order notification check manager.</param>
		/// <param name="orderLineNotificationCheckManager">The order line notification check manager.</param>
		/// <param name="assetProvider">The asset provider.</param>
		/// <param name="inboundMessageProvider">The inbound message provider.</param>
		/// <param name="projectTemplateTaskCreationManager">The project template task creation manager.</param>
		public IncomingOrderProvider(IIncomingOrderRepository incomingOrderRepository,
			ITransactionFactory transactionFactory,
			IProjectProvider projectProvider,
			ILogManager logManager,
			IBusinessMessageProvider businessMessageProvider,
			IPrincipalResolver principalResolver,
			ISmtpClientManager smtpClientManager,
			INotificationManager notificationManager,
			IOrderNotificationCheckManager orderNotificationCheckManager,
			IOrderLineNotificationCheckManager orderLineNotificationCheckManager,
			IAssetProvider assetProvider,
			IInboundMessageProvider inboundMessageProvider,
			IProjectTemplateTaskCreationManager projectTemplateTaskCreationManager)
			: base(
				incomingOrderRepository, transactionFactory, projectProvider, logManager, businessMessageProvider, principalResolver,
				smtpClientManager, notificationManager, orderNotificationCheckManager, orderLineNotificationCheckManager,
				assetProvider, inboundMessageProvider)
		{
			_projectTemplateTaskCreationManager = projectTemplateTaskCreationManager;
		}

		/// <summary>
		///     Publishes the project creation request.
		/// </summary>
		/// <param name="projectCreationRequest">The project creation request.</param>
		public Guid PublishProjectCreationRequest(ProjectCreationRequest projectCreationRequest)
		{
			return base.PublishProjectCreationRequest(projectCreationRequest,
				_projectTemplateTaskCreationManager.CreateProjectTemplateTasks);
		}
	}
}