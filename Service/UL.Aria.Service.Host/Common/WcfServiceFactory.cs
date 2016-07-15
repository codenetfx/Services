using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;
using UL.Aria.Common.BusinessMessage;
using UL.Enterprise.Foundation.Domain;
using UL.Enterprise.Foundation.Service.Logging;
using UL.Enterprise.Foundation.Service.STS;
using UL.Enterprise.Foundation.Service.Unity;
using UL.Enterprise.Foundation.Xslt;
using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Claim.Data;
using UL.Aria.Service.Claim.Domain;
using UL.Aria.Service.Claim.Implementation;
using UL.Aria.Service.Claim.Provider;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Implementation;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Proxy;
using UL.Aria.Service.Repository;
using UL.Aria.Service.Security;

namespace UL.Aria.Service.Host.Common
{
    /// <summary>
    ///     WCF Service host factory class.
    /// </summary>
    /// <remark>
    ///     If .svc files are used, the same WCFServiceFactory instance should be shared via both contstruction paths (i.e.
    ///     global.ascx, .svc Factory attribute.
    /// </remark>
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WcfServiceFactory" /> class.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public WcfServiceFactory(UnityContainer container)
            : base(container)
        {
        }

        /// <summary>
        ///     Configures the container.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.RegisterInstance<IContainerDefinitionBuilder>(
                new ContainerDefinitionBuilder(
                    new Dictionary<EntityTypeEnumDto, IContainerBuilder>
                    {
                        {EntityTypeEnumDto.Order, new OrderContainerBuilder()},
                        {EntityTypeEnumDto.Product, new ProductContainerBuilder()},
                        {EntityTypeEnumDto.Project, new ProjectContainerBuilder()}
                    }
                    ));
            //Register singleton types (i.e. ContainerControlledLifetimeManager)
            container
                .SetupInterception()
                .RegisterType<ILogCategoryResolver, LogCategoryResolver>(new ContainerControlledLifetimeManager())
                .SetupCacheLayer()
                .SetupSharepointBase()
                .SetupWcfBehaviors()
                .SetupServiceAuthorization()
                .SetupProductManager()
                .SetupScratchSpace()
                .SetupProductFamilyService()
                .SetupProductService()
				.SetupTaskService()
                .SetupOrderService()
                .SetupProjectService()
                .SetupNotifications()
                .SetupNotificationService()
                .SetupLinkService()
                .SetupTaskTypeService()
                .SetupUserTeamService()

                .RegisterType<IBusinessMessageProvider, BusinessMessageProvider>(
                    new ContainerControlledLifetimeManager())
                
                .RegisterType<ICompanyService, CompanyService>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserClaimProvider, UserClaimProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserClaimRepository, UserClaimRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IClaimDefinitionService, ClaimDefinitionService>(new ContainerControlledLifetimeManager())
                .RegisterType<IClaimDefinitionRepository, ClaimDefinitionRepository>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IClaimDefinitionProvider, ClaimDefinitionProvider>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IValidator<ClaimDefinition>, ClaimDefinitionValidator>(
                   new ContainerControlledLifetimeManager())
                .RegisterType<IScratchSpaceService, ScratchSpaceService>(new ContainerControlledLifetimeManager())
                .RegisterType<IIncomingOrderService, IncomingOrderService>(new ContainerControlledLifetimeManager())
                .RegisterType<IProjectTemplateService, ProjectTemplateService>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IUserBusinessClaimServiceValidator, UserBusinessClaimServiceValidator>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IUserBusinessClaimService, UserBusinessClaimService>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IAriaService, AriaService>(new ContainerControlledLifetimeManager())
                .RegisterType<IContentService, ContentService>(new ContainerControlledLifetimeManager())
                .RegisterType<IXsltHelper, XsltHelper>(new ContainerControlledLifetimeManager())
                .RegisterType<ICustomerService, CustomerService>(new ContainerControlledLifetimeManager())
                .RegisterType<ICustomerPartyService, CustomerPartyServiceProxy>(new ContainerControlledLifetimeManager())
				.RegisterType<IOutboundDocumentServiceProxy, OutboundDocumentServiceProxy>(new ContainerControlledLifetimeManager())
                .RegisterType<ICustomerManager, CustomerManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IBusinessUnitService, BusinessUnitService>(new ContainerControlledLifetimeManager())
		        .RegisterType<ICustomerPartyProxyConfigurationSource, CustomerPartyProxyConfigurationSource>(
			        new ContainerControlledLifetimeManager())
				.RegisterType<IOutboundDocumentProxyConfigurationSource, OutboundDocumentProxyConfigurationSource>(
					new ContainerControlledLifetimeManager())
				//Register instances
                .RegisterInstance<IUserClaimProvider>(
                    new BusinessMessageUserClaimProviderDecorator(container.Resolve<UserClaimProvider>(),
                        container.Resolve<IBusinessMessageProvider>(), container.Resolve<IUserClaimRepository>()))
		        .SetupDocumentService()
				.RegisterType<IDocumentTemplateService, DocumentTemplateService>(new ContainerControlledLifetimeManager())
                .RegisterType<IDocumentTemplateAssociationRepository, DocumentTemplateAssociationRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IDocumentTemplateAssociationProvider, DocumentTemplateAssociationProvider>(new ContainerControlledLifetimeManager())

                .RegisterType<IContactService, ContactService>(new ContainerControlledLifetimeManager())
                .RegisterType<IContactManager, QueuedContactManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IContactOrderProvider, ContactOrderProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<IAzureServiceBusQueueRepository<ContactOrderDto>, ContactOrderAzureServiceBusQueueRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IAzureServiceBusQueueLocatorProvider, ContactOrderAzureServiceBusQueueLocatorProvider>("ContactOrder")

                .RegisterType<ICertificationRequestService, CertificationRequestServiceProxy>(new ContainerControlledLifetimeManager())
                .RegisterType<ICertificationRequestServiceConfigurationSource, CertificationRequestServiceConfigurationSource>(new ContainerControlledLifetimeManager())
                .RegisterType<ICertificationRequestManager, CertificationRequestManager>(new ContainerControlledLifetimeManager())
                .RegisterType<ICertificationManagementService, CertificationManagementService>(new ContainerControlledLifetimeManager())
                .RegisterType<ITaskPropertyProvider, TaskPropertyProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<ITaskPropertyRepository, TaskPropertyRepository>(new ContainerControlledLifetimeManager())
				;


            container.RegisterType<IEmailService, EmailService>()
                ;
            
            container.RegisterType<ITermsAndConditionsService, TermsAndConditionsService>()
                .RegisterType<ITermsAndConditionsProvider, TermsAndConditionsProvider>()
                ;

            container.RegisterType<ISenderRepository, SenderRepository>(
                new ContainerControlledLifetimeManager());
            container.RegisterType<ISenderProvider, SenderProvider>(
                new ContainerControlledLifetimeManager());

            container.RegisterType<IProjectTemplateService, ProjectTemplateService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IProjectTemplateManager, ProjectTemplateManager>(new ContainerControlledLifetimeManager());
			container.SetupLookupService();
			container.SetupAzureStorage();
	        container.SetupTaskTemplate();
        }
        /// <summary>
        ///     Configures the host.
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        protected override void ConfigureHost(ServiceHostBase serviceHost)
        {
            var serviceCredentials = serviceHost.Description.Behaviors.Find<ServiceCredentials>();

            if (serviceCredentials == null)
            {
                serviceCredentials = new ServiceCredentials();

                serviceHost.Description.Behaviors.Add(serviceCredentials);
            }

            IdentityConfiguration identityConfiguration = new CustomerSecurityTokenServiceConfiguration();

            serviceCredentials.IdentityConfiguration = identityConfiguration;
            serviceCredentials.UseIdentityConfiguration = true;

            IdentityConfiguration identityConfiguration1 = serviceCredentials.IdentityConfiguration;
            identityConfiguration1.ClaimsAuthorizationManager = new CustomClaimsAuthorizationManager();
            identityConfiguration1.ClaimsAuthenticationManager = new CustomClaimsAuthenticationManager();
        }

        /// <summary>
        ///     Should not be used.  This is just so we can test the ConfigureHost method.
        /// </summary>
        /// <param name="serviceHost"></param>
        internal void ConfigureHostTest(ServiceHostBase serviceHost)
        {
            ConfigureHost(serviceHost);
        }
    }
}