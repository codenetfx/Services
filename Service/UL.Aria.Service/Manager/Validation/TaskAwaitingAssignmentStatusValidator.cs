using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Manager.Validation
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskAwaitingAssignmentStatusValidator : ITaskValidator
	{
		/// <summary>
		/// Validates the instance.
		/// </summary>
		/// <param name="taskValidationContext">The task validation context.</param>
		/// <param name="errors">The errors.</param>
		public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
		{
		    var isTaskOwnerBeingAssigned = (taskValidationContext.OriginalEntity == null || !taskValidationContext.OriginalEntity.HasTaskOwner)
		                                   && taskValidationContext.Entity.HasTaskOwner;

            //var isTaskOwnerBeingReassigned = taskValidationContext.OriginalEntity.HasTaskOwner
            //                                 && taskValidationContext.Entity.HasTaskOwner
            //                                 && taskValidationContext.OriginalEntity.TaskOwner != askValidationContext.Entity.TaskOwner;
		    
            if (!isTaskOwnerBeingAssigned )
		    {
                var entityToValidate = taskValidationContext.Entity;

                if (entityToValidate.Status == TaskStatusEnumDto.AwaitingAssignment && entityToValidate.HasTaskOwner)
                {
                    errors.Add(TaskValidationEnumDto.AwaitingAssignmentMustHaveNoOwner);
                }
		    }
                    
			
		}
	}
}