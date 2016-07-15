using System;
using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for assigning claims upon document creation/update.
    /// </summary>
    public class ProductClaimAssignmentManager : IProductClaimAssignmentManager
    {
        private readonly IUserBusinessClaimService _claimProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductClaimAssignmentManager"/> class.
        /// </summary>
        /// <param name="claimProvider">The claim provider.</param>
        public ProductClaimAssignmentManager(IUserBusinessClaimService claimProvider)
        {
            _claimProvider = claimProvider;
            
        }

        /// <summary>
        /// Assigns claims for the specified upload and container.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <param name="containerId">The container id.</param>
        /// <param name="profile"></param>
        public void AssignClaim(ProductUpload productUpload, Guid containerId, ProfileDto profile)
        {
            if (null != profile)
            {
                _claimProvider.Add(new UserBusinessClaimDto {Claim = new BusinessClaimDto {EntityClaim = SecuredClaims.ContainerEdit, EntityId = containerId, Value = containerId.ToString()}, Id = Guid.NewGuid(), LoginId = profile.LoginId, UserId = productUpload.CreatedById});
            }
        }
    }
}