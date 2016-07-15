using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Security
{
    /// <summary>
    ///     Methods to get to current principal.
    /// </summary>
    public class PrincipalResolver : IPrincipalResolver
    {
        /// <summary>
        ///     Gets or sets the current.
        /// </summary>
        /// <value>
        ///     The current.
        /// </value>
        public ClaimsPrincipal Current
        {
            get
            {
                if (null == HttpContext.Current)
                    return null;
                return HttpContext.Current.User as ClaimsPrincipal;
            }
            set
            {
                HttpContext.Current.User = value;
            }
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