using System;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Validator for User business claim.
    /// </summary>
    public class UserBusinessClaimServiceValidator : IUserBusinessClaimServiceValidator
    {
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBusinessClaimServiceValidator"/> class.
        /// </summary>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public UserBusinessClaimServiceValidator(IMapperRegistry mapperRegistry)
        {
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Parses the search parameters.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <param name="loginId">The user id.</param>
        /// <param name="mappedDto">The mapped dto.</param>
        /// <param name="validatedLoginId"></param>
        /// <returns></returns>
        public bool TryParseSearchParameters(string claim, string loginId, out BusinessClaim mappedDto, out string validatedLoginId)
        {
            mappedDto = null;
            validatedLoginId = loginId;
            if (string.IsNullOrWhiteSpace(claim))
                return false;

            BusinessClaimDto claimDto = new BusinessClaimDto();
            claimDto.EntityClaim = claim;
            mappedDto = _mapperRegistry.Map<BusinessClaim>(claimDto);

            
            if (string.IsNullOrWhiteSpace(loginId))
                return false;
            return true;
        }
    }
}