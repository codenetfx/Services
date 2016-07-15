using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;


namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// It will validate if any child task is prevent from delete
	/// </summary>
	public class TaskDeleteChildTaskPreventDeleteValidator : ITaskDeleteValidator
	{

		/// <summary>
		/// Validates the specified project.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(Project project, Task entityToValidate, List<TaskDeleteValidationEnumDto> errors)
		{
			if (entityToValidate.SubTasks.Any(x =>  x.PreventDeletion))
			{
				errors.Add(TaskDeleteValidationEnumDto.TaskChildrenPreventDeletion);
			}
		}

	}
}
