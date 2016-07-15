namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// A list of validation codes used by the application.
    /// </summary>
    public static class ValidationCodes
    {

        /// <summary>
        /// A list of validation codes used by the application for <see cref="BusinessUnitDto"/> entities.
        /// </summary>
        public static class BusinessUnit
        {
            /// <summary>
            /// The business unit name already exists.
            /// </summary>
            public static int BusinessUnitNameAlreadyExists = 100;
        }

        /// <summary>
        /// A list of validation codes used by the application for <see cref="TaskTypeDto"/> entities.
        /// </summary>
        public static class TaskType
        {
            /// <summary>
            /// Active record currently exists for this Task Definition/BU combination.
            /// </summary>
            public static int NameBusinessUnitAlreadyExists = 200;
        }

        /// <summary>
        /// A list of validation codes used by the application for <see cref="TaskTypeDto"/> entities.
        /// </summary>
        public static class DocumentTemplate
        {
            /// <summary>
            /// Active record currently exists for this Task Definition/BU combination.
            /// </summary>
            public const int NameBusinessUnitAlreadyExists = 200;
        }
    }
}