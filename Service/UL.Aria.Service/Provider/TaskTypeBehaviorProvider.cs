using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	/// Implements a provider for <see cref="TaskTypeBehavior"/> objects.
	/// </summary>
	public class TaskTypeBehaviorProvider : SearchProviderBase<TaskTypeBehavior>, ITaskTypeBehaviorProvider
	{
		private readonly ITaskTypeBehaviorRepository _repository;
		private readonly IPrincipalResolver _principalResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskTypeBehaviorProvider"/> class.
		/// </summary>
		/// <param name="repository">The repository.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		public TaskTypeBehaviorProvider(ITaskTypeBehaviorRepository repository, IPrincipalResolver principalResolver)
			: base(repository, principalResolver)
		{
			_repository = repository;
			_principalResolver = principalResolver;
		}

		/// <summary>
		/// Finds the by task type identifier.
		/// </summary>
		/// <param name="taskTypeId">The task type identifier.</param>
		/// <returns></returns>
		public IEnumerable<TaskTypeBehavior> FindByTaskTypeId(Guid taskTypeId)
		{
			return _repository.FindByTaskTypeId(taskTypeId);
		}

		/// <summary>
		/// Saves the specified task type behaviors.
		/// </summary>
		/// <param name="taskTypeBehaviors">The task type behaviors.</param>
		/// <param name="taskTypeId">The value.</param>
		public void Save(IEnumerable<TaskTypeBehavior> taskTypeBehaviors, Guid taskTypeId)
		{
			taskTypeBehaviors.ForEach(x =>
			{
				x.TaskTypeId = taskTypeId;
				x.UpdatedById = _principalResolver.UserId;
				x.UpdatedDateTime = DateTime.UtcNow;
				if (null == x.Id)
				{
					x.CreatedById = _principalResolver.UserId;
					x.CreatedDateTime = DateTime.UtcNow;
					x.Id = Guid.NewGuid();
				}
			});
			_repository.Save(taskTypeBehaviors, (db, cmd) =>
			{
				db.AddInParameter(cmd, "TaskTypeId", DbType.Guid, taskTypeId);
			});
		}

		/// <summary>
		/// Fetches the by multiple task type ids.
		/// </summary>
		/// <param name="taskTypeIds">The task type ids.</param>
		/// <returns></returns>
		public IDictionary<Guid, IEnumerable<TaskTypeBehavior>> FetchByMultipleTaskTypeIds(IEnumerable<Guid> taskTypeIds)
		{
			return taskTypeIds.ToDictionary(taskTypeId => taskTypeId, taskTypeId => _repository.FindByTaskTypeId(taskTypeId));
		}
	}
}