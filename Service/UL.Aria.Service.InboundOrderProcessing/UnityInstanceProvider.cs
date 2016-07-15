using System.Diagnostics.CodeAnalysis;
using Microsoft.Practices.Unity;
using UL.Aria.Common.BusinessMessage;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.InboundOrderProcessing.Manager;
using UL.Aria.Service.InboundOrderProcessing.MessageProcessor;
using UL.Aria.Service.InboundOrderProcessing.Provider;
using UL.Aria.Service.InboundOrderProcessing.Repository;
using UL.Aria.Service.InboundOrderProcessing.Resolver;
using UL.Aria.Service.InboundOrderProcessing.Service;
using UL.Aria.Service.InboundOrderProcessing.Validator;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Proxy;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Service.Host;
using IProxyConfigurationSource = UL.Aria.Service.InboundOrderProcessing.Service.IProxyConfigurationSource;

namespace UL.Aria.Service.InboundOrderProcessing
{
	/// <summary>
	///     Basic Instance Manager that uses Unity for dependency injection.
	/// </summary>
	[ExcludeFromCodeCoverage]
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
		public IUnityContainer Container
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
			container
				.SetupServiceCommon(true)
				.SetupInterception()
				.SetupCacheLayer()
				.RegisterType
				<IProxyConfigurationSource,
					ProxyConfigurationSource>(
						new ContainerControlledLifetimeManager());

			container.SetupProjectManager(true)
				.SetupNotifications()
				.SetupServiceAuthorization()
				.RegisterType<IInboundMessageWebJobManager, InboundMessageWebJobManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IAssetFieldMetadata, AssetFieldMetadata>(new ContainerControlledLifetimeManager())
				.RegisterType
				<Aria.Service.Provider.Proxy.IProxyConfigurationSource,
					Aria.Service.Provider.Proxy.ProxyConfigurationSource>(new ContainerControlledLifetimeManager())
				.RegisterType
				<ISharepointConfigurationSource,
					SharepointConfigurationSource>(new ContainerControlledLifetimeManager())
				.RegisterType<IInboundOrderProvider, InboundOrderProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IBusinessMessageProvider, BusinessMessageProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IProcessingManager, InboundMessageManager>(new ContainerControlledLifetimeManager())
				.RegisterType<IInboundOrderRepository, InboundOrderRepository>()
				.RegisterType<IOrderProvider, OrderProvider>()
				.RegisterType<IMessageProcessor, RequestMessageProcessor>("lhs")
				.RegisterType<IMessageProcessor, CustomerMessageProcessor>("customer")
				.RegisterType<IMessageProcessor, OrderMessageProcessor>("general")
				.RegisterType<IMessageProcessor, OrderServiceLineDetailMessageProcessor>("t3")
				.RegisterType<IContactProcessor, ContactProcessor>(new ContainerControlledLifetimeManager())
				.RegisterType<ICustomerPartyService, CustomerPartyServiceProxy>(new ContainerControlledLifetimeManager())
				.RegisterType<IProxyConfigurationSource, ProxyConfigurationSource>(new ContainerControlledLifetimeManager())
				.RegisterType<ICustomerPartyProxyConfigurationSource, ProxyConfigurationSource>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IValidator, OrderValidator>("Order")
				.RegisterType<IValidator, CustomerOrganizationValidator>(EntityTypeEnumDto.CustomerOrganization.ToString())
				.RegisterType<IValidator, RequestValidator>("Request")
				.RegisterType<IValidator, OrderServiceLineDetailValidator>("OrderServiceLineDetail")
				.RegisterType<IXmlParser, IncomingOrderXmlParser>("Order")
				.RegisterType<IXmlParser, CustomerOrganizationParser>(EntityTypeEnumDto.CustomerOrganization.ToString())
				.RegisterType<IXmlParser, IncomingOrderXmlParser>("Request")
				.RegisterType<IXmlParser, OrderServiceLineDetailXmlParser>("OrderServiceLineDetail")
				.RegisterType<IMessageProcessorResolver, MessageProcessorResolver>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IValidatorResolver, ValidatorResolver>(new ContainerControlledLifetimeManager())
				.RegisterType<IXmlParserResolver, XmlParserResolver>(new ContainerControlledLifetimeManager())
				.RegisterType<IOrderServiceLineDetailProvider, OrderServiceLineDetailProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IOrderServiceLineDetailRepository, OrderServiceLineDetailRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ISenderRepository, SenderRepository>(
					new ContainerControlledLifetimeManager())
				.RegisterType<ISenderProvider, SenderProvider>(
					new ContainerControlledLifetimeManager())
				.RegisterType<IIndustryCodeProvider, IndustryCodeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IIndustryCodeRepository, IndustryCodeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<IServiceCodeProvider, ServiceCodeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IServiceCodeRepository, ServiceCodeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ILocationCodeRepository, LocationCodeRepository>(new ContainerControlledLifetimeManager())
				.RegisterType<ILocationCodeProvider, LocationCodeProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IAzureServiceBusQueueLocatorProviderResolver, AzureServiceBusQueueLocatorProviderResolver>(
					new ContainerControlledLifetimeManager())
				;

			SetupContactOrderMessage(container);
			//container.SetupScratchSpace();

			return new UnityInstanceProvider(container);
		}

		/// <summary>
		/// Setups the contact order message.
		/// </summary>
		/// <param name="container">The container.</param>
		public static void SetupContactOrderMessage(IUnityContainer container)
		{
			container
				.RegisterType<IAzureServiceBusQueueLocatorProvider, ContactOrderAzureServiceBusQueueLocatorProvider>("ContactOrder")
				.RegisterType<IContactOrderProvider, ContactOrderProvider>(new ContainerControlledLifetimeManager())
				.RegisterType<IAzureServiceBusQueueRepository<ContactOrderDto>, ContactOrderAzureServiceBusQueueRepository>(
					new ContainerControlledLifetimeManager())
				;
		}
	}
}