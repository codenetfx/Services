using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITaskCategoryManager
	{

		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		TaskCategorySearchResultSet Search(SearchCriteria searchCriteria);

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		TaskCategory FetchById(Guid id);

		/// <summary>
		/// Creates the specified task template.
		/// </summary>
		/// <param name="taskCategory">The task template.</param>
		/// <returns></returns>
		TaskCategory Create(TaskCategory taskCategory);

		/// <summary>
		/// Updates the specified task template.
		/// </summary>
		/// <param name="taskCategory">The task template.</param>
		/// <returns></returns>
		TaskCategory Update(TaskCategory taskCategory);

		/// <summary>
		/// Deletes the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		void Delete(Guid id);

		/// <summary>
		/// Fetches all count.
		/// </summary>
		/// <returns></returns>
		int FetchAllCount();
	}
}
