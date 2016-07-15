using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Domain.View;
using UL.Aria.Service.Logging;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Notifications;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class IncomingOrderCoreProvider. This class cannot be inherited.
	/// </summary>
	public class IncomingOrderCoreProvider : IIncomingOrderCoreProvider
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IBusinessMessageProvider _businessMessageProvider;
		private readonly IInboundMessageProvider _inboundMessageProvider;
		private readonly IIncomingOrderRepository _incomingOrderRepository;
		private readonly ILogManager _logManager;
		private readonly INotificationManager _notificationManager;
		private readonly IOrderLineNotificationCheckManager _orderLineNotificationCheckManager;
		private readonly IOrderNotificationCheckManager _orderNotificationCheckManager;
		private readonly IPrincipalResolver _principalResolver;
		private readonly IProjectProvider _projectProvider;
		private readonly ISmtpClientManager _smtpClientManager;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="IncomingOrderCoreProvider" /> class.
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
		public IncomingOrderCoreProvider(IIncomingOrderRepository incomingOrderRepository,
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
			IInboundMessageProvider inboundMessageProvider)
		{
			_incomingOrderRepository = incomingOrderRepository;
			_transactionFactory = transactionFactory;
			_projectProvider = projectProvider;
			_logManager = logManager;
			_businessMessageProvider = businessMessageProvider;
			_principalResolver = principalResolver;
			_smtpClientManager = smtpClientManager;
			_notificationManager = notificationManager;
			_orderNotificationCheckManager = orderNotificationCheckManager;
			_orderLineNotificationCheckManager = orderLineNotificationCheckManager;
			_assetProvider = assetProvider;
			_inboundMessageProvider = inboundMessageProvider;
		}

		/// <summary>
		///     Creates the specified new order.
		/// </summary>
		/// <param name="incomingOrder">The new order.</param>
		/// <returns>Guid.</returns>
		public Guid Create(IncomingOrder incomingOrder)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var result = _incomingOrderRepository.Create(incomingOrder);
				transactionScope.Complete();
				return result;
			}
		}

		/// <summary>
		///     Updates the specified incoming order id.
		/// </summary>
		/// <param name="incomingOrderId">The incoming order id.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		public void Update(Guid incomingOrderId, IncomingOrder incomingOrder)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				//
				// Get incoming Order if it exists
				//
				IncomingOrder incomingOrderExisting = null;
				try
				{
					incomingOrderExisting = FindByOrderNumber(incomingOrder.OrderNumber);
				}

				catch (DatabaseItemNotFoundException)
				{
				}

				//
				// Get project(s) if they exist
				//
				IList<Project> projects = null;
				try
				{
					projects = _projectProvider.FetchByOrderNumber(incomingOrder.OrderNumber);
				}
				catch (DatabaseItemNotFoundException)
				{
				}

				//
				// If no incoming order to update and  no projects exist create incoming order and done
				//
				if (incomingOrderExisting == null && projects == null)
				{
					CreateIncomingOrder(incomingOrder);
					LogMessage(MessageIds.RequestCreated, "Request Created",
						incomingOrder);
                    transactionScope.Complete();
					return;
				}

				//
				// if just incoming Order with no projects update incoming order and done
				//
				if (incomingOrderExisting != null && projects == null)
				{
					RemoveInactiveIncomingOrderServiceLines(incomingOrder);
					if (IsValid(incomingOrder))
					{
						UpdateIncomingOrder(incomingOrder);
						LogMessage(MessageIds.RequestUpdated, "Request Updated",
							incomingOrder);
					}
					else
					{
						_incomingOrderRepository.Delete(incomingOrderExisting.Id.GetValueOrDefault());
						LogMessage(MessageIds.RequestDeleted, "Request Deleted",
							incomingOrder);
					}

					_inboundMessageProvider.DeleteSuccessfulMessage(incomingOrderExisting.MessageId);
                    transactionScope.Complete();
					return;
				}

				var originalMessageId = incomingOrderExisting != null ? incomingOrderExisting.MessageId : projects[0].MessageId;

				//
				// Get project service lines to update and reset them on project(s), removing them from the incoming order
				// also remove any deleted service lines from projects
				//
				var orderLineNotificationRules = ProcessProjectsServiceLines(incomingOrder, projects);

				// Update projects
				UpdateProjects(incomingOrder, projects, orderLineNotificationRules);

				RemoveInactiveIncomingOrderServiceLines(incomingOrder);

				//
				// If no service lines left over and an existing incoming order exists remove incoming order otherwise create/update it.
				//
				if (incomingOrder.ServiceLines.Count == 0 || !IsValid(incomingOrder))
				{
					if (incomingOrderExisting != null)
					{
						_incomingOrderRepository.Delete(incomingOrderExisting.Id.GetValueOrDefault());
						LogMessage(MessageIds.RequestDeleted, "Request Deleted",
							incomingOrder);
					}
				}
				else
				{
					//
					// If incoming order doesn't exist re-create with left over service lines else update existing incoming order
					//
					if (incomingOrderExisting == null)
					{
						CreateIncomingOrder(incomingOrder);
						LogMessage(MessageIds.RequestCreated, "Request Created",
							incomingOrder);
					}
					else
					{
						UpdateIncomingOrder(incomingOrder);
						LogMessage(MessageIds.RequestUpdated, "Request Updated",
							incomingOrder);
					}
				}

				_inboundMessageProvider.DeleteSuccessfulMessage(originalMessageId);
				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Updates the contact.
		/// </summary>
		/// <param name="contact">The contact.</param>
		public void UpdateContact(IncomingOrderContact contact)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_incomingOrderRepository.UpdateContact(contact);
				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Creates the contact.
		/// </summary>
		/// <param name="contact">The contact.</param>
		/// <returns>Guid.</returns>
		public Guid CreateContact(IncomingOrderContact contact)
		{
			Guid id;

			using (var transactionScope = _transactionFactory.Create())
			{
				id = _incomingOrderRepository.CreateContact(contact);
				transactionScope.Complete();
			}

			return id;
		}

		/// <summary>
		/// Updates the contact.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <param name="contact">The contact.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void UpdateAllContactsForExternalId(string externalId, IncomingOrderContact contact)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_incomingOrderRepository.UpdateAllContactsForExternalId(externalId, contact);
				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Updates all customers for external identifier.
		/// </summary>
		/// <param name="externalId">The external identifier.</param>
		/// <param name="customer"></param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void UpdateAllCustomersForExternalId(string externalId, IncomingOrderCustomer customer)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_incomingOrderRepository.UpdateAllCustomersForExternalId(externalId, customer);
				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Deletes the specified <see cref="IncomingOrder" />.
		/// </summary>
		/// <param name="id">The id.</param>
		public void Delete(Guid id)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_incomingOrderRepository.Delete(id);
				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Searches for <see cref="IncomingOrder" /> objects using the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public IncomingOrderSearchResultSet Search(SearchCriteria searchCriteria)
		{
			return _incomingOrderRepository.Search(searchCriteria);
		}

		/// <summary>
		///     Finds the <see cref="IncomingOrder" /> by id.
		/// </summary>
		/// <param name="entityId">The entity id.</param>
		/// <returns></returns>
		public IncomingOrder FindById(Guid entityId)
		{
			return _incomingOrderRepository.FindById(entityId);
		}

		/// <summary>
		/// Publishes the project creation request.
		/// </summary>
		/// <param name="projectCreationRequest">The project creation request.</param>
		/// <param name="additionalWorkCreateProjectTemplateTasks">The create project template tasks.</param>
		/// <returns>Guid.</returns>
		public Guid PublishProjectCreationRequest(ProjectCreationRequest projectCreationRequest,
			Action<Guid, Guid, int> additionalWorkCreateProjectTemplateTasks)
		{
			var containerId = Guid.NewGuid();
			Project project;

			using (var transactionScope = _transactionFactory.Create())
			{
				if (projectCreationRequest.IncomingOrderId.HasValue)
				{
					// Get incoming /oder from DB

					var incomingOrder = _incomingOrderRepository.FindById(projectCreationRequest.IncomingOrderId.Value);

					var companyId = Guid.Empty;

					if (incomingOrder.CompanyId.HasValue)
						companyId = incomingOrder.CompanyId.Value;

					var logMessage = new LogMessage(MessageIds.IncomingOrderProjectCreate, LogPriority.Medium, TraceEventType.Verbose,
						string.Format("Project Created from IncomingOrder, CompanyId:{0} ,Xml:{1}",
							companyId, incomingOrder.OriginalXmlParsed),
						LogCategory.Project);
					logMessage.LogCategories.Add(LogCategory.Project);
					_logManager.Log(logMessage);

					project = new Project
					{
						Id = Guid.NewGuid(),
						IncomingOrderId = incomingOrder.Id.GetValueOrDefault(),
						ServiceLines = incomingOrder.ServiceLines,
						CompanyId = companyId,
						MessageId = incomingOrder.MessageId
					};

					MapIncomingOrderToProject(incomingOrder, project);

					project.IncomingOrderContact.Id = Guid.NewGuid();
					project.BillToContact.Id = Guid.NewGuid();
					project.ShipToContact.Id = Guid.NewGuid();
					project.IncomingOrderCustomer.Id = Guid.NewGuid();

					var serviceLineCount = project.ServiceLines.Count;

					project.ServiceLines =
						project.ServiceLines.Join(projectCreationRequest.ServiceLineItems, i => i.Id, j => j.Id,
							(i, j) => i).ToList();

					var serviceLineCountUsed = project.ServiceLines.Count;

					if (serviceLineCount == serviceLineCountUsed)
					{
						_incomingOrderRepository.Delete(projectCreationRequest.IncomingOrderId.Value);
						project.IncomingOrderId = null;
					}
					else
					{
						foreach (var serviceLine in project.ServiceLines)
						{
							_incomingOrderRepository.DeleteServiceLine(serviceLine.Id.GetValueOrDefault());
						}
					}
				}
				else
				{
					project = new Project
					{
						Id = Guid.NewGuid(),
						ServiceLines = projectCreationRequest.ServiceLineItems,
						CompanyId = projectCreationRequest.CompanyId,
						QuoteNo = projectCreationRequest.QuoteNo
					};
				}


				project.Name = projectCreationRequest.Name;
				project.Description = projectCreationRequest.Description;
				project.StartDate = projectCreationRequest.StartDate;
				project.EndDate = projectCreationRequest.EndDate;
				project.ProjectHandler = projectCreationRequest.ProjectHandler;
				project.ProjectStatus = ProjectStatusEnumDto.InProgress;
				project.OrderNumber = projectCreationRequest.OrderNumber;
				project.ProjectTemplateId = projectCreationRequest.ProjectTemplateId;
				project.NumberOfSamples = projectCreationRequest.NumberOfSamples;
				project.SampleReferenceNumbers = projectCreationRequest.SampleReferenceNumbers;
				project.CCN = projectCreationRequest.CCN;
				project.FileNo = projectCreationRequest.FileNo;
				project.StatusNotes = projectCreationRequest.StatusNotes;
				project.AdditionalCriteria = projectCreationRequest.AdditionalCriteria;
				project.IndustryCode = projectCreationRequest.IndustryCode;
				project.ServiceCode = projectCreationRequest.ServiceCode;
				project.ServiceRequestNumber = projectCreationRequest.ServiceRequestNumber;
				project.OverrideAutoComplete = projectCreationRequest.OverrideAutoComplete;
				if (projectCreationRequest.OrderOwner != null)
				{
					projectCreationRequest.OrderOwner = projectCreationRequest.OrderOwner.ToLower();
					project.OrderOwner = projectCreationRequest.OrderOwner;
				}
				if (!project.CompanyId.HasValue)
					project.CompanyId = Guid.Empty;

				_projectProvider.Create(containerId, project);

				_projectProvider.Publish(null, project);

				try
				{
					// Create template tasks
					if (additionalWorkCreateProjectTemplateTasks != null)
					{
						additionalWorkCreateProjectTemplateTasks(projectCreationRequest.ProjectTemplateId, containerId, 0);
					}

					_businessMessageProvider.Publish(AuditMessageIdEnumDto.ProjectInitiated,
						"Buy message enqueued.", new Dictionary<string, string> {{"Id", project.Id.ToString()}});
				}
				catch
				{
					// ReSharper disable once PossibleInvalidOperationException
					_assetProvider.Delete(project.Id.Value);
					throw;
				}

				transactionScope.Complete();
			}

			if (projectCreationRequest.IsEmailRequested)
			{
				var projectEmail = new ProjectEmail
				{
					ActorId = _principalResolver.UserId,
					ProjectId = project.Id.GetValueOrDefault(),
					ProjectName = projectCreationRequest.Name,
					RecipientName = projectCreationRequest.ProjectHandler
				};

				_smtpClientManager.SendProjectCreated(project.ProjectHandler, projectEmail);
			}


			if ((projectCreationRequest.IsOrderOwnerEmailRequested) && (!string.IsNullOrEmpty(projectCreationRequest.OrderOwner)))
			{
				var orderOwnerEmail = new ProjectEmail
				{
					ActorId = _principalResolver.UserId,
					ProjectId = project.Id.GetValueOrDefault(),
					ProjectName = projectCreationRequest.Name,
					RecipientName = projectCreationRequest.OrderOwner
				};

				_smtpClientManager.SendProjectCreated(project.OrderOwner, orderOwnerEmail);
			}

			return project.Id.GetValueOrDefault();
		}

		/// <summary>
		///     Finds the project by service line id.
		/// </summary>
		public IncomingOrder FindByServiceLineId(Guid serviceLineId)
		{
			Guard.IsNotEmptyGuid(serviceLineId, "serviceLineId");

			return _incomingOrderRepository.FindByServiceLineId(serviceLineId);
		}

		/// <summary>
		///     Finds the by order number.
		/// </summary>
		/// <param name="orderNumber">The order number.</param>
		/// <returns>IncomingOrder.</returns>
		public IncomingOrder FindByOrderNumber(string orderNumber)
		{
			return _incomingOrderRepository.FindByOrderNumber(orderNumber);
		}

		/// <summary>
		/// Adds the service line.
		/// </summary>
		/// <param name="serviceLine">The service line.</param>
		public void AddServiceLine(IncomingOrderServiceLine serviceLine)
		{
			_incomingOrderRepository.AddServiceLine(serviceLine);
		}

		/// <summary>
		/// Deletes the service line.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void DeleteServiceLine(Guid id)
		{
			_incomingOrderRepository.DeleteServiceLine(id);
		}

		/// <summary>
		/// Fetches a IEnumerable of name id pair objects for Incoming orders as lookup objects.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Lookup> FetchIncomingOrderLookups()
		{
			return _incomingOrderRepository.FetchIncomingOrderLookups();
		}

		/// <summary>
		/// Updates the incoming order.
		/// </summary>
		/// <param name="incomingOrderId">The incoming order identifier.</param>
		/// <param name="incomingOrder">The incoming order.</param>
		public void UpdateIncomingOrder(Guid incomingOrderId, IncomingOrder incomingOrder)
		{
			UpdateIncomingOrder(incomingOrder);
		}

		internal void RemoveInactiveIncomingOrderServiceLines(IncomingOrder incomingOrder)
		{
			if (null != incomingOrder.ServiceLines)
				incomingOrder.ServiceLines = incomingOrder.ServiceLines.Where(
					x =>
					{
						if (null == x.Status)
							return true;
						var statusLower = x.Status.ToLowerInvariant();
						if (statusLower == "cancelled")
							return false;
						if (statusLower == "canceled")
							return false;
						if (statusLower == "closed")
							return false;
						if (statusLower == "fulfilled")
							return false;
						return true;
					}
					).ToList();
		}

		private void LogMessage(int messageId, string message, IncomingOrder incomingOrder, Project project = null)
		{
			var logMessage = new LogMessage(messageId, LogPriority.Low, TraceEventType.Information, message,
				LogCategory.System,
				LogCategory.InboundOrderMessageService);
			logMessage.Data.Add("OrderNumber",
				incomingOrder == null || string.IsNullOrWhiteSpace(incomingOrder.OrderNumber)
					? "N/A"
					: incomingOrder.OrderNumber);
			logMessage.Data.Add("Customer ExternalId",
				incomingOrder == null || incomingOrder.IncomingOrderCustomer == null ||
				string.IsNullOrWhiteSpace(incomingOrder.IncomingOrderCustomer.ExternalId)
					? "N/A"
					: incomingOrder.IncomingOrderCustomer.ExternalId);
			logMessage.Data.Add("Customer Name",
				incomingOrder == null || incomingOrder.IncomingOrderCustomer == null ||
				string.IsNullOrWhiteSpace(incomingOrder.IncomingOrderCustomer.Name)
					? "N/A"
					: incomingOrder.IncomingOrderCustomer.Name);
			if (project != null)
			{
				logMessage.Data.Add("ProjectName",
					string.IsNullOrWhiteSpace(project.Name) ? "N/A" : project.Name);
				logMessage.Data.Add("ProjectCompanyName",
					string.IsNullOrWhiteSpace(project.CompanyName) ? "N/A" : project.CompanyName);
				logMessage.Data.Add("ProjectStatus",
					string.IsNullOrWhiteSpace(project.Status) ? "N/A" : project.Status);
			}
			_logManager.Log(logMessage);
		}

		private void CreateIncomingOrder(IncomingOrder incomingOrder)
		{
			if (!IsValid(incomingOrder))
				return;
			var creationDateTime = DateTime.UtcNow;
			var createdBy = _principalResolver.UserId;
			incomingOrder.CreatedDateTime = creationDateTime;
			incomingOrder.CreatedById = createdBy;
			incomingOrder.UpdatedDateTime = creationDateTime;
			incomingOrder.UpdatedById = createdBy;
			incomingOrder.IncomingOrderContact.CreatedDateTime = creationDateTime;
			incomingOrder.IncomingOrderContact.CreatedById = createdBy;
			incomingOrder.IncomingOrderContact.UpdatedDateTime = creationDateTime;
			incomingOrder.IncomingOrderContact.UpdatedById = createdBy;
			incomingOrder.BillToContact.CreatedDateTime = creationDateTime;
			incomingOrder.BillToContact.CreatedById = createdBy;
			incomingOrder.BillToContact.UpdatedDateTime = creationDateTime;
			incomingOrder.BillToContact.UpdatedById = createdBy;
			incomingOrder.ShipToContact.CreatedDateTime = creationDateTime;
			incomingOrder.ShipToContact.CreatedById = createdBy;
			incomingOrder.ShipToContact.UpdatedDateTime = creationDateTime;
			incomingOrder.ShipToContact.UpdatedById = createdBy;
			incomingOrder.IncomingOrderCustomer.CreatedDateTime = creationDateTime;
			incomingOrder.IncomingOrderCustomer.CreatedById = createdBy;
			incomingOrder.IncomingOrderCustomer.UpdatedDateTime = creationDateTime;
			incomingOrder.IncomingOrderCustomer.UpdatedById = createdBy;
			foreach (var serviceline in incomingOrder.ServiceLines)
			{
				serviceline.CreatedDateTime = creationDateTime;
				serviceline.CreatedById = createdBy;
				serviceline.UpdatedDateTime = creationDateTime;
				serviceline.UpdatedById = createdBy;
			}
			_incomingOrderRepository.Create(incomingOrder);
		}

		private static bool IsValid(IncomingOrder incomingOrder)
		{
			if (incomingOrder.Status.Equals("booked", StringComparison.InvariantCultureIgnoreCase))
				return true;
			return false;
		}

		private void UpdateIncomingOrder(IncomingOrder incomingOrder)
		{
			var updateDateTime = DateTime.UtcNow;
			var updatedBy = _principalResolver.UserId;
			incomingOrder.UpdatedDateTime = updateDateTime;
			incomingOrder.UpdatedById = updatedBy;
			incomingOrder.IncomingOrderContact.UpdatedDateTime = updateDateTime;
			incomingOrder.IncomingOrderContact.UpdatedById = updatedBy;
			incomingOrder.BillToContact.UpdatedDateTime = updateDateTime;
			incomingOrder.BillToContact.UpdatedById = updatedBy;
			incomingOrder.ShipToContact.UpdatedDateTime = updateDateTime;
			incomingOrder.ShipToContact.UpdatedById = updatedBy;
			incomingOrder.IncomingOrderCustomer.UpdatedDateTime = updateDateTime;
			incomingOrder.IncomingOrderCustomer.UpdatedById = updatedBy;
			foreach (var serviceLine in incomingOrder.ServiceLines)
			{
				serviceLine.UpdatedDateTime = updateDateTime;
				serviceLine.UpdatedById = updatedBy;
			}
			_incomingOrderRepository.Update(incomingOrder);
		}

		internal Dictionary<Guid, List<NotificationTypeDto>> ProcessProjectsServiceLines(IncomingOrder incomingOrder,
			IEnumerable<Project> projects)
		{
			var projectsOrderLineNotifications = new Dictionary<Guid, List<NotificationTypeDto>>();

			foreach (var project in projects)
			{
				var orderLineNotifications = new List<NotificationTypeDto>();

				for (var i = project.ServiceLines.Count - 1; i >= 0; i--)
				{
					var serviceLine = project.ServiceLines[i];
					var serviceLineMatched =
						incomingOrder.ServiceLines.FirstOrDefault(x =>
						{
							var isMatched = x.ExternalId == serviceLine.ExternalId;
							return isMatched;
						});

					project.ServiceLines.RemoveAt(i);

					if (serviceLineMatched == null)
					{
						var a = new OrderLineCleanupNotificationCheck();
						a.Check(null, serviceLine, orderLineNotifications);
						continue;
					}

					//Get order line notifications
					_orderLineNotificationCheckManager.GetOrderLineNotifications(serviceLine, serviceLineMatched,
						orderLineNotifications);

					serviceLineMatched.Id = serviceLine.Id;

					project.ServiceLines.Add(serviceLineMatched);
					for (var j = 0; j < incomingOrder.ServiceLines.Count; ++j)
					{
						if (incomingOrder.ServiceLines[j].ExternalId == serviceLineMatched.ExternalId)
						{
							incomingOrder.ServiceLines.RemoveAt(j);
							break;
						}
					}
				}

				//There could be existing order lines w/o status change that are hold or cancel if so we do not want to cleanup order line notifications
				if (orderLineNotifications.Any(x => x == NotificationTypeDto.OrderLineCleanup) &&
				    (project.ServiceLines.Any(
					    x =>
						    x.Status.ToLower() == "apply" || x.Status.ToLower() == "hold" || x.Status.ToLower() == "canceled" ||
						    x.Status.ToLower() == "cancelled")))
				{
					orderLineNotifications.RemoveAll(x => x == NotificationTypeDto.OrderLineCleanup);
				}

				//Add order line notifications for the project
				projectsOrderLineNotifications.Add(project.Id.GetValueOrDefault(), orderLineNotifications);
			}

			return projectsOrderLineNotifications;
		}

		internal void UpdateProjects(IncomingOrder incomingOrder, IEnumerable<Project> projects,
			Dictionary<Guid, List<NotificationTypeDto>> projectsOrderLineNotifications)
		{
			foreach (var project in projects)
			{
				project.MessageId = incomingOrder.MessageId;
				var incomingOrderStatus = incomingOrder.Status.ToLower();
				if ((project.ProjectStatus == ProjectStatusEnumDto.Completed ||
				     project.ProjectStatus == ProjectStatusEnumDto.Canceled) &&
				    incomingOrderStatus != "canceled" && incomingOrderStatus != "cancelled") continue;

				var orderNotifications = _orderNotificationCheckManager.GetOrderNotifications(project, incomingOrder);

				if (incomingOrderStatus == "canceled")
					project.ProjectStatus = ProjectStatusEnumDto.Canceled;

				var projectIncomingOrderContactId = project.IncomingOrderContact.Id;
				var projectBillToContactId = project.BillToContact.Id;
				var projectShipToContactId = project.ShipToContact.Id;
				var projectIncomingOrderCustomerId = project.IncomingOrderCustomer.Id;
				MapIncomingOrderToProject(incomingOrder, project);

				if (!project.IncomingOrderContact.Id.HasValue)
					project.IncomingOrderContact.Id = projectIncomingOrderContactId;
				if (!project.BillToContact.Id.HasValue)
					project.BillToContact.Id = projectBillToContactId;
				if (!project.ShipToContact.Id.HasValue)
					project.ShipToContact.Id = projectShipToContactId;
				if (!project.IncomingOrderCustomer.Id.HasValue)
					project.IncomingOrderCustomer.Id = projectIncomingOrderCustomerId;

				//Process order notifications
				_notificationManager.ProcessNotifications(orderNotifications, project);

				//Process order line notifications unless order notifications exist that are not cleanup
				if (orderNotifications.All(x => x == NotificationTypeDto.EntityCleanup))
				{
					var projectOrderLineNotifications = projectsOrderLineNotifications[project.Id.GetValueOrDefault()];
					_notificationManager.ProcessNotifications(projectOrderLineNotifications, project);
				}

				_projectProvider.UpdateFromIncomingOrder(project);
				LogMessage(MessageIds.ProjectUpdated, "Project Updated",
					incomingOrder, project);
			}
		}

		/// <summary>
		///     Maps the incoming order to project.
		/// </summary>
		/// <param name="incomingOrder">The incoming order.</param>
		/// <param name="project">The project.</param>
		public static void MapIncomingOrderToProject(IncomingOrder incomingOrder,
			Project project)
		{
			project.Currency = incomingOrder.Currency;
			project.BusinessUnit = incomingOrder.BusinessUnit;
			project.CreationDate = incomingOrder.CreationDate;
			project.CustomerPo = incomingOrder.CustomerPo;
			project.CustomerRequestedDate = incomingOrder.CustomerRequestedDate;
			project.DateBooked = incomingOrder.DateBooked;
			project.DateOrdered = incomingOrder.DateOrdered;
			project.ExternalProjectId = incomingOrder.ExternalProjectId;
			project.LastUpdateDate = incomingOrder.LastUpdateDate;
			project.OrderNumber = incomingOrder.OrderNumber;
			project.OrderType = incomingOrder.OrderType;
			project.ProjectHeaderStatus = incomingOrder.ProjectHeaderStatus;
			project.ProjectName = incomingOrder.ProjectName;
			project.ProjectNumber = incomingOrder.ProjectNumber;
			project.Status = incomingOrder.Status;
			project.OriginalXmlParsed = incomingOrder.OriginalXmlParsed;
			project.WorkOrderBusinessComponentId = incomingOrder.WorkOrderBusinessComponentId;
			project.WorkOrderId = incomingOrder.WorkOrderId;
			project.QuoteNo = incomingOrder.QuoteNo;
			project.TotalOrderPrice = incomingOrder.TotalOrderPrice;
			if (project.ServiceLines != null && project.ServiceLines.Any())
			{
				project.IncomingOrderContact = MapIncomingOrderContact(incomingOrder.IncomingOrderContact,
					project.IncomingOrderContact);
                if (null != incomingOrder.IncomingOrderContact && null != project.IncomingOrderContact)
			        project.IncomingOrderContact.CompanyName = incomingOrder.IncomingOrderContact.CompanyName;
				project.BillToContact = MapIncomingOrderContact(incomingOrder.BillToContact, project.BillToContact);
				project.ShipToContact = MapIncomingOrderContact(incomingOrder.ShipToContact, project.ShipToContact);
				project.IncomingOrderCustomer =
					IncomingOrderCustomer.MapIncomingOrderCustomer(incomingOrder.IncomingOrderCustomer);
			}
		}

		private static IncomingOrderContact MapIncomingOrderContact(IncomingOrderContact incomingOrderContact,
			IncomingOrderContact projectIncomingOrderContact)
		{
			if (null == incomingOrderContact)
				return projectIncomingOrderContact;
			var contact = IncomingOrderContact.MapIncomingOrderContact(incomingOrderContact);
			if (null != projectIncomingOrderContact)
			{
				contact.CompanyName = projectIncomingOrderContact.CompanyName;
				//contact.Title = projectIncomingOrderContact.Title;
				//contact.Email = projectIncomingOrderContact.Email;
				//contact.Phone = projectIncomingOrderContact.Phone;
			}
			return contact;
		}
	}
}