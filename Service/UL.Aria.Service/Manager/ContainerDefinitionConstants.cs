using UL.Aria.Common.Authorization;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines constants for Container
    /// </summary>
    public class ContainerDefinitionConstants
    {
        /// <summary>
        /// The default claim type
        /// </summary>
        public const string DefaultClaimType = "http://aria.ul.com/CompoundClaim";
        /// <summary>
        /// The default claim value type
        /// </summary>
        public const string DefaultClaimValueType = "String";
        /// <summary>
        /// The permission contributor
        /// </summary>
        public const string PermissionContributor = SecuredPermissions.PermissionContributor ;
        /// <summary>
        /// The permission reader
        /// </summary>
        public const string PermissionReader = SecuredPermissions.PermissionReader;
        /// <summary>
        /// The private group
        /// </summary>
        public const string PrivateGroup = "PrivateGroup";
        /// <summary>
        /// The read only group
        /// </summary>
        public const string ReadOnlyGroup = "ReadOnlyGroup";
        /// <summary>
        /// The modify group
        /// </summary>
        public const string ModifyGroup = "ModifyGroup";
        /// <summary>
        /// The read only prefix
        /// </summary>
        public const string ReadOnlyPrefix = "r_";
        /// <summary>
        /// The read write prefix
        /// </summary>
        public const string ReadWritePrefix = "rw_";
        /// <summary>
        /// The ul employee
        /// </summary>
        public const string UlEmployee = "ul_employee";
    }
}