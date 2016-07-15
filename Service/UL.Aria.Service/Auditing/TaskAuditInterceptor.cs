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
	/// Provides a DI interceptor for the the Task Repository Interface.
	/// </summary>
	public sealed class TaskAuditInterceptor : AuditInterceptionBehaviorEntityBase<Task, TaskDto>
	{
		private readonly IPrincipalResolver _principalResolver;
		private readonly IMapperRegistry _mapperRegistry;


		/// <summary>
		/// Initializes a new instance of the <see cref="TaskAuditInterceptor" /> class.
		/// </summary>
		/// <param name="historyProvider">The history provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public TaskAuditInterceptor(IHistoryProvider historyProvider,
			IPrincipalResolver principalResolver, IProfileManager profileManager, IMapperRegistry mapperRegistry)
			: base(historyProvider, principalResolver, profileManager)
		{
			_principalResolver = principalResolver;
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// When implemented in a derived class, returns a DataContract Serializable
		/// object to be stored as the audit details.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		protected override TaskDto ConvertToDto(Task entity)
		{
			var dto = _mapperRegistry.Map<TaskDto>(entity);
			// Should only be the case when BulkCreate happens and we need to refactor the data like it comes from the web to make the history identical
			if (dto.SubTasks.Count > 0)
			{
				foreach (var subTask in dto.SubTasks)
				{
					subTask.ParentTaskNumber = dto.TaskNumber;
					dto.ChildTaskNumbers.Add(subTask.TaskNumber);
				}
				dto.SubTasks.Clear();
			}
			dto.Created = entity.CreatedDateTime;
			dto.CreatedById = entity.CreatedById;
			dto.Modified = entity.UpdatedDateTime;
			dto.ModifiedBy = entity.UpdatedById.ToString();
			return dto;
		}

		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>T.</returns>
		protected override Task GetEntity(Guid entityId)
		{
			var task = new Task {Id = entityId};
			task.UpdatedDateTime = DateTime.UtcNow;
			task.UpdatedById = _principalResolver.UserId;
			task.IsDeleted = true;
			return task;
		}
	}
}