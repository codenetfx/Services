using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

using RazorEngine.Templating;

using UL.Aria.Common.Authorization;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Auditing;
using UL.Aria.Service.Caching;
using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Claim.Data;
using UL.Aria.Service.Claim.Implementation;
using UL.Aria.Service.Claim.Provider;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Validator;
using UL.Aria.Service.Implementation;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Manager.DataRule;
using UL.Aria.Service.Manager.DataRule.Task;
using UL.Aria.Service.Manager.Validation;
using UL.Aria.Service.Notifications;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Provider.EntityHistoryStrategy;
using UL.Aria.Service.Provider.SearchCoordinator;
using UL.Aria.Service.Provider.SearchCoordinator.Task;
using UL.Aria.Service.Proxy;
using UL.Aria.Service.Repository;
using UL.Aria.Service.Security;
using UL.Aria.Service.TaskStatus;
using UL.Aria.Service.Templating;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Caching;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Claim;
using UL.Enterprise.Foundation.Service.Correlation;
using UL.Enterprise.Foundation.Service.ExceptionHandling;
using UL.Enterprise.Foundation.Service.Logging;
using UL.Enterprise.Foundation.Unity;
using UL.Iam.Common;
using UL.Iam.Common.OAuth;

namespace UL.Aria.Service.Configuration
{
	/// <summary>
	///     Static class containing helper methods for setting up unity containers.
	/// </summary>
	public static class ContainerSetup
	{
		// ReSharper disable once InconsistentNaming
		private static readonly object _padLockDocumentRepository = new object();
		// ReSharper disable once InconsistentNaming
		private static readonly object _padLock = new object();

