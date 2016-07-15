using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Repository;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Common base for Managers
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ManagerBase<T> : IManagerBase<T>
        where T : AuditableEntity, IAuditableEntity, ISearchResult, new()
    {
        private readonly IProviderBase<T> _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ManagerBase{T}" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        protected ManagerBase(IProviderBase<T> provider)
        {
            _provider = provider;
        }    

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///     IndustryCode
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual T Fetch(Guid id)
        {
            return _provider.Fetch(id);
        }

        /// <summary>
        ///     Creates the specified industry code.
        /// </summary>
        /// <param name="entity">The industry code.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual Guid Create(T entity)
        {
            ValidateForSaveAndThrowIfInvalid(entity);
            _provider.Create(entity);
            return entity.Id.Value;
        }

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The unit of measure.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Update(Guid id, T entity)
        {
            ValidateForSaveAndThrowIfInvalid(entity);
            _provider.Update(id, entity);
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual void Delete(Guid id)
        {
            _provider.Delete(id);
        }

        private void ValidateForSaveAndThrowIfInvalid(T entity)
        {
            var validationViolations = ValidateForSave(entity);
            if (validationViolations.Any())
            {
                var exception = ValidationViolationsToArgumentException(validationViolations);
                throw exception;
            }
        }

        private static ArgumentException ValidationViolationsToArgumentException(
            IEnumerable<ValidationViolationDto> validationViolations)
        {
            var exception = new ArgumentException("The entity was not valid.", "entity");
            foreach (var validationViolation in validationViolations)
            {
                exception.Data.Add(validationViolation.Code, validationViolation.ToJson());
            }
            return exception;
        }

        /// <summary>
        ///     When overridden in derived classes, validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A list of validations violations. If empty, successful. Should not return null.</returns>
        // internal for testing
        protected internal virtual IEnumerable<ValidationViolationDto> ValidateForSave(T entity)
        {
            return new List<ValidationViolationDto>();
        }

        /// <summary>
        /// Validates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationViolationDto> Validate(T entity)
        {
            return ValidateForSave(entity);
        }
    
    }
}