using System.Collections.Generic;
using Microsoft.Practices.Unity;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Export.Manager;
using UL.Aria.Service.Manager;
using RazorEngine.Templating;

namespace UL.Aria.Service.Export.Common
{
    /// <summary>
    ///     Manages unity instances for this application.
    /// </summary>
    public class UnityInstanceProvider : Disposable
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
        ///     Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static UnityInstanceProvider Create()
        {
            var container = new UnityContainer();

            container.RegisterInstance<IContainerDefinitionBuilder>(
                new ContainerDefinitionBuilder(
                    new Dictionary<EntityTypeEnumDto, IContainerBuilder>
                    {
                        {EntityTypeEnumDto.Order, new OrderContainerBuilder()},
                        {EntityTypeEnumDto.Product, new ProductContainerBuilder()},
                        {EntityTypeEnumDto.Project, new ProjectContainerBuilder()}
                    }
                    ));
            container
                .RegisterType<IFileStorageManager, BlobFileStorageManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IProjectExportDocumentManager, ProjectExportDocumentManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IProjectExportManager, ProjectExportManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IExportConfiguration, ExportConfiguration>(new ContainerControlledLifetimeManager())
                .RegisterType<IAssetFieldMetadata, AssetFieldMetadata>(new ContainerControlledLifetimeManager())
                .SetupProjectManager()
                .SetupNotifications()
                .SetupServiceAuthorization()
                ;
            return new UnityInstanceProvider(container);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (Container)
                {
                }
            }
            base.Dispose(disposing);
        }
    }
}