		/// <summary>
		///     Sets up the log manager and other dependencies that apply to ALL applications.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		// ReSharper disable once UnusedTypeParameter
		public static IUnityContainer SetupGlobalCommon<T>(this IUnityContainer container) where T : IPrincipalResolver
		{
			container
				.RegisterType<ILogManager, LogManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IPrincipalResolver, T>(new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the common dependencies that apply to ALL services.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="useThreadResolver">if set to <c>true</c> [use thread resolver].</param>
		/// <returns>IUnityContainer.</returns>
		public static IUnityContainer SetupServiceCommon(this IUnityContainer container, bool useThreadResolver = false)
		{
			if (useThreadResolver)
			{
				container
					.SetupGlobalCommon<ThreadPrincipalResolver>();
			}
			else
			{
				container
					.SetupGlobalCommon<PrincipalResolver>();
			}
			container
				.RegisterType<ITransactionFactory, TransactionFactory>(new ContainerControlledLifetimeManager())
				.RegisterType<IMapperRegistry, ServiceMapperRegistry>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessMessageProvider, BusinessMessageProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IServiceConfiguration, ServiceConfiguration>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITemplateResolver, AssemblyManifestTemplateResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAriaTemplateService, AriaTemplateService>(new ContainerControlledLifetimeManager())
				.RegisterType<ISmtpClientProvider, SmtpClientProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ISmtpClientManager, SmtpClientManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IAcceptanceClaimManager, AcceptanceClaimManager>(new HierarchicalLifetimeManager())
				.RegisterType<IAcceptanceClaimProvider, AcceptanceClaimProvider>(
					new HierarchicalLifetimeManager())
				.RegisterType<IAcceptanceClaimRepository, AcceptanceClaimRepository>(
					new HierarchicalLifetimeManager())
				.RegisterType<ITermsAndConditionsRepository, TermsAndConditionsRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProfileManager, ProfileManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ICompanyManager, CompanyManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ICompanyProvider, CompanyProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IFavoriteSearchProvider, FavoriteSearchProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IValidator<FavoriteSearch>, FavoriteSearchValidator>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IFavoriteSearchRepository, FavoriteSearchRepository>(
					new ContainerControlledLifetimeManager())
				;

			try
			{
				//this is getting registered multiple times, so its getting decorated multiple times
				container.Resolve<IProfileRepository>();
			}
			catch
			{
				container.RegisterType<IProfileRepository, ProfileRepository>(
					//  new ContainerControlledLifetimeManager(), 
					new Interceptor<InterfaceInterceptor>(),
					new InterceptionBehavior<CacheInterceptor<ProfileBo>>());
			}


			try
			{
				//this is getting registered multiple times, so its getting decorated multiple times
				container.Resolve<ICompanyRepository>();
			}
			catch
			{
				container.RegisterType<ICompanyRepository, CompanyRepository>(
					new ContainerControlledLifetimeManager(),
					new Interceptor<InterfaceInterceptor>(),
					new InterceptionBehavior<CacheInterceptor<Company>>());
			}


			return container;
		}

		/// <summary>
		/// Setups the document service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns>IUnityContainer.</returns>
		public static IUnityContainer SetupDocumentService(this IUnityContainer container)
		{
			container
				.SetupAzureStorage()
				.RegisterType<ICryptographyProviderConfigurationSource, CryptographyProviderConfigurationSource>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ICryptographyProvider, CryptographyProvider>(new ContainerControlledLifetimeManager())
				.RegisterType
				<IAzureBlobStorageRepositoryConfigurationSourceResolver, AzureBlobStorageRepositoryConfigurationSourceResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType
				<IAzureBlobStorageRepositoryConfigurationSource, DocumentContentAzureBlobStorageRepositoryConfigurationSource>(
					"DocumentContent")
				.RegisterType<IDocumentContentProviderConfigurationSource, DocumentContentProviderConfigurationSource>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentVersionRepository, DocumentVersionRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IAzureServiceBusQueueRepository<InboundMessageDto>, InboundMessageAzureServiceBusQueueRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureBlobStorageLocatorProviderResolver, AzureBlobStorageLocatorProviderResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureBlobStorageLocatorProvider, DocumentContentAzureBlobStorageLocatorProvider>("DocumentContent")
				.RegisterType<IDocumentContentProvider, DocumentContentProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentProvider, DocumentProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentContentManager, DocumentContentManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentManagementManager, DocumentManagementManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateAssociationRepository, DocumentTemplateAssociationRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateAssociationProvider, DocumentTemplateAssociationProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitAssociationProvider, BusinessUnitAssociationProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitAssociationRepository, BusinessUnitAssociationRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitProvider, BusinessUnitProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitRepository, BusinessUnitRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateRepository, DocumentTemplateRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateProvider, DocumentTemplateProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateManager, DocumentTemplateManager>(new ContainerControlledLifetimeManager())
				.SetupSharepointBase()
				.SetupServiceCommon()
				.SetupServiceAuthorization()
				.RegisterType<IDocumentContentService, DocumentContentService>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentManagementService, DocumentManagementService>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentService, DocumentsService>(new ContainerControlledLifetimeManager())
				.RegisterType<IOutboundDocumentServiceProxy, OutboundDocumentServiceProxy>(new ContainerControlledLifetimeManager())
				.RegisterType<IOutboundDocumentProxyConfigurationSource, OutboundDocumentProxyConfigurationSource>(
					new ContainerControlledLifetimeManager())
				;

			lock (_padLockDocumentRepository)
			{
				try
				{
					container.Resolve<IDocumentRepository>();
				}
				catch (ResolutionFailedException)
				{
					container.RegisterType<IDocumentRepository, DocumentRepository>(
						new ContainerControlledLifetimeManager(),
						new Interceptor<InterfaceInterceptor>(),
						new InterceptionBehavior<DocumentAuditInterceptor>());
				}
			}

			lock (_padLockDocumentRepository)
			{
				try
				{
					container.Resolve<IDocumentManager>();
				}
				catch (ResolutionFailedException)
				{
					container.RegisterType<IDocumentManager, DocumentManager>(
						new ContainerControlledLifetimeManager(),
						new Interceptor<InterfaceInterceptor>(),
						new InterceptionBehavior<DocumentManagerAuditInterceptor>());
				}
			}

			return container;
		}

		/// <summary>
		/// Setups the task notification.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupTaskNotification(this IUnityContainer container)
		{
			container
				.RegisterType<ITaskNotificationRepository, TaskNotificationRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskNotificationProvider, TaskNotificationProvider>(
					new ContainerControlledLifetimeManager());

			return container;
		}

		/// <summary>
		/// Setups the task type notification.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupTaskTypeNotification(this IUnityContainer container)
		{
			container
				.RegisterType<ITaskTypeNotificationRepository, TaskTypeNotificationRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeNotificationProvider, TaskTypeNotificationProvider>(
					new ContainerControlledLifetimeManager());

			return container;
		}

		/// <summary>
		/// Setups the cache layer.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupCacheLayer(this IUnityContainer container)
		{
			container.RegisterType<ICacheManager, CacheManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ICacheBehaviorFactory, CacheBehaviorFactory>(new ContainerControlledLifetimeManager());

			return container;
		}

		/// <summary>
		/// Setups the link serivce.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupLinkService(this IUnityContainer container)
		{
			container
				.RegisterType<ILinkRepository, LinkRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ILinkAssociationRepository, LinkAssociationRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ILinkAssociationProvider, LinkAssociationProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitAssociationRepository, BusinessUnitAssociationRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitAssociationProvider, BusinessUnitAssociationProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ILinkProvider, LinkProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ILinkManager, LinkManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ILinkService, LinkService>(new ContainerControlledLifetimeManager());

			return container;
		}

		/// <summary>
		/// Setups the task type serivce.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupTaskTypeService(this IUnityContainer container)
		{
			container
				.SetupTaskTypeAvailableBehaviorService()
				.RegisterType<ITaskTypeBehaviorRepository, TaskTypeBehaviorRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeBehaviorProvider, TaskTypeBehaviorProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeBehaviorManager, TaskTypeBehaviorManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeRepository, TaskTypeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeProvider, TaskTypeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeManager, TaskTypeManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeService, TaskTypeService>(new ContainerControlledLifetimeManager());

			return container;
		}

		private static IUnityContainer SetupTaskTypeAvailableBehaviorService(this IUnityContainer container)
		{
			container
				.RegisterType<ITaskTypeAvailableBehaviorRepository, TaskTypeAvailableBehaviorRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeAvailableBehaviorProvider, TaskTypeAvailableBehaviorProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeAvailableBehaviorManager, TaskTypeAvailableBehaviorManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeAvailableBehaviorFieldRepository, TaskTypeAvailableBehaviorFieldRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeAvailableBehaviorFieldProvider, TaskTypeAvailableBehaviorFieldProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeAvailableBehaviorFieldManager, TaskTypeAvailableBehaviorFieldManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTypeAvailableBehaviorService, TaskTypeAvailableBehaviorService>(
					new ContainerControlledLifetimeManager());

			return container;
		}

		/// <summary>
		/// Setups the user team service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupUserTeamService(this IUnityContainer container)
		{
			container
				.RegisterType<IUserTeamRepository, UserTeamRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IUserTeamMemberRepository, UserTeamMemberRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IUserTeamProvider, UserTeamProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IUserTeamManager, UserTeamManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IUserTeamService, UserTeamService>(new ContainerControlledLifetimeManager());

			return container;
		}

		/// <summary>
		/// Setups the product service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupProductService(this IUnityContainer container)
		{
			container
				.RegisterType<IProductService, ProductService>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadProductInsertManager, ProductUploadProductInsertManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadProductUpdateManager, ProductUploadProductUpdateManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadFamilyCharacteristicProvider, ProductUploadFamilyCharacteristicProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadDocumentImportManager, ProductUploadDocumentImportManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductClaimAssignmentManager, ProductClaimAssignmentManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadDocumentCharacteristicProvider, ProductUploadDocumentCharacteristicProvider>
				(new ContainerControlledLifetimeManager())
				.RegisterType<ICharacteristicValidator, ProductDocumentCharacteristicValidator>(
					typeof(ProductDocumentCharacteristicValidator).Name, new ContainerControlledLifetimeManager())
				.RegisterType<ICharacteristicValidator, ProductCharacteristicTypeValidator<DateTime>>(
					"AttributeDateValidator", new ContainerControlledLifetimeManager(),
					new InjectionConstructor(ProductFamilyCharacteristicDataType.Date))
				.RegisterType<ICharacteristicValidator, ProductCharacteristicTypeValidator<decimal>>(
					"AttributeNumberValidator", new ContainerControlledLifetimeManager(),
					new InjectionConstructor(ProductFamilyCharacteristicDataType.Number))
				.RegisterType<ICharacteristicValidator, ProductFeatureValueValidator>(
					typeof(ProductFeatureValueValidator).Name, new ContainerControlledLifetimeManager())
				.RegisterType<ICharacteristicValidator, ProductFeatureDependencyValidator>(
					typeof(ProductFeatureDependencyValidator).Name, new ContainerControlledLifetimeManager())
				.RegisterType<IProductValidator, ProductRequiredCharacteristicValidator>(
					typeof(ProductRequiredCharacteristicValidator).Name, new ContainerControlledLifetimeManager())
				.RegisterType<IProductValidationManager, ProductValidationManager>(
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(
						container.Resolve<IProductFamilyManager>(),
						new List<IProductValidator>
						{
							container.Resolve<IProductValidator>(typeof (ProductRequiredCharacteristicValidator).Name)
						},
						new List<ICharacteristicValidator>
						{
							container.Resolve<ICharacteristicValidator>(typeof (ProductDocumentCharacteristicValidator).Name),
							container.Resolve<ICharacteristicValidator>("AttributeDateValidator"),
							container.Resolve<ICharacteristicValidator>("AttributeNumberValidator")
						},
						new List<ICharacteristicValidator>
						{
							container.Resolve<ICharacteristicValidator>(typeof (ProductFeatureValueValidator).Name),
							container.Resolve<ICharacteristicValidator>(typeof (ProductFeatureDependencyValidator).Name)
						}))
				;

			return container;
		}

		/// <summary>
		/// Setups the project service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupProjectService(this IUnityContainer container)
		{
			container
				.RegisterType<IProjectService, ProjectService>(new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the history service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupHistoryService(this IUnityContainer container)
		{
			container
				.SetupEntityHistoryStrategy()
				.RegisterType<IHistoryRepository, HistoryRepository>()
				.RegisterType<IHistoryProvider, HistoryProvider>()
				.RegisterType<IHistoryManager, HistoryManager>()
				.RegisterType<IHistoryDocumentBuilder, HistoryDocumentBuilder>()
				.RegisterType<IHistoryService, HistoryService>()
				;

			return container;
		}

		/// <summary>
		/// Setups the Lookup service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupLookupService(this IUnityContainer container)
		{
			container
				.RegisterType<ILookupRepository, LookupRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ILookupProvider, LookupProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ILookupManager, LookupManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ILookupService, LookupService>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitAssociationRepository, BusinessUnitAssociationRepository>()
				.RegisterType<IBusinessUnitAssociationProvider, BusinessUnitAssociationProvider>()
				.RegisterType<IIndustryCodeService, IndustryCodeService>(new ContainerControlledLifetimeManager())
				.RegisterType<IIndustryCodeProvider, IndustryCodeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IIndustryCodeRepository, IndustryCodeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IServiceCodeService, ServiceCodeService>(new ContainerControlledLifetimeManager())
				.RegisterType<IServiceCodeProvider, ServiceCodeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IServiceCodeRepository, ServiceCodeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ILocationCodeRepository, LocationCodeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ILocationCodeProvider, LocationCodeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ILocationCodeService, LocationCodeService>(new ContainerControlledLifetimeManager())
				.RegisterType<IDepartmentCodeService, DepartmentCodeService>(new ContainerControlledLifetimeManager())
				.RegisterType<IDepartmentCodeProvider, DepartmentCodeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IDepartmentCodeRepository, DepartmentCodeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitRepository, BusinessUnitRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitProvider, BusinessUnitProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessUnitManager, BusinessUnitManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateRepository, DocumentTemplateRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateProvider, DocumentTemplateProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateManager, DocumentTemplateManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateAssociationProvider, DocumentTemplateAssociationProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IDocumentTemplateAssociationRepository, DocumentTemplateAssociationRepository>(
					new ContainerControlledLifetimeManager())
				;


			return container;
		}

		/// <summary>
		/// Sets up the sharepoint base.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="useThreadResolver">if set to <c>true</c> [use thread resolver].</param>
		/// <returns>IUnityContainer.</returns>
		public static IUnityContainer SetupSharepointBase(this IUnityContainer container, bool useThreadResolver = false)
		{
			container
				.SetupServiceCommon(useThreadResolver)
				.SetupSearchCoordinator()
				.SetupTaskProgressQueryBuilder()
				.RegisterInstance<IContainerDefinitionBuilder>(
					new ContainerDefinitionBuilder(
						new Dictionary<EntityTypeEnumDto, IContainerBuilder>
						{
							{EntityTypeEnumDto.Order, new OrderContainerBuilder()},
							{EntityTypeEnumDto.Product, new ProductContainerBuilder()},
							{EntityTypeEnumDto.Project, new ProjectContainerBuilder()}
						}
						))
				.RegisterType<IXmlParser, IncomingOrderXmlParser>(new ContainerControlledLifetimeManager())
				.RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductCharacteristicRepository, ProductCharacteristicRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IContainerManager, ContainerManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IContainerSerializer, SharePointContainerSerializer>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ISearchProvider, SearchProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IContainerRepository, ContainerRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IContainerProvider, ContainerProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ISharePointQuery, SharePointQuery>(new ContainerControlledLifetimeManager())
				.RegisterType<ISharepointConfigurationSource, SharepointConfigurationSource>(
					new ContainerControlledLifetimeManager())
				.RegisterType
				<ISharePointRestApiAccessTokenGeneratorConfiguration, SharePointRestApiAccessTokenGeneratorConfiguration
					>(new ContainerControlledLifetimeManager())
				.RegisterType<ICertificateService, CertificateService>(new ContainerControlledLifetimeManager())
				.RegisterType<ISharePointRestApiAccessTokenGenerator, SharePointRestApiAccessTokenGenerator>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAssetFieldMetadata, AssetFieldMetadata>(new ContainerControlledLifetimeManager())
				.RegisterType<IAriaMetaDataLinkRepository, AriaMetaDataLinkRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IAriaMetaDataRepository, AriaMetaDataRepository>(new ContainerControlledLifetimeManager())
				;

			// Multiple calls to this method during registration causes interceptor behavior to be decorated multiple times
			lock (_padLock)
			{
				try
				{
					container.Resolve<IAssetProvider>();
				}
				catch (ResolutionFailedException)
				{
					container.RegisterType<IAssetProvider, AssetProvider>(
						new ContainerControlledLifetimeManager(),
						new Interceptor<InterfaceInterceptor>(),
						new InterceptionBehavior<AssetLinkAuditInterceptor>());
				}
			}

			return container;
		}

		private static IUnityContainer SetupEntityHistoryStrategy(this IUnityContainer container)
		{
			container
				.RegisterType<IEntityHistoryStrategy, EntityAssetLinkHistoryStrategy>(
					typeof(AssetLinkDto).GetAssemblyQualifiedTypeName(),
					new ContainerControlledLifetimeManager())
				.RegisterType<IEntityHistoryStrategy, EntityTaskHistoryStrategy>(
					typeof(TaskDto).GetAssemblyQualifiedTypeName(),
					new ContainerControlledLifetimeManager())
				.RegisterType<IEntityHistoryStrategy, EntityProjectHistoryStrategy>(
					typeof(ProjectDto).GetAssemblyQualifiedTypeName(),
					new ContainerControlledLifetimeManager())
				.RegisterType<IEntityHistoryStrategyResolver, EntityHistoryStrategyResolver>(
					new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the inbound order message.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns>IUnityContainer.</returns>
		public static IUnityContainer SetupInboundOrderMessage(this IUnityContainer container)
		{
			container
				.RegisterType
				<IAzureBlobStorageRepositoryConfigurationSourceResolver, AzureBlobStorageRepositoryConfigurationSourceResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureBlobStorageLocatorProviderResolver, AzureBlobStorageLocatorProviderResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureBlobStorageLocatorProvider, InboundMessageFailedAzureBlobStorageLocatorProvider>(
					"InboundMessageFailed")
				.RegisterType<IAzureBlobStorageLocatorProvider, InboundMessageSuccessfulAzureBlobStorageLocatorProvider>(
					"InboundMessageSuccessful")
				.RegisterType<IAzureBlobStorageLocatorProvider, InboundMessageNewAzureBlobStorageLocatorProvider>(
					"InboundMessageNew")
				.RegisterType<IAzureBlobStorageLocatorProvider, InboundMessageOrderMessageAzureBlobStorageLocatorProvider>(
					"InboundMessageOrderMessage")
				.RegisterType<IAzureServiceBusQueueLocatorProviderResolver, AzureServiceBusQueueLocatorProviderResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureServiceBusQueueLocatorProvider, InboundMessageNewAzureServiceBusQueueLocatorProvider>(
					"InboundMessageNew")
				.RegisterType<IInboundMessageProvider, InboundMessageProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IInboundMessageProviderConfigurationSource, InboundMessageProviderConfigurationSource>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureServiceBusQueueRepository<InboundMessageDto>, InboundMessageAzureServiceBusQueueRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureBlobStorageRepository, AzureBlobStorageRepository>(new ContainerControlledLifetimeManager())
				.RegisterType
				<IAzureBlobStorageRepositoryConfigurationSource, InboundMessageAzureBlobStorageRepositoryConfigurationSource>(
					"InboundMessage")
				;

			return container;
		}

		/// <summary>
		/// Setups the outbound document.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns>IUnityContainer.</returns>
		public static IUnityContainer SetupAzureStorage(this IUnityContainer container)
		{
			container
				.RegisterType
				<IAzureBlobStorageRepositoryConfigurationSourceResolver, AzureBlobStorageRepositoryConfigurationSourceResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureBlobStorageLocatorProviderResolver, AzureBlobStorageLocatorProviderResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureServiceBusQueueLocatorProviderResolver, AzureServiceBusQueueLocatorProviderResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IAzureBlobStorageRepository, AzureBlobStorageRepository>(new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the data manipulation managers.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupDataManipulation(this IUnityContainer container)
		{
			container
				//.RegisterType
				//<ITaskWorkflowRule, SendProjectStatusMessageDataManipulationRule>
				//(
				//	typeof (SendProjectStatusMessageDataManipulationRule).FullName, new ContainerControlledLifetimeManager())
				.RegisterType
				<ITaskWorkflowRule,
					SetTaskSuccessorsStartDateWhenAllPredecessorsCompletedDataManipulationRule>(
						typeof(SetTaskSuccessorsStartDateWhenAllPredecessorsCompletedDataManipulationRule).FullName,
						new ContainerControlledLifetimeManager())
				.RegisterType
				<ITaskWorkflowRule, SetPercentCompleteTaskDataManipulationRule>(
					typeof(SetPercentCompleteTaskDataManipulationRule).FullName, new ContainerControlledLifetimeManager())
				.RegisterType
				<ITaskWorkflowRule, TaskOwnerToLowerTaskDataManipulationRule>(
					typeof(TaskOwnerToLowerTaskDataManipulationRule).FullName, new ContainerControlledLifetimeManager())
				.RegisterType
				<ITaskWorkflowRule, SetTaskSearchMetaDataDataManipulationRule>(
					typeof(SetTaskSearchMetaDataDataManipulationRule).FullName, new ContainerControlledLifetimeManager())
				.RegisterType
				<ITaskWorkflowRule,
					SetTaskOwnerForProjectHandlerTokenReplacementDataManipulationRule>(
						typeof(SetTaskOwnerForProjectHandlerTokenReplacementDataManipulationRule).FullName,
						new ContainerControlledLifetimeManager())
				.RegisterType<ITaskWorkflowRule, DeletedTaskDeletesAllChildren>(
					typeof(DeletedTaskDeletesAllChildren).FullName,
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskWorkflowRule, DeletedTaskRemoveSuccessorPredecessorRelationships>(
						typeof(DeletedTaskRemoveSuccessorPredecessorRelationships).FullName,
						new ContainerControlledLifetimeManager())
				.RegisterType
				<ITaskWorkflowRule, SetTaskStatusDateManipulationRule>(
					typeof(SetTaskStatusDateManipulationRule).FullName, new ContainerControlledLifetimeManager())
				.RegisterType
				<ITaskWorkflowRule, LastTaskCompletedCanceledSetProjectComplete>(
					typeof(LastTaskCompletedCanceledSetProjectComplete).FullName, new ContainerControlledLifetimeManager())
				.RegisterType<IDataRuleManager<IDataRuleContext<Project, Task>>, TaskDataManipulationRuleManager>
				(Workflow.TaskModification.ToString(), new ContainerControlledLifetimeManager(),
					new InjectionConstructor(
						new List<ITaskWorkflowRule>
						{
							//container.Resolve<ITaskWorkflowRule>(
							//	typeof (SendProjectStatusMessageDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (SetTaskSuccessorsStartDateWhenAllPredecessorsCompletedDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (SetPercentCompleteTaskDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (TaskOwnerToLowerTaskDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (SetTaskSearchMetaDataDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (SetTaskOwnerForProjectHandlerTokenReplacementDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (SetTaskStatusDateManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (LastTaskCompletedCanceledSetProjectComplete).FullName)
						}))
				.RegisterType<IDataRuleManager<IDataRuleContext<Project, Task>>, TaskDataManipulationRuleManager>
				(Workflow.TaskDeletion.ToString(), new ContainerControlledLifetimeManager(),
					new InjectionConstructor(
						new List<ITaskWorkflowRule>
						{
							container.Resolve<ITaskWorkflowRule>(
								typeof (DeletedTaskDeletesAllChildren).FullName),
							container.Resolve<ITaskWorkflowRule>(
                                typeof (DeletedTaskRemoveSuccessorPredecessorRelationships).FullName),                            
							//container.Resolve<ITaskWorkflowRule>(
							//	typeof (SendProjectStatusMessageDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (SetTaskSuccessorsStartDateWhenAllPredecessorsCompletedDataManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (SetTaskStatusDateManipulationRule).FullName),
							container.Resolve<ITaskWorkflowRule>(
								typeof (LastTaskCompletedCanceledSetProjectComplete).FullName)
						}))
				.RegisterType<IWorkflowManagerFactory, WorkflowManagerFactory>(new ContainerControlledLifetimeManager())
				.RegisterType<IWorkflowDataContextFactory, WorkflowDataContextFactory>(new ContainerControlledLifetimeManager());
			return container;
		}

		/// <summary>
		/// Sets up the project manager.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="useThreadResolver">if set to <c>true</c> [use thread resolver].</param>
		/// <returns>IUnityContainer.</returns>
		public static IUnityContainer SetupProjectManager(this IUnityContainer container, bool useThreadResolver = false)
		{
			container
				.SetupSharepointBase(useThreadResolver)
				.SetupHistoryService()
				.RegisterType<IMultiProjectDocumentBuilder, MultiProjectDocumentBuilder>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectStatusMessageRepository, ProjectStatusMessageRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectStatusMessageProvider, ProjectStatusMessageProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterInstance(
					Intercept.ThroughProxy<IProjectRepository>(
						new ProjectRepository(container.Resolve<ITransactionFactory>()),
						new InterfaceInterceptor(),
						new IInterceptionBehavior[]
						{
							new ProjectAuditInterceptor(
								container.Resolve<IHistoryProvider>(),
								container.Resolve<IPrincipalResolver>(),
								container.Resolve<IProfileManager>(),
								container.Resolve<IMapperRegistry>()
								)
						}
						),
					new ContainerControlledLifetimeManager())
				.SetupTaskStatus()
				.SetupDataManipulation()
				.RegisterType<IProjectProjectTemplateRepository, ProjectProjectTemplateRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectProvider, ProjectProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectCoreManager, ProjectCoreManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectManager, ProjectManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectTemplateTaskCreationManager, ProjectTemplateTaskCreationManager>(
					new ContainerControlledLifetimeManager())
				.RegisterInstance(
					Intercept.ThroughProxy<ITaskRepository>(
						new TaskRepository(),
						new InterfaceInterceptor(),
						new IInterceptionBehavior[]
						{
							new TaskAuditInterceptor(
								container.Resolve<IHistoryProvider>(),
								container.Resolve<IPrincipalResolver>(),
								container.Resolve<IProfileManager>(),
								container.Resolve<IMapperRegistry>()
								)
						}
						),
					new ContainerControlledLifetimeManager())
				.SetupTaskNotification()
				.SetupTaskTypeNotification()
				.RegisterType<ITaskProvider, TaskProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskManager, TaskManager>(
					new ContainerControlledLifetimeManager())
				.SetupTaskValidation()
				.SetupLookupService()
				.SetupDocumentService()
				.RegisterType<IProjectTemplateRepository, ProjectTemplateRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectTemplateProvider, ProjectTemplateProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectTemplateManager, ProjectTemplateManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTemplateRepository, TaskTemplateRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskTemplateProvider, TaskTemplateProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IIncomingOrderRepository, IncomingOrderRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IIncomingOrderCoreProvider, IncomingOrderCoreProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IIncomingOrderProvider, IncomingOrderProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IIncomingOrderManager, IncomingOrderManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectStatusMessageProvider, ProjectStatusMessageProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectStatusMessageRepository, ProjectStatusMessageRepository>(
					new ContainerControlledLifetimeManager())
				.SetupProjectValidation()
				.SetupInboundOrderMessage()
				.SetupTaskTypeService()
				.SetupLinkService();
			//.RegisterType<ITaskManager, TaskManager>(new ContainerControlledLifetimeManager())

			return container;
		}

		/// <summary>
		///     Sets up the product family manager.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupProductFamilyManager(this IUnityContainer container)
		{
			container
				.SetupSharepointBase()
				.RegisterType<IProductFamilyAttributeRepository, ProductFamilyAttributeRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyAttributeProvider, ProductFamilyAttributeProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureProvider, ProductFamilyFeatureProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureRepository, ProductFamilyFeatureRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyProvider, ProductFamilyProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyRepository, ProductFamilyRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyManager, ProductFamilyManager>(new ContainerControlledLifetimeManager())
				.RegisterType
				<IProductFamilyAssociationRepository<ProductFamilyFeatureAssociation>,
					ProductFamilyFeatureAssociationRepository>(new ContainerControlledLifetimeManager())
				.RegisterType
				<IProductFamilyAssociationRepository<ProductFamilyAttributeAssociation>,
					ProductFamilyAttributeAssociationRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductFeatureOptionRepository, ProductFeatureOptionRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductAttributeOptionRepository, ProductAttributeOptionRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyImportManager, ProductFamilyImportManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductMetaDataProvider, ProductMetaDataProviderStub>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureValueRepository, ProductFamilyFeatureValueRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureAllowedValueRepository, ProductFamilyFeatureAllowedValueRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyValidationManager, ProductFamilyValidationManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType
				<IProductFamilyFeatureAllowedValueDependencyRepository,
					ProductFamilyFeatureAllowedValueDependencyRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyCharacteristicGroupHelper, ProductFamilyCharacteristicGroupHelper>(
					new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the product family service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupProductFamilyService(this IUnityContainer container)
		{
			container
				.SetupProductFamilyManager()
				.RegisterType<IUnitOfMeasureProvider, UnitOfMeasureProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IUnitOfMeasureRepository, UnitOfMeasureRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IUnitOfMeasureService, UnitOfMeasureService>(new ContainerControlledLifetimeManager())
				.RegisterType<ICategoryRepository, CategoryRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ICategoryProvider, CategoryProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<ICategoryService, CategoryService>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductCharacteristicChildManager, ProductCharacteristicChildManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyService, ProductFamilyService>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyAttributeService, ProductFamilyAttributeService>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureService, ProductFamilyFeatureService>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyAttributeManager, ProductFamilyAttributeManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureManager, ProductFamilyFeatureManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyTemplateManager, ProductFamilyTemplateManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyDocumentBuilderLocator, ProductFamilyDocumentBuilderLocator>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyDocumentBuilder, ProductTemplateBuilder>("ProductTemplate",
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureValueService, ProductFamilyFeatureValueService>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyFeatureValueProvider, ProductFamilyFeatureValueProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IValidationBuilder, UnitOfMeasureValidationBuilder>(
					typeof(UnitOfMeasureValidationBuilder).Name, new ContainerControlledLifetimeManager())
				.RegisterType<IValidationBuilder, AllowedValuteTypeValidationBuilder>(
					typeof(AllowedValuteTypeValidationBuilder).Name, new ContainerControlledLifetimeManager())
				.RegisterType<IProductFamilyDocumentBuilder, ProductFamilyMultiTemplateBuilder>("Template",
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(
						container.Resolve<IProductMetaDataProvider>(),
						container.Resolve<IProductFamilyFeatureProvider>(),
						container.Resolve<IValidationBuilder>(typeof(UnitOfMeasureValidationBuilder).Name),
						container.Resolve<IValidationBuilder>(typeof(AllowedValuteTypeValidationBuilder).Name)
						))
				;

			return container;
		}

		/// <summary>
		///     Sets up the product service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupProductManager(this IUnityContainer container)
		{
			container
				.SetupProductFamilyManager()
				.RegisterType<IProductManager, ProductManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductProvider, ProductProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductRepository, ProductRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadRepository, ProductUploadRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductCharacteristicChildManager, ProductCharacteristicChildManager>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadResultRepository, ProductUploadResultRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductUploadMessageRepository, ProductUploadMessageRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProductDocumentBuilder, ProductDocumentBuilder>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductDocumentManager, ProductDocumentManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IProductValidationManager, ProductValidationManager>(
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(
						container.Resolve<IProductFamilyManager>(),
						new List<IProductValidator>(),
						new List<ICharacteristicValidator>(),
						new List<ICharacteristicValidator>()))
				;
			SetupDocumentService(container);
			return container;
		}

		/// <summary>
		///     Registers the scratch space.
		/// </summary>
		/// <param name="container">The container.</param>
		public static IUnityContainer SetupScratchSpace(this IUnityContainer container)
		{
			var configuration = container.Resolve<IServiceConfiguration>();
			if (configuration.ScratchSpaceStorageOption == ScratchSpaceStorageOption.Azure)
				container.RegisterType<IScratchSpaceRepository, AzureScratchSpaceRepository>(
					new ContainerControlledLifetimeManager());
			else
				container.RegisterType<IScratchSpaceRepository, LocalScratchSpaceRepository>(
					new ContainerControlledLifetimeManager());

			container
				.RegisterInstance<IScratchSpaceConfigurationSource>(new LocalScratchSpaceRepository())
				;

			return container;
		}

		/// <summary>
		///     Registers the claims managers.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupClaimsManagers(this IUnityContainer container)
		{
			container
				.RegisterType<IUserBusinessClaimProvider, UserBusinessClaimProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IUserClaimService, UserClaimService>(new ContainerControlledLifetimeManager())
				.RegisterType<IUserClaimProvider, UserClaimProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IUserClaimRepository, UserClaimRepository>(new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the common authorization.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupAuthorizationCommon(this IUnityContainer container)
		{
			container
				.RegisterType<ICacheManagerConfigurationManager, CacheManagerConfigurationManager>(
					typeof(CacheManagerConfigurationManager).FullName, new ContainerControlledLifetimeManager())
				.RegisterType<Enterprise.Foundation.Framework.ICacheManager, WebCacheManager>(
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(
						container.Resolve<ICacheManagerConfigurationManager>(
							typeof(CacheManagerConfigurationManager).FullName)))
				.RegisterType<IAuthorizationContextValidator, CommonAuthorizationContextValidator>()
				.RegisterType<IAuthorizationFilter, UlAdministratorAuthorizationFilter>(
					typeof(UlAdministratorAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(0))
				.RegisterType<IAuthorizationFilter, CompanyAdminAuthorizationFilter>(
					typeof(CompanyAdminAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(1, container.Resolve<IAuthorizationContextValidator>()))
				.RegisterType<IAuthorizationFilter, CompanyAccessAuthorizationFilter>(
					typeof(CompanyAccessAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(2, container.Resolve<IAuthorizationContextValidator>()))
				.RegisterType<IAuthorizationFilter, MultipleCompanyAccessAuthorizationFilter>(
					typeof(MultipleCompanyAccessAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(3, container.Resolve<IAuthorizationContextValidator>()))
				.RegisterType<IAuthorizationFilter, OrderAccessAuthorizationFilter>(
					typeof(OrderAccessAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(4, container.Resolve<IAuthorizationContextValidator>()))
				.RegisterType<IAuthorizationFilter, ProductUpdateAuthorizationFilter>(
					typeof(ProductUpdateAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(5, container.Resolve<IContainerService>()))
				.RegisterType<IAuthorizationFilter, ProductStatusOverrideAuthorizationFilter>(
					typeof(ProductStatusOverrideAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(6))
				.RegisterType<IAuthorizationFilter, ContainerAccessAuthorizationFilter>(
					typeof(ContainerAccessAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(7, container.Resolve<IContainerService>()))
				.RegisterType<IAuthorizationFilter, ProjectTemplateAdministratorAuthorizationFilter>(
					typeof(ProjectTemplateAdministratorAuthorizationFilter).FullName,
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(8))
				.RegisterInstance<IAuthorizationFilterFactory>(
					new AuthorizationFilterFactory(container.ResolveAll<IAuthorizationFilter>()))
				.RegisterType<ICacheManagerConfigurationManager, CacheManagerConfigurationManager>(
					typeof(CacheManagerConfigurationManager).FullName, new ContainerControlledLifetimeManager())
				.RegisterType<Enterprise.Foundation.Framework.ICacheManager, WebCacheManager>(
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor(
						container.Resolve<ICacheManagerConfigurationManager>(
							typeof(CacheManagerConfigurationManager).FullName)))
				.RegisterType<IAuthorizationManager, AuthorizationManager>(new ContainerControlledLifetimeManager())
				.RegisterInstance<IAuthorizationFilterFactory>(
					new AuthorizationFilterFactory(container.ResolveAll<IAuthorizationFilter>()))
				;

			return container;
		}

		/// <summary>
		/// Setups the task service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupTaskService(this IUnityContainer container)
		{
			container
				.SetupProjectManager()
				.RegisterType<ITaskService, TaskService>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskManager, TaskManager>(new ContainerControlledLifetimeManager())
				.RegisterType<ITaskDocumentBuilder, TaskDocumentBuilder>(new ContainerControlledLifetimeManager())
				;
			SetupTaskValidation(container);

			return container;
		}

		/// <summary>
		/// Setups the task validation.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupProjectValidation(this IUnityContainer container)
		{
			container
				.RegisterType<IProjectValidator, ProjectTaskStatusValidator>("ProjectTaskStatusValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectValidator, ProjectServiceLineValidator>("ProjectServiceLineValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectValidator, ProjectIndustryCodeValidator>("ProjectIndustryCodeValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<IProjectValidationManager, ProjectValidationManager>(
					new ContainerControlledLifetimeManager(), new InjectionConstructor(
						new List<IProjectValidator>
						{
							container.Resolve<IProjectValidator>("ProjectTaskStatusValidator"),
							container.Resolve<IProjectValidator>("ProjectServiceLineValidator"),
							container.Resolve<IProjectValidator>("ProjectIndustryCodeValidator")
						}))
				;

			return container;
		}

		/// <summary>
		/// Setups the task status.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns>IUnityContainer.</returns>
		public static IUnityContainer SetupTaskStatus(this IUnityContainer container)
		{
			container
				.RegisterType<ITaskFetchStatusStrategyFactory, TaskFetchStatusStrategyFactory>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusStrategyCalculate>(TaskStatusEnumDto.InProgress.ToString(),
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusStrategyCalculate>(
					TaskStatusEnumDto.AwaitingAssignment.ToString(), new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusStrategyCalculate>(
					TaskStatusEnumDto.NotScheduled.ToString(), new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusStrategyCalculate>(TaskStatusEnumDto.NotStarted.ToString(),
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusStrategyCalculate>(TaskStatusEnumDto.RemoveHold.ToString(),
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusStrategyNonCalculate>(TaskStatusEnumDto.OnHold.ToString(),
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusReActivateStrategy>(TaskStatusEnumDto.Canceled.ToString(),
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusStrategy, TaskFetchStatusReActivateStrategy>(TaskStatusEnumDto.Completed.ToString(),
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusListStrategyFactory, TaskFetchStatusListStrategyFactory>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusListStrategy, TaskFetchStatusListStrategyInProgress>(
					TaskStatusEnumDto.InProgress.ToString(), new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusListStrategy, TaskFetchStatusListStrategyAwaitingAssignment>(
					TaskStatusEnumDto.AwaitingAssignment.ToString(), new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusListStrategy, TaskFetchStatusListStrategyNotScheduled>(
					TaskStatusEnumDto.NotScheduled.ToString(), new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusListStrategy, TaskFetchStatusListStrategyOnHold>(TaskStatusEnumDto.OnHold.ToString(),
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusListStrategy, TaskFetchStatusListStrategyCanceled>(
					TaskStatusEnumDto.Canceled.ToString(), new ContainerControlledLifetimeManager())
				.RegisterType<ITaskFetchStatusListStrategy, TaskFetchStatusListStrategyCompleted>(
					TaskStatusEnumDto.Completed.ToString(), new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the task validation.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupTaskValidation(this IUnityContainer container)
		{
			container
				.SetupTaskTypeService()
				.RegisterType<ITaskValidator, TaskParentChildPredecessorValidator>(
					"TaskParentChildPredecessorValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskSubTaskPredecessorStatusValidator>(
					"TaskSubTaskPredecessorStatusValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskUnassignedValidator>("TaskUnassignedValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskAwaitingAssignmentStatusValidator>(
					"TaskAwaitingAssignmentStatusValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskRequiredStringPropertyValidator>(
					"TaskTitleRequiredStringPropertyValidator",
					new ContainerControlledLifetimeManager(), new InjectionConstructor("Title", TaskValidationEnumDto.TaskNameRequired))
				.RegisterType<ITaskValidator, TaskRequiredStringPropertyValidator>(
					"TaskDescriptionRequiredStringPropertyValidator",
					new ContainerControlledLifetimeManager(),
					new InjectionConstructor("Description", TaskValidationEnumDto.DescriptionMaxLength, 0, 50))
				.RegisterType<ITaskValidator, TaskOwnerIsUlEmailValidator>("TaskOwnerIsUlEmailValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskNotificationsRequireFreeformValidator>(
					"TaskNotificationsRequireFreeformValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskIsProjectHandlerRestrictedValidator>(
					"TaskIsProjectHandlerRestrictedValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskBehaviorRestrictedToProjectHandlerValidator>(
					"TaskBehaviorRestrictedToProjectHandlerValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskBehaviorChildOfRestrictedChildTaskNumbers>(
					"TaskChildOfTaskBehaviorRestrictedToProjectHandler",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskValidator, TaskBehaviorParentOfRestrictedParentNumbers>(
					"TaskBehaviorParentOfRestrictedParentNumbers",
					new ContainerControlledLifetimeManager())
                .RegisterType<ITaskValidator, TaskNotificationsRestrictedToProjectHandlerOrTaskOwner>(
                    "TaskNotificationsRestrictedToProjectHandlerOrTaskOwner",
					new ContainerControlledLifetimeManager())
                .RegisterType<ITaskValidator, TaskDueDateMustBeGreaterThanStartDateValidator>(
                    "TaskDueDateMustBeGreaterThanStartDateValidator",
                    new ContainerControlledLifetimeManager())
                .RegisterType<ITaskValidator, TaskSelfReferencingPredecessorNotAllowedValidator>(
                   typeof(TaskSelfReferencingPredecessorNotAllowedValidator).Name,
                    new ContainerControlledLifetimeManager())

				.RegisterType<ITaskValidationManager, TaskValidationManager>(
					new ContainerControlledLifetimeManager(), new InjectionConstructor(
						new List<ITaskValidator>
						{
                            container.Resolve<ITaskValidator>(typeof(TaskSelfReferencingPredecessorNotAllowedValidator).Name),
							container.Resolve<ITaskValidator>("TaskTitleRequiredStringPropertyValidator"),
							container.Resolve<ITaskValidator>("TaskDescriptionRequiredStringPropertyValidator"),
							container.Resolve<ITaskValidator>("TaskOwnerIsUlEmailValidator"),
							container.Resolve<ITaskValidator>("TaskNotificationsRequireFreeformValidator"),
							container.Resolve<ITaskValidator>("TaskParentChildPredecessorValidator"),
							container.Resolve<ITaskValidator>("TaskSubTaskPredecessorStatusValidator"),
							container.Resolve<ITaskValidator>("TaskUnassignedValidator"),
							container.Resolve<ITaskValidator>("TaskAwaitingAssignmentStatusValidator"),
							container.Resolve<ITaskValidator>("TaskIsProjectHandlerRestrictedValidator"),
							container.Resolve<ITaskValidator>("TaskBehaviorRestrictedToProjectHandlerValidator"),
							container.Resolve<ITaskValidator>("TaskChildOfTaskBehaviorRestrictedToProjectHandler"),
							container.Resolve<ITaskValidator>("TaskBehaviorParentOfRestrictedParentNumbers"),
                            container.Resolve<ITaskValidator>("TaskDueDateMustBeGreaterThanStartDateValidator"),
                            container.Resolve<ITaskValidator>("TaskNotificationsRestrictedToProjectHandlerOrTaskOwner")
						}))
				.RegisterType<ITaskDeleteValidator, TaskDeletePreventDeletionValidator>(
					"TaskDeletePreventDeletionValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskDeleteValidator, TaskDeleteProjectStatusValidator>(
					"TaskDeleteProjectStatusValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskDeleteValidator, TaskDeleteProjectHandlerValidator>(
					"TaskDeleteProjectHandlerValidator",
					new ContainerControlledLifetimeManager())

				.RegisterType<ITaskDeleteValidator, TaskDeleteChildTaskStatusValidator>(
					"TaskDeleteChildTaskStatusValidator",
					new ContainerControlledLifetimeManager())
				.RegisterType<ITaskDeleteValidator, TaskDeleteChildTaskPreventDeleteValidator>(
				"TaskDeleteChildTaskPreventDeleteValidator",
				new ContainerControlledLifetimeManager())

				.RegisterType<ITaskDeleteValidationManager, TaskDeleteValidationManager>(
					new ContainerControlledLifetimeManager(), new InjectionConstructor(
						new List<ITaskDeleteValidator>
						{
							container.Resolve<ITaskDeleteValidator>("TaskDeleteProjectStatusValidator"),
							container.Resolve<ITaskDeleteValidator>("TaskDeleteProjectHandlerValidator"),
							container.Resolve<ITaskDeleteValidator>("TaskDeletePreventDeletionValidator"),
							container.Resolve<ITaskDeleteValidator>("TaskDeleteChildTaskStatusValidator"),
							container.Resolve<ITaskDeleteValidator>("TaskDeleteChildTaskPreventDeleteValidator")
						}))
				;

			return container;
		}

		/// <summary>
		/// Setups the order service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupOrderService(this IUnityContainer container)
		{
			container
				.SetupInboundOrderMessage()
				.RegisterType<IOrderProvider, OrderProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IOrderManager, OrderManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IOrderService, OrderService>(new ContainerControlledLifetimeManager())
				.RegisterType<IOrderServiceLineDetailRepository, OrderServiceLineDetailRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IOrderServiceLineDetailProvider, OrderServiceLineDetailProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IOrderServiceLineDetailService, OrderServiceLineDetailService>(
					new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		///     Setups the service authorization.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupServiceAuthorization(this IUnityContainer container)
		{
			container.SetupSearchCoordinator()
				.SetupClaimsManagers()
				.RegisterType<IContainerService, ContainerService>(new ContainerControlledLifetimeManager())
				.RegisterType<IProfileService, ProfileService>(new ContainerControlledLifetimeManager())
				.RegisterType<ICompanyService, CompanyService>()
				.SetupAuthorizationCommon()
				;

			return container;
		}

		/// <summary>
		/// Setups the WCF behaviors.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupWcfBehaviors(this IUnityContainer container)
		{
			container
				.RegisterType<IHttpStatusProvider, HttpStatusProvider>(new HierarchicalLifetimeManager())
				.RegisterType<IErrorHandler, HttpExceptionErrorHandler>(new HierarchicalLifetimeManager())
				.RegisterType<IServiceBehavior, HttpExceptionServiceBehavior>("HttpExceptionServiceBehavior",
					new HierarchicalLifetimeManager())
				.RegisterType<IOperationBehavior, LoggingOperationBehavior>(typeof(LoggingOperationBehavior).FullName,
					new ContainerControlledLifetimeManager())
				.RegisterType<IDispatchMessageInspector, CorrelationMessageInspector>(
					typeof(CorrelationMessageInspector).FullName, new ContainerControlledLifetimeManager())
				.RegisterType<IDispatchMessageInspector, ClaimsMessageInspector>(
					typeof(ClaimsMessageInspector).FullName, new ContainerControlledLifetimeManager())
				.RegisterInstance<IEndpointBehavior>(typeof(CorrelationEndpointBehavior).FullName,
					new CorrelationEndpointBehavior(
						container.Resolve<IDispatchMessageInspector>(typeof(CorrelationMessageInspector).FullName)))
				.RegisterInstance<IEndpointBehavior>(typeof(ClaimsEndpointBehavior).FullName,
					new ClaimsEndpointBehavior(
						container.Resolve<IDispatchMessageInspector>(typeof(ClaimsMessageInspector).FullName)))
				;

			return container;
		}

		/// <summary>
		/// Setups the notification service.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupNotificationService(this IUnityContainer container)
		{
			container
				.RegisterType<INotificationService, NotificationService>(new ContainerControlledLifetimeManager())
				;

			return container;
		}

		/// <summary>
		/// Setups the notifications.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupNotifications(this IUnityContainer container)
		{
			var builder = new ContainerBuilder(container);
			// Order Notification Checks
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(OrderNotificationCheckManager)))
				.IgnoreSystemInterfaces()
				.SetActivatableClassifiersOnly()
				.OnlyIncludedInterface<IOrderNotificationCheck>()
				.AsImplementedInterfaces()
				;

			// Order Line Notification Checks
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(OrderLineNotificationCheckManager)))
				.IgnoreSystemInterfaces()
				.SetActivatableClassifiersOnly()
				.OnlyIncludedInterface<IOrderLineNotificationCheck>()
				.AsImplementedInterfaces()
				;

			// Task Notification Checks
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TaskNotificationCheckManager)))
				.IgnoreSystemInterfaces()
				.SetActivatableClassifiersOnly()
				.OnlyIncludedInterface<ITaskNotificationCheck>()
				.AsImplementedInterfaces()
				;


			// Project Notification Checks
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(ProjectNotificationCheckManager)))
				.IgnoreSystemInterfaces()
				.SetActivatableClassifiersOnly()
				.OnlyIncludedInterface<IProjectNotificationCheck>()
				.AsImplementedInterfaces()
				;


			// Notification Strategies
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(NotificationManager)))
				.IgnoreSystemInterfaces()
				.SetActivatableClassifiersOnly()
				.OnlyIncludedInterface<INotificationStrategy>()
				.UseAttributeForDuplicateNamingSchema<NotificationTypeAttribute>(x => x.NotificationType)
				.AsImplementedInterfaces()
				;
			//container.RegisterType<IProjectNotificationCheck, ProjectHandlerNotificationCheck>();
			container
				.RegisterType<INotificationManager, NotificationManager>()
				.RegisterType<INotificationProvider, NotificationProvider>()
				.RegisterType<INotificationRepository, NotificationRepository>()
				.RegisterType<INotificationStrategyFactory, NotificationStrategyFactory>()
				.RegisterType<IOrderNotificationCheckManager, OrderNotificationCheckManager>(
					new ContainerControlledLifetimeManager(), new InjectionConstructor(
						container.ResolveAll<IOrderNotificationCheck>()))
				.RegisterType<IProjectNotificationCheckManager, ProjectNotificationCheckManager>(
					new ContainerControlledLifetimeManager(), new InjectionConstructor(
						container.ResolveAll<IProjectNotificationCheck>()))
				.RegisterType<IOrderLineNotificationCheckManager, OrderLineNotificationCheckManager>(
					new ContainerControlledLifetimeManager(), new InjectionConstructor(
						container.ResolveAll<IOrderLineNotificationCheck>()))
				.RegisterType<ITaskNotificationCheckManager, TaskNotificationCheckManager>(
					new ContainerControlledLifetimeManager(), new InjectionConstructor(
						container.ResolveAll<ITaskNotificationCheck>()))
				;

			return container;
		}


		/// <summary>
		/// Setups the task progress query builder.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupTaskProgressQueryBuilder(this IUnityContainer container)
		{
			container.RegisterType<ITaskProgressQueryBuilderFactory, TaskProgressQueryBuilderFactory>();
			var builder = new ContainerBuilder(container);
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(TaskProgressQueryBuilderFactory)))
				.SetActivatableClassifiersOnly()
				.IgnoreSystemInterfaces()
				.UseAttributeForDuplicateNamingSchema<QueryBuilderAttribute>(x => x.Name)
				.Where(x => x.GetInterface(typeof(IQueryBuilder).FullName) != null)
				.AsImplementedInterfaces();
			return container;
		}


		/// <summary>
		/// Setups the search coordinator which enables us to add muniputaion behavior 
		/// to the search criteria and then rollback the changes.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupSearchCoordinator(this IUnityContainer container)
		{
			// ISearch Coordinator registration -- If another Coordinator is added use the builder
			//Builder will not use the type name for named registration until there is at least two
			//classes that implement the same interface.
			//var builder = new ContainerBuilder(container);
			//builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(SearchCoordinatorFactory)))
			//  .SetLifetimeManager(typeof(TransientLifetimeManager))
			//  .IgnoreSystemInterfaces()
			//  .SetActivatableClassifiersOnly()
			//  .OnlyIncludedInterface<ISearchCoordinator>()
			//  .AsImplementedInterfaces();

			//because search coordinators act like a decorator and need some state information, a new instances at least one per thread is need.
			container.RegisterType<ISearchCoordinator, ProjectPastDueSearchFilterDependencyCoordinator>(
				typeof(ProjectPastDueSearchFilterDependencyCoordinator).Name, new TransientLifetimeManager());

			container.RegisterType<ISearchCoordinator, TaskProgressSearchFilterCoordinator>(
				typeof(TaskProgressSearchFilterCoordinator).Name, new TransientLifetimeManager());

			container.RegisterType<ISearchCoordinatorFactory, SearchCoordinatorFactory>(new ContainerControlledLifetimeManager());

			return container;
		}

		/// <summary>
		/// Setups the entity change tracking.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupInterception(this IUnityContainer container)
		{
			var interception = new Interception();
			container.AddExtension(interception);

			return container;
		}

		/// <summary>
		/// Setups the task template.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <returns></returns>
		public static IUnityContainer SetupTaskTemplate(this IUnityContainer container)
		{
			container
				.RegisterType<ITaskCategoryService, TaskCategoryService>()
				.RegisterType<ITaskCategoryProvider, TaskCategoryProvider>()
				.RegisterType<ITaskCategoryManager, TaskCategoryManager>()
				.RegisterType<ITaskCategoryRepository, TaskCategoryRepository>();
			return container;
		}
	}
}