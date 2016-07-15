using System;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for assigning claims upon document creation/update.
    /// </summary>
    public interface IProductClaimAssignmentManager
    {
        /// <summary>
        /// Assigns claims for the specified upload and container.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <param name="containerId">The container id.</param>
        /// <param name="profile"></param>
        void AssignClaim(ProductUpload productUpload, Guid containerId, ProfileDto profile);
    }
}