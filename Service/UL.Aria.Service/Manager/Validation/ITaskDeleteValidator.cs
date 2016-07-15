using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Interface ITaskDeleteValidator
	/// </summary>
	public interface ITaskDeleteValidator
	{
		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <param name="errors">The errors.</param>
		void Validate(Project project, Task entityToValidate, List<TaskDeleteValidationEnumDto> errors);
	}
}