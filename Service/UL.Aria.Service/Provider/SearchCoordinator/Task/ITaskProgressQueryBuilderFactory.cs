using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider.SearchCoordinator.Task
{
	/// <summary>
	/// 
	/// </summary>
	public interface ITaskProgressQueryBuilderFactory
	{
		/// <summary>
		/// Gets the query builder.
		/// </summary>
		/// <param name="taskProgress">The task progress.</param>
		/// <returns></returns>
		IQueryBuilder GetQueryBuilder(string taskProgress);
	}
}