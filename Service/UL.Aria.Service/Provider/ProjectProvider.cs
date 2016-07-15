using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Class ProjectProvider
    /// </summary>
    public class ProjectProvider : IProjectProvider
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ICompanyProvider _companyProvider;
	    private readonly ITaskProvider _taskProvider;
	    private readonly ILogManager _logManager;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectProjectTemplateRepository _projectProjectTemplateRepository;
        private readonly IProjectStatusMessageProvider _projectStatusMessageProvider;
        private readonly ITransactionFactory _transactionFactory;


		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectProvider" /> class.
		/// </summary>
		/// <param name="logManager">The log manager.</param>
		/// <param name="projectRepository">The project repository.</param>
		/// <param name="projectProjectTemplateRepository">The project project template repository.</param>
		/// <param name="projectStatusMessageProvider">The project status message provider.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="assetProvider">The asset provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="companyProvider">The company provider.</param>
		/// <param name="taskProvider">The task provider.</param>
        public ProjectProvider(ILogManager logManager, IProjectRepository projectRepository, IProjectProjectTemplateRepository projectProjectTemplateRepository,
            IProjectStatusMessageProvider projectStatusMessageProvider, ITransactionFactory transactionFactory,
            IAssetProvider assetProvider, IPrincipalResolver principalResolver, ICompanyProvider companyProvider, ITaskProvider taskProvider)
        {
            _logManager = logManager;
            _projectRepository = projectRepository;
            _projectProjectTemplateRepository = projectProjectTemplateRepository;
            _projectStatusMessageProvider = projectStatusMessageProvider;
            _transactionFactory = transactionFactory;
            _assetProvider = assetProvider;
            _principalResolver = principalResolver;
            _companyProvider = companyProvider;
	        _taskProvider = taskProvider;
        }

        /// <summary>
        /// Fetches a IEnumerable of name id pair objects for  project as lookup objects.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<Lookup> FetchProjectLookups()
        {
            return _projectRepository.FetchProjectLookups();
        }

        /// <summary>
        /// Fetches all headers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Guid> FetchAllIds()
        {
            return _projectRepository.FetchAllIds();
        }

        /// <summary>
        ///     Fetches the projects.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>IEnumerable{Project}.</returns>
        public IEnumerable<Project> FetchProjects(IEnumerable<Guid> ids)
        {
            return _projectRepository.FetchProjects(ids);
        }

        /// <summary>
        ///     Fetches all projects.
        /// </summary>
        /// <returns>IEnumerable{Project}.</returns>
        public IEnumerable<Project> FetchProjects()
        {
            return _projectRepository.FetchAll();
        }

        /// <summary>
        ///     Publishes the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="changedProject">The changed project.</param>
        public void Publish(Project project, Project changedProject)
        {
            var changedLines = new List<ProjectStatusMessageServiceLine>();

            if (VerifyStatusChange(project, changedProject, changedLines))
            {
                var logMessage = new LogMessage(MessageIds.ProjectProviderStatusChanged)
                {
                    Message = "Project status changed",
                    LogPriority = LogPriority.Medium,
                    Severity = TraceEventType.Information
                };
                logMessage.Data.Add("ProjectId", changedProject.Id.ToString());
                logMessage.Data.Add("ProjectStatus", changedProject.ProjectStatus.ToString());
                logMessage.LogCategories.Add(LogCategory.AuditMessage);
                logMessage.LogCategories.Add(LogCategory.Project);
                _logManager.Log(logMessage);
                var authoritativeProject = changedProject;
                if (project != null && string.IsNullOrWhiteSpace(authoritativeProject.ExternalProjectId))
                    authoritativeProject = project;
                _projectStatusMessageProvider.Publish(new ProjectStatusMessage
                {
// ReSharper disable once PossibleInvalidOperationException
                    ProjectId = changedProject.Id.Value,
                    OldStatus = project != null ? project.ProjectStatus : ProjectStatusEnumDto.OnHold,
                    NewStatus = changedProject.ProjectStatus,
                    EventId = BusinessMessageIds.Projects.ProjectStatusMessage,
                    ExternalProjectId = authoritativeProject.ExternalProjectId,
                    ProjectName = authoritativeProject.ProjectName,
                    ProjectNumber = authoritativeProject.ProjectNumber,
                    OrderNumber = authoritativeProject.OrderNumber,
                    ProjectServiceLineStatuses = changedLines,
                    WorkOrderBusinessComponentId = changedProject.WorkOrderBusinessComponentId,
                    WorkOrderId = changedProject.WorkOrderId
                });
            }
        }

        /// <summary>
        ///     Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Project.</returns>
        public Project FetchById(Guid id)
        {
            return _projectRepository.FetchById(id);
        }

        /// <summary>
        ///     Creates the specified container id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <param name="project">The project.</param>
        /// <returns>Guid.</returns>
        public Guid Create(Guid containerId, Project project)
        {
            var createdDateTime = DateTime.UtcNow;
            var createdBy = _principalResolver.UserId;
            project.CreatedDateTime = createdDateTime;
            project.CreatedById = createdBy;
            project.UpdatedDateTime = createdDateTime;
            project.UpdatedById = createdBy;
            project.IncomingOrderContact.CreatedDateTime = createdDateTime;
            project.IncomingOrderContact.CreatedById = createdBy;
            project.IncomingOrderContact.UpdatedDateTime = createdDateTime;
            project.IncomingOrderContact.UpdatedById = createdBy;
            project.BillToContact.CreatedDateTime = createdDateTime;
            project.BillToContact.CreatedById = createdBy;
            project.BillToContact.UpdatedDateTime = createdDateTime;
            project.BillToContact.UpdatedById = createdBy;
            project.ShipToContact.CreatedDateTime = createdDateTime;
            project.ShipToContact.CreatedById = createdBy;
            project.ShipToContact.UpdatedDateTime = createdDateTime;
            project.ShipToContact.UpdatedById = createdBy;
            project.IncomingOrderCustomer.CreatedDateTime = createdDateTime;
            project.IncomingOrderCustomer.CreatedById = createdBy;
            project.IncomingOrderCustomer.UpdatedDateTime = createdDateTime;
            project.IncomingOrderCustomer.UpdatedById = createdBy;
            foreach (var serviceLine in project.ServiceLines)
            {
                serviceLine.CreatedDateTime = createdDateTime;
                serviceLine.CreatedById = createdBy;
                serviceLine.UpdatedDateTime = createdDateTime;
                serviceLine.UpdatedById = createdBy;
            }
            project.ContainerId = containerId;
            SetCompany(project);

            using (var transactionScope = _transactionFactory.Create())
            {
                _projectRepository.Create(project);
                SaveProjectTemplateOnCreate(project, project.ProjectTemplateId);
// ReSharper disable once PossibleInvalidOperationException
                _assetProvider.Create(containerId, project.Id.Value, project);
                transactionScope.Complete();
            }

            return project.Id.Value;
        }

        private void SaveProjectTemplateOnCreate(Project project, Guid projectTemplateId)
        {
            _projectProjectTemplateRepository.Save(new ProjectProjectTemplate
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id.Value,
                ProjectTemplateId = projectTemplateId,
                CreatedById = project.CreatedById,
                UpdatedById = project.CreatedById,
                CreatedDateTime = project.CreatedDateTime,
                UpdatedDateTime = project.UpdatedDateTime,
                IsOriginal = true
            });
        }

        private void SaveProjectTemplateOnUpdate(Project project, Guid projectTemplateId, Guid updatedBy, DateTime updatedOn)
        {
            _projectProjectTemplateRepository.Save(new ProjectProjectTemplate
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id.Value,
                ProjectTemplateId = projectTemplateId,
                CreatedById = updatedBy,
                UpdatedById = updatedBy,
                CreatedDateTime = updatedOn,
                UpdatedDateTime = updatedOn,
                IsOriginal = false
            });
        }

        /// <summary>
        ///     Updates the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        public void Update(Project project)
        {
            var updateDateTime = DateTime.UtcNow;
            var updatedBy = _principalResolver.UserId;
            project.UpdatedDateTime = updateDateTime;
            project.UpdatedById = updatedBy;
            project.IncomingOrderContact.UpdatedDateTime = updateDateTime;
            project.IncomingOrderContact.UpdatedById = updatedBy;
            project.BillToContact.UpdatedDateTime = updateDateTime;
            project.BillToContact.UpdatedById = updatedBy;
            project.ShipToContact.UpdatedDateTime = updateDateTime;
            project.ShipToContact.UpdatedById = updatedBy;
            project.IncomingOrderCustomer.UpdatedDateTime = updateDateTime;
            project.IncomingOrderCustomer.UpdatedById = updatedBy;
            foreach (var serviceLine in project.ServiceLines)
            {
                serviceLine.UpdatedDateTime = updateDateTime;
                serviceLine.UpdatedById = updatedBy;
            }
            SetCompany(project);

            using (var transactionScope = _transactionFactory.Create())
            {
                _projectRepository.Update(project);
				// ReSharper disable once PossibleInvalidOperationException
                _assetProvider.Update(project.Id.Value, project);
				CancelProjectTasks(project);
                if (project.AdditionalProjectTemplateId.GetValueOrDefault() != default(Guid))
                {
                    SaveProjectTemplateOnUpdate(project, project.AdditionalProjectTemplateId.Value, updatedBy, updateDateTime);
                }
                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Updates the status from order.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void UpdateStatusFromOrder(Project project)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                _projectRepository.UpdateStatusFromOrder(project);
				// ReSharper disable once PossibleInvalidOperationException
                _assetProvider.Update(project.Id.Value, project);
                transactionScope.Complete();
            }
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
                _projectRepository.UpdateAllContactsForExternalId(externalId, contact);
                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="contact">The contact.</param>
        public void UpdateContact(Guid projectId, IncomingOrderContact contact)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                _projectRepository.UpdateContact(projectId, contact);
                transactionScope.Complete();
            }
        }

		/// <summary>
		/// Creates the contact.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="contact">The contact.</param>
		/// <returns>Guid.</returns>
		public Guid CreateContact(Guid projectId, IncomingOrderContact contact)
		{
			Guid id;

			using (var transactionScope = _transactionFactory.Create())
			{
				id = _projectRepository.CreateContact(projectId, contact);
				transactionScope.Complete();
			}

			return id;
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
                _projectRepository.UpdateAllCustomersForExternalId(externalId, customer);
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Fetches the by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>IList{Project}.</returns>
        public IList<Project> FetchByOrderNumber(string orderNumber)
        {
            return _projectRepository.FetchByOrderNumber(orderNumber);
        }

        /// <summary>
        ///     Updates from incoming order.
        /// </summary>
        /// <param name="project">The project.</param>
        public void UpdateFromIncomingOrder(Project project)
        {
            var updateDateTime = DateTime.UtcNow;
            var updatedBy = _principalResolver.UserId;
            project.UpdatedDateTime = updateDateTime;
            project.UpdatedById = updatedBy;
            project.IndustryCode = null;
            project.ServiceCode = null;
            project.IncomingOrderContact.UpdatedDateTime = updateDateTime;
            project.IncomingOrderContact.UpdatedById = updatedBy;
            project.BillToContact.UpdatedDateTime = updateDateTime;
            project.BillToContact.UpdatedById = updatedBy;
            project.ShipToContact.UpdatedDateTime = updateDateTime;
            project.ShipToContact.UpdatedById = updatedBy;
            project.IncomingOrderCustomer.UpdatedDateTime = updateDateTime;
            project.IncomingOrderCustomer.UpdatedById = updatedBy;
            foreach (var serviceLine in project.ServiceLines)
            {
                serviceLine.UpdatedDateTime = updateDateTime;
                serviceLine.UpdatedById = updatedBy;
            }
            SetCompany(project);

            using (var transactionScope = _transactionFactory.Create())
            {
                _projectRepository.UpdateFromIncomingOrder(project);
				// ReSharper disable once PossibleInvalidOperationException
                _assetProvider.Update(project.Id.Value, project);
	            CancelProjectTasks(project);
                transactionScope.Complete();
            }
        }

        private void SetCompany(IncomingOrder project)
        {
            if (project.CompanyId.HasValue && project.CompanyId != Guid.Empty)
            {
                // ReSharper disable once PossibleInvalidOperationException
                var company = _companyProvider.FetchById(project.CompanyId.Value);
                project.CompanyName = company.Name;
            }
        }

        private static bool VerifyStatusChange(Project project, Project changedProject,
            List<ProjectStatusMessageServiceLine> changedLines)
        {
            if (null == project)
            {
                changedLines.AddRange(
                    changedProject.ServiceLines.Select(
                        x =>
                            new ProjectStatusMessageServiceLine
                            {
// ReSharper disable once PossibleInvalidOperationException
                                ProjectServiceLineId = x.Id.Value,
                                ServiceLineAction = ServiceLineAction.Add,
                                ServiceLine = x
                            }));
                return true;
            }

            changedLines.AddRange(changedProject.ServiceLines.Select(
                x => new ProjectStatusMessageServiceLine
                {
// ReSharper disable once PossibleInvalidOperationException
                    ProjectServiceLineId = x.Id.Value,
                    ServiceLineAction =
                        project.ServiceLines.All(y => y.Id != x.Id) ? ServiceLineAction.Add : ServiceLineAction.NoChange,
                    ServiceLine = x
                }));
            changedLines.AddRange(project.ServiceLines.Where(x => changedProject.ServiceLines.All(y => y.Id != x.Id))
                .Select(x => new ProjectStatusMessageServiceLine
                {
// ReSharper disable once PossibleInvalidOperationException
                    ProjectServiceLineId = x.Id.Value,
                    ServiceLineAction = ServiceLineAction.Remove,
                    ServiceLine = x
                }));

            if (project.ProjectStatus != changedProject.ProjectStatus)
                return true;

            return changedLines.Any(x => x.ServiceLineAction != ServiceLineAction.NoChange);
        }


		internal void CancelProjectTasks(Project project)
		{

			if(project.ProjectStatus != ProjectStatusEnumDto.Canceled)
				return;
			// ReSharper disable once PossibleInvalidOperationException
			var tasks = _taskProvider.FetchAllFlatList(project.ContainerId.Value);
			var taskList = tasks.Where(x => !x.IsClosed).ToList();
			foreach (var task in taskList)
			{
				task.Status = TaskStatusEnumDto.Canceled;
			}
			_taskProvider.BulkUpdate(project.ContainerId.Value, taskList);
		}

    }
}