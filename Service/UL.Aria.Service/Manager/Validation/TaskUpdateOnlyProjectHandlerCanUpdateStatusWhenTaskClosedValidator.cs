using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Manager.Validation
{

	/// <summary>
	/// Class TaskDeletePreventDeletionValidator. This class cannot be inherited.
	/// </summary>    
    public sealed class TaskUpdateOnlyProjectHandlerCanUpdateStatusWhenTaskClosedValidator : ITaskValidator
	{
        private readonly IPrincipalResolver _principalResolver;
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskUpdateOnlyProjectHandlerCanUpdateStatusWhenTaskClosedValidator"/> class.
        /// </summary>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskUpdateOnlyProjectHandlerCanUpdateStatusWhenTaskClosedValidator(IPrincipalResolver principalResolver)
        {
            _principalResolver = principalResolver;
        }
        /// <summary>
        /// Validates the specified entity to validate.
        /// </summary>
        /// <param name="taskValidationContext">The task validation context.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
        {
            var project = taskValidationContext.Project;
            var originalTask = taskValidationContext.OriginalEntity;
            
            if (originalTask != null && (originalTask.Status == TaskStatusEnumDto.Canceled || originalTask.Status == TaskStatusEnumDto.Canceled) &&
                    !string.Equals(_principalResolver.Current.Identity.Name, project.ProjectHandler, StringComparison.OrdinalIgnoreCase))
            {
                errors.Add(TaskValidationEnumDto.ProjectHandlerCanUpdateStatusWhenClosed);
            }
        }
    }
}