using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectValidationManager
    {

        /// <summary>
        /// Validates the specified entity to validate.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <param name="originalEntity">The original entity.</param>
        /// <returns></returns>
        IList<ProjectValidationEnumDto> Validate(Project entityToValidate, Project originalEntity);
    }
}
