using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Interface ITaskFetchStatusStrategyFactory
	/// </summary>
	public interface ITaskFetchStatusStrategyFactory
	{
		/// <summary>
		/// Gets the strategy.
		/// </summary>
		/// <param name="taskStatus">The task status.</param>
		/// <returns>ITaskFetchStatusStrategy.</returns>
		ITaskFetchStatusStrategy GetStrategy(TaskStatusEnumDto taskStatus);
	}
}