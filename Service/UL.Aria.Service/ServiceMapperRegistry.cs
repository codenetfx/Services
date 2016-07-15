using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Claim.Data;
using UL.Aria.Service.Claim.Domain;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Domain.Value;
using UL.Aria.Service.Domain.View;
using UL.Aria.Service.MapperResolver;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service
{
    /// <summary>
    /// Host project mapper registry class.
    /// </summary>
    public class ServiceMapperRegistry : MapperEngineMapperRegistryBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceMapperRegistry" /> class.
        /// </summary>
        // ReSharper disable once FunctionComplexityOverflow
        public ServiceMapperRegistry()
        {
            Configuration.AddGlobalIgnore("RecordAction");
            //Configuration.CreateMap<object, UL.Enterprise.Foundation.Domain.DomainEntity>()         
            //    .ForMember(dest => dest.RecordAction, opt => opt.Ignore()).ReverseMap();

            Configuration.CreateMap<ProfileBo, ProfileDto>()
                .ForMember(x => x.EmployeeId, opt => opt.Ignore());

            Configuration.CreateMap<ClaimDefinitionDto, ClaimDefinition>()
                .ForMember(x => x.Id, opt => opt.UseValue(Guid.NewGuid()));
            Configuration.CreateMap<ClaimDefinition, ClaimDefinitionDto>();

            Configuration.CreateMap<PrimarySearchEntityBase, PrimarySearchEntityBaseDto>();
            Configuration.CreateMap<PrimarySearchEntityBaseDto, PrimarySearchEntityBase>()
                .ForMember(x => x.ContainerId, opt => opt.Ignore())
                .ForMember(x => x.CreatedById, opt => opt.Ignore())
                .ForMember(x => x.CreatedDateTime, opt => opt.Ignore())
                .ForMember(x => x.UpdatedById, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDateTime, opt => opt.Ignore());
            Configuration.CreateMap<ContainerRole, ContainerRoleDto>();
            Configuration.CreateMap<ContainerRoleDto, ContainerRole>();

            Configuration.CreateMap<ProfileBasicDto, ProfileEditBasicBo>();
            Configuration.CreateMap<ProfileEditBasicBo, ProfileBasicDto>()
                .ForMember(x => x.EmployeeId, opt => opt.Ignore());


            Configuration.CreateMap<ProfileImageBo, ProfileImageDto>();
            Configuration.CreateMap<ProfileImageDto, ProfileImageBo>();

            CreateCompanyMaps();

            CreateClaimMaps();

            Configuration.CreateMap<Guid, string>().ConvertUsing<GuidToStringConverter>();
            Configuration.CreateMap<string, Guid>().ConvertUsing<StringToGuidConverter>();
            Configuration.CreateMap<Uri, string>().ConvertUsing<UriToStringConverter>();
            Configuration.CreateMap<string, Uri>().ConvertUsing<StringToUriConverter>();


            Configuration.CreateMap<ProfileSearchSpecificationDto, ProfileSearchSpecification>();
            Configuration.CreateMap<ProfileSearchSpecificationDto, SearchCriteria>()
                .ForSourceMember(x => x.ContainerId, y => y.Ignore())
                .ForMember(x => x.EndIndex, y => y.Ignore())
                .ForMember(x => x.EntityType, y => y.Ignore())
                .ForMember(x => x.StartIndex, y => y.Ignore())
                .ForMember(x => x.Refiners, y => y.Ignore())
                .ForMember(x => x.Filters, y => y.Ignore())
                .ForMember(x => x.Sorts, y => y.Ignore())
                .ForMember(x => x.UserId, y => y.Ignore())
                .ForMember(x => x.IncludeDeletedRecords, opt => opt.Ignore())
                .ForMember(x => x.FilterContainers, opt => opt.Ignore())
				.ForMember(x => x.AdditionalFilterString, opt => opt.Ignore())
                .ForMember(x => x.SearchCoordinators, opt => opt.Ignore());

            Configuration.CreateMap<ProfileSearchSpecification, ProfileSearchSpecificationDto>();


            Configuration.CreateMap<DocumentType, DocumentTypeDto>();
            Configuration.CreateMap<DocumentTypeDto, DocumentType>();

            //
            // search
            //
            CreateSearchMaps();

            //
            //Product Family mappings
            //
            Configuration.CreateMap<ProductFamilyDto, ProductFamily>();
            Configuration.CreateMap<ProductFamily, ProductFamilyDto>();

            Configuration.CreateMap<KeyValuePair<Guid, ProductFamily>, ProductFamilyIdValuePairDto>()
                .ConvertUsing(x => new ProductFamilyIdValuePairDto
                {
                    Id = x.Key,
                    Value = MappingEngine.Map<ProductFamilyDto>(x.Value)
                });
            Configuration.CreateMap<IReadOnlyDictionary<Guid, ProductFamily>, ProductFamiliesDto>()
                .ConvertUsing(
					x => new ProductFamiliesDto {ProductFamilies = MappingEngine.Map<ProductFamilyIdValuePairDto[]>(x)});

            //
            // Product Family Attribute mappings
            //
            Configuration.CreateMap<ProductFamilyFeatureDto, ProductFamilyFeature>()
                .ForMember(x => x.AllowedValues, opt => opt.Ignore());
            Configuration.CreateMap<ProductFamilyFeature, ProductFamilyFeatureDto>();
            Configuration.CreateMap<ProductFamilyAttributeDto, ProductFamilyAttribute>();
            Configuration.CreateMap<ProductFamilyAttribute, ProductFamilyAttributeDto>();
            Configuration.CreateMap<ProductFamilyCharacteristicOption, ProductFamilyCharacteristicOptionDto>();
            Configuration.CreateMap<ProductFamilyCharacteristicOptionDto, ProductFamilyCharacteristicOption>();

            CreateIncomingOrderMaps();
            CreateProductMaps();
            CreateProductUploadMaps();

            // NOTE: This is for SCRATCH SPACE
            Configuration.CreateMap<ScratchFileInfo, ScratchFileDescriptorDto>();
            Configuration.CreateMap<ScratchFileDescriptorDto, ScratchFileInfo>();

            // Terms And Conditions Service
            Configuration.CreateMap<TermsAndConditions, TermsAndConditionsDto>()
                .ForMember(x => x.Type, opt => opt.MapFrom(x => x.Type.ToString("G")));

            Configuration.CreateMap<TermsAndConditionsDto, TermsAndConditions>()
				.ForMember(x => x.Type, opt => opt.MapFrom(x => Enum.Parse(typeof (TermsAndConditionsType), x.Type)))
                .ForMember(x => x.Id, opt => opt.Ignore());

            //
            // Container
            //
            Configuration.CreateMap<Container, ContainerDto>();
            Configuration.CreateMap<ContainerDto, Container>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ConstructUsing(x => new Container(x.Id));
            Configuration.CreateMap<ContainerList, ContainerListDto>();
            Configuration.CreateMap<ContainerListDto, ContainerList>();
            Configuration.CreateMap<ContainerListPermission, ContainerListPermissionDto>();
            Configuration.CreateMap<ContainerListPermissionDto, ContainerListPermission>();
            Configuration.CreateMap<ContainerAvailableClaim, ContainerAvailableClaimDto>();
            Configuration.CreateMap<ContainerAvailableClaimDto, ContainerAvailableClaim>();

            //
            // Claim
            //
            Configuration.CreateMap<Domain.Entity.Claim, ClaimDto>();
            Configuration.CreateMap<ClaimDto, Domain.Entity.Claim>();

            //
            // Task
            //
            Configuration.CreateMap<Task, Task>()
                .ForMember(x => x.Project, x => x.Ignore())
                .ForMember(x => x.SubTasks, x => x.Ignore())
                .ForMember(x => x.Parent, x => x.Ignore())
                .ForMember(x => x.SuccessorRefs, x => x.Ignore())
                .ForMember(x => x.PredecessorRefs, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.PrimarySearchEntityId, x => x.Ignore())
                .ForMember(x => x.PrimarySearchEntityType, x => x.Ignore())
                .ForMember(x => x.TaskTypeBehaviors, x => x.Ignore());

			Configuration.CreateMap<TaskStatusList, TaskStatusListDto>();
	        Configuration.CreateMap<Task, TaskDto>();
				

            Configuration.CreateMap<Task, TaskSearchResultDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(x => x.Title))
                .ForMember(x => x.EntityType, opt => opt.UseValue(EntityTypeEnumDto.Task))
                .ForMember(x => x.ChangeDate, opt => opt.MapFrom(x => x.UpdatedDateTime))
                .ForMember(x => x.Metadata, opt => opt.Ignore())           
                .ForMember(x => x.DueDate, opt => opt.MapFrom(x => x.DueDate))
                .ForMember(x => x.StartDate, opt => opt.MapFrom(x => x.StartDate))
                .ForMember(x => x.ReminderDate, opt => opt.MapFrom(x => x.ReminderDate))
				.ForMember(x => x.PreventDeletion, opt => opt.MapFrom(x => x.PreventDeletion))
				.ForMember(x => x.ShouldTriggerBilling, opt => opt.MapFrom(x => x.ShouldTriggerBilling))
				.ForMember(x => x.ProjectTaskShouldTriggerBillingCount, opt => opt.MapFrom(x => x.ProjectTaskShouldTriggerBillingCount))
                .ForMember(x => x.Documents, opt => opt.MapFrom(x => x.Documents))
                .ForMember(x => x.StatusList, opt => opt.MapFrom(x => x.StatusList))
				.ForMember(x=>x.Predecessors, opt=>opt.MapFrom(x=>x.Predecessors))
                .ForMember(x => x.IsProjectHandlerRestricted, opt => opt.MapFrom(x => x.IsProjectHandlerRestricted));

            Configuration.CreateMap<TaskDto, Task>()
                .ForMember(x => x.ContainerId, opt => opt.Ignore())
                .ForMember(x => x.Type, x => x.Ignore())
                .ForMember(x => x.Name, x => x.Ignore())
                .ForMember(x => x.CompanyId, x => x.Ignore())
				.ForMember(x => x.CompanyName, x => x.Ignore())
				.ForMember(x => x.OrderNumber, x => x.Ignore())
                .ForMember(x => x.CreatedDateTime, x => x.Ignore())
                .ForMember(x => x.UpdatedDateTime, x => x.Ignore())
                .ForMember(x => x.UpdatedById, x => x.Ignore())
                .ForMember(x => x.CompletedDate, x => x.Ignore())
                .ForMember(x => x.CreationOrder, x => x.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Documents, opt => opt.Ignore())
                .ForMember(x => x.StatusList, opt => opt.Ignore())
                .ForMember(x => x.LastComment, opt => opt.Ignore())
                 .ForMember(x => x.Project, x => x.Ignore())
                .ForMember(x => x.Parent, x => x.Ignore())
                .ForMember(x => x.SuccessorRefs, x => x.Ignore())
                .ForMember(x => x.PredecessorRefs, x => x.Ignore())
				.ForMember(x=>x.TaskTypeBehaviors, opt=>opt.Ignore())
                .ConstructUsing(x => new Task(x.Id));

            Configuration.CreateMap<TaskPredecessor, TaskPredecessorDto>();
            Configuration.CreateMap<TaskPredecessorDto, TaskPredecessor>();
            Configuration.CreateMap<TaskComment, TaskCommentDto>();
            Configuration.CreateMap<TaskCommentDto, TaskComment>();

            Configuration.CreateMap<TaskStatusHistory, TaskStatusHistoryDto>();
            Configuration.CreateMap<TaskStatusHistoryDto, TaskStatusHistory>();

            Configuration.CreateMap<TaskCompletionHistory, TaskCompletionHistoryDto>();
            Configuration.CreateMap<TaskCompletionHistoryDto, TaskCompletionHistory>();

            Configuration.CreateMap<TaskComment, TaskCommentDto>();
            Configuration.CreateMap<TaskCommentDto, TaskComment>();

            Configuration.CreateMap<TaskHistory, TaskHistoryDto>();
            Configuration.CreateMap<TaskHistoryDto, TaskHistory>();

            Configuration.CreateMap<TaskNotification, TaskNotificationDto>();
            Configuration.CreateMap<TaskNotificationDto, TaskNotification>();

            Configuration.CreateMap<TaskTypeNotification, TaskTypeNotificationDto>();
            Configuration.CreateMap<TaskTypeNotificationDto, TaskTypeNotification>();

            Configuration.CreateMap<ProjectTemplate, ProjectTemplateDto>()
                .ForMember(x => x.TaskTemplates, x => x.MapFrom(src => (src.TaskTemplates != null)
					? src.TaskTemplates.Select(z => MappingEngine.Map<TaskTemplateDto>(z)).ToList()
					: new List<TaskTemplateDto>()));

            Configuration.CreateMap<ProjectTemplateDto, ProjectTemplate>()
                .ForMember(x => x.Metadata, x => x.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.TaskTemplates, x => x.MapFrom(src => (src.TaskTemplates != null)
					? src.TaskTemplates.Select(z => MappingEngine.Map<TaskTemplate>(z)).ToList()
					: new List<TaskTemplate>()))
                .ConstructUsing(x => new ProjectTemplate(x.Id));

            Configuration.CreateMap<TaskTemplate, TaskTemplateDto>()
                 .ForMember(x => x.SubTasks, x => x.MapFrom(src => (src.SubTasks != null)
					? src.SubTasks.Select(z => MappingEngine.Map<TaskTemplateDto>(z)).ToList()
					: new List<TaskTemplateDto>()));

            Configuration.CreateMap<TaskTemplateDto, TaskTemplate>()
                 .ForMember(x => x.SubTasks, x => x.MapFrom(src => (src.SubTasks != null)
					? src.SubTasks.Select(z => MappingEngine.Map<TaskTemplate>(z)).ToList()
					: new List<TaskTemplate>()))
                .ForMember(x => x.UpdatedById, x => x.Ignore())
                .ForMember(x => x.UpdatedDateTime, x => x.Ignore())
                .ForMember(x => x.ContainerId, x => x.Ignore())
                .ForMember(x => x.CreatedById, x => x.Ignore())
                .ForMember(x => x.CreationOrder, x => x.Ignore())
                .ForMember(x => x.CreatedDateTime, x => x.Ignore())
                .ForMember(x => x.Created, x => x.Ignore())
                .ForMember(x => x.CompanyId, x => x.Ignore());

            //used for Clone new operation
            Configuration.CreateMap<ProjectTemplate, ProjectTemplate>()
              .ForMember(x => x.Id, x => x.Ignore())
              .ForMember(x => x.CorrelationId, x => x.Ignore());

            //used for Clone new operation
            Configuration.CreateMap<TaskTemplate, TaskTemplate>()
				.ForMember(x => x.Id, x => x.MapFrom(y => Guid.NewGuid()))
				.ForMember(x => x.ProjectTemplateId, x => x.Ignore())
               .ForMember(x => x.SubTasks, x => x.MapFrom(src => (src.SubTasks != null)
					? src.SubTasks.Select(z => MappingEngine.Map<TaskTemplate>(z)).ToList()
					: new List<TaskTemplate>()));

            //used for Clone new operation
            Configuration.CreateMap<BusinessUnit, BusinessUnit>()
                 .ForMember(x => x.Id, opt => opt.Ignore());


            Configuration.CreateMap<TaskTemplate, Task>()
				.ConstructUsing((TaskTemplate y) => new Task(Guid.NewGuid()))
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Type, opt => opt.Ignore())
                .ForMember(x => x.Comments, opt => opt.Ignore())
                .ForMember(x => x.StatusHistories, opt => opt.Ignore())
                .ForMember(x => x.CompletionHistories, opt => opt.Ignore())
                .ForMember(x => x.ContainerId, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.CompanyId, opt => opt.Ignore())
                .ForMember(x => x.CompanyName, x => x.Ignore())
                .ForMember(x => x.OrderNumber, x => x.Ignore())
                .ForMember(x => x.ParentId, opt => opt.Ignore())
                .ForMember(x => x.ParentTaskNumber, opt => opt.Ignore())
                .ForMember(x => x.ChildTaskNumbers, opt => opt.Ignore())
                .ForMember(x => x.CreatedById, opt => opt.Ignore())
                .ForMember(x => x.CreatedDateTime, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDateTime, opt => opt.Ignore())
                .ForMember(x => x.UpdatedById, opt => opt.Ignore())
                .ForMember(x => x.ReminderDate, opt => opt.Ignore())
                .ForMember(x => x.IsDeleted, opt => opt.Ignore())
                .ForMember(x => x.LastDocumentAdded, opt => opt.Ignore())
                .ForMember(x => x.LastDocumentRemoved, opt => opt.Ignore())
                .ForMember(x => x.PrimarySearchEntityId, opt => opt.Ignore())
                .ForMember(x => x.PrimarySearchEntityType, opt => opt.Ignore())
                .ForMember(x => x.HasComments, opt => opt.Ignore())
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.TaskTypeName, opt => opt.Ignore())
				.ForMember(x => x.Progress, opt => opt.MapFrom(y => TaskProgressEnumDto.OnTrack))
                .ForMember(x => x.TaskTemplateId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Documents, opt => opt.Ignore())
                .ForMember(x => x.LastComment, opt => opt.Ignore())
                .ForMember(x => x.Project, x => x.Ignore())
                .ForMember(x => x.Parent, x => x.Ignore())
                .ForMember(x => x.SuccessorRefs, x => x.Ignore())
                .ForMember(x => x.PredecessorRefs, x => x.Ignore())
                .ForMember(x => x.PreventDeletion, opt => opt.Ignore())
                .ForMember(x => x.ProjectTaskShouldTriggerBillingCount, opt => opt.Ignore())
                .ForMember(x => x.StatusList, opt => opt.Ignore())
                .ForMember(x => x.RecordVersion, opt => opt.Ignore())
                .ForMember(x => x.Notifications, opt => opt.Ignore())
				.ForMember(x => x.TaskTypeBehaviors, opt => opt.Ignore())
				.ForMember(x => x.IsReActivateRequest, opt => opt.Ignore())
                ;
               
             
            Configuration.CreateMap<FavoriteSearch, FavoriteSearchDto>();
            Configuration.CreateMap<FavoriteSearchDto, FavoriteSearch>();
            Configuration.CreateMap<FavoriteSearchDto, PartialUpdateFavoriteSearch>();
            Configuration.CreateMap<FavoriteSearchSearchResult, FavoriteSearchSearchModelDto>();
            Configuration.CreateMap<ProjectStatusMessage, ProjectStatusMessageDto>();
            Configuration.CreateMap<ProjectStatusMessageDto, ProjectStatusMessage>();

            Configuration.CreateMap<ProjectStatusMessageServiceLineDto, ProjectStatusMessageServiceLine>();
            Configuration.CreateMap<ProjectStatusMessageServiceLine, ProjectStatusMessageServiceLineDto>();

            Configuration.CreateMap<Category, CategoryDto>();
            Configuration.CreateMap<CategoryDto, Category>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ConstructUsing(x => new Category(x.Id));

            Configuration.CreateMap<UnitOfMeasure, UnitOfMeasureDto>();
            Configuration.CreateMap<UnitOfMeasureDto, UnitOfMeasure>();

            Configuration.CreateMap<EmailContactUsDto, ContactUs>();

            Configuration.CreateMap<ProductFamilyFeatureValue, ProductFamilyFeatureValueDto>();
            Configuration.CreateMap<ProductFamilyFeatureValueDto, ProductFamilyFeatureValue>();

            Configuration.CreateMap<ProductFamilyCharacteristicUploadDto, ProductFamilyCharacteristicUpload>();
            Configuration.CreateMap<ProductFamilyCharacteristicDomainEntityDto, ProductFamilyCharacteristicDomainEntity>();

            Configuration.CreateMap<ProductFamilyDetail, ProductFamilyDetailDto>();
            Configuration.CreateMap<ProductFamilyDetailDto, ProductFamilyDetail>();
            Configuration.CreateMap<ProductFamilyCharacteristicAssociationModel, ProductFamilyCharacteristicAssociationDto>();
            Configuration.CreateMap<ProductFamilyCharacteristicAssociationDto, ProductFamilyCharacteristicAssociationModel>();
            Configuration.CreateMap<ProductFamilyFeatureDependency, ProductFamilyFeatureDependencyDto>();
            Configuration.CreateMap<ProductFamilyFeatureDependencyDto, ProductFamilyFeatureDependency>();

            Configuration
                .CreateMap
                <ProductFamilyFeatureAllowedValueDependencyMapping, ProductFamilyFeatureAllowedValueDependencyMappingDto
                    >();
            Configuration.CreateMap<ProductFamilyFeatureAssociation, ProductFamilyFeatureAssociationDto>();
            Configuration.CreateMap<ProductFamilyFeatureAllowedValue, ProductFamilyFeatureAllowedValueDto>();
            Configuration.CreateMap<ProductFamilyCharacteristicDomainEntity, ProductFamilyCharacteristicDomainEntityDto>();

            Configuration.CreateMap<OrderServiceLineDetail, OrderServiceLineDetailDto>()
                .ForMember(x => x.OrderNumber, opt => opt.Ignore());
            Configuration.CreateMap<OrderServiceLineDetailDto, OrderServiceLineDetail>();
            Configuration.CreateMap<OrderServiceLineDetailSearchResultSet, OrderServiceLineDetailSearchResultSetDto>();
            Configuration.CreateMap<OrderServiceLineDetailSearchResultSetDto, OrderServiceLineDetailSearchResultSet>()
                .ForMember(x => x.RefinerResults, x => x.Ignore());
            Configuration.CreateMap<OrderServiceLineDetailSearchResult, OrderServiceLineDetailSearchResultDto>();
            Configuration.CreateMap<OrderServiceLineDetailSearchResultDto, OrderServiceLineDetailSearchResult>();

            Configuration.CreateMap<AriaMetaDataLinkDto, MetaDataLink>();
            Configuration.CreateMap<Notification, NotificationDto>();
            Configuration.CreateMap<NotificationDto, Notification>()
                .ConstructUsing(src => new Notification(src.Id.GetValueOrDefault()))
                .ForMember(x => x.Id, x => x.Ignore());

            // History
            Configuration.CreateMap<History, HistoryDto>();
            Configuration.CreateMap<HistoryDto, History>();
            Configuration.CreateMap<NameValuePair, NameValuePairDto>().ReverseMap();
            Configuration.CreateMap<TaskDelta, History>()
                .ForMember(x => x.ActionDate, x => x.MapFrom(y => y.CreatedDate))
               .ForMember(x => x.EntityType, x => x.UseValue(EntityTypeEnumDto.Task))
               .ForMember(x => x.ActionUserId, x => x.Ignore())
               .ForMember(x => x.ActionUserText, x => x.Ignore())
               .ForMember(x => x.TrackedInfo, x => x.MapFrom(y => y.MetaDeltaList))
               .ForMember(x => x.ActionType, x => x.MapFrom(y => y.Action))
               .ForMember(x => x.ActionDetail, x => x.Ignore())
               .ForMember(x => x.HistoryId, x => x.Ignore())
               .ForMember(x => x.EntityId, x => x.Ignore())
               .ForMember(x => x.ActionDetailEntityType, x => x.Ignore())
               .AfterMap((s, d) =>
               {
                   Guid cbId;
					d.ActionUserId = Guid.TryParse(s.CreatedBy, out cbId) ? cbId : Guid.Empty;
               });

            Configuration.CreateMap<MetaDelta, NameValuePair>()
            .ForMember(x => x.Value, x => x.MapFrom(y => y.ModifiedValue))
            .ForMember(x => x.Name, x => x.MapFrom(y => y.AriaFieldName))
            .ForSourceMember(x => x.Id, x => x.Ignore());

            // Lookup
            Configuration.CreateMap<BusinessUnit, BusinessUnitDto>();
            Configuration.CreateMap<BusinessUnitDto, BusinessUnit>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Metadata, opt => opt.Ignore());
            Configuration.CreateMap<BusinessUnit, BusinessUnitSearchResultDto>()
                .ForMember(x => x.BusinessUnit, x => x.MapFrom(y => Map<BusinessUnit>(y)));
            Configuration.CreateMap<SearchResultSetBase<BusinessUnit>, BusinessUnitSearchResultSetDto>();
            Configuration.CreateMap<BusinessUnitSearchResultSetDto, SearchResultSetBase<BusinessUnit>>()
                .ForMember(x => x.Results, x => x.MapFrom(y => y.Results.Select(z => Map<BusinessUnit>(z))))
                .ForMember(x => x.RefinerResults, x => x.Ignore());

			Configuration.CreateMap<DocumentTemplate, DocumentTemplateDto>();
			Configuration.CreateMap<DocumentTemplateDto, DocumentTemplate>()
				.ForMember(x => x.Id, opt => opt.Ignore())
				.ForMember(x => x.Metadata, opt => opt.Ignore());
	        Configuration.CreateMap<DocumentTemplate, DocumentTemplateSearchResultDto>()
				.ForMember(x => x.DocumentTemplate, x => x.MapFrom(y => Map<DocumentTemplate>(y)))
				.ForMember(x => x.CreatedBy, x => x.Ignore())
				.ForMember(x => x.LastModifiedBy, x => x.Ignore());
	        Configuration.CreateMap<SearchResultSetBase<DocumentTemplate>, DocumentTemplateSearchResultSetDto>();
			Configuration.CreateMap<DocumentTemplateSearchResultSetDto, SearchResultSetBase<DocumentTemplate>>()
				.ForMember(x => x.Results, x => x.MapFrom(y => y.Results.Select(z => Map<DocumentTemplate>(z))))
				.ForMember(x => x.RefinerResults, x => x.Ignore());

            CreateServiceCodeMaps();
	        CreateDepartmentCodeMaps();

            CreateIndustryCodeMaps();

            Configuration.CreateMap<LocationCodeDto, LocationCode>();
            Configuration.CreateMap<LocationCode, LocationCodeDto>();
            Configuration.CreateMap<Document, DocumentDto>();
            Configuration.CreateMap<DocumentDto, Document>();

            CreateLinkMaps();

            CreateTaskTypeMaps();
            CreateTaskBehaviorMaps();
            CreateTaskCategoryMaps();

            CreateUserTeamMaps();

            CreateCertificationRequestMaps();
        }

        private void CreateTaskBehaviorMaps()
        {
            Configuration.CreateMap<TaskTypeBehaviorDto, TaskTypeBehavior>()
                .ForMember(x => x.Metadata, opt => opt.Ignore());
            Configuration.CreateMap<TaskTypeBehavior, TaskTypeBehaviorDto>();

            Configuration.CreateMap<TaskTypeAvailableBehaviorFieldDto, TaskTypeAvailableBehaviorField>()
                .ForMember(x => x.Metadata, opt => opt.Ignore()); 
            Configuration.CreateMap<TaskTypeAvailableBehaviorField, TaskTypeAvailableBehaviorFieldDto>();
        }

        private void CreateIndustryCodeMaps()
        {
            Configuration.CreateMap<IndustryCodeDto, IndustryCode>();

            Configuration.CreateMap<IndustryCode, IndustryCodeDto>();

            Configuration.CreateMap<SearchResultSetBase<IndustryCode>, LookupCodeSearchResultSetDto>();

            Configuration.CreateMap<LookupCodeSearchResultSetDto, SearchResultSetBase<IndustryCode>>()
              .ForMember(x => x.Results, x => x.MapFrom(y => y.Results.Select(z => Map<IndustryCode>(z))))
                .ForMember(x => x.RefinerResults, x => x.Ignore());

            Configuration.CreateMap<IndustryCode, LookupCodeSearchResultDto>()
                .ForMember(x => x.ExternalId, y => y.MapFrom(z => z.ExternalId))
                .ForMember(x => x.Label, y => y.MapFrom(z => z.Label))
                .ForMember(x => x.ChangeDate, x => x.Ignore())
                .ForMember(x => x.EntityType, x => x.Ignore())
                .ForMember(x => x.Title, x => x.Ignore())
                .ForMember(x => x.Name, x => x.Ignore())
                .ForMember(x => x.Metadata, x => x.Ignore());
        }

        private void CreateServiceCodeMaps()
        {
			Configuration.CreateMap<ServiceCodeDto, ISearchResult>().ForAllMembers(x => x.Ignore());
            Configuration.CreateMap<ServiceCodeDto, ServiceCode>();
            Configuration.CreateMap<ServiceCode, ServiceCodeDto>();
            Configuration.CreateMap<SearchResultSetBase<ServiceCode>, LookupCodeSearchResultSetDto>();

            Configuration.CreateMap<LookupCodeSearchResultSetDto, SearchResultSetBase<ServiceCode>>()
                .ForMember(x => x.Results, x => x.MapFrom(y => y.Results.Select(z => Map<ServiceCode>(z))))
                .ForMember(x => x.RefinerResults, x => x.Ignore());

            Configuration.CreateMap<ServiceCode, LookupCodeSearchResultDto>()
                .ForMember(x => x.ExternalId, y => y.MapFrom(z => z.ExternalId))
                .ForMember(x => x.Label, y => y.MapFrom(z => z.Label))
                 .ForMember(x => x.ChangeDate, x => x.Ignore())
                .ForMember(x => x.EntityType, x => x.Ignore())
                .ForMember(x => x.Title, x => x.Ignore())
                 .ForMember(x => x.Name, x => x.Ignore())
                .ForMember(x => x.Metadata, x => x.Ignore());
        }

		private void CreateDepartmentCodeMaps()
		{
			Configuration.CreateMap<DepartmentCodeDto, ISearchResult>().ForAllMembers(x => x.Ignore());
			Configuration.CreateMap<DepartmentCodeDto, DepartmentCode>();
			Configuration.CreateMap<DepartmentCode, DepartmentCodeDto>();
			Configuration.CreateMap<SearchResultSetBase<DepartmentCode>, LookupCodeSearchResultSetDto>();

			Configuration.CreateMap<LookupCodeSearchResultSetDto, SearchResultSetBase<DepartmentCode>>()
				.ForMember(x => x.Results, x => x.MapFrom(y => y.Results.Select(z => Map<DepartmentCode>(z))))
				.ForMember(x => x.RefinerResults, x => x.Ignore());

			Configuration.CreateMap<DepartmentCode, LookupCodeSearchResultDto>()
				.ForMember(x => x.ExternalId, y => y.MapFrom(z => z.ExternalId))
				.ForMember(x => x.Label, y => y.MapFrom(z => z.Label))
				 .ForMember(x => x.ChangeDate, x => x.Ignore())
				.ForMember(x => x.EntityType, x => x.Ignore())
				.ForMember(x => x.Title, x => x.Ignore())
				 .ForMember(x => x.Name, x => x.Ignore())
				.ForMember(x => x.Metadata, x => x.Ignore());
		}

        private void CreateCompanyMaps()
        {
            Configuration.CreateMap<Company, CompanyDto>();
            Configuration.CreateMap<CompanyDto, Company>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ConstructUsing(x => new Company(x.Id));

            Configuration.CreateMap<CompanySearchResultSet, CompanySearchModelDto>()
                .ForMember(x => x.Results,
                    y => y.MapFrom(z => z.Results != null ? z.Results.Select(a => a.Company) : new List<Company>()));
            Configuration.CreateMap<CompanySearchModelDto, CompanySearchResultSet>()
                .ForMember(x => x.RefinerResults, x => x.Ignore())
                .ForMember(x => x.Results, x => x.Ignore());
        }

        internal void CreateClaimMaps()
        {
            Configuration.CreateMap<UserClaimHistory, UserClaimHistoryDto>();

            Configuration.CreateMap<UserClaimDto, UserClaim>();
            Configuration.CreateMap<UserClaim, UserClaimDto>();

            Configuration.CreateMap<UserBusinessClaim, UserBusinessClaimDto>();
            Configuration.CreateMap<UserBusinessClaimDto, UserBusinessClaim>();

            Configuration.CreateMap<BusinessClaim, BusinessClaimDto>();
            Configuration.CreateMap<BusinessClaimDto, BusinessClaim>();

            Configuration.CreateMap<UserBusinessClaimHistory, UserBusinessClaimHistoryDto>();
            Configuration.CreateMap<UserBusinessClaimHistoryDto, UserBusinessClaimHistory>();

            Configuration.CreateMap<UserClaimDto, UserBusinessClaim>()
                .ForMember(source => source.Claim, opt => opt.ResolveUsing<BusinessClaimResolver>());

            Configuration.CreateMap<UserBusinessClaim, UserClaimDto>()
                .ForMember(x => x.ClaimId, opt => opt.MapFrom(dto => dto.Claim.EntityClaim))
                .ForMember(x => x.ClaimValue, opt => opt.MapFrom(dto => dto.Claim.Value));

            Configuration.CreateMap<UserClaimHistoryDto, BusinessClaim>()
                .ForMember(x => x.EntityClaim, opt => opt.MapFrom(dto => dto.ClaimId))
                .ForMember(x => x.Value, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(x => x.EntityId, opt => opt.Ignore());

            Configuration.CreateMap<UserClaimHistoryDto, UserBusinessClaimHistory>()
                .ForMember(x => x.Claim, opt => opt.MapFrom(dto => MappingEngine.Map<BusinessClaim>(dto)));
        }

        internal void CreateProductUploadMaps()
        {
            //
            // Product Upload
            //
            Configuration.CreateMap<ProductUploadSearchResultSet, ProductUploadSearchResultSetDto>();
            Configuration.CreateMap<ProductUploadSearchResult, ProductUploadSearchResultDto>();
            Configuration.CreateMap<ProductUpload, ProductUploadDto>()
                .ForMember(dto => dto.CreatedBy, opt => opt.MapFrom(vm => vm.CreatedById))
                .ForMember(dto => dto.CreatedOn, opt => opt.MapFrom(vm => vm.CreatedDateTime))
                .ForMember(dto => dto.UpdatedBy, opt => opt.MapFrom(vm => vm.UpdatedById))
                .ForMember(dto => dto.UpdatedOn, opt => opt.MapFrom(vm => vm.UpdatedDateTime))
                ;
            Configuration.CreateMap<ProductUploadDto, ProductUpload>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.FileContent, x => x.Ignore())
                .ForMember(dto => dto.CreatedById, opt => opt.MapFrom(vm => vm.CreatedBy))
                .ForMember(dto => dto.CreatedDateTime, opt => opt.MapFrom(vm => vm.CreatedOn))
                .ForMember(dto => dto.UpdatedById, opt => opt.MapFrom(vm => vm.UpdatedBy))
                .ForMember(dto => dto.UpdatedDateTime, opt => opt.MapFrom(vm => vm.UpdatedOn))
                .ConstructUsing(x => new ProductUpload(x.Id));

            //
            // Product Upload Result
            //
            Configuration.CreateMap<ProductUploadResultSearchResultSet, ProductUploadResultSearchResultSetDto>();
            Configuration.CreateMap<ProductUploadResultSearchResult, ProductUploadResultSearchResultDto>();
            Configuration.CreateMap<ProductUploadResult, ProductUploadResultDto>()
                .ForMember(dto => dto.CreatedBy, opt => opt.MapFrom(vm => vm.CreatedById))
                .ForMember(dto => dto.CreatedOn, opt => opt.MapFrom(vm => vm.CreatedDateTime))
                .ForMember(dto => dto.UpdatedBy, opt => opt.MapFrom(vm => vm.UpdatedById))
                .ForMember(dto => dto.UpdatedOn, opt => opt.MapFrom(vm => vm.UpdatedDateTime))
                ;
            Configuration.CreateMap<ProductUploadMessage, ProductUploadMessageDto>()
                .ForMember(dto => dto.CreatedBy, opt => opt.MapFrom(vm => vm.CreatedById))
                .ForMember(dto => dto.CreatedOn, opt => opt.MapFrom(vm => vm.CreatedDateTime))
                .ForMember(dto => dto.UpdatedBy, opt => opt.MapFrom(vm => vm.UpdatedById))
                .ForMember(dto => dto.UpdatedOn, opt => opt.MapFrom(vm => vm.UpdatedDateTime))
                ;

            Configuration.CreateMap<ProductUploadResultDto, ProductUploadResult>()
                .ForMember(x => x.CreatedById, opt => opt.MapFrom(y => y.CreatedBy))
                .ForMember(x => x.CreatedDateTime, opt => opt.MapFrom(y => y.CreatedOn))
                .ForMember(x => x.UpdatedById, opt => opt.MapFrom(y => y.UpdatedBy))
                .ForMember(x => x.UpdatedDateTime, opt => opt.MapFrom(y => y.UpdatedOn));
            Configuration.CreateMap<ProductUploadMessageDto, ProductUploadMessage>()
                .ForMember(x => x.CreatedById, opt => opt.MapFrom(y => y.CreatedBy))
                .ForMember(x => x.CreatedDateTime, opt => opt.MapFrom(y => y.CreatedOn))
                .ForMember(x => x.UpdatedById, opt => opt.MapFrom(y => y.UpdatedBy))
                .ForMember(x => x.UpdatedDateTime, opt => opt.MapFrom(y => y.UpdatedOn));
        }

        /// <summary>
        /// Creates the link maps.
        /// </summary>
        public void CreateLinkMaps()
        {
            Configuration.CreateMap<LinkDto, Link>()
                .ForMember(x => x.Metadata, opt => opt.Ignore());
            Configuration.CreateMap<Link, LinkDto>();
            Configuration.CreateMap<Lookup, LookupDto>();
            Configuration.CreateMap<SearchResultSetBase<Link>, LinkSearchDto>();

            //Configuration.CreateMap<Link, SearchBaseDto<LinkDto>>()
            //    .ForMember(x => x.Equals(), x => x.MapFrom(y => Map<BusinessUnit>(y)));
            //Configuration.CreateMap<SearchResultSetBase<BusinessUnit>, BusinessUnitSearchResultSetDto>();
            //Configuration.CreateMap<BusinessUnitSearchResultSetDto, SearchResultSetBase<BusinessUnit>>()
            //    .ForMember(x => x.Results, x => x.MapFrom(y => y.Results.Select(z => Map<BusinessUnit>(z))))
            //    .ForMember(x => x.RefinerResults, x => x.Ignore());
        }

        internal void CreateProductMaps()
        {
            //
            //  Product to contract mappings
            //
            Configuration.CreateMap<Product, ProductDto>()
                .ForSourceMember(x => x.Type, x => x.Ignore());
            Configuration.CreateMap<ProductDto, Product>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Type, x => x.Ignore())
                .ConstructUsing(x => new Product(x.Id));
            Configuration.CreateMap<ProductCharacteristic, ProductCharacteristicDto>();
            Configuration.CreateMap<ProductCharacteristicDto, ProductCharacteristic>();
        }

        internal void CreateIncomingOrderMaps()
        {
            //
            // Incoming order
            //
            Configuration.CreateMap<Order, IncomingOrderDto>()
                .ForSourceMember(x => x.HideFromCustomer, x => x.Ignore());

            Configuration.CreateMap<IncomingOrderDto, Order>()
                .ForMember(x => x.ContainerId, opt => opt.Ignore())
                .ForMember(x => x.Type, x => x.Ignore())
                .ForMember(x => x.Name, x => x.Ignore())
                .ForMember(x => x.HideFromCustomer, x => x.Ignore())
                .ForMember(x => x.MessageId, x => x.Ignore())
                ;

            Configuration.CreateMap<IncomingOrder, IncomingOrderDto>()
                .ForSourceMember(x => x.HideFromCustomer, x => x.Ignore());

            Configuration.CreateMap<IncomingOrderDto, IncomingOrder>()
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.ContainerId, opt => opt.Ignore())
                .ForMember(x => x.Type, x => x.Ignore())
                .ForMember(x => x.HideFromCustomer, x => x.Ignore())
                .ForMember(x => x.MessageId, x => x.Ignore())
                ;

            Configuration.CreateMap<Project, IncomingOrder>()
                .ForMember(x => x.IncomingOrderContact, x => x.Ignore())
                .ForMember(x => x.BillToContact, x => x.Ignore())
                .ForMember(x => x.ShipToContact, x => x.Ignore())
                .ForMember(x => x.IncomingOrderCustomer, x => x.Ignore())
                .ForMember(x => x.ServiceLines, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore());
            Configuration.CreateMap<IncomingOrderContact, IncomingOrderContactDto>();
            Configuration.CreateMap<IncomingOrderContactDto, IncomingOrderContact>()
                .ForMember(x => x.ContactRoleId, x => x.Ignore());
            Configuration.CreateMap<IncomingOrderCustomer, IncomingOrderCustomerDto>();
            Configuration.CreateMap<IncomingOrderCustomerDto, IncomingOrderCustomer>();
            Configuration.CreateMap<IncomingOrderServiceLine, IncomingOrderServiceLineDto>();
            Configuration.CreateMap<IncomingOrderServiceLineDto, IncomingOrderServiceLine>();
            Configuration.CreateMap<ProjectCreationRequestDto, ProjectCreationRequest>();
            Configuration.CreateMap<ProjectCreationRequest, ProjectCreationRequestDto>();
            Configuration.CreateMap<IncomingOrderSearchResultSet, IncomingOrderSearchResultSetDto>();
            Configuration.CreateMap<IncomingOrderSearchResultSetDto, IncomingOrderSearchResultSet>()
                .ForMember(x => x.RefinerResults, x => x.Ignore());
            Configuration.CreateMap<IncomingOrderSearchResult, IncomingOrderSearchResultDto>();
            Configuration.CreateMap<IncomingOrderSearchResultDto, IncomingOrderSearchResult>();
            Configuration.CreateMap<Project, ProjectDto>()
                .ForMember(x => x.OriginalProjectTemplateId, x => x.MapFrom(src => src.ProjectTemplateId));
            Configuration.CreateMap<ProjectDto, Project>()
                .ForMember(x => x.Type, x => x.Ignore())
                .ForMember(x => x.MessageId, x => x.Ignore())
                .ForMember(x => x.TaskMinimumDueDate, x => x.Ignore())
                .ForMember(x => x.MinimumDueDateTaskId, x => x.Ignore())
                .ForMember(x => x.ProjectTemplateId, x => x.MapFrom(src => src.OriginalProjectTemplateId))
                .ForMember(x=> x.Tasks, x=> x.Ignore())
                ;

            Configuration.CreateMap<SearchResultSetBase<ProjectTemplate>, ProjectTemplateSearchResultSetDto>();
            Configuration.CreateMap<ProjectTemplateSearchResultSetDto, SearchResultSetBase<ProjectTemplate>>()
                .ForMember(x => x.Results, x => x.MapFrom(y => y.Results.Select(z => Map<ProjectTemplate>(z))))
                .ForMember(x => x.RefinerResults, x => x.Ignore());

            Configuration.CreateMap<ProjectTemplate, ProjectTemplateSearchResultDto>()
                .ForMember(x => x.ProjectTemplate, x => x.MapFrom(y => Map<ProjectTemplate>(y)))
                .ForMember(x => x.CreatedBy, x => x.Ignore())
                .ForMember(x => x.LastModifiedBy, x => x.Ignore());

            Configuration.CreateMap<AssetLinkDto, AssetLink>()
                .ForMember(x => x.Id, x => x.Ignore());
            Configuration.CreateMap<AssetLink, AssetLinkDto>();

            Configuration.CreateMap<EntityTaskHistory, TaskHistory>();
        }

        internal void CreateSearchMaps()
        {
            Configuration.CreateMap<SortDto, Sort>();
            Configuration.CreateMap<Sort, SortDto>();
            Configuration.CreateMap<SortDto, ISort>()
				.ForMember(x => x.Order, x => x.MapFrom(y => (int) y.Order));
            Configuration.CreateMap<ISort, SortDto>()
				.ForMember(x => x.Order, x => x.MapFrom(y => (int) y.Order));

            Configuration.CreateMap<SearchSummaryDto, SearchSummary>();
            Configuration.CreateMap<SearchSummary, SearchSummaryDto>();

            Configuration.CreateMap<SearchSummaryDto, ISearchSummary>();
            Configuration.CreateMap<ISearchSummary, SearchSummaryDto>()
                 .ForMember(x => x.LastCommand, x => x.Ignore());

            Configuration.CreateMap<SearchCriteriaDto, SearchCriteria>()
                .ForMember(x => x.SearchCoordinators, opt => opt.Ignore())
				.ForMember(x => x.AdditionalFilterString, opt => opt.Ignore())
				.ForMember(x => x.Sorts, opt => opt.MapFrom(
                    y => y.Sorts == null 
                        ? new List<ISort>() 
						: y.Sorts.Select(z => new Sort {FieldName = z.FieldName, Order = (SortDirection) z.Order} as ISort).ToList()));
            Configuration.CreateMap<SearchCriteria, SearchCriteriaDto>();

            Configuration.CreateMap<SearchCriteriaDto, ISearchCriteria>()
                .ForMember(x => x.EntityTypeLabel, x => x.MapFrom(y => (y.EntityType != null)
                    ? y.EntityType.ToString()
                    : string.Empty));

            Configuration.CreateMap<ISearchCriteria, SearchCriteriaDto>()
                .ForMember(x => x.EntityType, x => x.MapFrom(y => SafeParse<EntityTypeEnumDto>(y.EntityTypeLabel)));

           
            Configuration.CreateMap<SearchResultSet, SearchResultSetDto>();
            Configuration.CreateMap<SearchResult, SearchResultDto>();
            Configuration.CreateMap<ISearchResult, SearchResultDto>()
                .ForMember(x => x.EntityType, x => x.MapFrom(y => SafeParse<EntityTypeEnumDto>(y.EntityType)));

            Configuration.CreateMap<ProfileEditBasicBo, ProfileBo>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.ProfileId))
                .ForMember(x => x.Title, x => x.MapFrom(y => y.JobTitle))
                .ForMember(x => x.CompanyId, x => x.Ignore())
                .ForMember(x => x.CompanyName, x => x.Ignore())
                .ForMember(x => x.Claims, x => x.Ignore())
                .ForMember(x => x.CreatedById, x => x.Ignore())
                .ForMember(x => x.CreatedDateTime, x => x.Ignore())
                .ForMember(x => x.UpdatedDateTime, x => x.Ignore())
                .ForMember(x => x.UpdatedById, x => x.Ignore());
            Configuration.CreateMap<RefinementItemDto, RefinementItem>();
            Configuration.CreateMap<RefinementItem, RefinementItemDto>();

            Configuration.CreateMap<RefinementItemDto, IRefinementItem>();
            Configuration.CreateMap<IRefinementItem, RefinementItemDto>();
        }

        private T? SafeParse<T>(string value) where T : struct
        {
			T output;
			if (Enum.TryParse(value, out output))
            {
                return output;
        }

            return null;
        }

        internal void CreateTaskTypeMaps()
        {
            Configuration.CreateMap<TaskTypeDto, TaskType>();
            Configuration.CreateMap<TaskType, TaskTypeDto>()
                .ForMember(x => x.BusinessUnits,
					opt =>
						opt.MapFrom(
							src =>
								src.BusinessUnits != null
									? src.BusinessUnits.Select(y => MappingEngine.Map<BusinessUnitDto>(y)).ToList()
									: new List<BusinessUnitDto>()))
                .ForMember(x => x.Links,
					opt =>
						opt.MapFrom(
							src => src.Links != null ? src.Links.Select(y => MappingEngine.Map<LinkDto>(y)).ToList() : new List<LinkDto>()))
                .ForMember(x => x.DocumentTemplates,
					opt =>
						opt.MapFrom(
							src =>
								src.DocumentTemplates != null
									? src.DocumentTemplates.Select(y => MappingEngine.Map<DocumentTemplateDto>(y)).ToList()
									: new List<DocumentTemplateDto>()));

            Configuration.CreateMap<SearchResultSetBase<TaskType>, TaskTypeSearchModelDto>();
        }

        internal void CreateTaskCategoryMaps()
        {
            Configuration.CreateMap<TaskCategory, TaskCategoryDto>();

            Configuration.CreateMap<TaskCategoryDto, TaskCategory>()
                 .ForMember(x => x.UpdatedById, x => x.Ignore())
                .ForMember(x => x.UpdatedDateTime, x => x.Ignore())
                 .ForMember(x => x.CreatedById, x => x.Ignore())
                 .ForMember(x => x.CreatedDateTime, x => x.Ignore())
                ;
            Configuration.CreateMap<TaskCategorySearchResultSet, TaskCategorySearchModelDto>();
        }

        internal void CreateUserTeamMaps()
        {
            Configuration.CreateMap<UserTeam, UserTeamDto>();
            Configuration.CreateMap<UserTeamDto, UserTeam>();

            Configuration.CreateMap<UserTeamMember, UserTeamMemberDto>();
            Configuration.CreateMap<UserTeamMemberDto, UserTeamMember>();
        }

        internal void CreateCertificationRequestMaps()
        {
            Configuration.CreateMap<CertificationRequestDto, CertificationRequestTaskProperty>()
                .ForMember(x => x.Parent, opt => opt.Ignore())
                .ForMember(x => x.ParentTaskPropertyId, opt => opt.Ignore())
                .ForMember(x => x.UpdatedById, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDateTime, opt => opt.Ignore())
                .ForMember(x => x.Children, opt => opt.Ignore())
                .ForMember(x => x.CreatedById, opt => opt.Ignore())
                //.ForMember(x => x.CreatedDateTime, opt => opt.Ignore())
                .ForMember(x => x.Value, opt => opt.Ignore())
                .ForMember(x => x.TaskPropertyType, opt => opt.Ignore())
                .ForMember(x => x.TaskPropertyTypeId, opt => opt.Ignore())
                .ForMember(x => x.Metadata, opt => opt.Ignore())
                ;
            Configuration.CreateMap<CertificationRequestTaskProperty, CertificationRequestDto>()
                .ForMember(x=>x.IsResubmittal, opt => opt.Ignore())
                ;
            Configuration.CreateMap<CertificationManagementDto, CertificationRequestTaskProperty>()
                .ForMember(x => x.Parent, opt => opt.Ignore())
                .ForMember(x => x.ParentTaskPropertyId, opt => opt.Ignore())
                .ForMember(x => x.UpdatedById, opt => opt.Ignore())
                .ForMember(x => x.UpdatedDateTime, opt => opt.Ignore())
                .ForMember(x => x.Children, opt => opt.Ignore())
                .ForMember(x => x.CreatedById, opt => opt.Ignore())
                //.ForMember(x => x.CreatedDateTime, opt => opt.Ignore())
                 .ForMember(x => x.CCN, opt => opt.Ignore())
                .ForMember(x => x.FileNo, opt => opt.Ignore())
                .ForMember(x => x.ProjectEndDate, opt => opt.Ignore())
                .ForMember(x => x.ProjectHandler, opt => opt.Ignore())
                .ForMember(x => x.ContactEmail, opt => opt.Ignore())
                .ForMember(x => x.ContactName, opt => opt.Ignore())
                .ForMember(x => x.SubscriberNumber, opt => opt.Ignore())
                .ForMember(x => x.ProjectNumber, opt => opt.Ignore())
                .ForMember(x => x.Metadata, opt => opt.Ignore())
                .ForMember(x => x.Value, opt => opt.Ignore())
                .ForMember(x => x.TaskPropertyType, opt => opt.Ignore())
                .ForMember(x => x.TaskPropertyTypeId, opt => opt.Ignore())
                ;
            Configuration.CreateMap<CertificationRequestTaskProperty, CertificationManagementDto>()
                .ForMember(x => x.IsResubmittal, opt => opt.Ignore());
                
            Configuration.CreateMap<CertificationRequestTaskProperty, TaskProperty>()
                ;
            Configuration.CreateMap<TaskProperty, CertificationRequestTaskProperty>()
                .ForMember(x => x.Value, opt => opt.Ignore())
                .ForMember(x => x.CCN, opt => opt.Ignore())
                .ForMember(x => x.FileNo, opt => opt.Ignore())
                .ForMember(x => x.ProjectEndDate, opt => opt.Ignore())
                .ForMember(x => x.ProjectHandler, opt => opt.Ignore())
                .ForMember(x => x.ContactEmail, opt => opt.Ignore())
                .ForMember(x => x.ContactName, opt => opt.Ignore())
                .ForMember(x => x.SubscriberNumber, opt => opt.Ignore())
                .ForMember(x => x.ProjectNumber, opt => opt.Ignore())
                .ForMember(x => x.Comments, opt => opt.Ignore())
                .ForMember(x => x.DepartmentCode, opt => opt.Ignore())
                .ForMember(x => x.SubmittingUserName, opt => opt.Ignore())
                .ForMember(x => x.ProjectId, opt => opt.Ignore())
                .ForMember(x => x.ScopeOfRequest, opt => opt.Ignore())
                .ForMember(x => x.CcnIndustry, opt => opt.Ignore())
                .ForMember(x => x.IsOutsideLab, opt => opt.Ignore())
                .ForMember(x => x.CcnDescription, opt => opt.Ignore())
                .ForMember(x => x.StandardsAndEditions, opt => opt.Ignore())
                .ForMember(x => x.HandlerLocation, opt => opt.Ignore())
                ;
        }

		// ReSharper disable once ClassNeverInstantiated.Local
        private class GuidToStringConverter : ITypeConverter<Guid, string>
        {
            public string Convert(ResolutionContext context)
            {
                return context.SourceValue.ToString();
            }
        }

		// ReSharper disable once ClassNeverInstantiated.Local
        private class StringToGuidConverter : ITypeConverter<string, Guid>
        {
            public Guid Convert(ResolutionContext context)
            {
                return Guid.Parse(context.SourceValue.ToString());
            }
        }

		// ReSharper disable once ClassNeverInstantiated.Local
        private class StringToUriConverter : ITypeConverter<string, Uri>
        {
            public Uri Convert(ResolutionContext context)
            {
                return new Uri(context.SourceValue.ToString());
            }
        }

	    // ReSharper disable once ClassNeverInstantiated.Local
        private class UriToStringConverter : ITypeConverter<Uri, string>
        {
            public string Convert(ResolutionContext context)
            {
                return context.SourceValue.ToString();
            }
        }
    }
}