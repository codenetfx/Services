using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	/// Class TaskTypeService.
	/// </summary>
	[AutoRegisterRestServiceAttribute]
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false, InstanceContextMode = InstanceContextMode.PerCall)]
	public class TaskTypeService : ITaskTypeService
	{
		private readonly IMapperRegistry _mapperRegistry;
		private readonly ITaskTypeManager _taskTypeManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskTypeService"/> class.
		/// </summary>
		/// <param name="taskTypeManager">The task type manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public TaskTypeService(ITaskTypeManager taskTypeManager, IMapperRegistry mapperRegistry)
		{
			_taskTypeManager = taskTypeManager;
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// Assures the unique identifier identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <returns>Guid.</returns>
		internal Guid AssureGuidId(string id, string parameterName)
		{
			Guard.IsNotNullOrEmptyTrimmed(id, parameterName);
			Guid guidId = id.ParseOrDefault<Guid>(Guid.Empty);
			Guard.IsNotEmptyGuid(guidId, parameterName);
			return guidId;
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>TaskTypeDto.</returns>
		public TaskTypeDto FetchById(string id)
		{
			var guidId = AssureGuidId(id, "id");
			var result = _taskTypeManager.Fetch(guidId);

			return _mapperRegistry.Map<TaskTypeDto>(result);
		}

		/// <summary>
		/// Deletes the specified task type identifier.
		/// </summary>
		/// <param name="taskTypeId">The task type identifier.</param>
		public void Delete(string taskTypeId)
		{
			var guidId = AssureGuidId(taskTypeId, "taskTypeId");
			_taskTypeManager.Delete(guidId);
		}

		/// <summary>
		/// Creates the specified task type.
		/// </summary>
		/// <param name="taskTypeDto">Type of the task.</param>
		/// <returns>TaskTypeDto.</returns>
		public TaskTypeDto Create(TaskTypeDto taskTypeDto)
		{
			Guard.IsNotNull(taskTypeDto, "taskTypeDto");
			var id = _taskTypeManager.Create(_mapperRegistry.Map<TaskType>(taskTypeDto));

		    var retrieved = _taskTypeManager.Fetch(id);

            return _mapperRegistry.Map<TaskTypeDto>(retrieved);
		}

		/// <summary>
		/// Updates the specified task type dto.
		/// </summary>
		/// <param name="taskTypeId">The task type identifier.</param>
		/// <param name="taskTypeDto">The task type dto.</param>
		public void Update(string taskTypeId, TaskTypeDto taskTypeDto)
		{
            Guard.IsNotNullOrEmpty(taskTypeId, "taskTypeId");
            Guard.IsNotNull(taskTypeDto, "taskType");
            var convertedId = Guid.Parse(taskTypeId);
            Guard.IsNotEmptyGuid(convertedId, "taskTypeId");

		    var taskType = _mapperRegistry.Map<TaskType>(taskTypeDto);
            _taskTypeManager.Update(convertedId, taskType);

		}

		/// <summary>
		/// Searches the specified search criteria dto.
		/// </summary>
		/// <param name="searchCriteriaDto">The search criteria dto.</param>
		/// <returns>TaskTypeSearchModelDto.</returns>
		public TaskTypeSearchModelDto Search(SearchCriteriaDto searchCriteriaDto)
		{
			Guard.IsNotNull(searchCriteriaDto, "searchCriteriaDto");
			var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteriaDto);
		    var searchResultSet = _taskTypeManager.Search(criteria);
			return _mapperRegistry.Map<TaskTypeSearchModelDto>(searchResultSet);
		}

		/// <summary>
		/// Gets the lookups.
		/// </summary>
		/// <returns>List&lt;LookupDto&gt;.</returns>
		public IEnumerable<LookupDto> GetLookups()
		{
			return _mapperRegistry.Map<List<LookupDto>>(_taskTypeManager.GetLookups());
		}

		/// <summary>
		/// Gets the lookups.
		/// </summary>
		/// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
		/// <returns></returns>
		public IEnumerable<LookupDto> GetLookups(string includeDeleted)
		{
			return _mapperRegistry.Map<List<LookupDto>>(_taskTypeManager.GetLookups(
				includeDeleted.ParseOrDefault<bool>(false)));
		}

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<TaskTypeDto> FetchAll()
		{
			var result = new List<TaskTypeDto>();
			var tasktypes = _taskTypeManager.FetchAll();
			_mapperRegistry.Map(tasktypes, result);
			return result;
		}

        /// <summary>
        /// Validates the specified task type.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IEnumerable<ValidationViolationDto> Validate(TaskTypeDto entity)
	    {
            Guard.IsNotNull(entity, "entity");

            var taskType = _mapperRegistry.Map<TaskType>(entity);
            var validations = _taskTypeManager.Validate(taskType);

            return validations;	        
	    }
	}
}
