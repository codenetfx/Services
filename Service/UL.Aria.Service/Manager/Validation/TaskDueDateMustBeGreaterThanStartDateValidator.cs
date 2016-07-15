using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{

    /// <summary>
    /// Validates that a tasks start date is less than the due date
    /// </summary>
    public class TaskDueDateMustBeGreaterThanStartDateValidator : ITaskValidator
	{
		/// <summary>
		/// Validates the instance.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
		{
			var entityToValidate = taskValidationContext.Entity;
            
            if (null != entityToValidate && entityToValidate.StartDate.HasValue && entityToValidate.DueDate.HasValue)
		    {
                if (entityToValidate.DueDate.Value.Date < entityToValidate.StartDate.Value.Date)
		        {
                    errors.Add(TaskValidationEnumDto.DueDateMustBeGreaterThanStartDate);
		            
		        }
		    }

		}
	}
}