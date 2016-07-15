using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.ServiceBus.Messaging;

using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Class AssetFieldMetadata
    /// </summary>
    public class AssetFieldMetadata : IAssetFieldMetadata
    {
        private static readonly IDictionary<AssetTypeEnumDto, string> _selectProperties =
            new Dictionary<AssetTypeEnumDto, string>();

        private static readonly AssetField _ariaAssetId = new AssetField(AssetFieldNames.SharePointAssetId);

        private static readonly AssetField _ariaAssetType = new AssetField(AssetFieldNames.AriaAssetType)
        {
            Refine = true
        };

        private static readonly AssetField _ariaLastModifiedBy = new AssetField(AssetFieldNames.AriaLastModifiedBy);

        private static readonly AssetField _ariaUpdatedOn = new AssetField(AssetFieldNames.AriaLastModifiedOn)
        {
            Sort = true
        };

        private static readonly AssetField _ignore = new AssetField(null) {Ignore = true};

        private static readonly AssetField _ariaContainerId = new AssetField(AssetFieldNames.AriaContainerId)
        {
            Refine = true
        };

	    private static Dictionary<string, AssetField> ConfigureDocument()
	    {
		    return new Dictionary<string, AssetField>
		    {
			    {AssetFieldNames.AriaContainerId, _ariaContainerId},
			    {AssetFieldNames.SharePointAssetId, _ariaAssetId},
			    {AssetFieldNames.Keys.Type, _ariaAssetType},
			    {AssetFieldNames.Keys.UpdatedById, _ariaLastModifiedBy},
			    {AssetFieldNames.Keys.UpdatedDateTime, _ariaUpdatedOn},
			    {AssetFieldNames.AriaPermission, new AssetField(AssetFieldNames.AriaPermission) {Refine = true}},
			    {AssetFieldNames.AriaName, new AssetField(AssetFieldNames.AriaName) {Sort = true}},
			    {AssetFieldNames.AriaTitle, new AssetField(AssetFieldNames.AriaTitle) {Sort = true}},
			    {AssetFieldNames.AriaProductDescription, new AssetField(AssetFieldNames.AriaProductDescription)},
			    {
				    AssetFieldNames.AriaDocumentTypeId,
				    new AssetField(AssetFieldNames.AriaDocumentTypeId) {Refine = true}
			    },
			    {AssetFieldNames.AriaSize, new AssetField(AssetFieldNames.AriaSize)},
			    {AssetFieldNames.AriaContentType, new AssetField(AssetFieldNames.AriaContentType) {Refine = true}},
			    {AssetFieldNames.LastModifiedTime, new AssetField(AssetFieldNames.LastModifiedTime)},
			    {AssetFieldNames.AriaLockedBy, new AssetField(AssetFieldNames.AriaLockedBy)},
			    {AssetFieldNames.AriaLockedDateTime, new AssetField(AssetFieldNames.AriaLockedDateTime)},
		    };
	    }

	    private static Dictionary<string, AssetField> ConfigureContainer()
	    {
		    return new Dictionary<string, AssetField>
		    {
			    {AssetFieldNames.Keys.ContainerId, _ariaContainerId},
			    {AssetFieldNames.SharePointAssetId, _ariaAssetId},
			    {AssetFieldNames.Keys.Type, _ariaAssetType},
			    {AssetFieldNames.Keys.UpdatedById, _ariaLastModifiedBy},
			    {AssetFieldNames.Keys.UpdatedDateTime, _ariaUpdatedOn}
		    };
	    }

	    private static Dictionary<string, AssetField> ConfigureOrder()
	    {
		    return new Dictionary<string, AssetField>
		    {
			    {AssetFieldNames.Keys.ContainerId, _ariaContainerId},
			    {AssetFieldNames.Keys.OriginalXmlParsed, _ignore},
			    {AssetFieldNames.SharePointAssetId, _ariaAssetId},
			    {AssetFieldNames.Keys.Type, _ariaAssetType},
			    {AssetFieldNames.Keys.UpdatedById, _ariaLastModifiedBy},
			    {AssetFieldNames.Keys.UpdatedDateTime, _ariaUpdatedOn},
			    {AssetFieldNames.Keys.Id, new AssetField(AssetFieldNames.AriaOrderId) {IncludeInContainer = true}},
			    {
				    AssetFieldNames.Keys.OrderNumber,
				    new AssetField(AssetFieldNames.AriaOrderNumber) {IncludeInContainer = true, Sort = true}
			    },
			    {AssetFieldNames.Keys.DateBooked, new AssetField(AssetFieldNames.AriaOrderBookedDate)},
			    {AssetFieldNames.Keys.DateOrdered, new AssetField(AssetFieldNames.AriaOrderDate) {Sort = true}},
			    {AssetFieldNames.Keys.OrderType, new AssetField(AssetFieldNames.AriaOrderType)},
			    {AssetFieldNames.Keys.OrderDescription, new AssetField(AssetFieldNames.AriaOrderDescription)},
			    {AssetFieldNames.Keys.BusinessUnit, new AssetField(AssetFieldNames.AriaOrderBusinessUnit)},
			    {AssetFieldNames.Keys.ProjectHeaderStatus, new AssetField(AssetFieldNames.AriaOrderProjectHeaderStatus)},
			    {AssetFieldNames.Keys.CreationDate, new AssetField(AssetFieldNames.AriaOrderCreationDate)},
			    {AssetFieldNames.Keys.CustomerRequestedDate, new AssetField(AssetFieldNames.AriaOrderCustomerRequestedDate)},
			    {AssetFieldNames.Keys.LastUpdateDate, new AssetField(AssetFieldNames.AriaOrderLastUpdateDate)},
			    {AssetFieldNames.Keys.ExternalProjectId, new AssetField(AssetFieldNames.AriaOrderExternalProjectId)},
			    {AssetFieldNames.Keys.CustomerPo, new AssetField(AssetFieldNames.AriaOrderCustomerPo)},
			    {AssetFieldNames.Keys.Status, new AssetField(AssetFieldNames.AriaOrderStatus) {Refine = true, Sort = true}},
			    {AssetFieldNames.Keys.CompanyId, new AssetField(AssetFieldNames.AriaCompanyId) {Refine = true}},
			    {AssetFieldNames.AriaCustomerName, new AssetField(AssetFieldNames.AriaCustomerName)},
			    {AssetFieldNames.AriaCustomerProjectName, new AssetField(AssetFieldNames.AriaCustomerProjectName)},
				{AssetFieldNames.Keys.IncomingOrderServiceLineIndustryCode, new AssetField(AssetFieldNames.AriaOrderIndustryCode){Refine = true, Sort = true,  Query = true, Retrieve = true, IncludeInContainer = true, Multi = true, SuppressEmpty = true}},
				{AssetFieldNames.Keys.IncomingOrderServiceLineServiceCode, new AssetField(AssetFieldNames.AriaOrderServiceCode){Refine = true, Sort = true,  Query = true, Retrieve = true, IncludeInContainer = true, Multi = true, SuppressEmpty = true}},
				{AssetFieldNames.Keys.IncomingOrderServiceLineLocationCode, new AssetField(AssetFieldNames.AriaOrderLocationCode){Refine = true, Sort = true,  Query = true, Retrieve = true, IncludeInContainer = true, Multi = true, SuppressEmpty = true}},
		    };
	    }

	    private static Dictionary<string, AssetField> ConfigureProduct()
	    {
		    return new Dictionary<string, AssetField>
		    {
			    {AssetFieldNames.Keys.ContainerId, _ariaContainerId},
			    {AssetFieldNames.Keys.Characteristics, _ignore},
			    {AssetFieldNames.SharePointAssetId, _ariaAssetId},
			    {AssetFieldNames.Keys.Type, _ariaAssetType},
			    {AssetFieldNames.Keys.UpdatedById, _ariaLastModifiedBy},
			    {AssetFieldNames.Keys.UpdatedDateTime, _ariaUpdatedOn},
			    {AssetFieldNames.Keys.CompanyId, new AssetField(AssetFieldNames.AriaCompanyId) {Refine = true}},
			    {AssetFieldNames.Keys.Id, new AssetField(AssetFieldNames.AriaProductId) {IncludeInContainer = true}},
			    {AssetFieldNames.IsDeleted, new AssetField(AssetFieldNames.IsDeleted) {IncludeInContainer = true}},
			    {
				    AssetFieldNames.Keys.d0daa8917386e211bcf520c9d042ed3e,
				    new AssetField(AssetFieldNames.AriaProductModelNumber)
				    {
					    Refine = true,
					    IsCharacteristic = true,
					    IncludeInContainer = true,
					    DisplayName = "Model Number"
				    }
			    },
			    //these characteristics are here so as to be added to the select properties list and thus returned in search results
			    {
				    AssetFieldNames.Keys.Status,
				    new AssetField(AssetFieldNames.AriaProductStatus)
				    {
					    Refine = true,
					    IsCharacteristic = true,
					    IncludeInContainer = false,
					    DisplayName = "Status"
				    }
			    },
			    {
				    AssetFieldNames.Keys.e97b75554ccce2118bfe54d9dfe94c0d,
				    new AssetField(AssetFieldNames.Ariae97b75554ccce2118bfe54d9dfe94c0d)
				    {
					    Refine = true,
					    IsCharacteristic = true,
					    IncludeInContainer = false,
					    DisplayName = "Manufacturer"
				    }
			    },
			    {AssetFieldNames.Keys.Name, new AssetField(AssetFieldNames.AriaProductName) {IncludeInContainer = true, Sort = true}},
			    {AssetFieldNames.Keys.Description, new AssetField(AssetFieldNames.AriaProductDescription) {IncludeInContainer = true}},
			    {AssetFieldNames.Keys.SubmittedDateTime, new AssetField(AssetFieldNames.AriaProductSubmittedDate) {Sort = true}},
		    };
	    }

	    private static Dictionary<string, AssetField> ConfigureProject()
	    {
		    return new Dictionary<string, AssetField>
		    {
			    {AssetFieldNames.Keys.Id, new AssetField(AssetFieldNames.AriaProjectId) {IncludeInContainer = true, Multi = true}},
			    {AssetFieldNames.Keys.ContainerId, _ariaContainerId},
			    {AssetFieldNames.Keys.OriginalXmlParsed, _ignore},
			    {AssetFieldNames.Keys.CompanyId, new AssetField(AssetFieldNames.AriaCompanyId) {IncludeInContainer = true, Refine = true}},
			    {AssetFieldNames.SharePointAssetId, _ariaAssetId},
			    {AssetFieldNames.Keys.Type, _ariaAssetType},
			    {AssetFieldNames.Keys.UpdatedById, _ariaLastModifiedBy},
			    {AssetFieldNames.Keys.UpdatedDateTime, _ariaUpdatedOn},
			    {
				    AssetFieldNames.Keys.ProjectHandler,
				    new AssetField(AssetFieldNames.AriaProjectHandler)
				    {
					    Refine = true,
					    Sort = true,
					    IsLowerCase = true,
					    DisplayName = "Project Handler"
				    }
			    },
                {
				    AssetFieldNames.Keys.OrderOwner,
				    new AssetField(AssetFieldNames.AriaOrderOwner)
				    {
					    Refine = true,
					    Sort = true,
					    IsLowerCase = true,
					    DisplayName = "Order Owner"
				    }
			    },
			    {AssetFieldNames.Keys.Name, new AssetField(AssetFieldNames.AriaProjectName) {Sort = true, IncludeInContainer = true}},
			    {AssetFieldNames.Keys.ProjectNumber, new AssetField(AssetFieldNames.AriaProjectNumber)},
			    {AssetFieldNames.Keys.DateBooked, new AssetField(AssetFieldNames.AriaProjectBookedDate) {Sort = true, Query = true}},
			    {AssetFieldNames.Keys.ServiceLineItemCount, new AssetField(AssetFieldNames.AriaProjectServiceLineCount) {Sort = true, Query = true}},
			    {AssetFieldNames.Keys.HideFromCustomer, new AssetField(AssetFieldNames.AriaHideFromCustomer)},
			    {AssetFieldNames.Keys.DateOrdered, new AssetField(AssetFieldNames.AriaProjectOrderedDate)},
			    {AssetFieldNames.Keys.BusinessUnit, new AssetField(AssetFieldNames.AriaProjectBusinessUnit)},
			    {AssetFieldNames.Keys.ProjectHeaderStatus, new AssetField(AssetFieldNames.AriaProjectHeaderStatus)},
			    {AssetFieldNames.Keys.CreationDate, new AssetField(AssetFieldNames.AriaProjectCreationDate)},
			    {AssetFieldNames.Keys.CustomerRequestedDate, new AssetField(AssetFieldNames.AriaProjectDueDate) {Sort = true}},
			    {AssetFieldNames.Keys.EndDate, new AssetField(AssetFieldNames.AriaProjectEndDate) {Sort = true, Refine = true, Query = true}},
			    {AssetFieldNames.Keys.CompletionDate, new AssetField(AssetFieldNames.AriaProjectCompletionDate) {Sort = true, Refine = true, Query = true}},
			    {AssetFieldNames.Keys.LastUpdateDate, new AssetField(AssetFieldNames.AriaProjectLastUpdateDate)},
			    {AssetFieldNames.Keys.ExternalProjectId, new AssetField(AssetFieldNames.AriaExternalProjectId)},
			    {
				    AssetFieldNames.Keys.OrderNumber,
				    new AssetField(AssetFieldNames.AriaOrderNumber) {IncludeInContainer = true, Sort = true, Query = true, Refine = true}
			    },
			    {
				    AssetFieldNames.Keys.FileNo,
				    new AssetField(AssetFieldNames.AriaProjectFileNo)
				    {
					    IncludeInContainer = false,
					    Sort = true,
					    Query = true,
					    Search = true,
					    Refine = true
				    }
			    },
			    {
				    AssetFieldNames.Keys.CCN,
				    new AssetField(AssetFieldNames.AriaProjectCcn)
				    {
					    IncludeInContainer = false,
					    Sort = true,
					    Query = true,
					    Search = true,
					    Refine = true
				    }
			    },
			    {
				    AssetFieldNames.Keys.QuoteNo,
				    new AssetField(AssetFieldNames.AriaQuoteNumber)
				    {
					    IncludeInContainer = false,
					    Sort = true,
					    Query = true,
					    Search = true,
					    Refine = true
				    }
			    },
			    {
				    AssetFieldNames.Keys.HasOrderNumber,
				    new AssetField(AssetFieldNames.AriaProjectHasOrderNumber)
				    {
					    Sort = true,
					    Query = true,
					    Search = true,
					    Refine = true
				    }
			    },
			    {
				    AssetFieldNames.Keys.ProjectExpedited,
				    new AssetField(AssetFieldNames.AriaProjectExpedited)
				    {
					    Query = true,
					    Search = true,
					    Refine = true,
					    DisplayName = "Expedited"
				    }
			    },
			    {AssetFieldNames.Keys.OrderType, new AssetField(AssetFieldNames.AriaProjectOrderType)},
			    {AssetFieldNames.Keys.CustomerPo, new AssetField(AssetFieldNames.AriaProjectCustomerPo)},
			    {
				    AssetFieldNames.Keys.Status,
				    new AssetField(AssetFieldNames.AriaProjectStatus)
				    {
					    Refine = true,
					    Sort = true,
					    DisplayName = "Phase"
				    }
			    },
			    {
				    AssetFieldNames.Keys.ProjectStatus,
				    new AssetField(AssetFieldNames.AriaProjectProjectStatus) {Refine = true, Sort = true}
			    },
			    {
				    AssetFieldNames.Keys.ProjectStatusLabel,
				    new AssetField(AssetFieldNames.AriaProjectProjectStatusLabel) {Sort = true}
			    },
			    {
				    AssetFieldNames.Keys.ProjectTemplateId,
				    new AssetField(AssetFieldNames.AriaProjectTemplateId) {Refine = true, IncludeInContainer = true}
			    },
			    {
				    AssetFieldNames.AriaProjectServiceLineNumber,
				    new AssetField(AssetFieldNames.AriaProjectServiceLineNumber)
			    },
			    // for manually created projects
			    {
				    AssetFieldNames.Keys.IndustryCode,
				    new AssetField(AssetFieldNames.AriaProjectIndustryCode)
				    {
					    Refine = true,
					    Sort = true,
					    Query = true,
					    Retrieve = true,
					    IncludeInContainer = true,
					    Multi = true,
					    SuppressEmpty = true
				    }

			    },
			    {
				    AssetFieldNames.Keys.ServiceCode,
				    new AssetField(AssetFieldNames.AriaProjectServiceCode)
				    {
					    Refine = true,
					    Sort = true,
					    Query = true,
					    Retrieve = true,
					    IncludeInContainer = true,
					    Multi = true,
					    SuppressEmpty = true
				    }
			    },

			    {
				    AssetFieldNames.Keys.Location,
				    new AssetField(AssetFieldNames.AriaProjectLocationName)
				    {
					    Refine = true,
					    Sort = true,
					    Query = true,
					    Retrieve = true,
					    IncludeInContainer = true,
					    Multi = true,
					    SuppressEmpty = true
				    }
			    },
			    // end... for manually created  projects

			    // for order based projects
			    {
				    AssetFieldNames.Keys.ServiceLines,
				    new AssetField(AssetFieldNames.AriaProjectServiceLine) {Ignore = true}
			    },
			    {
				    AssetFieldNames.Keys.IncomingOrderServiceLineIndustryCode,
				    new AssetField(AssetFieldNames.AriaProjectIndustryCode)
				    {
					    Refine = true,
					    Sort = true,
					    Query = true,
					    Retrieve = true,
					    IncludeInContainer = true,
					    Multi = true,
					    SuppressEmpty = true
				    }

			    },
			    {
				    AssetFieldNames.Keys.IncomingOrderServiceLineServiceCode,
				    new AssetField(AssetFieldNames.AriaProjectServiceCode)
				    {
					    Refine = true,
					    Sort = true,
					    Query = true,
					    Retrieve = true,
					    IncludeInContainer = true,
					    Multi = true,
					    SuppressEmpty = true
				    }

			    },
			    // end... for order based projects
			    {
				    AssetFieldNames.Keys.IncomingOrderServiceLineLocationName,
				    new AssetField(AssetFieldNames.AriaProjectLocationName)
				    {
					    Refine = true,
					    Sort = true,
					    Query = true,
					    Retrieve = true,
					    IncludeInContainer = true,
					    Multi = true,
					    SuppressEmpty = true
				    }

			    },

			    {
				    AssetFieldNames.Keys.IncomingOrderServiceLineLocationCode,
				    new AssetField(AssetFieldNames.AriaProjectLocationCode)
				    {
					    Refine = true,
					    Sort = true,
					    Query = true,
					    Retrieve = true,
					    IncludeInContainer = true
				    }

			    },

			    {
				    AssetFieldNames.Keys.CompanyName,
				    new AssetField(AssetFieldNames.AriaCompanyName) {IncludeInContainer = true, Sort = true, Query = true}
			    },
				{AssetFieldNames.Keys.CustomerProjectName, new AssetField(AssetFieldNames.AriaCustomerProjectName){Query = true, Retrieve = true }},
			    {AssetFieldNames.Keys.ProjectTaskMinimumDueDate, new AssetField(AssetFieldNames.AriaProjectTaskMinimumDueDate) {Sort = true, Refine = true, Query = true}},
			    {AssetFieldNames.Keys.BillToPartySiteNumber, new AssetField(AssetFieldNames.AriaBillToContactPartySiteNumber) {Sort = true, Refine = true, Query = true}},
			    {AssetFieldNames.Keys.ShipToPartySiteNumber, new AssetField(AssetFieldNames.AriaShipToContactPartySiteNumber) {Sort = true, Refine = true, Query = true}},
			    {AssetFieldNames.Keys.IncomingOrderContactPartySiteNumber, new AssetField(AssetFieldNames.AriaIncomingOrderContactPartySiteNumber) {Sort = true, Refine = true, Query = true}},
			    {AssetFieldNames.Keys.PartySiteNumber, new AssetField(AssetFieldNames.AriaPartySiteNumber) {Multi = true, Sort = true, Refine = true, Query = true}},
                 
		    };
	    }

	    private static Dictionary<string, AssetField> ConfigureTask()
	    {
		    return new Dictionary<string, AssetField>
		    {
			    {AssetFieldNames.Keys.Id, new AssetField(AssetFieldNames.AriaTaskId)},
			    {AssetFieldNames.Keys.ContainerId, _ariaContainerId},
			    {AssetFieldNames.SharePointAssetId, _ariaAssetId},
			    {AssetFieldNames.Keys.Type, _ariaAssetType},
			    {AssetFieldNames.Keys.Name, new AssetField(AssetFieldNames.AriaTitle) {Sort = true}},
			    {AssetFieldNames.Keys.Status, _ignore},
			    {AssetFieldNames.Keys.Progress, _ignore},
			    {"StatusSearchValue", new AssetField(AssetFieldNames.AriaTaskPhase) {Refine = true, Sort = true}},
			    {"ProgressSearchValue", new AssetField(AssetFieldNames.AriaTaskProgress) {Refine = true, Sort = true}},
			    {
				    AssetFieldNames.Keys.ProgressLabel,
				    new AssetField(AssetFieldNames.AriaTaskProgressLabel)
				    {
					    Sort = true
				    }
			    },
			    {
				    AssetFieldNames.Keys.StatusLabel,
				    new AssetField(AssetFieldNames.AriaTaskPhaseLabel)
				    {
					    Sort = true
				    }
			    },
			    {AssetFieldNames.Keys.TaskOwner, new AssetField(AssetFieldNames.AriaTaskOwner) {Refine = true, Sort = true, IsLowerCase = true}},
			    {AssetFieldNames.Keys.ProjectId, new AssetField(AssetFieldNames.AriaProjectId)},
			    {AssetFieldNames.Keys.TaskNumber, new AssetField(AssetFieldNames.AriaTaskNumber) {Sort = true}},
			    {AssetFieldNames.Keys.Title, new AssetField(AssetFieldNames.AriaTaskTitle) {Sort = true}},
			    {AssetFieldNames.Keys.StartDate, new AssetField(AssetFieldNames.AriaTaskStartDate) {Refine = true, Sort = true, Query = true}},
			    {AssetFieldNames.Keys.DueDate, new AssetField(AssetFieldNames.AriaTaskDueDate) {Refine = true, Sort = true, Query = true}},
			    {AssetFieldNames.Keys.PercentComplete, new AssetField(AssetFieldNames.AriaTaskPercentComplete)},
			    {AssetFieldNames.Keys.ReminderDate, new AssetField(AssetFieldNames.AriaTaskReminderDate) {Refine = true, Sort = true, Query = true}},
			    {AssetFieldNames.Keys.Category, new AssetField(AssetFieldNames.AriaTaskCategory)},
			    {AssetFieldNames.Keys.Modified, new AssetField(AssetFieldNames.AriaTaskModified)},
			    {AssetFieldNames.Keys.ModifiedBy, new AssetField(AssetFieldNames.AriaTaskModifiedBy)},
			    {AssetFieldNames.Keys.UpdatedDateTime, new AssetField(AssetFieldNames.AriaLastModifiedOn){Sort = true}},
			    {AssetFieldNames.Keys.ActualDuration, new AssetField(AssetFieldNames.AriaTaskDuration) {Sort = true}},
			    {AssetFieldNames.AriaTaskChildTaskId, new AssetField(AssetFieldNames.AriaTaskChildTaskId)},
			    {AssetFieldNames.Keys.HasComments, new AssetField(AssetFieldNames.AriaTaskHasComments)},
				{AssetFieldNames.Keys.CompanyId, new AssetField(AssetFieldNames.AriaCompanyId) {IncludeInContainer = true, Refine = true}},
				{AssetFieldNames.Keys.OrderNumber, new AssetField(AssetFieldNames.AriaOrderNumber) {IncludeInContainer = true, Sort = true, Query = true}},
				{AssetFieldNames.Keys.CompanyName, new AssetField(AssetFieldNames.AriaCompanyName) {IncludeInContainer = true, Sort = true, Query = true}},
				{AssetFieldNames.Keys.Description, new AssetField(AssetFieldNames.AriaTaskDescription) { Sort = true, Query = true}},
				{AssetFieldNames.Keys.IsProjectHandlerRestricted, new AssetField(AssetFieldNames.AriaTaskIsProjectHandlerRestricted) {Refine = true, Sort = true, Query = true}},
				{AssetFieldNames.Keys.ShouldTriggerBilling, new AssetField(AssetFieldNames.AriaTaskShouldTriggerBilling) {Query = true}},

		    };
	    }

        private static readonly Dictionary<AssetTypeEnumDto, Dictionary<string, AssetField>> _containerAssetFields = new Dictionary
            <AssetTypeEnumDto, Dictionary<string, AssetField>>
        {
            {AssetTypeEnumDto.Document, ConfigureDocument()},
            {AssetTypeEnumDto.Container, ConfigureContainer()},
            {AssetTypeEnumDto.Order, ConfigureOrder()},
            {AssetTypeEnumDto.Product, ConfigureProduct()},
            {AssetTypeEnumDto.Project, ConfigureProject()},
            {AssetTypeEnumDto.Task, ConfigureTask()},
        };

        static AssetFieldMetadata()
        {
            InitializeSelectPropertiesForAllAssets();
        }

        /// <summary>
        ///     Gets the container asset fields for container.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>IEnumerable{KeyValuePair{System.StringAssetField}}.</returns>
        public IEnumerable<KeyValuePair<string, AssetField>> GetContainerAssetFieldsForContainer(
            AssetTypeEnumDto assetType)
        {
            return _containerAssetFields[assetType].Where(a => a.Value.IncludeInContainer);
        }

        /// <summary>
        ///     Gets the container asset fields for container.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>IEnumerable{KeyValuePair{System.StringAssetField}}.</returns>
        public IEnumerable<KeyValuePair<string, AssetField>> GetContainerAssetFieldsForContainer(
            EntityTypeEnumDto? entityType)
        {
            var entityTypeSpecified = entityType == null ? EntityTypeEnumDto.Container : entityType.Value;
            var assetType = (AssetTypeEnumDto) Enum.Parse(typeof (AssetTypeEnumDto), entityTypeSpecified.ToString());
            return GetContainerAssetFieldsForContainer(assetType);
        }

        /// <summary>
        ///     Gets the container asset fields.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>Dictionary{System.StringAssetField}.</returns>
        public Dictionary<string, AssetField> GetContainerAssetFields(AssetTypeEnumDto assetType)
        {
            return _containerAssetFields[assetType];
        }

        /// <summary>
        ///     Gets the container asset fields.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>Dictionary{System.StringAssetField}.</returns>
        public Dictionary<string, AssetField> GetContainerAssetFields(EntityTypeEnumDto? entityType)
        {
            var entityTypeSpecified = entityType == null ? EntityTypeEnumDto.Container : entityType.Value;
            var assetType = (AssetTypeEnumDto) Enum.Parse(typeof (AssetTypeEnumDto), entityTypeSpecified.ToString());
            return GetContainerAssetFields(assetType);
        }

        /// <summary>
        ///     Gets the container asset field.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="assetFieldPropertyCharacteristicName">Name of the asset field property or characteristic.</param>
        /// <returns>AssetField.</returns>
        public AssetField GetContainerAssetField(AssetTypeEnumDto assetType,
            string assetFieldPropertyCharacteristicName)
        {
            Guard.IsNotNullOrEmpty(assetFieldPropertyCharacteristicName, "AssetFieldPropertyCharacteristicName");

            var containerAssetFields = GetContainerAssetFields(assetType);

            return containerAssetFields.ContainsKey(assetFieldPropertyCharacteristicName)
                ? containerAssetFields[assetFieldPropertyCharacteristicName]
                : null;
        }

        /// <summary>
        ///     Gets the container asset field.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="assetFieldPropertyCharacteristicName">Name of the asset field property characteristic.</param>
        /// <returns>System.Nullable{AssetField}.</returns>
        public AssetField GetContainerAssetField(EntityTypeEnumDto? entityType,
            string assetFieldPropertyCharacteristicName)
        {
            var entityTypeSpecified = entityType == null ? EntityTypeEnumDto.Container : entityType.Value;
            var assetType = (AssetTypeEnumDto) Enum.Parse(typeof (AssetTypeEnumDto), entityTypeSpecified.ToString());
            return GetContainerAssetField(assetType, assetFieldPropertyCharacteristicName);
        }

        /// <summary>
        ///     Gets the select properties.
        /// </summary>
        /// <param name="assetType">Type of the asset.</param>
        /// <returns>System.String.</returns>
        public string GetSelectProperties(AssetTypeEnumDto assetType)
        {
            return _selectProperties[assetType];
        }

        /// <summary>
        ///     Gets the select properties.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>System.String.</returns>
        public string[] GetSelectProperties(EntityTypeEnumDto? entityType)
        {
            var entityTypeSpecified = entityType == null ? EntityTypeEnumDto.Container : entityType.Value;
            var assetType = (AssetTypeEnumDto) Enum.Parse(typeof (AssetTypeEnumDto), entityTypeSpecified.ToString());
            var csv = GetSelectProperties(assetType);

            // turn csv into list
            var chrArray = new[] {','};
            var list = csv.Split(chrArray, StringSplitOptions.RemoveEmptyEntries);

            return list;
        }

        private static void InitializeSelectPropertiesForAllAssets()
        {
            foreach (var containerAssetFields in _containerAssetFields)
                _selectProperties.Add(containerAssetFields.Key, GetSelectPropertiesForAsset(containerAssetFields.Key));
        }

        private static string GetSelectPropertiesForAsset(AssetTypeEnumDto assetType)
        {
            var stringBuilder = new StringBuilder();
            var assetFieldNames = new List<string>();

            if (assetType == AssetTypeEnumDto.Container)
                foreach (var containerAssetFields in _containerAssetFields)
                    GetSelectPropertiesForAsset(assetFieldNames, stringBuilder, containerAssetFields.Value);
            else
                GetSelectPropertiesForAsset(assetFieldNames, stringBuilder, _containerAssetFields[assetType]);

            stringBuilder.Length--;

            return stringBuilder.ToString();
        }

        private static void GetSelectPropertiesForAsset(ICollection<string> assetFieldNames, StringBuilder stringBuilder,
            IEnumerable<KeyValuePair<string, AssetField>> containerAssetFields)
        {
            foreach (
                var assetField in
                    containerAssetFields.Select(assetFieldKeyValuePair => assetFieldKeyValuePair.Value)
                        .Where(
                            assetField =>
                                !assetField.Ignore))
            {
                var assetFieldName = assetFieldNames.FirstOrDefault(x => x == assetField.Name);

                if (assetFieldName == null)
                {
                    assetFieldNames.Add(assetField.Name);
                    stringBuilder
                        .Append(assetField.Name)
                        .Append(",");
                }
            }

            //
            // Include asset fields that are marked include in container from all assets
            //
            foreach (var includeInContainerContainerAssetFields in _containerAssetFields)
            {
                foreach (
                    var assetField in
                        includeInContainerContainerAssetFields.Value.Select(
                            assetFieldKeyValuePair => assetFieldKeyValuePair.Value)
                            .Where(
                                assetField =>
                                    assetField.IncludeInContainer))
                {
                    var assetFieldName = assetFieldNames.FirstOrDefault(x => x == assetField.Name);

                    if (assetFieldName == null)
                    {
                        assetFieldNames.Add(assetField.Name);
                        stringBuilder
                            .Append(assetField.Name)
                            .Append(",");
                    }
                }
            }
        }

        /// <summary>
        ///     Struct AssetField
        /// </summary>
        public class AssetField
        {
            private string _name;

            /// <summary>
            ///     Initializes a new instance of the <see cref="AssetField" /> struct.
            /// </summary>
            public AssetField(string name)
            {
                Name = name;
                Search = true;
                Query = true;
                Retrieve = true;

                Alias = null;
                DisplayName = null;
                Ignore = false;
                IncludeInContainer = false;
                IsCharacteristic = false;
                IsLowerCase = false;
                Multi = false;
                Refine = false;
                Safe = false;
                Sort = false;
                SuppressEmpty = false;
            }

            /// <summary>
            /// Gets or sets a value indicating whether [suppress emtpy].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [suppress emtpy]; otherwise, <c>false</c>.
            /// </value>
            public bool SuppressEmpty { get; set; }

            /// <summary>
            ///     The alias
            /// </summary>
            public string Alias { get; set; }

            /// <summary>
            ///     The GUI display name
            /// </summary>
            public string DisplayName { get; set; }

            /// <summary>
            ///     The ignore
            /// </summary>
            public bool Ignore { get; set; }

            /// <summary>
            ///     The include in container
            /// </summary>
            public bool IncludeInContainer { get; set; }

            /// <summary>
            ///     The is characteristic
            /// </summary>
            public bool IsCharacteristic { get; set; }

            /// <summary>
            ///     The is lowercase
            /// </summary>
            public bool IsLowerCase { get; set; }

            /// <summary>
            ///     The asset field flag that tells if it is mutil value (true) or not (false)
            /// </summary>
            public bool Multi { get; set; }

            /// <summary>
            ///     The asset field flag that tells if it is queryable (true) or not (false)
            /// </summary>
            public bool Query { get; set; }

            /// <summary>
            ///     The asset field flag that tells if it is refinable (true) or not (false) (If refinable also must be queryable but
            ///     this is done automatically no need to set queryable too)
            /// </summary>
            public bool Refine { get; set; }

            /// <summary>
            ///     The asset field flag that tells if it is retrievable (true) or not (false)
            /// </summary>
            public bool Retrieve { get; set; }

            /// <summary>
            ///     The asset field flag that tells if it is safe (true) or not (false)
            /// </summary>
            public bool Safe { get; set; }

            /// <summary>
            ///     The asset field flag that tells if it is searchable (true) or not (false)
            /// </summary>
            public bool Search { get; set; }

            /// <summary>
            ///     The asset field flag that tells if it is sortable (true) or not (false)
            /// </summary>
            public bool Sort { get; set; }

            /// <summary>
            ///     The SharePoint metadata name, only needed if property name on class isn't correct.  If not provide builds name from
            ///     property name as follows: aria[type][propertyname] (aria[type] prefix ex. if name is 'MyName' and container type is
            ///     'Product' then the metadata name will be 'ariaProductMyName')
            /// </summary>
            public string Name
            {
                get { return _name; }
                set { _name = value == null ? null : value.Substring(0, Math.Min(64, value.Length)); }
            }
        }
    }
}