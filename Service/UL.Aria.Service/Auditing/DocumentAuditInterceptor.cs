using System;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// Class DocumentAuditInterceptor.
	/// </summary>
	public class DocumentAuditInterceptor : AuditInterceptionBehaviorEntityBase<Document, DocumentDto>
	{
		private readonly IPrincipalResolver _principalResolver;
		private readonly IMapperRegistry _mapperRegistry;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentAuditInterceptor"/> class.
		/// </summary>
		/// <param name="historyProvider">The history provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public DocumentAuditInterceptor(IHistoryProvider historyProvider,
			IPrincipalResolver principalResolver, IProfileManager profileManager, IMapperRegistry mapperRegistry)
			: base(historyProvider, principalResolver, profileManager)
		{
			_principalResolver = principalResolver;
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// when implemented in a derived class, returns a DataContract Serializable object to be stored as the audit details.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>DocumentDto.</returns>
		protected override DocumentDto ConvertToDto(Document entity)
		{
			var dto = _mapperRegistry.Map<DocumentDto>(entity);
			return dto;
		}

		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>T.</returns>
		protected override Document GetEntity(Guid entityId)
		{
			var document = new Document(entityId) {UpdatedDateTime = DateTime.UtcNow, UpdatedById = _principalResolver.UserId};
			return document;
		}
	}
}