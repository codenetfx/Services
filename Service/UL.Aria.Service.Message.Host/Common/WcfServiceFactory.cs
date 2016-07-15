using System.ServiceModel;
using System.ServiceModel.Description;
using Microsoft.Practices.Unity;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Logging;
using UL.Enterprise.Foundation.Service.Unity;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Message.Implementation;
using UL.Aria.Service.Message.Provider;
using UL.Aria.Service.Message.Repository;
using UL.Aria.Service.Message.Common;

namespace UL.Aria.Service.Message.Host.Common
{
    /// <summary>
    /// WCF Service host factory class.
    /// </summary>
    /// <remark>If .svc files are used, the same WCFServiceFactory instance should be shared via both contstruction paths (i.e. global.ascx, .svc Factory attribute. </remark>
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHostFactory"/> class.
        /// </summary>
        /// <param name="container">The unity container.</param>
        public WcfServiceFactory(UnityContainer container) : base(container)
        {
        }

        /// <summary>
        /// Configures the container.
        /// </summary>
        /// <param name="container">The container.</param>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container
                .RegisterType<IOrderMessageRepository, OrderMessageRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IOrderMessageProvider, OrderMessageProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<ILogManager,LogManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IMapperRegistry,ServiceMapperRegistry>(new ContainerControlledLifetimeManager())
                .RegisterType<ILogCategoryResolver, LogCategoryResolver>(new ContainerControlledLifetimeManager())
                .RegisterType<IOperationBehavior, LoggingOperationBehavior>(typeof(LoggingOperationBehavior).FullName, new ContainerControlledLifetimeManager())
                .RegisterType<IOrderMessageService, OrderMessageService>(new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Configures the host.
        /// </summary>
        /// <param name="serviceHost">The service host.</param>
        protected override void ConfigureHost(ServiceHostBase serviceHost)
        {
            //throw new System.NotImplementedException();
        }
    }
}