using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using Microsoft.ServiceBus;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Proxy
{
	/// <summary>
	/// Class OutboundDocumentServiceProxy.
	/// </summary>
	public class OutboundDocumentServiceProxy : ServiceAgentManagedProxy<IOutboundDocumentService>,
		IOutboundDocumentServiceProxy
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OutboundDocumentServiceProxy"/> class.
		/// </summary>
		/// <param name="configurationSource">The configuration source.</param>
		public OutboundDocumentServiceProxy(IOutboundDocumentProxyConfigurationSource configurationSource) :
			this(
			configurationSource,
			new WebChannelFactory<IOutboundDocumentService>(configurationSource.OutboundDocumentServiceBinding,
				configurationSource.OutboundDocumentServiceUri))
		{
		}

		private OutboundDocumentServiceProxy(IOutboundDocumentProxyConfigurationSource configurationSource,
			ChannelFactory<IOutboundDocumentService> serviceProxyFactory)
			: base(serviceProxyFactory)
		{
			if (null != configurationSource.TokenProvider)
			{
				serviceProxyFactory.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
				{
					TokenProvider = configurationSource.TokenProvider
				});
				serviceProxyFactory.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 2, 0);
				serviceProxyFactory.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 2, 0);
				serviceProxyFactory.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 0, 2, 0);
				serviceProxyFactory.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 2, 0);
			}
		}

		/// <summary>
		/// Documents the exists.
		/// </summary>
		/// <param name="outboundDocument">The outbound document.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool DocumentExists(OutboundDocumentDto outboundDocument)
		{
			var value = false;
			var outboundDocumentService = ClientProxy;
			using (new OperationContextScope((IContextChannel) outboundDocumentService))
			{
				ExecuteAction(() => value = outboundDocumentService.DocumentExists(outboundDocument));
			}
			return value;
		}

		/// <summary>
		/// Saves the document.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="stream">The stream.</param>
		public void SaveDocument(string metadata, Stream stream)
		{
			var outboundDocumentService = ClientProxy;
			using (new OperationContextScope((IContextChannel)outboundDocumentService))
			{
				WebOperationContext.Current.OutgoingRequest.Headers.Add("metadata", metadata);
				ExecuteAction(() => outboundDocumentService.SaveDocument(stream));
			}
		}
	}
}