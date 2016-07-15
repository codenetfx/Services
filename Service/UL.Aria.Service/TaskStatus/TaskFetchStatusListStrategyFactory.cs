using Microsoft.Practices.Unity;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.TaskStatus
{
	/// <summary>
	/// Class TaskFetchStatusListStrategyFactory. This class cannot be inherited.
	/// </summary>
	public sealed class TaskFetchStatusListStrategyFactory : ITaskFetchStatusListStrategyFactory
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskFetchStatusListStrategyFactory"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public TaskFetchStatusListStrategyFactory(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Gets the strategy.
		/// </summary>
		/// <param name="taskStatus">The task status.</param>
		/// <returns>ITaskFetchStatusListStrategy.</returns>
		public ITaskFetchStatusListStrategy GetStrategy(TaskStatusEnumDto taskStatus)
		{
			return _container.Resolve<ITaskFetchStatusListStrategy>(taskStatus.ToString());
		}
	}
}