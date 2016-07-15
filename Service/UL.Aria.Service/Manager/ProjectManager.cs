using System;
using System.Diagnostics.CodeAnalysis;

using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager.Validation;
using UL.Aria.Service.Notifications;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	///     implements concrete functionality for
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ProjectManager : ProjectCoreManager, IProjectManager
	{
		private readonly IProjectTemplateTaskCreationManager _projectTemplateTaskCreationManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectManager" /> class.
		/// </summary>
		/// <param name="projectDocumentBuilder">The project document builder.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="incomingOrderProvider">The incoming order provider.</param>
		/// <param name="projectProvider">The project provider.</param>
		/// <param name="taskProvider">The task provider.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="searchProvider">The search provider.</param>
		/// <param name="smtpClientManager">The SMTP client manager.</param>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="projectValidationManager">The project validation manager.</param>
		/// <param name="projectNotificationCheckManager">The project notification check manager.</param>
		/// <param name="orderNotificationCheckManager">The order notification check manager.</param>
		/// <param name="orderLineNotificationCheckManager">The order line notification check manager.</param>
		/// <param name="projectTemplateTaskCreationManager">The project template task creation manager.</param>
		public ProjectManager(IMultiProjectDocumentBuilder projectDocumentBuilder, ITransactionFactory transactionFactory,
			IIncomingOrderProvider incomingOrderProvider, IProjectProvider projectProvider, ITaskProvider taskProvider,
			ILogManager logManager, IMapperRegistry mapperRegistry, ISearchProvider searchProvider,
			ISmtpClientManager smtpClientManager, INotificationManager notificationManager,
			IProjectValidationManager projectValidationManager, IProjectNotificationCheckManager projectNotificationCheckManager,
			IOrderNotificationCheckManager orderNotificationCheckManager,
			IOrderLineNotificationCheckManager orderLineNotificationCheckManager,
			IProjectTemplateTaskCreationManager projectTemplateTaskCreationManager)
			: base(
				projectDocumentBuilder, transactionFactory, incomingOrderProvider, projectProvider, taskProvider, logManager,
				mapperRegistry, searchProvider, smtpClientManager, notificationManager, projectValidationManager,
				projectNotificationCheckManager, orderNotificationCheckManager, orderLineNotificationCheckManager)
		{
			_projectTemplateTaskCreationManager = projectTemplateTaskCreationManager;
		}

		/// <summary>
		/// Updates the specified project.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="project">The project.</param>
		/// <exception cref="System.ArgumentException">Project cannot be canceled it still has line items.
		/// or
		/// Cannot change company on Project while it still has line items.</exception>
		public void Update(Guid id, Project project)
		{
			base.Update(id, project, false, _projectTemplateTaskCreationManager.CreateProjectTemplateTasksForUpdate);
		}

	}
}