using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Relay.Common
{
    /// <summary>
    /// Implements <see cref="IPrincipalResolver"/> for non-http contexts.
    /// </summary>
    public class LocalPrincipalResolver : IPrincipalResolver
    {
        /// <summary>
        /// Gets or sets the current.
        /// </summary>
        /// <value>
        /// The current.
        /// </value>
        public ClaimsPrincipal Current
        {
            get { return Thread.CurrentPrincipal as ClaimsPrincipal; }
            set { Thread.CurrentPrincipal = value; }
        }

        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId
        {
            get
            {
                if (Current == null)
                    return Guid.Empty;
                System.Security.Claims.Claim single = Current.Claims.Single(i => i.Type == SecuredClaims.UserId);
                return new Guid(single.Value);
            }
        }
    }
}