using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	/// 
	/// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
		InstanceContextMode = InstanceContextMode.PerCall)]
	public class TaskCategoryService : ITaskCategoryService
	{
		private readonly ITaskCategoryManager _taskCategoryManager;
		private readonly IMapperRegistry _mapperRegistry;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskCategoryService" /> class.
		/// </summary>
		/// <param name="taskCategoryManager">The task template manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		public TaskCategoryService(ITaskCategoryManager taskCategoryManager, IMapperRegistry mapperRegistry,
							  ITransactionFactory transactionFactory)
		{

			_taskCategoryManager = taskCategoryManager;
			_mapperRegistry = mapperRegistry;
			_transactionFactory = transactionFactory;
		}
		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Contracts.Dto.TaskCategorySearchModelDto Search(Contracts.Dto.SearchCriteriaDto searchCriteria)
		{

			Guard.IsNotNull(searchCriteria, "searchCriteria");

			var criteria = _mapperRegistry.Map<SearchCriteria>(searchCriteria);
			return _mapperRegistry.Map<Contracts.Dto.TaskCategorySearchModelDto>(_taskCategoryManager.Search(criteria));

		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Contracts.Dto.TaskCategoryDto FetchById(string id)
		{
			Guard.IsNotNullOrEmpty(id, "TaskTemplateId");
			var templateId = id.ToGuid();
			Guard.IsNotEmptyGuid(templateId, "TaskTemplateId");

			var taskTemplate = _taskCategoryManager.FetchById(templateId);
            return _mapperRegistry.Map<TaskCategoryDto>(taskTemplate);
        }

		

		/// <summary>
		/// Creates the specified task template.
		/// </summary>
		/// <param name="taskTemplate">The task template.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Contracts.Dto.TaskCategoryDto Create(Contracts.Dto.TaskCategoryDto taskTemplate)
		{
			Guard.IsNotNull(taskTemplate, "taskTemplate");

			var taskTemplateEntity = _mapperRegistry.Map<TaskCategory>(taskTemplate);
			TaskCategory returntaskTemplate;

			using (TransactionScope transactionScope = _transactionFactory.Create())
			{
				returntaskTemplate = _taskCategoryManager.Create(taskTemplateEntity);
				transactionScope.Complete();
			}
			return _mapperRegistry.Map<TaskCategoryDto>(returntaskTemplate);

		}

		/// <summary>
		/// Updates the task template.
		/// </summary>
		/// <param name="taskCategoryId"></param>
		/// <param name="taskCategory"></param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public TaskCategoryDto Update(string taskCategoryId, TaskCategoryDto taskCategory)
		{
			Guard.IsNotNull(taskCategory, "taskCategory");

			var taskTemplateEntity = _mapperRegistry.Map<TaskCategory>(taskCategory);
			TaskCategory returntaskTemplate;

			using (TransactionScope transactionScope = _transactionFactory.Create())
			{
				returntaskTemplate = _taskCategoryManager.Update(taskTemplateEntity);
				transactionScope.Complete();
			}
			return _mapperRegistry.Map<TaskCategoryDto>(returntaskTemplate);


		}

		/// <summary>
		/// Deletes the task template by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Delete(string id)
		{
			Guard.IsNotNullOrEmpty(id, "TaskTemplateId");
			var templateId = id.ToGuid();
			Guard.IsNotEmptyGuid(templateId, "TaskTemplateId");

			using (TransactionScope transactionScope = _transactionFactory.Create())
			{
				_taskCategoryManager.Delete(templateId);
				transactionScope.Complete();
			}

		}

		/// <summary>
		/// Fetches all count.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public int FetchAllCount()
		{
			return _taskCategoryManager.FetchAllCount();
		}
	}
}
