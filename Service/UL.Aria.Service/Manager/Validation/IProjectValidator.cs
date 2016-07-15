using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using Task = System.Threading.Tasks.Task;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectValidator
    {
        /// <summary>
        /// Validates the specified entity to validate.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <param name="originalEntity">The original entity.</param>
        /// <param name="errors">The errors.</param>
        void Validate(Project entityToValidate, Project originalEntity, List<ProjectValidationEnumDto> errors);
    }
    
}
