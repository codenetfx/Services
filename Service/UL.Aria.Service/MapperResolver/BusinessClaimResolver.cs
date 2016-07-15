using System;
using AutoMapper;
using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.MapperResolver
{
    /// <summary>
    ///     Business Claim Resolver
    /// </summary>
    public class BusinessClaimResolver : ValueResolver<UserClaimDto, BusinessClaim>
    {
        /// <summary>
        ///     Resolves the Business Claim.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override BusinessClaim ResolveCore(UserClaimDto source)
        {
            Guid value;
            var businessClaim = new BusinessClaim
                {
                    EntityClaim = source.ClaimId.ToString()
                };

            if (Guid.TryParse(source.ClaimValue, out value))
            {
                businessClaim.EntityId = value;
            }

            businessClaim.Value = source.ClaimValue;

            return businessClaim;
        }
    }
}