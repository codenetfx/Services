using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// Class AssetLinkAuditInterceptor.
	/// </summary>
	public sealed class AssetLinkAuditInterceptor : AuditInterceptionBehaviorBase<AssetLink, AssetLinkDto>
	{
		private readonly IMapperRegistry _mapperRegistry;


		/// <summary>
		/// Initializes a new instance of the <see cref="AssetLinkAuditInterceptor"/> class.
		/// </summary>
		/// <param name="historyProvider">The history provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public AssetLinkAuditInterceptor(IHistoryProvider historyProvider,
			IPrincipalResolver principalResolver, IProfileManager profileManager, IMapperRegistry mapperRegistry)
			: base(historyProvider, principalResolver, profileManager)
		{
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// when implemented in a derived class, returns a DataContract Serializable object to be stored as the audit details.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>AssetLinkDto.</returns>
		protected override AssetLinkDto ConvertToDto(AssetLink entity)
		{
			return _mapperRegistry.Map<AssetLinkDto>(entity);
		}

// ReSharper disable once InconsistentNaming
		private static readonly IDictionary<string, Type> _entityTypeConversions = new Dictionary<string, Type>
		{
			{EntityTypeEnumDto.Task.ToString(), typeof(TaskDto)},
			{EntityTypeEnumDto.Project.ToString(), typeof(ProjectDto)}
		};

		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		/// <param name="dto">The dto.</param>
		/// <returns>System.String.</returns>
		protected override string GetEntityType(AssetLinkDto dto)
		{
			return _entityTypeConversions[dto.ParentAssetType].GetAssemblyQualifiedTypeName();
		}

		/// <summary>
		/// Gets the entity identifier.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>Guid.</returns>
		protected override Guid GetEntityId(AssetLink entity)
		{
			return entity.ParentAssetId;
		}
	}
}