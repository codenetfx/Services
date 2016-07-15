using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskCategoryProvider : ITaskCategoryProvider
	{
		private readonly ITaskCategoryRepository _taskCategoryRepository;
		private readonly IBusinessMessageProvider _businessMessageProvider;
		private readonly IPrincipalResolver _principalResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskCategoryProvider" /> class.
		/// </summary>
		/// <param name="taskCategoryRepository">The task template repository.</param>
		/// <param name="businessMessageProvider">The business message provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		public TaskCategoryProvider(ITaskCategoryRepository taskCategoryRepository, IBusinessMessageProvider businessMessageProvider, IPrincipalResolver principalResolver)
		{
			_taskCategoryRepository = taskCategoryRepository;
			_businessMessageProvider = businessMessageProvider;

			_principalResolver = principalResolver;

		}
		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public Domain.Search.TaskCategorySearchResultSet Search(Domain.Search.SearchCriteria searchCriteria)
		{
			return _taskCategoryRepository.Search(searchCriteria);
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public Domain.Entity.TaskCategory FetchById(Guid id)
		{
			return _taskCategoryRepository.FindById(id);
		}

		/// <summary>
		/// Creates the specified task template.
		/// </summary>
		/// <param name="taskTemplate">The task template.</param>
		/// <returns></returns>
		public Domain.Entity.TaskCategory Create(Domain.Entity.TaskCategory taskTemplate)
		{
			SetupTaskCategory(_principalResolver, taskTemplate);

			_taskCategoryRepository.Add(taskTemplate);
			var createdTaskTemplate = _taskCategoryRepository.FindById(taskTemplate.Id.Value);

			return createdTaskTemplate;

		}

		/// <summary>
		/// Updates the specified task template.
		/// </summary>
		/// <param name="taskTemplate">The task template.</param>
		/// <returns></returns>
		public Domain.Entity.TaskCategory Update(Domain.Entity.TaskCategory taskTemplate)
		{
			SetupTaskCategory(_principalResolver, taskTemplate);
			taskTemplate.UpdatedById = _principalResolver.UserId;
			_taskCategoryRepository.Update(taskTemplate);
			return taskTemplate;
		}

		/// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete(Guid id)
		{
			var data = new Dictionary<string, string>();
			data.Add("Id", id.ToString());
			_businessMessageProvider.Publish(AuditMessageIdEnumDto.TaskTemplateDeleted, "Task Template Deleted", data);
			_taskCategoryRepository.Remove(id);

		}

		/// <summary>
		/// Fetches all count.
		/// </summary>
		/// <returns></returns>
		public int FetchAllCount()
		{
			return _taskCategoryRepository.FetchAllCount();
		}


		


		/// <summary>
		/// Fetches the active by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
	 public	TaskCategory FetchActiveById(Guid id)
	 {
		 return _taskCategoryRepository.FetchActiveById(id);
	 }


	 internal static void SetupTaskCategory(IPrincipalResolver principalResolver, TaskCategory taskCategory)
	 {
		 var currentDateTime = DateTime.UtcNow;
		 if (!taskCategory.Id.HasValue)
		 {
			 taskCategory.Id = Guid.NewGuid();
			 taskCategory.CreatedById = principalResolver.UserId;
			 taskCategory.CreatedDateTime = currentDateTime;
		 }
		 taskCategory.UpdatedById = principalResolver.UserId;
		 taskCategory.UpdatedDateTime = currentDateTime;
	 }

	}
}
