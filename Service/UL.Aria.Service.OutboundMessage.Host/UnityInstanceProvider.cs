using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using UL.Aria.Common;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Host;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Proxy;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.OutboundMessage.Host
{
    /// <summary>
    /// 
    /// </summary>
    public class UnityInstanceProvider:Disposable
    {
        private readonly IUnityContainer _container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="UnityInstanceProvider" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        internal UnityInstanceProvider(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        ///     Gets the container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        internal IUnityContainer Container
        {
            get { return _container; }
        }


        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources. </param>
        /// <remarks>
        /// </remarks>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using(_container){}
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///     Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static UnityInstanceProvider Create()
        {
            var container = new UnityContainer();

            container
                .RegisterType<IProxyConfigurationSource, ProxyConfigurationSource>(new ContainerControlledLifetimeManager())
                .RegisterType<IServiceHost, WindowsServiceHost>(new ContainerControlledLifetimeManager())
                .RegisterType<IProjectStatusMessageProvider, ProjectStatusMessageProvider>(new ContainerControlledLifetimeManager())
                .RegisterType<IProjectStatusMessageRepository, ProjectStatusMessageRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IProcessingManager, OutboundMessageManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IPrincipalResolver, ThreadPrincipalResolver>(new ContainerControlledLifetimeManager())
                .RegisterType<ILogManager, LogManager>(new ContainerControlledLifetimeManager())
                .RegisterType<ITransactionFactory, TransactionFactory>(new ContainerControlledLifetimeManager())
                .RegisterType<IMessageService, MessageServiceProxy>(new ContainerControlledLifetimeManager())
                .RegisterType<IMapperRegistry, ServiceMapperRegistry>(new ContainerControlledLifetimeManager())
                
                ;


            return new UnityInstanceProvider(container);
        }
    }
}
