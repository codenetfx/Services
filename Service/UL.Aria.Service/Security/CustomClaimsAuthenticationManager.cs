using System.Security.Claims;

namespace UL.Aria.Service.Security
{
    /// <summary>
    /// Defines the base implementation for a claims authentication manager. The claims authentication manager provides a place in the claims processing pipeline for applying processing logic (filtering, validation, extension) to the claims collection in the incoming principal before execution reaches your application code.
    /// </summary>
    public class CustomClaimsAuthenticationManager : ClaimsAuthenticationManager
    {

        /// <summary>
        /// When overridden in a derived class, returns a <see cref="T:System.Security.Claims.ClaimsPrincipal" /> object consistent with the requirements of the RP application. The default implementation does not modify the incoming <see cref="T:System.Security.Claims.ClaimsPrincipal" />.
        /// </summary>
        /// <param name="resourceName">The address of the resource that is being requested.</param>
        /// <param name="incomingPrincipal">The claims principal that represents the authenticated user that is attempting to access the resource.</param>
        /// <returns>
        /// A claims principal that contains any modifications necessary for the RP application. The default implementation returns the incoming claims principal unmodified.
        /// </returns>
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            // TODO: claims here will not be populated yet ... until we do the real implementation
            // The stop gap measure of using a IDispatchMessageInspector to bring claims
            // to the services side happens after this call

            return base.Authenticate(resourceName, incomingPrincipal);
        }
    }
}