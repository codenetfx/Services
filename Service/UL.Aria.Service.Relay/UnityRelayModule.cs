using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Relay.Common;
using UL.Aria.Service.Relay.Manager;
using UL.Aria.Service.Relay.Service;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Claim;
using UL.Enterprise.Foundation.Service.Correlation;
using UL.Enterprise.Foundation.Service.Host;
using UL.Enterprise.Foundation.Service.Logging;
using UL.Enterprise.Foundation.Unity;

namespace UL.Aria.Service.Relay
{
    /// <summary>
    /// unity boot module for the assembly.
    /// </summary>
    [UnityBootStrap(BootOrder=0)]
    public class UnityRelayModule : IUnityModule
    {
        /// <summary>
        /// Registers the specified unity container.
        /// </summary>
        /// <param name="unityContainer">The unity container.</param>
        public void Register(IUnityContainer unityContainer)
        {
            new Common.ServiceMapperRegistry();
            unityContainer.RegisterType<IProcessingManager, WcfProcessHost>(new ContainerControlledLifetimeManager())
              .RegisterType<IProxyConfigurationSource, ProxyConfigurationSource>(new ContainerControlledLifetimeManager())
			  .RegisterType<IDocumentService, RelayDocumentServiceProxy>(new ContainerControlledLifetimeManager())
			  .RegisterType<IRelayDocumentContentServiceProxy, RelayDocumentContentServiceProxy>(new ContainerControlledLifetimeManager())
			  .RegisterType<IProductService, RelayProductServiceProxy>(new ContainerControlledLifetimeManager())
              .RegisterType<ISimpleProfileService, RelayProfileServiceProxy>(new ContainerControlledLifetimeManager())
              .RegisterType<IAriaService, RelayAriaServiceProxy>(new ContainerControlledLifetimeManager())
              .RegisterType<ICompanyService, RelayCompanyServiceProxy>(new ContainerControlledLifetimeManager())
              .RegisterType<IRelayCompanyManager, RelayCompanyManager>(new ContainerControlledLifetimeManager())
              .RegisterType<IProjectService, RelayProjectServiceProxy>(new ContainerControlledLifetimeManager())
              .RegisterType<IRelayDocumentManager, RelayDocumentManager>(new ContainerControlledLifetimeManager())
              .RegisterType<IRelayProjectManager, RelayProjectManager>(new ContainerControlledLifetimeManager())
              .RegisterType<IRelayProductManager, RelayProductManager>(new ContainerControlledLifetimeManager())
              .RegisterType<IServiceHost, WindowsServiceHost>(new ContainerControlledLifetimeManager())
              .RegisterType<ILogManager, LogManager>(new ContainerControlledLifetimeManager())
              .RegisterType<ILogCategoryResolver, LogCategoryResolver>(new ContainerControlledLifetimeManager())
              .RegisterType<IOperationBehavior, LoggingOperationBehavior>(typeof(LoggingOperationBehavior).FullName, new ContainerControlledLifetimeManager())
              .RegisterType<IDispatchMessageInspector, CorrelationMessageInspector>(
                  typeof(CorrelationMessageInspector).FullName, new ContainerControlledLifetimeManager())
              .RegisterType<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>(typeof(UseRequestHeadersForMetadataAddressBehavior).FullName, new ContainerControlledLifetimeManager())
              .RegisterType<IRelayProductService, RelayProductService>(new ContainerControlledLifetimeManager())
              .RegisterType<IRelayDocumentService, RelayDocumentService>(new ContainerControlledLifetimeManager())
              .RegisterType<IRelayProjectService, RelayProjectService>(new ContainerControlledLifetimeManager())
              .RegisterType<IMapperRegistry, Common.ServiceMapperRegistry>(new ContainerControlledLifetimeManager())
              .RegisterType<IDispatchMessageInspector, ClaimsMessageInspector>(
                  typeof(ClaimsMessageInspector).FullName, new ContainerControlledLifetimeManager())

              .RegisterType<IPrincipalResolver, LocalPrincipalResolver>(new ContainerControlledLifetimeManager())
              ;

            unityContainer.RegisterInstance<IEndpointBehavior>(typeof(ClaimsEndpointBehavior).FullName,
                new ClaimsEndpointBehavior(
                    unityContainer.Resolve<IDispatchMessageInspector>(typeof(ClaimsMessageInspector).FullName)));
            unityContainer.RegisterInstance<IEndpointBehavior>(typeof(CorrelationEndpointBehavior).FullName,
                new CorrelationEndpointBehavior(
                    unityContainer.Resolve<IDispatchMessageInspector>(typeof(CorrelationMessageInspector).FullName)));
                       
        }
    }
}
