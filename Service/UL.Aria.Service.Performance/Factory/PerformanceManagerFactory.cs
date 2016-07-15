using Microsoft.Practices.Unity;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Performance.Managers;

namespace UL.Aria.Service.Performance.Factory
{
	/// <summary>
	/// 
	/// </summary>
	public class PerformanceManagerFactory : IPerformanceManagerFactory
	{
		private readonly IUnityContainer _container;

		/// <summary>
		/// Initializes a new instance of the <see cref="PerformanceManagerFactory"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public PerformanceManagerFactory(IUnityContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// Gets the manager.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns></returns>
		public IPerformanceManager GetManager(EntityTypeEnumDto entityType)
		{
			return _container.Resolve<IPerformanceManager>(entityType.ToString());
		}
	}
}