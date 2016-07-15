using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Class TaskDeleteProjectHandlerValidator. This class cannot be inherited.
	/// </summary>
	public sealed class TaskDeleteProjectHandlerValidator : ITaskDeleteValidator
	{
		private readonly IPrincipalResolver _principalResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskDeleteProjectHandlerValidator"/> class.
		/// </summary>
		/// <param name="principalResolver">The principal resolver.</param>
		public TaskDeleteProjectHandlerValidator(IPrincipalResolver principalResolver)
		{
			_principalResolver = principalResolver;
		}

		/// <summary>
		/// Validates the specified entity to validate.
		/// </summary>
		/// <param name="project">The project.</param>
		/// <param name="entityToValidate">The entity to validate.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(Project project, Task entityToValidate, List<TaskDeleteValidationEnumDto> errors)
		{
            var isProjectHandlerCurrentUser = string.Equals(project.ProjectHandler, _principalResolver.Current.Identity.Name, System.StringComparison.OrdinalIgnoreCase);
            if (!isProjectHandlerCurrentUser)
			{
				errors.Add(TaskDeleteValidationEnumDto.TaskProjectUserNotHandler);
			}
		}
	}
}