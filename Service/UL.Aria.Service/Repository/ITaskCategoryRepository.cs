using System;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITaskCategoryRepository : IRepositoryBase<TaskCategory>
	{
		/// <summary>
		/// Searches the specified search criteria.
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		TaskCategorySearchResultSet Search(SearchCriteria searchCriteria);

		/// <summary>
		/// Fetches all count.
		/// </summary>
		/// <returns></returns>
		int FetchAllCount();

		/// <summary>
		/// Fetches the active by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		TaskCategory FetchActiveById(Guid id);


	}
}
