using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates task parent, children, and predecessors <see cref="Task"/> objects
    /// </summary>
    public class TaskSelfReferencingPredecessorNotAllowedValidator : ITaskValidator
    {
        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="taskValidationContext">The task validation context.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(TaskValidationContext taskValidationContext, List<TaskValidationEnumDto> errors)
        {
            var entityToValidate = taskValidationContext.Entity;           

            var violationExists = entityToValidate.Predecessors.Exists(x => x.TaskId == entityToValidate.Id || x.TaskNumber == entityToValidate.TaskNumber)
                || entityToValidate.SuccessorRefs.Exists(x => x.Id == entityToValidate.Id)
                || entityToValidate.PredecessorRefs.Exists(x => x.Id == entityToValidate.Id);
            
            if (violationExists)
            {
                errors.Add(TaskValidationEnumDto.SelfReferencingPredecessor);
            }
        }
    }
}