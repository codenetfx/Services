using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// Validates task parent, children, and predecessors <see cref="Task"/> objects
	/// </summary>
	public class TaskParentChildPredecessorValidator : ITaskValidator
	{
		/// <summary>
		/// Validates the instance.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
		{
			var entityToValidate = taskValidationContext.Entity;
			var tasks = taskValidationContext.Project.Tasks;

			if (entityToValidate.ParentTaskNumber.HasValue && (tasks.All(x => x.TaskNumber != entityToValidate.ParentTaskNumber) || entityToValidate.ParentTaskNumber == entityToValidate.TaskNumber))
			{
				errors.Add(TaskValidationEnumDto.ParentTaskInvalid);
			}

			if (entityToValidate.ChildTaskNumbers.Any(childTaskNumber => tasks.All(x => x.TaskNumber != childTaskNumber)))
			{
				errors.Add(TaskValidationEnumDto.ChildTasksMissing);
			}

			if (entityToValidate.Predecessors.Any(predecessor => tasks.All(x => x.TaskNumber != predecessor.TaskNumber)))
			{
				errors.Add(TaskValidationEnumDto.PredecessorTasksMissing);
			}

			if (entityToValidate.ParentTaskNumber.HasValue)
			{
				if (entityToValidate.ChildTaskNumbers.Any(x => x == entityToValidate.ParentTaskNumber))
				{
					errors.Add(TaskValidationEnumDto.ParentTaskChild);
				}
			}

			foreach(var childTaskNumber in entityToValidate.ChildTaskNumbers)
			{
				var parent = entityToValidate;
				while (parent != null)
				{
					if (parent.TaskNumber == childTaskNumber)
					{
						errors.Add(TaskValidationEnumDto.ChildTaskParent);
						break;
					}
					parent = parent.Parent;
				}
			}
		}
	}
}