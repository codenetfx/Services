using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    ///     Defines operations to validate <see cref="Task" /> objects.
    /// </summary>
    public interface ITaskValidationManager
    {
		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <returns>IList&lt;TaskValidationEnumDto&gt;.</returns>
        IList<TaskValidationEnumDto> Validate(TaskValidationContext taskValidationContext);

		/// <summary>
		/// Validates the specified entities to validate.
		/// </summary>
		/// <param name="entitiesToValidate">The entities to validate.</param>
		/// <param name="project">The project.</param>
		/// <returns>Dictionary&lt;Guid, IList&lt;TaskValidationEnumDto&gt;&gt;.</returns>
	    Dictionary<Guid, IList<TaskValidationEnumDto>> Validate(IList<Task> entitiesToValidate, Project project);

    }
}
