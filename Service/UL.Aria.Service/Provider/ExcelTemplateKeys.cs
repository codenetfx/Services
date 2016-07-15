namespace UL.Aria.Service.Provider
{
    internal static class ExcelTemplateKeys
    {
        public const string StaticCharacteristicType = "static";
        public const int CharacteristicNameRow = 4;
        public const int CharacteristicTypeRow = 3;
        public const int CharacteristicDataTypeRow = 2;
        public const int CharacteristicScopeRow = 1;
        public const int CharacteristicIdRow = 0;
        public const char StandardDelimiter = '|';

        public const string ProductFamilyIdentifier = "Product Family";
        public const string ProductNameIdentifier = "Product Name";
        public const string ProductDescriptionIdentifier = "Product Description";
        public const string ProductCompanyIdentifier = "Product Company";
        public const string IdIdentifier = "ID";

        public const string AttributesAndFeatures = "Attributes and Features";
        public const string FamilyBasics = "Family Basics";
        public const string FamilyName = "Family Name";
        public const string FamilyDescription = "Family Description";
        public const string CategoryTree = "Category Tree";
        public const string BusinessUnit = "Business Unit";
        public const string AuthorEmail = "Email";
        public const string AuthorName = "Name";
        public const string ActionLabel = "Action";

        public const string CharacteristicIdLabel = "Id";
        public const string NameLabel = "Name";
        public const string DescriptionLabel = "Description";
        public const string TypeLabel = "Type";
        public const string ScopeLabel = "Scope";
        public const string RequiredLabel = "Required";
        public const string UoMLabel = "Unit of Measure";
        public const string LoVLabel = "List of Values";
        public const string ListOpenLabel = "Is Open List";
        public const string DataTypeLabel = "Data Type";
        public const string AllowedValueTypeLabel = "Allowed Value Types";
        public const string AllowedValueLabel = "Allowed Values";
        public const string ValueRequiredLabel = "Value Required";

        public const string ValidationsLabel = "Validations";
        public const int UnitOfMeasureIdColumn = 0;
        public const int UnitOfMeasureLabelColum = 1;

        public const int AllowedValueTypeValidatorColumn = 2;

        public const string GlobalScopeId = "4CAA5706-6D86-E211-BCF5-20C9D042ED3E";

        public const string Dependencies = "Dependencies";
        public const string DependencyParentId = "Parent Feature Id";
        public const string DependencyChildId = "Child Feature Id";
        public const string DependencyChildValues = "Child Values";
        public const string DependencyParentValues = "Parent Values";

		public const string Tasks = "Tasks";
		public const string Histories = "Histories";
	}

    /// <summary>
    /// Keys for projects
    /// </summary>
    public class ProjectTemplateKeys
    {
        /// <summary>
        /// The projects sheet name
        /// </summary>
        public const string ProjectsSheetName = "Projects";

        /// <summary>
        /// The tasks sheet name
        /// </summary>
        public const string TasksSheetName = "Tasks";

        /// <summary>
        /// The line item i ds label
        /// </summary>
        public const string LineItemIDsLabel = "Line Item IDs";

        /// <summary>
        /// The project status label
        /// </summary>
        public const string ProjectStatusLabel = "Project Status";

        /// <summary>
        /// The description label
        /// </summary>
        public const string DescriptionLabel = "Description";

        /// <summary>
        /// The end date label
        /// </summary>
        public const string EndDateLabel = "End Date";

        /// <summary>
        /// The completion date label
        /// </summary>
        public const string CompletionDateLabel = "Completion Date";

        /// <summary>
        /// The number of samples label
        /// </summary>
        public const string NumberOfSamplesLabel = "Number of Samples";

        /// <summary>
        /// The company label
        /// </summary>
        public const string CompanyLabel = "Company";

        /// <summary>
        /// The sample reference numbers label
        /// </summary>
        public const string SampleReferenceNumbersLabel = "Sample Reference Numbers";

        /// <summary>
        /// The CCN label
        /// </summary>
        public const string CcnLabel = "CCN";

        /// <summary>
        /// The file number label
        /// </summary>
        public const string FileNumberLabel = "File Number";

        /// <summary>
        /// The status notes label
        /// </summary>
        public const string StatusNotesLabel = "Status Notes";

        /// <summary>
        /// The est engineering effort hours label
        /// </summary>
        public const string EstEngineeringEffortHoursLabel = "Est Engineering Effort Hours";

        /// <summary>
        /// The est lab effort hours label
        /// </summary>
        public const string EstLabEffortHoursLabel = "Est Lab Effort Hours";

        /// <summary>
        /// The est reviewer effort hours label
        /// </summary>
        public const string EstReviewerEffortHoursLabel = "Est Reviewer Effort Hours";

        /// <summary>
        /// The estimated tat label
        /// </summary>
        public const string EstimatedTatLabel = "Estimated TAT";

        /// <summary>
        /// The scope label
        /// </summary>
        public const string ScopeLabel = "Scope";

        /// <summary>
        /// The assumptions label
        /// </summary>
        public const string AssumptionsLabel = "Assumptions";

        /// <summary>
        /// The engineering office limitations label
        /// </summary>
        public const string EngineeringOfficeLimitationsLabel = "Engineering Office Limitations";

        /// <summary>
        /// The laboratory limitations label
        /// </summary>
        public const string LaboratoryLimitationsLabel = "Laboratory Limitations";

        /// <summary>
        /// The complexity label
        /// </summary>
        public const string ComplexityLabel = "Complexity";

        /// <summary>
        /// The standards label
        /// </summary>
        public const string StandardsLabel = "Standards";

        /// <summary>
        /// The inventory item catalogue nos label
        /// </summary>
        public const string InventoryItemCatalogueNosLabel = "Inventory Item/Catalogue Nos";

        /// <summary>
        /// The inventory item nos description label
        /// </summary>
        public const string InventoryItemNosDescriptionLabel = "Inventory Item Nos. Description";

        /// <summary>
        /// The project type label
        /// </summary>
        public const string ProjectTypeLabel = "Project Type";

        /// <summary>
        /// The quote no label
        /// </summary>
        public const string QuoteNoLabel = "Quote No.";

        /// <summary>
        /// The price label
        /// </summary>
        public const string PriceLabel = "Price";

        /// <summary>
        /// The expedited label
        /// </summary>
        public const string ExpeditedLabel = "Expedited (Y/N)";

        /// <summary>
        /// The additional criteria label
        /// </summary>
        public const string AdditionalCriteriaLabel = "Additional Criteria";

        /// <summary>
        /// The industry label
        /// </summary>
        public const string IndustryLabel = "Industry";

        /// <summary>
        /// The industry sub group label
        /// </summary>
        public const string IndustrySubGroupLabel = "Industry Sub-Group";

        /// <summary>
        /// The industry category label
        /// </summary>
        public const string IndustryCategoryLabel = "Industry Category";

        /// <summary>
        /// The location label
        /// </summary>
        public const string LocationLabel = "Location";

        /// <summary>
        /// The product group label
        /// </summary>
        public const string ProductGroupLabel = "Product Group";

        /// <summary>
        /// The project task template type label
        /// </summary>
        public const string ProjectTaskTemplateTypeLabel = "Project Task Template Type";

        /// <summary>
        /// The project handler label
        /// </summary>
        public const string ProjectHandlerLabel = "Project Handler";

        /// <summary>
        /// The customer po label
        /// </summary>
        public const string CustomerPOLabel = "Customer PO";

        /// <summary>
        /// The order type label
        /// </summary>
        public const string OrderTypeLabel = "Order Type";

        /// <summary>
        /// The service description label
        /// </summary>
        public const string ServiceDescriptionLabel = "Service Description";

        /// <summary>
        /// The file no label
        /// </summary>
        public const string FileNoLabel = "File No.";

        /// <summary>
        /// The customer requested date label
        /// </summary>
        public const string CustomerRequestedDateLabel = "Customer Requested Date";

        /// <summary>
        /// The date booked label
        /// </summary>
        public const string DateBookedLabel = "Date Booked";

        /// <summary>
        /// The date ordered label
        /// </summary>
        public const string DateOrderedLabel = "Date Ordered";

        /// <summary>
        /// The last update date label
        /// </summary>
        public const string LastUpdateDateLabel = "Last Update Date";

        /// <summary>
        /// The order start date label
        /// </summary>
        public const string OrderStartDateLabel = "Order Start Date";

        /// <summary>
        /// The oracle project identifier label
        /// </summary>
        public const string OracleProjectIDLabel = "Oracle Project ID";

        /// <summary>
        /// The oracle project name label
        /// </summary>
        public const string OracleProjectNameLabel = "Oracle Project Name";

        /// <summary>
        /// The oracle project number label
        /// </summary>
        public const string OracleProjectNumberLabel = "Oracle Project Number";

        /// <summary>
        /// The associated product i ds label
        /// </summary>
        public const string AssociatedProductIDsLabel = "Associated Product IDs";

        /// <summary>
        /// The order identifier label
        /// </summary>
        public const string OrderIDLabel = "Order ID";

        /// <summary>
        /// The line item identifier label
        /// </summary>
        public const string LineItemIDLabel = "Line Item ID";

        /// <summary>
        /// The project identifier label
        /// </summary>
        public const string ProjectIDLabel = "Project ID";

        /// <summary>
        /// The project name label
        /// </summary>
        public const string ProjectNameLabel = "Project Name";

        /// <summary>
        /// The task identifier label
        /// </summary>
        public const string TaskIDLabel = "Task ID";

        /// <summary>
        /// The task name label
        /// </summary>
        public const string TaskNameLabel = "Task Name";

        /// <summary>
        /// The child ids label
        /// </summary>
        public const string ChildIdsLabel = "Child IDs";

        /// <summary>
        /// The parent identifier label
        /// </summary>
        public const string ParentIdLabel = "Parent ID";

        /// <summary>
        /// The predecessor tasks label
        /// </summary>
        public const string PredecessorTasksLabel = "Predecessor Tasks";

        /// <summary>
        /// The phase label
        /// </summary>
        public const string PhaseLabel = "Task Status";

        /// <summary>
        /// The progress label
        /// </summary>
        public const string ProgressLabel = "Progress";

        /// <summary>
        /// The start date label
        /// </summary>
        public const string StartDateLabel = "Start Date";

        /// <summary>
        /// The due date label
        /// </summary>
        public const string DueDateLabel = "Due Date";

        /// <summary>
        /// The estimated duration hours label
        /// </summary>
        public const string EstimatedDurationHoursLabel = "Estimated Duration Hours";

        /// <summary>
        /// The actual duration hours label
        /// </summary>
        public const string ActualDurationHoursLabel = "Actual Duration Hours";

        /// <summary>
        /// The client barrier hours label
        /// </summary>
        public const string ClientBarrierHoursLabel = "Client Barrier Hours";

        /// <summary>
        /// The percent complete label
        /// </summary>
        public const string PercentCompleteLabel = "Percent Complete";

        /// <summary>
        /// The task owner label
        /// </summary>
        public const string TaskOwnerLabel = "Task Owner";

        /// <summary>
        /// The comments label
        /// </summary>
        public const string CommentsLabel = "Comments";

        /// <summary>
        /// The comments label
        /// </summary>
        public const string CreatedLabel = "Created";

        /// <summary>
        /// The modified label
        /// </summary>
        public const string ModifiedLabel = "Modified";

        /// <summary>
        /// The is deleted
        /// </summary>
        public const string IsDeleted = "Is Deleted";
    }
}