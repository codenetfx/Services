using System;
using System.Collections.Generic;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Validator
{
    /// <summary>
    /// Favorite search validator class.
    /// </summary>
    public class FavoriteSearchValidator : ValidatorBase<FavoriteSearch>
    {
        private readonly IPrincipalResolver _principalResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteSearchValidator"/> class.
        /// </summary>
        /// <param name="principalResolver">The principal resolver.</param>
        public FavoriteSearchValidator(IPrincipalResolver principalResolver)
        {
            _principalResolver = principalResolver;
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="entityToValidate">The entity to validate.</param>
        /// <param name="errors">The errors.</param>
        protected override void ValidateInstance(FavoriteSearch entityToValidate, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(entityToValidate.Name))
            {
                errors.Add("Must specify a name.");
            }
            
            if (string.IsNullOrWhiteSpace(entityToValidate.Location))
            {
                errors.Add("Must specify a location.");
            }
            
            if (null == entityToValidate.SearchCriteria)
            {
                errors.Add("Must specify a search criteria.");
            }

            if (!entityToValidate.CreatedById.Equals(_principalResolver.UserId))
            {
                errors.Add("CreatedById does not match current user.");
            }

            if (entityToValidate.CreatedDateTime.Equals(DateTime.MinValue))
            {
                errors.Add("CreatedDateTime not set.");                    
            }

            if (entityToValidate.UserId.Equals(Guid.Empty))
            {
                errors.Add("UserId not set.");                    
            }
            

            if (!entityToValidate.UpdatedById.Equals(Guid.Empty))
            {
                if(!entityToValidate.UpdatedById.Equals(_principalResolver.UserId))
                {
                    errors.Add("UpdatedById does not match current user.");
                }

                if (entityToValidate.UpdatedDateTime.Equals(DateTime.MinValue))
                {
                    errors.Add("UpdatedDateTime not set.");                    
                }
            }
        }
    }
}