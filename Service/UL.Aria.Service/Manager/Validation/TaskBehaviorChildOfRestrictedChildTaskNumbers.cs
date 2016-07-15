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
    /// Validates the child task of a parent with Children Restricted Behavior.
    /// </summary>
    public class TaskBehaviorChildOfRestrictedChildTaskNumbers : ITaskValidator
    {
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskBehaviorChildOfRestrictedChildTaskNumbers"/> class.
        /// </summary>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskBehaviorChildOfRestrictedChildTaskNumbers(IPrincipalResolver principalResolver)
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

            if (isProjectHandler)
            {
                return;
            }

            if (null == orginalEntity || !orginalEntity.IsParentTaskNumberRestrictedByRelationship)
            {
                return;
            }

            if (orginalEntity.ParentId != entityToValidate.ParentId)
            {
                errors.Add(TaskValidationEnumDto.ParentTaskNumberRestrictedToProjecthandler);
            }
        }


    }
}
