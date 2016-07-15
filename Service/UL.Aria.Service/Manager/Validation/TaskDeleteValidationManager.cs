using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Class TaskDeleteValidationManager. This class cannot be inherited.
	/// </summary>
	public sealed class TaskDeleteValidationManager : ITaskDeleteValidationManager
	{
		private readonly IEnumerable<ITaskDeleteValidator> _taskDeleteValidators;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDeleteValidationManager"/> class.
		/// </summary>
		/// <param name="taskDeleteValidators">The task delete validators.</param>
		public TaskDeleteValidationManager(IEnumerable<ITaskDeleteValidator> taskDeleteValidators)
		{
			_taskDeleteValidators = taskDeleteValidators;
		}

		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <returns>IList&lt;TaskDeleteValidationEnumDto&gt;.</returns>
		public IList<TaskDeleteValidationEnumDto> Validate(Project project, Task entityToValidate)
		{
			var errors = new List<TaskDeleteValidationEnumDto>();

			foreach (var validator in _taskDeleteValidators)
			{
				validator.Validate(project, entityToValidate, errors);
			}

			return errors;
		}
	}
}