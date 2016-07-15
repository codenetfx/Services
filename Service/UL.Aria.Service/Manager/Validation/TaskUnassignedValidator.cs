using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Validation required <see cref="ProductCharacteristic"/> objects on <see cref="Product"/> objects
	/// </summary>
	public class TaskUnassignedValidator : ITaskValidator
	{
		/// <summary>
		/// Validates the instance.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
		{
			var entityToValidate = taskValidationContext.Entity;
			var originalEntity = taskValidationContext.OriginalEntity;

			if (entityToValidate.Status != TaskStatusEnumDto.Completed)
			{
				return;
			}

			if (null != originalEntity && entityToValidate.Status == originalEntity.Status)
			{
				return;
			}

			if (entityToValidate.Status == TaskStatusEnumDto.Completed)
			{
				if (string.IsNullOrWhiteSpace(entityToValidate.TaskOwner) ||
				    entityToValidate.TaskOwner.ToLowerInvariant() == "unassigned")
				{
					errors.Add(TaskValidationEnumDto.TaskStillUnassigned);
				}
			}
		}
	}
}