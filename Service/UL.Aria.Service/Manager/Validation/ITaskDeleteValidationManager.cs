using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Interface ITaskDeleteValidationManager
	/// </summary>
	public interface ITaskDeleteValidationManager
	{
		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <returns>IList&lt;TaskDeleteValidationEnumDto&gt;.</returns>
		IList<TaskDeleteValidationEnumDto> Validate(Project project, Task entityToValidate);
	}
}