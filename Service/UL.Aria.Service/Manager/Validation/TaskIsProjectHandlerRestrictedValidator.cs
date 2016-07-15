using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskIsProjectHandlerRestrictedValidator : ITaskValidator
	{
		private readonly IPrincipalResolver _principalResolver;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskIsProjectHandlerRestrictedValidator" /> class.
		/// </summary>
		/// <param name="principalResolver">The principal resolver.</param>
		public TaskIsProjectHandlerRestrictedValidator(IPrincipalResolver principalResolver)
		{
			_principalResolver = principalResolver;
		}

		/// <summary>
		/// Validates the instance.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <param name="errors">The errors.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
		{
			var entityToValidate = taskValidationContext.Entity;

			if (entityToValidate.IsProjectHandlerRestricted)
			{
				var project = taskValidationContext.Project;
				var originalEntity = taskValidationContext.OriginalEntity;
				var isTaskOwnerChanged = originalEntity == null && !string.IsNullOrEmpty(entityToValidate.TaskOwner) ||
				                         originalEntity != null && originalEntity.TaskOwner != entityToValidate.TaskOwner;
                var isProjectHandlerCurrentUser = string.Equals(project.ProjectHandler, _principalResolver.Current.Identity.Name, System.StringComparison.OrdinalIgnoreCase);

                if (isTaskOwnerChanged && !isProjectHandlerCurrentUser)
				{
					errors.Add(TaskValidationEnumDto.IsProjectHandlerRestricted);
				}
			}
		}
	}
}