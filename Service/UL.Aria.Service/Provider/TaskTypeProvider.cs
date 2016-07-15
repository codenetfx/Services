using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Class TaskTypeProvider.
	/// </summary>
    public class TaskTypeProvider : SearchProviderBase<TaskType>, ITaskTypeProvider
	{
		/// <summary>
		/// The _task type repository
		/// </summary>
		private readonly ITaskTypeRepository _taskTypeRepository;

		/// <summary>
		/// The _principal resolver
		/// </summary>
		private readonly IPrincipalResolver _principalResolver;

		private readonly ILinkProvider _linkProvider;
		private readonly ILookupProvider _lookupProvider;
		private readonly IDocumentTemplateProvider _documentTemplateProvider;
	    private readonly ITaskTypeNotificationProvider _taskTypeNotificationProvider;
	    private readonly ITaskTypeBehaviorProvider _taskTypeBehaviorProvider;
	    private readonly ITransactionFactory _transactionFactory;

	    /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeProvider" /> class.
        /// </summary>
        /// <param name="taskTypeRepository">The task type repository.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="linkProvider">The link provider.</param>
        /// <param name="lookupProvider">The lookup provider.</param>
        /// <param name="documentTemplateProvider">The document template provider.</param>
        /// <param name="taskTypeNotificationProvider">The task type notification provider.</param>
	    /// <param name="taskTypeBehaviorProvider"></param>
        /// <param name="transactionFactory">The transaction factory.</param>
		public TaskTypeProvider(ITaskTypeRepository taskTypeRepository, IPrincipalResolver principalResolver,
			ILinkProvider linkProvider, ILookupProvider lookupProvider, IDocumentTemplateProvider documentTemplateProvider,
            ITaskTypeNotificationProvider taskTypeNotificationProvider, ITaskTypeBehaviorProvider taskTypeBehaviorProvider, ITransactionFactory transactionFactory)
            :base(taskTypeRepository, principalResolver)
		{
			_taskTypeRepository = taskTypeRepository;
			_principalResolver = principalResolver;
			_linkProvider = linkProvider;
			_lookupProvider = lookupProvider;
			_documentTemplateProvider = documentTemplateProvider;
            _taskTypeNotificationProvider = taskTypeNotificationProvider;
	        _taskTypeBehaviorProvider = taskTypeBehaviorProvider;
	        _transactionFactory = transactionFactory;
		}
        
		/// <summary>
		/// Gets the lookups.
		/// </summary>
		/// <returns>IEnumerable&lt;Lookup&gt;.</returns>
		public IEnumerable<Lookup> GetLookups()
		{
			return _taskTypeRepository.GetLookups();
		}

		/// <summary>
		/// Gets the lookups.
		/// </summary>
		/// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
		/// <returns></returns>
		public IEnumerable<Lookup> GetLookups(bool includeDeleted)
		{
			return _taskTypeRepository.GetLookups(includeDeleted);
		}

        /// <summary>
        /// Fetches the active by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <returns></returns>
        public TaskType Fetch(Guid id, bool isDeleted)
		{
            return _taskTypeRepository.Fetch(id, isDeleted);
		}

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TaskType> FetchAll()
		{
		    var taskTypes = _taskTypeRepository.FetchAll();
		    taskTypes.ForEach(taskType =>
		    {
		        taskType.BusinessUnits = _lookupProvider.FetchBusinessUnitByEntity(taskType.Id.GetValueOrDefault()).ToList();
		    });

		    return taskTypes;
		}

        /// <summary>
        /// Fetches the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>TaskType.</returns>
        public override TaskType Fetch(Guid id)
        {
            var taskType = base.Fetch(id);
            taskType.Links = _linkProvider.FetchLinksByEntity(taskType.Id.GetValueOrDefault()).ToList();
            taskType.BusinessUnits = _lookupProvider.FetchBusinessUnitByEntity(taskType.Id.GetValueOrDefault()).ToList();
            taskType.DocumentTemplates = _documentTemplateProvider.FetchDocumentTemplatesByEntity(id).ToList();
            taskType.Notifications = _taskTypeNotificationProvider.FetchByTaskTypeId(id).ToList();
            taskType.TaskTypeBehaviors = _taskTypeBehaviorProvider.FindByTaskTypeId(id);
            return taskType;
        }

        /// <summary>
        /// Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The unit of measure.</param>
	    public override void Update(Guid id, TaskType entity)
	    {
	        using (var transaction = _transactionFactory.Create())
	        {
                base.Update(id, entity);
                _taskTypeNotificationProvider.Save(entity.Notifications, id);
                transaction.Complete();
	        }
	    }

        /// <summary>
        /// Creates the specified industry code.
        /// </summary>
        /// <param name="entity">The industry code.</param>
	    public override void Create(TaskType entity)
	    {
            using (var transaction = _transactionFactory.Create())
            {
                base.Create(entity);
                _taskTypeNotificationProvider.Save(entity.Notifications, entity.Id.GetValueOrDefault());
                transaction.Complete();
            }
        }
	}
}
