namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     Field names for properties stored in our SQL repository, i.e. column names
    /// </summary>
    public sealed class SqlFieldNames
    {
        /// <summary>
        ///     The company name
        /// </summary>
        public const string IncomingOrderCompanyName = "CompanyName";

        /// <summary>
        ///     The date booked
        /// </summary>
        public const string IncomingOrderDateBooked = "DateBooked";


        /// <summary>
        ///     The company id
        /// </summary>
        public const string CompanyId = "id";

        /// <summary>
        ///     The company name
        /// </summary>
        public const string CompanyName = "name";

        /// <summary>
        /// The product attribute group name for Descriptive
        /// </summary>
        public const string ProductAttributeGroupDescriptive = "Descriptive";

        /// <summary>
        /// The product attribute group name for Additional Attributes
        /// </summary>
        public const string ProductAttributeGroupAdditional = "Additional Attributes";

        /// <summary>
        /// The project template name
        /// </summary>
        public const string ProjectTemplateName = "Name";

        /// <summary>
        /// The updated on
        /// </summary>
        public const string UpdatedOn = "UpdatedOn";

        /// <summary>
        /// The favorite search name
        /// </summary>
        public const string FavoriteSearchName = "Name";
        /// <summary>
        /// The updated dt
        /// </summary>
        public const string CreatedDateTime = "CreatedDateTime";

        /// <summary>
        /// The task template name
        /// </summary>
        public const string TaskTemplateName = "Name";

        /// <summary>
        /// The name
        /// </summary>
        public const string Name = "Name";

        /// <summary>
        /// The company external identifier
        /// </summary>
        public const string CompanyExternalId = "ExternalId";
    }
}