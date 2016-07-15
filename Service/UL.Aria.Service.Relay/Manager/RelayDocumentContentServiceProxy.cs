using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Relay.Manager
{
	/// <summary>
	/// Class RelayDocumentContentServiceProxy.
	/// </summary>
	public class RelayDocumentContentServiceProxy : ServiceAgentManagedProxy<IDocumentContentService>,
		IRelayDocumentContentServiceProxy
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RelayDocumentContentServiceProxy"/> class.
		/// </summary>
		/// <param name="configurationSource">The configuration source.</param>
		public RelayDocumentContentServiceProxy(IProxyConfigurationSource configurationSource) :
			this(
			new WebChannelFactory<IDocumentContentService>(new WebHttpBinding(), configurationSource.DocumentContentService))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayDocumentContentServiceProxy"/> class.
		/// </summary>
		/// <param name="serviceProxyFactory">The service proxy factory.</param>
		private RelayDocumentContentServiceProxy(WebChannelFactory<IDocumentContentService> serviceProxyFactory)
			: base(serviceProxyFactory)
		{
		}

		/// <summary>
		/// Saves the specified stream.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="stream">The stream.</param>
		public void Save(string metadata, Stream stream)
		{
			var documentContentService = ClientProxy;
			using (new OperationContextScope((IContextChannel) documentContentService))
			{
				WebOperationContext.Current.OutgoingRequest.Headers.Add("metadata", metadata);
				documentContentService.Save(stream);
			}
		}
	}
}