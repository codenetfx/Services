using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Performance.Managers;

namespace UL.Aria.Service.Performance.Factory
{
	/// <summary>
	/// Interface IPerformanceManagerFactory
	/// </summary>
	public interface IPerformanceManagerFactory
	{
		/// <summary>
		/// Gets the manager.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <returns></returns>
		IPerformanceManager GetManager(EntityTypeEnumDto entityType);
	}
}