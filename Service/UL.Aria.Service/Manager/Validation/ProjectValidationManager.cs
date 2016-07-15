using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectValidationManager : IProjectValidationManager
    {
        private readonly IEnumerable<IProjectValidator> _projectValidators;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectValidationManager"/> class.
        /// </summary>
        /// <param name="projectValidators">The project validates.</param>
        public ProjectValidationManager(IEnumerable<IProjectValidator> projectValidators)
        {
            _projectValidators = projectValidators;

        }
        /// <summary>
        /// Validates the specified entity to validate.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <param name="originalEntity">The original entity.</param>
        /// <returns></returns>
        public IList<Contracts.Dto.ProjectValidationEnumDto> Validate(Domain.Entity.Project entityToValidate, Domain.Entity.Project originalEntity)
        {
            var errors = new List<ProjectValidationEnumDto>();
            foreach (var validator in _projectValidators)
            {
                validator.Validate(entityToValidate, originalEntity, errors);
            }
            return errors;
        }
    }
}
