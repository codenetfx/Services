using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.UI.WebControls;
using AutoMapper;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Dto.Integration;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Message.Domain;
using UL.Aria.Service.Relay.Domain;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Relay.Common
{
	/// <summary>
	/// Mapper registry for <see cref="UL.Aria.Service.Relay"/>
	/// </summary>
	public class ServiceMapperRegistry : MapperEngineMapperRegistryBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceMapperRegistry"/> class.
		/// </summary>
		public ServiceMapperRegistry()
		{
            Configuration.AddGlobalIgnore("RecordAction");
            Configuration.CreateMap<IncomingOrderServiceLineDto, OrderServiceLine>();
		    Configuration.CreateMap<ProjectDto, FulfillmentProject>()
		        .ConvertUsing<ProjectDtoToFullfilmentProjectConverter>();
            Configuration.CreateMap<RelaySearchResultSet<ProjectDto>, FulfillmentOrderProjectSearchResponse>()
                .ConvertUsing<RelaySearchResultSetToFulfillmentOrderProjectSearchResponse>();
			Configuration.CreateMap<OrderMessage, OrderMessageDto>();
			Configuration.CreateMap<OrderMessageDto, OrderMessage>()
				.ConstructUsing((OrderMessageDto x) => new OrderMessage(Guid.NewGuid()))
				.ForMember(x => x.Id, y => y.Ignore());
			Configuration.CreateMap<DocumentDto, Document>()
				.ConstructUsing(x => new Document(x.Id))
				.ForMember(x => x.Id, y => y.Ignore());
			Configuration.CreateMap<Document, DocumentDto>();
		}

	    // ReSharper disable once ClassNeverInstantiated.Local
        private class RelaySearchResultSetToFulfillmentOrderProjectSearchResponse : TypeConverter<RelaySearchResultSet<ProjectDto>, FulfillmentOrderProjectSearchResponse>
	    {
            protected override FulfillmentOrderProjectSearchResponse ConvertCore(RelaySearchResultSet<ProjectDto> source)
            {
                var fulfillmentRefiners = new FulfillmentRefiners();
                foreach (var refinerResult in source.RefinerResults)
                {
                    FulfillmentProjectRefinerField fieldName;
                   
                    if (ParseRefinerField(refinerResult.Key, out fieldName))
                    {
                        var results = new FulfillmentProjectRefinerResults();
                        results.AddRange(refinerResult.Value.Select(val => new FulfillmentProjectRefinerResult {Count = val.Count, Value = val.Value}));
                        var refiner = new FulfillmentProjectRefinerResultSet
                        {
                            RefinerField = fieldName,
                            RefinerResults = results
                        };
                        fulfillmentRefiners.Add(refiner);
                    }
                }
                return new FulfillmentOrderProjectSearchResponse(){Refiners = fulfillmentRefiners}; 
            }

            private static bool ParseRefinerField(string refinerResultKey, out FulfillmentProjectRefinerField field)
            {
                field = default(FulfillmentProjectRefinerField);
                if (refinerResultKey.Equals(AssetFieldNames.AriaProjectProjectStatus,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    field = FulfillmentProjectRefinerField.ProjectStatus;
                    return true;
                }
                else if (refinerResultKey.Equals(AssetFieldNames.AriaOrderNumber,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    field = FulfillmentProjectRefinerField.OrderNumber;
                    return true;
                }
                return false;
            }
	    }

	    // ReSharper disable once ClassNeverInstantiated.Local
		private class ProjectDtoToFullfilmentProjectConverter : TypeConverter<ProjectDto, FulfillmentProject>
		{
			protected override FulfillmentProject ConvertCore(ProjectDto dto)
			{
				var fulfillmentProject = new FulfillmentProject
				{
                    ContainerId = dto.ContainerId,
					CustomerCompanyName = dto.CompanyName,
					EstimatedCompletionDate = dto.EndDate.HasValue
						? new DateTime?(new DateTime(dto.EndDate.Value.Ticks, DateTimeKind.Utc))
						: null,
					FulfillmentId = dto.Id.GetValueOrDefault().ToString("N"),
					FulfillmentProjectName = dto.Name,
					FulfillmentStatus = dto.ProjectStatus.ToString(),
                    FulfillmentStatusNotes  = dto.StatusNotes,
                    FulfillmentProjectCreationDate  = dto.CreationDate,
					OrderLineNumbers = dto.ServiceLines.Select(x => x.LineNumber).ToList(),
					OrderNumber = dto.OrderNumber,
					ProductCcn = dto.CCN,
					ProductFileNumber = dto.FileNo,
					ProjectHandlerId = ExtractEmployeeIdFromLoginId(dto.ProjectHandler),
					ProjectHandler = dto.ProjectHandler,
					ProjectNumber = dto.ProjectNumber,
					QuoteNumber = dto.QuoteNo,
					SystemId = "50",
					IsReadOnly = dto.ProjectStatus == ProjectStatusEnumDto.Completed ||
								 dto.ProjectStatus == ProjectStatusEnumDto.Canceled,
                    
				};
                fulfillmentProject.OrderLines = new FulfillmentProjectOrderServiceLines();
                if (null !=dto.ServiceLines)
                {
                    fulfillmentProject.OrderLines.AddRange(dto.ServiceLines.Select(x =>
                {
                    return new OrderServiceLine
                    {
                        AllowChargesFromOtherOperatingUnits = x.AllowChargesFromOtherOperatingUnits,
                        ApplicationObjectKeyId = x.ApplicationObjectKeyId,
                        Billable = x.Billable,
                        BillableExpenses = x.BillableExpenses,
                        Category = x.Category,
                        ClientDetailService = x.ClientDetailService,
                        ConfigurationId = x.ConfigurationId,
                        CreatedById = x.CreatedById,
                        CreatedDateTime = x.CreatedDateTime,
                        Currency = x.Currency,
                        CustomerModelNumber = x.CustomerModelNumber,
                        DetailedService = x.DetailedService,
                        Description = x.Description,
                        DetailedServiceDescription = x.DetailedServiceDescription,
                        FulfillmentMethodCode = x.FulfillmentMethodCode,
                        FulfillmentSet = x.FulfillmentSet,
                        Hold = x.Hold,
                        Id = x.Id,
                        IncomingOrderId = x.IncomingOrderId,
                        Industry = x.Industry,
                        IndustryCategory = x.IndustryCategory,
                        IndustryCategoryDescription = x.IndustryCategoryDescription,
                        IndustryCode = x.IndustryCode,
                        IndustryCodeLabel = x.IndustryCodeLabel,
                        IndustryDescription = x.IndustryDescription,
                        IndustrySubCategory = x.IndustrySubCategory,
                        IndustrySubCategoryDescription = x.IndustrySubCategoryDescription,
                        LineNumber = x.LineNumber,
                        LocationCode = x.LocationCode,
                        LocationCodeLabel = x.LocationCodeLabel,
                        LocationName = x.LocationName,
                        Name = x.Name,
                        Price = x.Price,
                        ParentExternalId = x.ParentExternalId,
                        ProductGroup = x.ProductGroup,
                        ProductGroupDescription = x.ProductGroupDescription,
                        ProductType = x.ProductType,
                        Program = x.Program,
                        ProductTypeDescription = x.ProductTypeDescription,
                        PromiseDate = x.PromiseDate,
                        Quantity = x.Quantity,
                        RequestDate = x.RequestDate,
                        Segment = x.Segment,
                        ServiceCategory = x.ServiceCategory,
                        ServiceCategoryDescription = x.ServiceCategoryDescription,
                        ServiceCode = x.ServiceCode,
                        ServiceCodeLabel = x.ServiceCodeLabel,
                        ServiceProgram = x.ServiceProgram,
                        ServiceProgramDescription = x.ServiceProgramDescription,
                        ServiceSegment = x.ServiceSegment,
                        ServiceSegmentDescription = x.ServiceSegmentDescription,
                        ServiceSubCategory = x.ServiceSubCategory,
                        ServiceSubCategoryDescription = x.ServiceSubCategoryDescription,
                        StartDate = x.StartDate,
                        Status = x.Status,
                        SubCategory = x.SubCategory,
                        TypeCode = x.TypeCode,
                        UpdatedById = x.UpdatedById,
                        UpdatedDateTime = x.UpdatedDateTime,
                        WorkOrderLineBusinessComponentId = x.WorkOrderLineBusinessComponentId,
                        WorkOrderLineId = x.WorkOrderLineId
                    };
                }));}
				if (null != dto.ShipToContact)
				{
					fulfillmentProject.PartySiteNumber = SanitizeString(dto.ShipToContact.PartySiteNumber).ParseOrDefault(null as int?);
					fulfillmentProject.SubscriberNumber =
						SanitizeString(dto.ShipToContact.SubscriberNumber).ParseOrDefault(null as int?);
				}
				if (null != dto.IncomingOrderCustomer && string.IsNullOrWhiteSpace(dto.CompanyName))
				{
					fulfillmentProject.CustomerCompanyName = dto.IncomingOrderCustomer.Name;
				}
				return fulfillmentProject;
			}

			private int? ExtractEmployeeIdFromLoginId(string value)
			{
				if (null == value)
					return null;
				return value.Split('@')[0].ParseOrDefault(null as int?);
			}

			private string SanitizeString(string value)
			{
				if (null == value)
					return null;
				return value.Replace("-", string.Empty);
			}
		}
	}
}