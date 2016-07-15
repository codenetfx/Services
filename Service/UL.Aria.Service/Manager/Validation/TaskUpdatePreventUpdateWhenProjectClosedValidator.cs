using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Class TaskDeletePreventDeletionValidator. This class cannot be inherited.
	/// </summary>
    public sealed class TaskUpdatePreventUpdateWhenProjectClosedValidator : ITaskValidator
	{
        /// <summary>
        /// Validates the specified entity to validate.
        /// </summary>
        /// <param name="taskValidationContext">The task validation context.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
        {
            var project = taskValidationContext.Project;
            if (project.IsClosed)
            {
                errors.Add(TaskValidationEnumDto.TaskClosedBecauseProjectClosed);
            }
        }
    }
}