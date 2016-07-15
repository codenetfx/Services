using System.Security.Claims;

namespace UL.Aria.Service.Security
{
    /// <summary>
    /// Defines the base implementation for a claims authorization manager.
    /// </summary>
    public class CustomClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        /// <summary>
        /// When implemented in a derived class, checks authorization for the subject in the specified context to perform the specified action on the specified resource.
        /// </summary>
        /// <param name="context">The authorization context that contains the subject, resource, and action for which authorization is to be checked.</param>
        /// <returns>
        /// true if the subject is authorized to perform the specified action on the specified resource; otherwise, false.
        /// </returns>
        public override bool CheckAccess(AuthorizationContext context)
        {

            // TODO: claims here will not be populated yet using the framework ... until we do the real implementation
            // The stop gap measure of using a IDispatchMessageInspector to bring claims
            // to the services side happens after this CheckAccess call
            return base.CheckAccess(context);
        }
    }
}