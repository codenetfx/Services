using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates the parent task of any subtasks with parent Restricted Behavior.
    /// </summary>
    public class TaskBehaviorParentOfRestrictedParentNumbers : ITaskValidator
    {
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskBehaviorParentOfRestrictedParentNumbers"/> class.
        /// </summary>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskBehaviorParentOfRestrictedParentNumbers(IPrincipalResolver principalResolver)
        {
            _principalResolver = principalResolver;
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="taskValidationContext">The task validation context.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(TaskValidationContext taskValidationContext, List<Contracts.Dto.TaskValidationEnumDto> errors)
        {
            var orginalEntity = taskValidationContext.OriginalEntity;
            var entityToValidate = taskValidationContext.Entity;
            var isProjectHandler = string.Equals(_principalResolver.Current.Identity.Name,
                   taskValidationContext.Project.ProjectHandler, StringComparison.OrdinalIgnoreCase);

            if (isProjectHandler || null == orginalEntity || !orginalEntity.IsChildTaskNumbersRestrictedByRelationship)
            {
                return;
            }
           
            var changed = entityToValidate.ChildTaskNumbers.Intersect(orginalEntity.ChildTaskNumbers).Count() != orginalEntity.ChildTaskNumbers.Count();
            if (changed)
            {
                errors.Add(TaskValidationEnumDto.ChildTaskNumbersRestrictedToProjecthandler);
            }

        }
    }
}
