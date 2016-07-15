using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;


namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    ///     Defines operations to Validate <see cref="Domain.Entity.Product" /> objects for a specific rule.
    /// </summary>
    public interface ITaskValidator
    {
		/// <summary>
		/// Validates the instance.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <param name="errors">The errors.</param>
		void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors);
    }
}
