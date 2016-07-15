using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Validates status of subtasks and predecessors <see cref="Task"/> objects
	/// </summary>
	public class TaskSubTaskPredecessorStatusValidator : ITaskValidator
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

			if (null == entityToValidate.Id)
			{
				return;
			}

			if (null != originalEntity && entityToValidate.Status == originalEntity.Status)
			{
				return;
			}

			if (entityToValidate.Status == TaskStatusEnumDto.InProgress ||
			    entityToValidate.Status == TaskStatusEnumDto.Completed)
			{
				if (
					entityToValidate.PredecessorRefs.Any(
						x => x.Status != TaskStatusEnumDto.Canceled && x.Status != TaskStatusEnumDto.Completed))
				{
					errors.Add(TaskValidationEnumDto.PredecessorsStillIncomplete);
				}
			}

			if (entityToValidate.Status == TaskStatusEnumDto.Completed)
			{
				VerifyChildrenComplete(entityToValidate, errors);
			}
		}

		internal void VerifyChildrenComplete(Task task, List<TaskValidationEnumDto> errors)
		{
			foreach (var childTask in task.SubTasks)
			{
				if (childTask.Status != TaskStatusEnumDto.Canceled && childTask.Status != TaskStatusEnumDto.Completed)
				{
					errors.Add(TaskValidationEnumDto.ChildrenStillIncomplete);
					break;
				}

				VerifyChildrenComplete(childTask, errors);

				if (errors.Any(x => x == TaskValidationEnumDto.ChildrenStillIncomplete))
				{
					break;
				}
			}
		}
	}
}