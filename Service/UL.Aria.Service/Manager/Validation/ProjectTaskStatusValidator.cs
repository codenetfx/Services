using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// 
	/// </summary>
	public class ProjectTaskStatusValidator : IProjectValidator
	{
		private readonly ITaskProvider _taskProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectTaskStatusValidator"/> class.
		/// </summary>
		/// <param name="taskProvider">The task provider.</param>
		public ProjectTaskStatusValidator(ITaskProvider taskProvider)
		{
			_taskProvider = taskProvider;
		}

		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <param name="originalEntity">The original entity.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(Project entityToValidate, Project originalEntity, List<ProjectValidationEnumDto> errors)
		{
			if (entityToValidate == null || entityToValidate.ProjectStatus != ProjectStatusEnumDto.Completed)
			{
				return;
			}

// ReSharper disable once PossibleInvalidOperationException
			var pendingTasks = _taskProvider.PendingTasks(entityToValidate.ContainerId.Value);

			if (pendingTasks)
			{
				errors.Add(ProjectValidationEnumDto.PendingTasks);
			}
		}
	}
}