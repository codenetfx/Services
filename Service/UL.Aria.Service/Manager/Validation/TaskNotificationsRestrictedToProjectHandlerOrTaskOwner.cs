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
    /// Validates the current user is the project handler or task owner if adding/editing notificatons
    /// </summary>
    public class TaskNotificationsRestrictedToProjectHandlerOrTaskOwner : ITaskValidator
    {
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskBehaviorChildOfRestrictedChildTaskNumbers"/> class.
        /// </summary>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskNotificationsRestrictedToProjectHandlerOrTaskOwner(IPrincipalResolver principalResolver)
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
            var isTaskOwner = string.Equals(_principalResolver.Current.Identity.Name,
                entityToValidate.TaskOwner, StringComparison.OrdinalIgnoreCase) 
                || string.Equals("AssignedToMe",
                entityToValidate.TaskOwner, StringComparison.OrdinalIgnoreCase);

            if (null == orginalEntity|| isProjectHandler || isTaskOwner)
            {
                return;
            }
            var changeCnt = entityToValidate.Notifications.Intersect(orginalEntity.Notifications).Count();
            var changed = changeCnt > 0 || changeCnt != orginalEntity.Notifications.Count();
            if (changed)
            {
                errors.Add(TaskValidationEnumDto.TaskNotificationChange);
            }
        }


    }
}
