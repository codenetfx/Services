using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Linq;
using ITaskTypeProvider = UL.Aria.Service.Provider.ITaskTypeProvider;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Class TaskTypeManager.
	/// </summary>
	public class TaskTypeManager : SearchManagerBase<TaskType>, ITaskTypeManager
	{
		/// <summary>
		/// The _task type provider
		/// </summary>
		private readonly ITaskTypeProvider _taskTypeProvider;
        private readonly ILinkProvider _linkProvider;
        private readonly ILookupProvider _lookupProvider;
        private readonly IDocumentTemplateProvider _documentTemplateProvider;
	    private readonly ITaskTypeBehaviorProvider _taskTypeBehaviorProvider;
	    private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeManager" /> class.
        /// </summary>
        /// <param name="taskTypeProvider">The task type provider.</param>
        /// <param name="linkProvider">The link provider.</param>
        /// <param name="lookupProvider">The lookup provider.</param>
        /// <param name="documentTemplateProvider">The document template provider.</param>
        /// <param name="taskTypeBehaviorProvider">The task type behavior provider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public TaskTypeManager(ITaskTypeProvider taskTypeProvider, ILinkProvider linkProvider, ILookupProvider lookupProvider, 
            IDocumentTemplateProvider documentTemplateProvider, ITaskTypeBehaviorProvider taskTypeBehaviorProvider, ITransactionFactory transactionFactory)
            :base(taskTypeProvider)
		{
			_taskTypeProvider = taskTypeProvider;
            _linkProvider = linkProvider;
            _lookupProvider = lookupProvider;
            _documentTemplateProvider = documentTemplateProvider;
            _taskTypeBehaviorProvider = taskTypeBehaviorProvider;
            _transactionFactory = transactionFactory;
		}


        /// <summary>
        /// Creates the specified task type dto.
        /// </summary>
        /// <param name="taskType">The task type dto.</param>
        /// <returns>TaskTypeDto.</returns>
        public override Guid Create(TaskType taskType)
        {
            var id = new Guid();
            using (var transaction = _transactionFactory.Create())
            {
                id = base.Create(taskType);
                _linkProvider.UpdateLinkAssociations(taskType.Links, id);
                _lookupProvider.UpdateBulk(taskType.BusinessUnits, id);
                _documentTemplateProvider.UpdateDocumentTemplateAssociations(taskType.DocumentTemplates, taskType.Id.GetValueOrDefault());
                _taskTypeBehaviorProvider.Save(taskType.TaskTypeBehaviors, id);
                transaction.Complete();
            }

            return id;
        }

        /// <summary>
        /// Updates the specified task type dto.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="taskType">The task type dto.</param>
        /// <exception cref="Exception"></exception>
        public override void Update(Guid id, TaskType taskType)
        {
           
            using (var transaction = _transactionFactory.Create())
            {
                base.Update(id, taskType);
                _linkProvider.UpdateLinkAssociations(taskType.Links, id);
                _lookupProvider.UpdateBulk(taskType.BusinessUnits, id);
                _documentTemplateProvider.UpdateDocumentTemplateAssociations(taskType.DocumentTemplates, id);
                _taskTypeBehaviorProvider.Save(taskType.TaskTypeBehaviors, id);
                transaction.Complete();
            }
        }


		/// <summary>
		/// Gets the lookups.
		/// </summary>
		/// <returns>List&lt;Lookup&gt;.</returns>
		public IEnumerable<Lookup> GetLookups()
		{
			return _taskTypeProvider.GetLookups();
		}

        /// <summary>
        /// Gets the lookups.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        public IEnumerable<Lookup> GetLookups(bool includeDeleted)
        {
            return _taskTypeProvider.GetLookups(includeDeleted);
        }

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TaskType> FetchAll()
		{
			return _taskTypeProvider.FetchAll();
		}

	    /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override IEnumerable<ValidationViolationDto> Validate(TaskType entity)
        {
            var violations = new List<ValidationViolationDto>();
            var allBusinessUnits = _lookupProvider.FetchAllBusinessUnits();
            var allId = allBusinessUnits.Where(x => x.Code == AssetFieldNames.BusinessUnitAllToken).Select(x => x.Id).FirstOrDefault();
            var searchCriteria = new SearchCriteria { IncludeDeletedRecords = false, Keyword = entity.Name, EndIndex = 99 };
            var results = _taskTypeProvider.Search(searchCriteria);
            bool matchFound = false;

            if (allId != null && entity.BusinessUnits.Any(x => x.Id == allId))
            {
                matchFound = results.Results.Count(x => x.Id != entity.Id) > 0;
            }
            else
            {
                var entityBusinessUnitCodes = allBusinessUnits
                    .Where(x => entity.BusinessUnits.Select(y => y.Id).Contains(x.Id))
                    .Select(x => x.Code).ToList();

                var activeBu = results.Results
                   .Where(x => x.Id != entity.Id)
                   .SelectMany(x => x.BusinessUnitCodes.Replace(" ", "").Split(','));

                matchFound = activeBu.Contains(AssetFieldNames.BusinessUnitAllToken);

                if (!matchFound)
                    matchFound = activeBu.Intersect(entityBusinessUnitCodes).Any();
            }

            if (matchFound)
            {
                violations.Add(new ValidationViolationDto
                {
                    Code = ValidationCodes.TaskType.NameBusinessUnitAlreadyExists,
                    Message = "Active record currently exists for this Predefined Task Name/Business Unit combination(s).",
                    Level = ValidationLevelEnumDto.Error
                });
            }

            return violations;
        }

	}
}
