using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskCategoryManager : ITaskCategoryManager
	{
		private readonly ITaskCategoryProvider _taskCategoryProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskCategoryManager"/> class.
		/// </summary>
		/// <param name="taskCategoryProvider">The task template provider.</param>
		public TaskCategoryManager(ITaskCategoryProvider taskCategoryProvider)
		{
			_taskCategoryProvider = taskCategoryProvider;
		}
		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public Domain.Search.TaskCategorySearchResultSet Search(Domain.Search.SearchCriteria searchCriteria)
		{
			return _taskCategoryProvider.Search(searchCriteria);
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public Domain.Entity.TaskCategory FetchById(Guid id)
		{
			return _taskCategoryProvider.FetchById(id);
		}

		/// <summary>
		/// Creates the specified task template.
		/// </summary>
		/// <param name="taskTemplate">The task template.</param>
		/// <returns></returns>
		public Domain.Entity.TaskCategory Create(Domain.Entity.TaskCategory taskTemplate)
		{
			return _taskCategoryProvider.Create(taskTemplate);
		}

		/// <summary>
		/// Updates the specified task template.
		/// </summary>
		/// <param name="taskTemplate">The task template.</param>
		/// <returns></returns>
		public Domain.Entity.TaskCategory Update(Domain.Entity.TaskCategory taskTemplate)
		{
			return _taskCategoryProvider.Update(taskTemplate);
		}

		/// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete(Guid id)
		{
			if (_taskCategoryProvider.FetchActiveById(id) != null)
				throw new InvalidOperationException("Task Category cannot be deleted it is in use.");


			_taskCategoryProvider.Delete(id);
		}

		/// <summary>
		/// Fetches all count.
		/// </summary>
		/// <returns></returns>
		public int FetchAllCount()
		{
			return _taskCategoryProvider.FetchAllCount();
		}
	}
}
