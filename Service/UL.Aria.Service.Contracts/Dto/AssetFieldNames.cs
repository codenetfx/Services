using System;
using System.Globalization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// The field names for content stored in our repositories
    /// </summary>
    public sealed class AssetFieldNames
    {
        /// <summary>
        /// The character that multi-value SharePoint fields will be separated by. 
        /// NOTE: This char must match what is declared in the SharePoint solution project:
        /// UL.Aria.SharePoint.Service.Content.Connector.AriaMetaDataForConnector
        /// for both UnicodeMultiLineDelimiter and UnicodeMultiLineDelimiterForSplit fields
        /// </summary>
        public static readonly char SharePointMultivalueSeparator = (char)int.Parse("2029", NumberStyles.AllowHexSpecifier);


        /// <summary>
        /// The aria refiner control flags for date descriptions
        /// </summary>
        public static string[] AriaDateRefinerControlFlags = new string[] 
          {
              PastDue, Overdue, ThisMonth, ThisWeek, DateEquals, DateBefore, DateAfter
          };


        /// <summary>
        /// The no quotes
        /// </summary>
        public const string NoQuotes = "__NoQuotes__";


        /// <summary>
        /// The past due
        /// </summary>
        public const string PastDue = "__PastDue__";
        /// <summary>
        /// The overdue
        /// </summary>
        public const string Overdue = "__Overdue__";

        /// <summary>
        /// The this month
        /// </summary>
        public const string ThisMonth = "__ThisMonth__";

        /// <summary>
        /// The this week
        /// </summary>
        public const string ThisWeek = "__ThisWeek__";

        /// <summary>
        /// The date equals
        /// </summary>
        public const string DateEquals = "__DateEquals__";

        /// <summary>
        /// The date before
        /// </summary>
        public const string DateBefore = "__DateBefore__";

        /// <summary>
        /// The date after
        /// </summary>
        public const string DateAfter = "__DateAfter__";

        /// <summary>
        ///     The SharePoint id common to all assets, which is added by BCS at index time
        ///     <remarks>
        ///         Unfortunately, we need to use the alias defined in SharePoint rather than the true field name which is
        ///         'Asset.AssetId'
        ///     </remarks>
        /// </summary>
        public const string SharePointAssetId = "ariaAssetId";

        /// <summary>
        ///     The aria container id
        /// </summary>
        public const string AriaContainerId = "ariaContainerId";

        /// <summary>
        ///     The document's id, which is the SharePoint asset id because they do not exist in Aria SQL.
        /// </summary>
        public const string AriaDocumentId = "ariaAssetId";

        /// <summary>
        ///     The asset name
        /// </summary>
        public const string AriaName = "ariaName";

        /// <summary>
        ///     The asset title
        /// </summary>
        public const string AriaTitle = "ariaTitle";

        /// <summary>
        ///     The asset size in bytes
        /// </summary>
        public const string AriaSize = "ariaSize";

        /// <summary>
        ///     The asset last modified by user login id
        /// </summary>
        public const string AriaLastModifiedBy = "ariaLastModifiedBy";

        /// <summary>
        /// The aria created by
        /// </summary>
	    public const string AriaCreatedBy = "ariaCreatedBy";

        /// <summary>
        ///     The aria last modified on
        /// </summary>
        public const string AriaLastModifiedOn = "ariaUpdatedOn";

        /// <summary>
        ///     The content type of a document.  i.e. MIME types.
        /// </summary>
        public const string AriaContentType = "ariaContentType";

        /// <summary>
        ///     The document type id
        /// </summary>
        public const string AriaDocumentTypeId = "ariaDocumentTypeId";

        /// <summary>
        ///     The asset permission
        /// </summary>
        public const string AriaPermission = "ariaPermission";

        /// <summary>
        ///     The asset type.  i.e. EntityTypeEnum
        /// </summary>
        public const string AriaAssetType = "ariaAssetType";

        /// <summary>
        ///     The asset description
        /// </summary>
        public const string AriaProductDescription = "ariaProductDescription";

        /// <summary>
        ///     The asset name
        /// </summary>
        public const string AriaProductName = "ariaProductName";

        /// <summary>
        ///     The asset product model number
        /// </summary>
        public static readonly string AriaProductModelNumber = "aria" + ProductAttributeIds.ModelNumber.ToString("N");

      

        /// <summary>
        ///     The asset product id
        /// </summary>
        public const string AriaProductId = "ariaProductId";

        /// <summary>
        ///     The asset product status
        /// </summary>
        public const string AriaProductStatus = "ariaProductStatus";

        /// <summary>
        ///     The aria product submitted date field name
        /// </summary>
        public const string AriaProductSubmittedDate = "ariaProductSubmittedDate";

        /// <summary>
        ///     The aria project id
        /// </summary>
        public const string AriaProjectId = "ariaProjectId";

        /// <summary>
        ///     The aria project name
        /// </summary>
        public const string AriaProjectName = "ariaProjectName";

        /// <summary>
        ///     The project handler
        /// </summary>
        public const string AriaProjectHandler = "ariaProjectHandler";

		/// <summary>
		/// The aria order owner
		/// </summary>
		public const string AriaOrderOwner = "ariaOrderOwner";

        /// <summary>
        ///     The aria project due date
        /// </summary>
        public const string AriaProjectDueDate = "ariaProjectDueDate";

        /// <summary>
        ///     The project's status
        /// </summary>
        public const string AriaProjectStatus = "ariaProjectStatus";

        /// <summary>
        /// The aria project service line count
        /// </summary>
        public const string AriaProjectServiceLineCount = "ariaProjectServiceLineCount";

        /// <summary>
        /// The aria project has order number
        /// </summary>
        public const string AriaProjectHasOrderNumber = "ariaProjectHasOrderNumber";

        /// <summary>
        ///     The project's order's company name
        /// </summary>
        public const string AriaCompanyName = "ariaCompanyName";

        /// <summary>
        ///     The aria company id field name
        /// </summary>
        public const string AriaCompanyId = "ariaCompanyId";

        /// <summary>
        /// The aria company external identifier
        /// </summary>
        public const string AriaCompanyExternalId = "ariaCompanyExternal Id";

        /// <summary>
        ///     The Order's status
        /// </summary>
        public const string AriaOrderStatus = "ariaOrderStatus";

        /// <summary>
        ///     The aria order number
        /// </summary>
        public const string AriaOrderNumber = "ariaOrderNumber";

        


        /// <summary>
        ///     The aria project is expedited true/false
        /// </summary>
        public const string AriaProjectExpedited = "ariaProjectExpedited";

        /// <summary>
        ///     The aria order date
        /// </summary>
        public const string AriaOrderDate = "ariaOrderDate";

        /// <summary>
        ///     The aria order discription
        /// </summary>
        public const string AriaOrderDescription = "ariaOrderDescription";

        /// <summary>
        ///     The aria order id
        /// </summary>
        public const string AriaOrderId = "ariaOrderId";

        /// <summary>
        ///     The is deleted
        /// </summary>
        public const string IsDeleted = "IsDeleted";

        /// <summary>
        ///     The aria task phase
        /// </summary>
        public const string AriaTaskPhase = "ariaTaskPhase";

        /// <summary>
        /// The aria task phase label
        /// </summary>
        public const string AriaTaskPhaseLabel = "ariaTaskPhaseLabel";

        /// <summary>
        ///     The aria task id
        /// </summary>
        public const string AriaTaskId = "ariaTaskId";

        /// <summary>
        /// The child task id of an aria task parent
        /// </summary>
        public const string AriaTaskChildTaskId = "ariaTaskTask.Id";

        /// <summary>
        ///     The aria task task owner
        /// </summary>
        public const string AriaTaskOwner = "ariaTaskTaskOwner";

		/// <summary>
		/// The aria task type
		/// </summary>
		public const string ariaTaskType = "ariaTaskType";

        /// <summary>
        ///     The aria task progress
        /// </summary>
        public const string AriaTaskProgress = "ariaTaskProgress";


		/// <summary>
		/// The aria task progress cal
		/// </summary>
		public const string AriaTaskProgressCal = "ariaTaskProgressCal";

        /// <summary>
        /// The aria task progress
        /// </summary>
        public const string AriaTaskProgressLabel = "ariaTaskProgressLabel";

        /// <summary>
        ///     The aria task number
        /// </summary>
        public const string AriaTaskNumber = "ariaTaskNumber";

        /// <summary>
        ///     The aria task title
        /// </summary>
        public const string AriaTaskTitle = "ariaTaskTitle";

        /// <summary>
        ///     The aria task start date
        /// </summary>
        public const string AriaTaskStartDate = "ariaTaskStartDate";


		/// <summary>
		/// The aria task reminder date
		/// </summary>
		public const string AriaTaskReminderDate = "ariaTaskReminderDate";

        /// <summary>
        ///     The aria task due date
        /// </summary>
        public const string AriaTaskDueDate = "ariaTaskDueDate";

        /// <summary>
        ///     The aria task percent complete
        /// </summary>
        public const string AriaTaskPercentComplete = "ariaTaskPercentComplete";

        /// <summary>
        ///     The aria task category
        /// </summary>
        public const string AriaTaskCategory = "ariaTaskCategory";

        /// <summary>
        ///     The aria task modified
        /// </summary>
        public const string AriaTaskModified = "ariaTaskModified";

        /// <summary>
        ///     The aria task modified by
        /// </summary>
        public const string AriaTaskModifiedBy = "ariaTaskModifiedBy";

		/// <summary>
		/// The aria task description
		/// </summary>
		public const string AriaTaskDescription = "ariaTaskDescription";

		/// <summary>
		/// The aria task is project handler restricted
		/// </summary>
		public const string AriaTaskIsProjectHandlerRestricted = "ariaTaskIsProjectHandlerRestricted";
		/// <summary>
		/// The aria task is project handler restricted
		/// </summary>
		public const string AriaTaskShouldTriggerBilling = "ariaTaskShouldTriggerBilling";


		/// <summary>
		/// The aria project task minimum due date
		/// </summary>
		public const string AriaProjectTaskMinimumDueDate = "ariaProjectTaskMinimumDueDate";

        /// <summary>
        ///     The aria project project status
        /// </summary>
        public const string AriaProjectProjectStatus = "ariaProjectProjectStatus";

        /// <summary>
        /// The aria project project status label
        /// </summary>
        public const string AriaProjectProjectStatusLabel = "ariaProjectProjectStatusLabel";

        /// <summary>
        ///     The aria project template id field name.
        /// </summary>
        public const string AriaProjectTemplateId = "ariaProjectTemplateId";

        /// <summary>
        ///     The aria project service line's line number  field name.
        /// </summary>
        public const string AriaProjectServiceLineNumber = "ariaProjectIncomingOrderServiceLine.LineNumber";

        /// <summary>
        /// The aria project industry
        /// </summary>
        public const string AriaProjectIndustryCode = "ariaProjectIncomingOrderServiceLineIndustryCode";

        /// <summary>
        /// The aria project service code
        /// </summary>
        public const string AriaProjectServiceCode = "ariaProjectIncomingOrderServiceLineServiceCode";

        /// <summary>
        /// The aria project preferred fulfillment location
        /// </summary>
        public const string AriaProjectLocationName = "ariaProjectIncomingOrderServiceLineLocationName";

        /// <summary>
        /// The aria project preferred fulfillment location code
        /// </summary>
        public const string AriaProjectLocationCode = "ariaProjectIncomingOrderServiceLineLocationCode";

        /// <summary>
        ///     The aria task duration
        /// </summary>
        public const string AriaTaskDuration = "ariaTaskDuration";

        /// <summary>
        ///     The aria parent asset id
        /// </summary>
        public const string AriaParentAssetId = "ariaParentAssetId";

        /// <summary>
        ///     The aria customer name
        /// </summary>
        public const string AriaCustomerName = "ariaOrderIncomingOrderCustomer.Name";

        /// <summary>
        ///     The aria customer project name
        /// </summary>
        public const string AriaCustomerProjectName = "ariaOrderIncomingOrderCustomer.ProjectName";

        /// <summary>
        /// The aria notification NotificationType Field name
        /// </summary>
        public const string AriaNotificationType = "NotificationType";

        /// <summary>
        /// The aria notification title
        /// </summary>
        public const string AriaNotificationTitle = "Title";

        /// <summary>
        /// The aria notification date
        /// </summary>
        public const string AriaNotificationDate = "Date";

        /// <summary>
        ///     The aria project end date
        /// </summary>
        public const string AriaProjectEndDate = "ariaProjectEndDate";

        /// <summary>
        /// The aria date booked
        /// </summary>
        public const string AriaDateBooked = "ariaProjectBookedDate";

        /// <summary>
        /// The aria hide from customer
        /// </summary>
        public const string AriaHideFromCustomer = "ariaHideFromCustomer";

        /// <summary>
        /// The aria project CCN
        /// </summary>
        public const string AriaProjectCcn = "ariaProjectCCN";

        /// <summary>
        /// The aria project file no
        /// </summary>
        public const string AriaProjectFileNo = "ariaProjectFileNo";

        /// <summary>
        /// The aria project number
        /// </summary>
        public const string AriaProjectNumber = "ariaProjectNumber";

        /// <summary>
        /// The aria quote number
        /// </summary>
        public const string AriaQuoteNumber = "ariaQuoteNumber";

		/// <summary>
		/// The project template business unit code
		/// </summary>
		public const string ProjectTemplateBusinessUnitCode = "BusinessUnitCode";

		/// <summary>
		/// The project template business unit identifier
		/// </summary>
		public const string ProjectTemplateBusinessUnitId = "BusinessUnitId";

		/// <summary>
		/// The project template is draft
		/// </summary>
		public const string ProjectTemplateIsDraft = "IsDraft";

		/// <summary>
		/// The aria order booked date
		/// </summary>
		public const string AriaOrderBookedDate = "ariaOrderBookedDate";

		/// <summary>
		/// The aria order type
		/// </summary>
		public const string AriaOrderType = "ariaOrderType";

		/// <summary>
		/// The aria order business unit
		/// </summary>
		public const string AriaOrderBusinessUnit = "ariaOrderBusinessUnit";

		/// <summary>
		/// The aria order project header status
		/// </summary>
		public const string AriaOrderProjectHeaderStatus = "ariaOrderProjectHeaderStatus";

		/// <summary>
		/// The aria order creation date
		/// </summary>
		public const string AriaOrderCreationDate = "ariaOrderCreationDate";

		/// <summary>
		/// The aria order customer requested date
		/// </summary>
		public const string AriaOrderCustomerRequestedDate = "ariaOrderCustomerRequestedDate";

		/// <summary>
		/// The aria order last update date
		/// </summary>
		public const string AriaOrderLastUpdateDate = "ariaOrderLastUpdateDate";

		/// <summary>
		/// The aria order external project identifier
		/// </summary>
		public const string AriaOrderExternalProjectId = "ariaOrderExternalProjectId";

		/// <summary>
		/// The aria order customer po
		/// </summary>
		public const string AriaOrderCustomerPo = "ariaOrderCustomerPo";

		/// <summary>
		/// The aria project order type
		/// </summary>
		public const string AriaProjectOrderType = "ariaProjectOrderType";

		/// <summary>
		/// The aria project customer po
		/// </summary>
		public const string AriaProjectCustomerPo = "ariaProjectCustomerPo";

		/// <summary>
		/// The aria project service line
		/// </summary>
		public const string AriaProjectServiceLine = "ariaProjectServiceLine";

		/// <summary>
		/// The aria project booked date
		/// </summary>
		public const string AriaProjectBookedDate = "ariaProjectBookedDate";

		/// <summary>
		/// The aria project ordered date
		/// </summary>
		public const string AriaProjectOrderedDate = "ariaProjectOrderedDate";

		/// <summary>
		/// The aria project business unit
		/// </summary>
		public const string AriaProjectBusinessUnit = "ariaProjectBusinessUnit";

		/// <summary>
		/// The aria project header status
		/// </summary>
		public const string AriaProjectHeaderStatus = "ariaProjectHeaderStatus";

		/// <summary>
		/// The aria project creation date
		/// </summary>
		public const string AriaProjectCreationDate = "ariaProjectCreationDate";

		/// <summary>
		/// The aria project last update date
		/// </summary>
		public const string AriaProjectLastUpdateDate = "ariaProjectLastUpdateDate";

		/// <summary>
		/// The aria external project identifier
		/// </summary>
		public const string AriaExternalProjectId = "ariaExternalProjectId";

		/// <summary>
		/// The ariae97b75554ccce2118bfe54d9dfe94c0d
		/// </summary>
		public const string Ariae97b75554ccce2118bfe54d9dfe94c0d = "ariae97b75554ccce2118bfe54d9dfe94c0d";

		/// <summary>
		/// The last modified time
		/// </summary>
		public const string LastModifiedTime = "LastModifiedTime";

		/// <summary>
		/// The aria task has comments
		/// </summary>
		public const string AriaTaskHasComments = "ariaTaskHasComments";

        /// <summary>
        /// The aria project completion date
        /// </summary>
	    public const string AriaProjectCompletionDate = "ariaProjectCompletionDate";


		/// <summary>
		/// The aria Order industry
		/// </summary>
		public const string AriaOrderIndustryCode = "ariaOrderIncomingOrderServiceLineIndustryCode";

		/// <summary>
		/// The aria Order service code
		/// </summary>
		public const string AriaOrderServiceCode = "ariaOrderIncomingOrderServiceLineServiceCode";
		/// <summary>
		/// The aria Order preferred fulfillment location code
		/// </summary>
		public const string AriaOrderLocationCode = "ariaOrderIncomingOrderServiceLineLocationCode";

        /// <summary>
        /// The aria customer country
        /// </summary>
	    public const string AriaCustomerCountry = "ariaCustomerCountry";

        /// <summary>
        /// The aria customer state
        /// </summary>
	    public const string AriaCustomerState = "ariaCustomerState";

		/// <summary>
		/// The aria locked by
		/// </summary>
		public const string AriaLockedBy = "ariaLockedBy";

		/// <summary>
		/// The aria locked date time
		/// </summary>
		public const string AriaLockedDateTime = "ariaLockedDateTime";

        /// <summary>
        /// The incoming order contact party site number
        /// </summary>
        public const string AriaIncomingOrderContactPartySiteNumber = "ariaIncomingOrderContactPartySiteNumber";
        /// <summary>
        /// The aria party site number
        /// </summary>
        public const string AriaPartySiteNumber = "ariaPartySiteNumber";

        /// <summary>
        /// The aria ship to contact party site number
        /// </summary>
        public const string AriaShipToContactPartySiteNumber = "ariaShipToPartySiteNumber";

        /// <summary>
        /// The aria bill to contact party site number
        /// </summary>
        public const string AriaBillToContactPartySiteNumber = "ariaBillToPartySiteNumber";

		/// <summary>
		/// Class Keys. This class cannot be inherited.
		/// </summary>
		public sealed class Keys
		{
			// *******************************************************************************
			// Beginning of Keys in dictionary are below that do not match the aria search keys
			// *******************************************************************************

			/// <summary>
			/// The type
			/// </summary>
			public const string Type = "Type";

			/// <summary>
			/// The updated by identifier
			/// </summary>
			public const string UpdatedById = "UpdatedById";

			/// <summary>
			/// The updated date time
			/// </summary>
			public const string UpdatedDateTime = "UpdatedDateTime";

			/// <summary>
			/// The container identifier
			/// </summary>
			public const string ContainerId = "ContainerId";

			/// <summary>
			/// The original XML parsed
			/// </summary>
			public const string OriginalXmlParsed = "OriginalXmlParsed";

			/// <summary>
			/// The identifier
			/// </summary>
			public const string Id = "Id";

			/// <summary>
			/// The order number
			/// </summary>
			public const string OrderNumber = "OrderNumber";

            /// <summary>
            /// The order owner
            /// </summary>
            public const string OrderOwner = "OrderOwner";


			/// <summary>
			/// The date booked
			/// </summary>
			public const string DateBooked = "DateBooked";

			/// <summary>
			/// The date ordered
			/// </summary>
			public const string DateOrdered = "DateOrdered";

			/// <summary>
			/// The order type
			/// </summary>
			public const string OrderType = "OrderType";

			/// <summary>
			/// The order description
			/// </summary>
			public const string OrderDescription = "OrderDescription";

			/// <summary>
			/// The business unit
			/// </summary>
			public const string BusinessUnit = "BusinessUnit";

			/// <summary>
			/// The project header status
			/// </summary>
			public const string ProjectHeaderStatus = "ProjectHeaderStatus";

			/// <summary>
			/// The creation date
			/// </summary>
			public const string CreationDate = "CreationDate";

			/// <summary>
			/// The customer requested date
			/// </summary>
			public const string CustomerRequestedDate = "CustomerRequestedDate";

			/// <summary>
			/// The last update date
			/// </summary>
			public const string LastUpdateDate = "LastUpdateDate";

			/// <summary>
			/// The external project identifier
			/// </summary>
			public const string ExternalProjectId = "ExternalProjectId";

			/// <summary>
			/// The customer po
			/// </summary>
			public const string CustomerPo = "CustomerPo";

			/// <summary>
			/// The status
			/// </summary>
			public const string Status = "Status";

			/// <summary>
			/// The company identifier
			/// </summary>
			public const string CompanyId = "CompanyId";

			/// <summary>
			/// The characteristics
			/// </summary>
			public const string Characteristics = "Characteristics";

			/// <summary>
			/// The d0daa8917386e211bcf520c9d042ed3e
			/// </summary>
			public const string d0daa8917386e211bcf520c9d042ed3e = "d0daa8917386e211bcf520c9d042ed3e";

			/// <summary>
			/// The e97b75554ccce2118bfe54d9dfe94c0d,
			/// </summary>
			public const string e97b75554ccce2118bfe54d9dfe94c0d = "e97b75554ccce2118bfe54d9dfe94c0d";

			/// <summary>
			/// The name
			/// </summary>
			public const string Name = "Name";

			/// <summary>
			/// The description
			/// </summary>
			public const string Description = "Description";

			/// <summary>
			/// The submitted date time
			/// </summary>
			public const string SubmittedDateTime = "SubmittedDateTime";

			/// <summary>
			/// The project handler
			/// </summary>
			public const string ProjectHandler = "ProjectHandler";

			/// <summary>
			/// The project number
			/// </summary>
			public const string ProjectNumber = "ProjectNumber";

			/// <summary>
			/// The service line item count
			/// </summary>
			public const string ServiceLineItemCount = "ServiceLineItemCount";

			/// <summary>
			/// The hide from customer
			/// </summary>
			public const string HideFromCustomer = "HideFromCustomer";

			/// <summary>
			/// The end date
			/// </summary>
			public const string EndDate = "EndDate";

			/// <summary>
			/// The file no
			/// </summary>
			public const string FileNo = "FileNo";

			/// <summary>
			/// The CCN
			/// </summary>
			public const string CCN = "SubmittedDateTime";

			/// <summary>
			/// The quote no
			/// </summary>
			public const string QuoteNo = "QuoteNo";

			/// <summary>
			/// The has order number
			/// </summary>
			public const string HasOrderNumber = "HasOrderNumber";

			/// <summary>
			/// The project expedited
			/// </summary>
			public const string ProjectExpedited = "ProjectExpedited";

			/// <summary>
			/// The project status
			/// </summary>
			public const string ProjectStatus = "ProjectStatus";

			/// <summary>
			/// The project status label
			/// </summary>
			public const string ProjectStatusLabel = "ProjectStatusLabel";

			/// <summary>
			/// The project template identifier
			/// </summary>
			public const string ProjectTemplateId = "ProjectTemplateId";

			/// <summary>
			/// The industry code
			/// </summary>
			public const string IndustryCode = "IndustryCode";

			/// <summary>
			/// The service code
			/// </summary>
			public const string ServiceCode = "ServiceCode";

			/// <summary>
			/// The location
			/// </summary>
			public const string Location = "Location";

			/// <summary>
			/// The service lines
			/// </summary>
			public const string ServiceLines = "ServiceLines";

			/// <summary>
			/// The incoming order service line industry code
			/// </summary>
			public const string IncomingOrderServiceLineIndustryCode = "IncomingOrderServiceLineIndustryCode";

			/// <summary>
			/// The incoming order service line service code
			/// </summary>
			public const string IncomingOrderServiceLineServiceCode = "IncomingOrderServiceLineServiceCode";

			/// <summary>
			/// The incoming order service line location name
			/// </summary>
			public const string IncomingOrderServiceLineLocationName = "IncomingOrderServiceLineLocationName";

			/// <summary>
			/// The incoming order service line location code
			/// </summary>
			public const string IncomingOrderServiceLineLocationCode = "IncomingOrderServiceLineLocationCode";

			/// <summary>
			/// The company name
			/// </summary>
			public const string CompanyName = "CompanyName";

			/// <summary>
			/// The progress
			/// </summary>
			public const string Progress = "Progress";

			/// <summary>
			/// The progress label
			/// </summary>
			public const string ProgressLabel = "ProgressLabel";

			/// <summary>
			/// The status label
			/// </summary>
			public const string StatusLabel = "StatusLabel";

			/// <summary>
			/// The task owner
			/// </summary>
			public const string TaskOwner = "TaskOwner";

			/// <summary>
			/// The project identifier
			/// </summary>
			public const string ProjectId = "ProjectId";

			/// <summary>
			/// The task number
			/// </summary>
			public const string TaskNumber = "TaskNumber";

			/// <summary>
			/// The title
			/// </summary>
			public const string Title = "Title";

			/// <summary>
			/// The start date
			/// </summary>
			public const string StartDate = "StartDate";

			/// <summary>
			/// The due date
			/// </summary>
			public const string DueDate = "DueDate";

			/// <summary>
			/// The percent complete
			/// </summary>
			public const string PercentComplete = "PercentComplete";

			/// <summary>
			/// The reminder date
			/// </summary>
			public const string ReminderDate = "ReminderDate";

			/// <summary>
			/// The category
			/// </summary>
			public const string Category = "Category";

			/// <summary>
			/// The modified
			/// </summary>
			public const string Modified = "Modified";



			/// <summary>
			/// The modified by
			/// </summary>
			public const string ModifiedBy = "ModifiedBy";

			/// <summary>
			/// The actual duration
			/// </summary>
			public const string ActualDuration = "ActualDuration";

			/// <summary>
			/// The has comments
			/// </summary>
			public const string HasComments = "HasComments";

			/// <summary>
			/// The customer project name
			/// </summary>
			public const string CustomerProjectName = "IncomingOrderCustomer.ProjectName";

            /// <summary>
			/// The customer project name
			/// </summary>
            public const string IncomingOrderContactPartySiteNumber = "IncomingOrderContactPartySiteNumber";
            /// <summary>
            /// The party site number
            /// </summary>
            public const string PartySiteNumber = "PartySiteNumber";
            /// <summary>
            /// The ship to party site number
            /// </summary>
            public const string ShipToPartySiteNumber = "ShipToPartySiteNumber";

            /// <summary>
            /// The bill to party site number
            /// </summary>
		    public const string BillToPartySiteNumber = "BillToPartySiteNumber";

            /// <summary>
            /// The aria project completion date
            /// </summary>
            public const string CompletionDate = "CompletionDate";

			/// <summary>
			/// The project task minimum due date
			/// </summary>
			public const string ProjectTaskMinimumDueDate = "TaskMinimumDueDate";


			/// <summary>
			/// The assigned to
			/// </summary>
			public const string DashboardAssignedTo = "DashboardAssignedTo";

			/// <summary>
			/// The dashboard phase
			/// </summary>
			public const string DashboardPhase = "DashboardPhase";


			/// <summary>
			/// The dashboard due date
			/// </summary>
			public const string DashboardDueDate = "DashboardDueDate";

			/// <summary>
			/// My self
			/// </summary>
			public const string MySelf = "MySelf";

			/// <summary>
			/// My team
			/// </summary>
			public const string MyTeam = "MyTeam";

			/// <summary>
			/// The is project handler restricted
			/// </summary>
			public const string IsProjectHandlerRestricted = "IsProjectHandlerRestricted";


			/// <summary>
			/// The should trigger billing
			/// </summary>
			public const string ShouldTriggerBilling = "ShouldTriggerBilling";


			/// <summary>
			/// The default task type identifier
			/// </summary>
			public static readonly Guid DefaultTaskTypeId = new Guid("BEE714E0-FF7E-41B7-9B25-4D628F4FEF45");

			// *******************************************************************************
			// ending of Keys in dictionary are below that do not match the aria search keys
			// *******************************************************************************
		}

        /// <summary>
        /// The project handler token
        /// </summary>
        public const string ProjectHandlerToken = "[[project-handler]]";

        /// <summary>
        /// The business unit all token
        /// </summary>
        public const string BusinessUnitAllToken = "ALL";
    }
}