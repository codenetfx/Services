using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Interface ITaskFetchStatusListStrategyFactory
	/// </summary>
	public interface ITaskFetchStatusListStrategyFactory
	{
		/// <summary>
		/// Gets the strategy.
		/// </summary>
		/// <param name="taskStatus">The task status.</param>
		/// <returns>ITaskFetchStatusListStrategy.</returns>
		ITaskFetchStatusListStrategy GetStrategy(TaskStatusEnumDto taskStatus);
	}
}