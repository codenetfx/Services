using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.View;
using UL.Aria.Service.Logging;
using UL.Aria.Service.Manager.Validation;
using UL.Aria.Service.Notifications;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	///     implements concrete functionality for
	/// </summary>
	public class ProjectCoreManager : IProjectCoreManager
	{
		private readonly IIncomingOrderCoreProvider _incomingOrderProvider;
		private readonly ILogManager _logManager;
		private readonly IMapperRegistry _mapperRegistry;
		private readonly INotificationManager _notificationManager;
		private readonly IOrderLineNotificationCheckManager _orderLineNotificationCheckManager;
		private readonly IOrderNotificationCheckManager _orderNotificationCheckManager;
		private readonly IMultiProjectDocumentBuilder _projectDocumentBuilder;
		private readonly IProjectNotificationCheckManager _projectNotificationCheckManager;
		private readonly IProjectProvider _projectProvider;
		private readonly IProjectValidationManager _projectValidationManager;
		private readonly ISearchProvider _searchProvider;
		private readonly ISmtpClientManager _smtpClientManager;
		private readonly ITaskProvider _taskProvider;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectCoreManager" /> class.
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
		public ProjectCoreManager(IMultiProjectDocumentBuilder projectDocumentBuilder, ITransactionFactory transactionFactory,
			IIncomingOrderCoreProvider incomingOrderProvider, IProjectProvider projectProvider, ITaskProvider taskProvider,
			ILogManager logManager, IMapperRegistry mapperRegistry, ISearchProvider searchProvider,
			ISmtpClientManager smtpClientManager, INotificationManager notificationManager,
			IProjectValidationManager projectValidationManager, IProjectNotificationCheckManager projectNotificationCheckManager,
			IOrderNotificationCheckManager orderNotificationCheckManager,
			IOrderLineNotificationCheckManager orderLineNotificationCheckManager)
		{
			_projectDocumentBuilder = projectDocumentBuilder;
			_transactionFactory = transactionFactory;
			_incomingOrderProvider = incomingOrderProvider;
			_projectProvider = projectProvider;
			_taskProvider = taskProvider;
			_logManager = logManager;
			_mapperRegistry = mapperRegistry;
			_searchProvider = searchProvider;
			_smtpClientManager = smtpClientManager;
			_notificationManager = notificationManager;
			_projectValidationManager = projectValidationManager;
			_projectNotificationCheckManager = projectNotificationCheckManager;
			_orderNotificationCheckManager = orderNotificationCheckManager;
			_orderLineNotificationCheckManager = orderLineNotificationCheckManager;
		}

		/// <summary>
		///     Gets the project by id.
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns></returns>
		public Project GetProjectById(Guid projectId)
		{
			var project = GetProjectWithoutTaskRollupsById(projectId);

			//
			// derive the group summary by looking at every task's status
			//
			if (project.ContainerId == null) return project;
			var tasks = _taskProvider.FetchAllFlatList(project.ContainerId.Value);
				//TODO: find a better way to get this other than retrieving every row in the system
			if (tasks != null)
				project.IsOnTrack = tasks.All(x => x.Progress == TaskProgressEnumDto.OnTrack);

			return project;
		}

		/// <summary>
		/// Gets the project without task rollups(IsOnTrack will not be set correctly) by identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public Project GetProjectWithoutTaskRollupsById(Guid projectId)
		{
			var project = _projectProvider.FetchById(projectId);
			if (project.CompanyId == Guid.Empty)
				project.CompanyId = null;
			return project;
		}

		/// <summary>
		///     Gets the project export.
		/// </summary>
		/// <param name="projectId">The project id.</param>
		/// <returns></returns>
		public Stream GetProjectDownload(Guid projectId)
		{
			return GetMultipleProjectDownload(new[] {projectId});
		}


		/// <summary>
		/// Gets all of the <see cref="Project"/>s. use only for things like exports, never from the UI.
		/// </summary>
		/// <returns>the <see cref="Project"/>s.</returns>
		public IEnumerable<Project> GetAllProjects()
		{
			return _projectProvider.FetchProjects();
		}

		/// <summary>
		/// Gets all project headerss.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Guid> GetAllProjectIds()
		{
			return _projectProvider.FetchAllIds();
		}

		/// <summary>
		/// Gets all of the <see cref="Task"/> objects assocatied with a given <see cref="Project"/> flattened for that project.
		/// </summary>
		/// <param name="project"></param>
		/// <returns>the <see cref="Task"/>s.</returns>
		public ProjectDetail GetProjectDetail(Project project)
		{
			var projectDetail = new ProjectDetail {Project = project};
			var tasks = _taskProvider.FetchAllWithDeleted(projectDetail.Project.ContainerId.Value);
			if (null != tasks)
			{
				projectDetail.Tasks.AddRange(tasks);
			}
			return projectDetail;
		}

		/// <summary>
		///     Gets the multiple project download.
		/// </summary>
		/// <param name="projectIds">The project ids.</param>
		/// <returns></returns>
		public Stream GetMultipleProjectDownload(IEnumerable<Guid> projectIds)
		{
			var projects = new List<ProjectDetail>();
			foreach (var projectId in projectIds)
			{
				var projectDetail = new ProjectDetail {Project = GetProjectById(projectId)};
				var tasks = _taskProvider.FetchAllFlatList(projectDetail.Project.ContainerId.Value);
				if (null != tasks)
				{
					projectDetail.Tasks.AddRange(tasks.OrderBy(t => t.TaskNumber));
				}
				//search SP for all assocaited projects
				var products = _searchProvider.FetchProductsByProjectId(projectId);
				projectDetail.ProductIds.AddRange(products);

				projects.Add(projectDetail);
			}
			return _projectDocumentBuilder.Build(projects);
		}

		/// <summary>
		/// Updates the specified project.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="project">The project.</param>
		/// <param name="sendProjectCompleteEmail">if set to <c>true</c> [send project complete email].</param>
		/// <param name="additionalWorkCreateProjectTemplateTasks">The create project template tasks.</param>
		/// <exception cref="System.ArgumentException">Project cannot be canceled it still has line items.
		/// or
		/// Cannot change company on Project while it still has line items.</exception>
		public void Update(Guid id, Project project, bool sendProjectCompleteEmail, Action<Guid?, Guid> additionalWorkCreateProjectTemplateTasks = null)
		{
			using (var scope = _transactionFactory.Create())
			{
				var existingProject = GetProjectById(id);
				project.MessageId = existingProject.MessageId;

				//
				//cleanup or alter notifications based on changes to the project
				//
				BrokerNotificationActions(existingProject, project);

				//
				// If project status is being changed to canceled and it has line items it cannot be updated
				//
				if (project.ProjectStatus == ProjectStatusEnumDto.Canceled && project.ServiceLines.Count > 0)
				{
					var logMessage = new LogMessage(MessageIds.ProjectManangerProjectCannotBeCanceledWithLines, LogPriority.Medium,
						TraceEventType.Verbose,
						"Project cannot be canceled it still has line items.",
						LogCategory.Project);
					logMessage.Data.Add("ProjectId", project.Id.Value.ToString());
					logMessage.Data.Add("LineItemCount",
						project.ServiceLines.Count.ToString(CultureInfo.InvariantCulture));
					logMessage.LogCategories.Add(LogCategory.Project);
					_logManager.Log(logMessage);
					throw new ArgumentException("Project cannot be canceled it still has line items.");
				}

				if (existingProject.ServiceLines.Count > 0 &&
				    project.CompanyId.GetValueOrDefault(Guid.Empty) !=
				    existingProject.CompanyId.GetValueOrDefault(Guid.Empty))
				{
					var logMessage = new LogMessage(MessageIds.ProjectManangerProjectCannotChangeCompany, LogPriority.Medium,
						TraceEventType.Verbose,
						"Cannot change company on Project while it still has line items.",
						LogCategory.Project);
					logMessage.Data.Add("ProjectId", project.Id.Value.ToString());
					logMessage.Data.Add("LineItemCount",
						project.ServiceLines.Count.ToString(CultureInfo.InvariantCulture));
					logMessage.LogCategories.Add(LogCategory.Project);
					_logManager.Log(logMessage);
					throw new ArgumentException("Cannot change company on Project while it still has line items.");
				}


				var errorList = _projectValidationManager.Validate(project, null);
				if (null != errorList && errorList.Any())
				{
					throw new ArgumentException("Project was not valid", "project");
				}

				//
				// Get incoming order if it exists
				//
				var incomingOrder = project.IncomingOrderId == null
					? null
					: _incomingOrderProvider.FindById(project.IncomingOrderId.Value);

				// If this is a project from an order, wipe  manually entered fields that should no longer be preserved..
				if (project.ServiceLines != null && project.ServiceLines.Any())
				{
					project.ServiceCode = null;
					project.IndustryCode = null;
					project.Location = null;
				}
				//
				// If project has no incoming order and incoming order exists (it must) map incomingorder to project, otherwise map existing order to prject
				// this is to keep the buy pay fields intact.
				//
				var newOrderNumber = project.OrderNumber;
				if (!existingProject.IncomingOrderId.HasValue && incomingOrder != null)
					IncomingOrderProvider.MapIncomingOrderToProject(incomingOrder, project);
				else if (!string.IsNullOrWhiteSpace(project.OrderNumber))
				{
					IncomingOrderProvider.MapIncomingOrderToProject(existingProject, project);
					project.OrderNumber = newOrderNumber;
				}

				//This is where the project is mapping existing order details even though we removed order in UI. Commenting else statement 
				//Defect Fix 22031


				//
				// Set the ids for the child objects since they are not sent as part of the request
				//
				if (project.IncomingOrderContact.Id.GetValueOrDefault() == Guid.Empty)
					project.IncomingOrderContact.Id = existingProject.IncomingOrderContact.Id;

				if (project.BillToContact.Id.GetValueOrDefault() == Guid.Empty)
					project.BillToContact.Id = existingProject.BillToContact.Id;

				if (project.ShipToContact.Id.GetValueOrDefault() == Guid.Empty)
					project.ShipToContact.Id = existingProject.ShipToContact.Id;

				if (project.IncomingOrderCustomer.Id.GetValueOrDefault() == Guid.Empty)
					project.IncomingOrderCustomer.Id = existingProject.IncomingOrderCustomer.Id;
				//end id set work around

				project.TaskMinimumDueDate = existingProject.TaskMinimumDueDate;
				project.MinimumDueDateTaskId = existingProject.MinimumDueDateTaskId;
				project.TotalOrderPrice = existingProject.TotalOrderPrice;

				//
				// Return removed service lines to incoming order
				//
				ReturnServiceLinesToIncomingOrderThatWereDeleted(existingProject, project, incomingOrder);

				//
				// Remove added service lines from incoming order
				//
				RemoveServiceLinesFromIncomingOrderThatWereAdded(existingProject, project, incomingOrder);

				if (!project.CompanyId.HasValue)
					project.CompanyId = Guid.Empty;
				if (ProjectStatusEnumDto.Completed == project.ProjectStatus &&
				    ProjectStatusEnumDto.Completed != existingProject.ProjectStatus)
				{
					project.CompletionDate = DateTime.UtcNow;
				}

				project.ProjectHandler = project.ProjectHandler.ToLowerInvariant();
				if (additionalWorkCreateProjectTemplateTasks != null)
				{
					additionalWorkCreateProjectTemplateTasks(project.AdditionalProjectTemplateId, project.ContainerId.Value);
				}
				_projectProvider.Update(project);
				_projectProvider.Publish(existingProject, project);

				if (project.IsEmailRequested)
				{
					var projectAssignmentEmail = new ProjectAssignmentEmail
					{
						DueDate = project.EndDate,
						ProjectId = id,
						ProjectName = project.Name,
						RecipientName = project.ProjectHandler
					};
					_smtpClientManager.SendProjectAssignedNewHandler(project.ProjectHandler, projectAssignmentEmail);

					projectAssignmentEmail.RecipientName = existingProject.ProjectHandler;
					_smtpClientManager.SendProjectAssigned(existingProject.ProjectHandler, projectAssignmentEmail);
				}

				if (project.IsOrderOwnerEmailRequested && project.OrderOwner != existingProject.OrderOwner)
				{
					var projectOrderOwnerReassignedEmail = new ProjectOrderOwnerReassignedEmail
					{
						NewOrderOwner = project.OrderOwner,
						OriginalOrderOwner = existingProject.OrderOwner,
						ProjectId = id,
						ProjectName = project.Name
					};

					if (!string.IsNullOrWhiteSpace(project.OrderOwner))
					{
						projectOrderOwnerReassignedEmail.RecipientName = project.OrderOwner;
						_smtpClientManager.SendProjectOrderOwnerReassigned(project.OrderOwner, projectOrderOwnerReassignedEmail);
					}

					if (!string.IsNullOrWhiteSpace(existingProject.OrderOwner))
					{
						projectOrderOwnerReassignedEmail.RecipientName = existingProject.OrderOwner;
						_smtpClientManager.SendProjectOrderOwnerReassigned(existingProject.OrderOwner, projectOrderOwnerReassignedEmail);
					}
				}

				if (sendProjectCompleteEmail && project.ProjectStatus == ProjectStatusEnumDto.Completed)
				{
					var projectCompletedEmail = new ProjectCompletedEmail
					{
						DueDate = project.EndDate,
						ProjectId = id,
						ProjectName = project.Name,
						RecipientName = project.ProjectHandler
					};
					_smtpClientManager.SendProjectCompleted(project.ProjectHandler, projectCompletedEmail);

					if (!string.IsNullOrWhiteSpace(project.OrderOwner))
					{
						projectCompletedEmail.RecipientName = project.OrderOwner;
						_smtpClientManager.SendProjectCompleted(project.OrderOwner, projectCompletedEmail);
					}
				}

				//
				var notifications = _projectNotificationCheckManager.GetProjectNotifications(project, existingProject);
				_notificationManager.ProcessNotifications(notifications, project);
				scope.Complete();
			}
		}

		/// <summary>
		/// Validates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="project">The project.</param>
		/// <returns></returns>
		public IList<ProjectValidationEnumDto> Validate(string id, Project project)
		{
			return _projectValidationManager.Validate(project, null);
		}


		/// <summary>
		/// Updates the status from order.
		/// </summary>
		/// <param name="incomingOrder">The incoming order.</param>
		public void UpdateStatusFromOrder(IncomingOrder incomingOrder)
		{
			const string apply = "apply";
			var canUpdateProject = (incomingOrder.Status.ToLower() == apply) ||
			                       incomingOrder.ServiceLines.Any(x => x.Hold.ToLowerInvariant() == "y");
			if (!canUpdateProject) return;

			using (var scope = _transactionFactory.Create())
			{
				var projects = _projectProvider.FetchByOrderNumber(incomingOrder.OrderNumber);

				if (projects != null && projects.Any())
				{
					foreach (var project in projects)
					{
						if (IsProjectStateFinal(project, incomingOrder)) continue;
						var shouldPerformFullUpdate = false;
						if (IsUpdateUnnecessary(incomingOrder, project)) continue;
						var notifications = _orderNotificationCheckManager.GetOrderNotifications(project, incomingOrder);
						incomingOrder.ServiceLines.ForEach(x =>
						{
							if (x.Hold.ToLowerInvariant() == "n")
								return;
							var serviceLine =
								project.ServiceLines.FirstOrDefault(
									y => y.ExternalId == x.ExternalId && y.Hold.ToLowerInvariant() == "n");
							if (null != serviceLine)
							{
								_orderLineNotificationCheckManager.GetOrderLineNotifications(serviceLine, x, notifications);
								serviceLine.Hold = x.Hold;
								shouldPerformFullUpdate = true;
							}
						});


						if (!shouldPerformFullUpdate &&
						    !string.Equals(incomingOrder.Status, project.Status, StringComparison.InvariantCultureIgnoreCase))
						{
							project.Status = incomingOrder.Status;
							_projectProvider.UpdateStatusFromOrder(project);
						}
						else if (shouldPerformFullUpdate)
						{
							project.Status = incomingOrder.Status;
							_projectProvider.UpdateFromIncomingOrder(project);
						}
						_notificationManager.ProcessNotifications(notifications, project);
					}
				}
				try
				{
					var request = _incomingOrderProvider.FindByOrderNumber(incomingOrder.OrderNumber);

					if (request != null &&
					    !string.Equals(incomingOrder.Status, request.Status, StringComparison.InvariantCultureIgnoreCase))
					{
						request.Status = incomingOrder.Status;
						_incomingOrderProvider.UpdateIncomingOrder(request.Id.Value, request);
					}
				}
				catch (DatabaseItemNotFoundException)
				{
					// just means there was no request. will be logged lower.
				}
				scope.Complete();
			}
		}

		/// <summary>
		/// Fetches the by order number.
		/// </summary>
		/// <param name="orderNumber">The order number.</param>
		/// <returns></returns>
		public IList<Project> FetchByOrderNumber(string orderNumber)
		{
			return _projectProvider.FetchByOrderNumber(orderNumber);
		}

		internal void BrokerNotificationActions(Project existingProj, Project updatedProj)
		{
			if (existingProj.ProjectStatus == ProjectStatusEnumDto.Completed
			    || existingProj.ProjectStatus == ProjectStatusEnumDto.Canceled)
				return;

			if (updatedProj.ProjectStatus == ProjectStatusEnumDto.Canceled)
			{
				var projectTasks = _taskProvider.FetchAll(existingProj.ContainerId.GetValueOrDefault());
				if (projectTasks != null)
					RemoveNotifications(projectTasks.Select(x => x.Id).ToList());
			}
		}


		private void RemoveNotifications(IEnumerable<Guid?> taskIds)
		{
			foreach (var id in taskIds)
			{
				_notificationManager.DeleteNotificationsForEntity(id.GetValueOrDefault());
			}
		}

		internal void RemoveServiceLinesFromIncomingOrderThatWereAdded(Project existingProject, Project project,
			IncomingOrder incomingOrder)
		{
			var serviceLinesToRemove = new List<IncomingOrderServiceLine>();
			var serviceLinesToAdd = new List<IncomingOrderServiceLine>();

			// Process the added service lines
			foreach (var serviceLine in project.ServiceLines)
			{
				var serviceLineFound =
					existingProject.ServiceLines.FirstOrDefault(x => x.Id == serviceLine.Id);

				if (serviceLineFound == null)
				{
					if (incomingOrder == null)
						continue;

					// If not found find the incoming order service line added and remove it from the incoming order and add it to the project
					var incomingOrderServiceLineFound =
						incomingOrder.ServiceLines.FirstOrDefault(x => x.Id == serviceLine.Id);
					_incomingOrderProvider.DeleteServiceLine(serviceLine.Id.Value);
					incomingOrder.ServiceLines.Remove(incomingOrderServiceLineFound);
					serviceLinesToRemove.Add(serviceLine);
					serviceLinesToAdd.Add(incomingOrderServiceLineFound);
				}
				else
				{
					// If found then put back the original service line since the one passsed up was the id only
					serviceLinesToRemove.Add(serviceLine);
					serviceLinesToAdd.Add(serviceLineFound);
				}
			}

			foreach (var serviceLineToRemove in serviceLinesToRemove)
				project.ServiceLines.Remove(serviceLineToRemove);

			foreach (var serviceLineToAdd in serviceLinesToAdd)
			{
				serviceLineToAdd.IncomingOrderId = project.Id.Value;
				project.ServiceLines.Add(serviceLineToAdd);
			}

			if (incomingOrder != null && incomingOrder.ServiceLines.Count == 0)
			{
				_incomingOrderProvider.Delete(project.IncomingOrderId.Value);
				project.IncomingOrderId = null;
			}
		}

		internal void ReturnServiceLinesToIncomingOrderThatWereDeleted(Project existingProject, Project project,
			IncomingOrder incomingOrder)
		{
			// Find removed service lines
			var serviceLines = (from serviceLine in existingProject.ServiceLines
				let serviceLineFound = project.ServiceLines.FirstOrDefault(x => x.Id == serviceLine.Id)
				where serviceLineFound == null
				select serviceLine).ToList();

			if (serviceLines.Count > 0)
			{
				// If Incoming Order doesn't exist create it
				if (incomingOrder == null)
					project.IncomingOrderId = CreateIncomingOrder(project, serviceLines);
				else
					AddLinesToIncomingOrder(incomingOrder, serviceLines);
			}
		}

		internal static List<IncomingOrderServiceLine> GetAndValidateServiceLines(Project project,
			IList<string> lineItemIds)
		{
			var serviceLines = new List<IncomingOrderServiceLine>();

			foreach (
				var serviceLine in
					lineItemIds.Select(
						lineItemId => project.ServiceLines.FirstOrDefault(x => x.Id == lineItemId.ToGuid())))
			{
				if (serviceLine == null)
					throw new DatabaseItemNotFoundException();

				serviceLines.Add(serviceLine);
			}

			return serviceLines;
		}

		internal Guid CreateIncomingOrder(Project project, List<IncomingOrderServiceLine> serviceLines)
		{
			var incomingOrder = _mapperRegistry.Map<IncomingOrder>(project);
			incomingOrder.MessageId = project.MessageId;
			incomingOrder.IncomingOrderContact =
				IncomingOrderContact.MapIncomingOrderContact(project.IncomingOrderContact);
			incomingOrder.BillToContact = IncomingOrderContact.MapIncomingOrderContact(project.BillToContact);
			incomingOrder.ShipToContact = IncomingOrderContact.MapIncomingOrderContact(project.ShipToContact);
			incomingOrder.IncomingOrderCustomer =
				IncomingOrderCustomer.MapIncomingOrderCustomer(project.IncomingOrderCustomer);
			incomingOrder.Id = Guid.NewGuid();
			incomingOrder.ServiceLines = serviceLines;
			incomingOrder.IncomingOrderContact.Id = Guid.NewGuid();
			incomingOrder.BillToContact.Id = Guid.NewGuid();
			incomingOrder.ShipToContact.Id = Guid.NewGuid();
			incomingOrder.IncomingOrderCustomer.Id = Guid.NewGuid();
			return _incomingOrderProvider.Create(incomingOrder);
		}

		internal void AddLinesToIncomingOrder(IncomingOrder incomingOrder, List<IncomingOrderServiceLine> serviceLines)
		{
			foreach (var serviceLine in serviceLines)
			{
				serviceLine.IncomingOrderId = incomingOrder.Id.GetValueOrDefault();
				_incomingOrderProvider.AddServiceLine(serviceLine);
				incomingOrder.ServiceLines.Add(serviceLine);
			}
		}

		internal void RemoveLinesFromProject(Project project, List<IncomingOrderServiceLine> serviceLines)
		{
			foreach (var serviceLine in serviceLines)
				project.ServiceLines.Remove(serviceLine);

			_projectProvider.Update(project);
		}

		private static bool IsUpdateUnnecessary(IncomingOrder incomingOrder, Project project)
		{
			if (!string.Equals(incomingOrder.Status, project.Status, StringComparison.InvariantCultureIgnoreCase)) return false;
			if (
				incomingOrder.ServiceLines.Any(
					x =>
						x.Hold.ToLowerInvariant() == "y" &&
						project.ServiceLines.Any(y => y.ExternalId == x.ExternalId && y.Hold.ToLowerInvariant() != "y")))
				return false;
			return true;
		}

		private static bool IsProjectStateFinal(Project project, IncomingOrder incomingOrder)
		{
			if (project.ProjectStatus == ProjectStatusEnumDto.Completed ||
			    project.ProjectStatus == ProjectStatusEnumDto.Canceled) return true;
			return false;
		}
	}
}