using System.Collections.Generic;
using Microsoft.Practices.Unity;
using RazorEngine.Templating;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Host;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;
using UL.Aria.Web.Common.Configuration;
using UL.Aria.Web.Common.Services;

using IProxyConfigurationSource = UL.Aria.Web.Common.Services.IProxyConfigurationSource;
using ProxyConfigurationSource = UL.Aria.Web.Common.Services.ProxyConfigurationSource;

namespace UL.Aria.Service.ProductProcessing.Host
{
    internal class UnityInstanceProvider : Disposable
    {
        public static UnityInstanceProvider Create()
        {

            var container = new UnityContainer();
            container
                     .RegisterType<ITransactionFactory, TransactionFactory>(new ContainerControlledLifetimeManager())
                     .RegisterType<IMapperRegistry, ServiceMapperRegistry>(new ContainerControlledLifetimeManager())
                     .RegisterType<IProductCharacteristicImportManager, ProductCharacteristicImportManager>("Characteristics",new ContainerControlledLifetimeManager())
                     .RegisterType<IProductCharacteristicImportManager, StaticColumnProductCharacteristicImportManager>("Statics", new ContainerControlledLifetimeManager());


            container
                .RegisterType
                <IProxyConfigurationSource,
                    ProxyConfigurationSource>(new ContainerControlledLifetimeManager())
                .RegisterType<IProxyConfigurationSource, ProxyConfigurationSource>(new PerThreadLifetimeManager())
                .RegisterType<IServiceHost, WindowsServiceHost>(new ContainerControlledLifetimeManager())
                .RegisterType<IPortalConfiguration, ServiceConfiguration>(new ContainerControlledLifetimeManager())
                .RegisterType<IProcessingManager, ProductUploadImportManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IProductImportManager, ProductImportManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadRepository, ProductUploadRepository>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadResultRepository, ProductUploadResultRepository>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadMessageRepository, ProductUploadMessageRepository>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadProductUpdateManager, ProductUploadProductUpdateManager>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadProductInsertManager, ProductUploadProductInsertManager>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadProductInsertManager, ProductUploadProductInsertManager>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadFamilyCharacteristicProvider, ProductUploadFamilyCharacteristicProvider>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadDocumentImportManager, ProductUploadDocumentImportManager>(
                    new ContainerControlledLifetimeManager())
                .RegisterType<IProductUploadDocumentCharacteristicProvider, ProductUploadDocumentCharacteristicProvider>
                (new ContainerControlledLifetimeManager())
                .RegisterType<IProductService, ProductServiceProxy>(new ContainerControlledLifetimeManager())
                .RegisterType<IScratchSpaceService, ScratchSpaceServiceProxy>(new ContainerControlledLifetimeManager())
                .RegisterType<IScratchSpaceRepository, LocalScratchSpaceRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<IProductClaimAssignmentManager, ProductClaimAssignmentManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IUserBusinessClaimService, UserBusinessClaimServiceProxy>(new ContainerControlledLifetimeManager())
                .RegisterInstance<IScratchSpaceConfigurationSource>(new LocalScratchSpaceRepository())
                .RegisterType<IProfileService, ProfileServiceProxy>(new ContainerControlledLifetimeManager())
                ;
            container = (UnityContainer)container.SetupProductManager();

            IEnumerable<IProductCharacteristicImportManager> productCharacteristicImportManagers = new List<IProductCharacteristicImportManager>
                {
                    container.Resolve<IProductCharacteristicImportManager>("Statics"), container.Resolve<IProductCharacteristicImportManager>("Characteristics")
                };
            container.RegisterInstance(productCharacteristicImportManagers
                );

            container.RegisterInstance<IContainerDefinitionBuilder>(
               new ContainerDefinitionBuilder(
                        new Dictionary<EntityTypeEnumDto, IContainerBuilder>
                        {
                            {EntityTypeEnumDto.Order, new OrderContainerBuilder()},
                            {EntityTypeEnumDto.Product, new ProductContainerBuilder()},
                            {EntityTypeEnumDto.Project, new ProjectContainerBuilder()}
                        }
                    ));
            return new UnityInstanceProvider(container);
        }

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
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }


        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        internal IUnityContainer Container
        {
            get { return _container; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _container.Dispose();
                
            base.Dispose(disposing);
        }
    }
}
