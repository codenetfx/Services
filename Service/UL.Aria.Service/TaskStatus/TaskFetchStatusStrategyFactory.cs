using Microsoft.Practices.Unity;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusStrategyFactory. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusStrategyFactory : ITaskFetchStatusStrategyFactory
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskFetchStatusStrategyFactory"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public TaskFetchStatusStrategyFactory(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Gets the strategy.
		/// </summary>
		/// <param name="taskStatus">The task status.</param>
		/// <returns>ITaskFetchStatusStrategy.</returns>
		public ITaskFetchStatusStrategy GetStrategy(TaskStatusEnumDto taskStatus)
		{
			return _container.Resolve<ITaskFetchStatusStrategy>(taskStatus.ToString());
		}
	}
}