using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// Provides a DI interceptor for the the Project Manager Interface.
	/// </summary>
	public sealed class ProjectAuditInterceptor : AuditInterceptionBehaviorEntityBase<Project, ProjectDto>
	{
		private readonly IMapperRegistry _mapperRegistry;


		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectAuditInterceptor" /> class.
		/// </summary>
		/// <param name="historyProvider">The history provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public ProjectAuditInterceptor(IHistoryProvider historyProvider,
			IPrincipalResolver principalResolver, IProfileManager profileManager, IMapperRegistry mapperRegistry)
			: base(historyProvider, principalResolver, profileManager)
		{
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// When implemented in a derived class, returns a DataContract Serializable
		/// object to be stored as the audit details.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		protected override ProjectDto ConvertToDto(Project entity)
		{
			//todo: replace with strategy
			var dto = _mapperRegistry.Map<ProjectDto>(entity);
			dto.OriginalXmlParsed = null;
			return dto;
		}
	}
}