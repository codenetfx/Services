using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;


namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates the task is freeform if notification email addresses have been specified.
    /// </summary>
    public class TaskNotificationsRequireFreeformValidator : ITaskValidator
    {
        private readonly IServiceConfiguration _serviceConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotificationsRequireFreeformValidator"/> class.
        /// </summary>
        public TaskNotificationsRequireFreeformValidator(IServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="taskValidationContext">The task validation context.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(TaskValidationContext taskValidationContext, List<Contracts.Dto.TaskValidationEnumDto> errors)
        {
            if (taskValidationContext.Entity.Notifications.Any()
                && taskValidationContext.Entity.TaskTypeId != _serviceConfiguration.FreeformTaskTypeId)
            {
                errors.Add(TaskValidationEnumDto.NotificationsRequireFreeform);
            }
        }
    }
}
