using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Class TaskDeleteProjectStatusValidator. This class cannot be inherited.
	/// </summary>
	public sealed class TaskDeleteProjectStatusValidator : ITaskDeleteValidator
	{
		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(Project project, Task entityToValidate, List<TaskDeleteValidationEnumDto> errors)
		{
			if ((project.ProjectStatus == ProjectStatusEnumDto.Completed ||
				 project.ProjectStatus == ProjectStatusEnumDto.Canceled))
			{
				errors.Add(TaskDeleteValidationEnumDto.TaskProjectCanceledCompleted);
			}
		}
	}
}
