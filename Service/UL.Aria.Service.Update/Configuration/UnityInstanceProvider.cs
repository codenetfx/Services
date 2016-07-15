using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.Unity;

using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Update.Factory;
using UL.Aria.Service.Update.Managers;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Unity;

namespace UL.Aria.Service.Update.Configuration
{
	internal class UnityInstanceProvider : Disposable
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
		/// Gets the container.
		/// </summary>
		/// <value>
		/// The container.
		/// </value>
		internal IUnityContainer Container
		{
			get { return _container; }
		}

		public static UnityInstanceProvider Create()
		{
			var container = new UnityContainer();
			RegisterExternalInstances(container);


			RegisterCommon(container);
			RegisterCommonIam(container);
			RegisterServiceAssembly(container);
			container.SetupTaskService();
			//register special externals


			//register internals (types and instances)            
			container.RegisterInstance<IUpdateConfigurationSource>(new UpdateConfigurationSource())
				.RegisterType<IUpdateManagerFactory, UpdateManagerFactory>(new ContainerControlledLifetimeManager())
				.RegisterType<IFileLocator, FileLocator>(new ContainerControlledLifetimeManager())
				.RegisterType<IFileLogger, FileLogger>(new ContainerControlledLifetimeManager())
				.RegisterType<IFileStreamProvider, FileStreamProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IXmlParserResolver, XmlParserResolver>(new ContainerControlledLifetimeManager())
				.RegisterType<IXmlParser, IncomingOrderXmlParser>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessMessageProvider, BusinessMessageProvider>(new ContainerControlledLifetimeManager());
			container.SetupTaskValidation()
				.SetupProjectManager()
				.SetupNotifications()
				.SetupOrderService()
				.SetupAuthorizationCommon();

			RegisterManagers(container);
			return new UnityInstanceProvider(container);
		}

		internal static void RegisterExternalInstances(IUnityContainer container)
		{
			container.RegisterInstance<IContainerDefinitionBuilder>(
				new ContainerDefinitionBuilder(
					new Dictionary<EntityTypeEnumDto, IContainerBuilder>
					{
						{EntityTypeEnumDto.Order, new OrderContainerBuilder()},
						{EntityTypeEnumDto.Product, new ProductContainerBuilder()},
						{EntityTypeEnumDto.Project, new ProjectContainerBuilder()}
					}));
		}

		internal static void RegisterManagers(IUnityContainer container)
		{
			var builder = new ContainerBuilder(container);
			builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
				.SetActivatableClassifiersOnly()
				.Where(x => x.GetInterface(typeof (IUpdateManager).FullName) != null)
				.UseAttributeForDuplicateNamingSchema<EntityAttribute>(x => x.EntityType)
				.AsImplementedInterfaces();
		}

		internal static void RegisterServiceAssembly(IUnityContainer container)
		{
			var builder = new ContainerBuilder(container);
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof (IProjectProvider)))
				.SetActivatableClassifiersOnly()
				.IgnoreSystemInterfaces()
				.UseAttributeForDuplicateNamingSchema<EntityAttribute>(x => x.EntityType)
				.Where(x => x.GetInterface(typeof (IContainerDefinitionBuilder).FullName) == null)
				.AsImplementedInterfaces();
		}

		internal static void RegisterCommon(IUnityContainer container)
		{
			var builder = new ContainerBuilder(container);
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof (ILogManager)))
				.IgnoreInterface<IFileLocator>()
				.IgnoreInterface<IFileLogger>()
				.IgnoreInterface<IFileStreamProvider>()
				.Where(x => x != typeof (FileLogger)
				            && x != typeof (FileStreamProvider))
				.IgnoreSystemInterfaces()
				.SetActivatableClassifiersOnly()
				.AsImplementedInterfaces();
		}

		internal static void RegisterCommonIam(IUnityContainer container)
		{
			var builder = new ContainerBuilder(container);
			builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof (Iam.Common.ICertificateService)))
				.IgnoreSystemInterfaces()
				.SetActivatableClassifiersOnly()
				.AsImplementedInterfaces();
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
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			using (_container)
			{
			}
			base.Dispose(disposing);
		}
	}
}