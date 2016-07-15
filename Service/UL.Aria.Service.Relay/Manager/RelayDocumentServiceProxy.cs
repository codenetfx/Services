using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Enterprise.Foundation.Client;

namespace UL.Aria.Service.Relay.Manager
{
	/// <summary>
	/// Class RelayDocumentServiceProxy.
	/// </summary>
	public class RelayDocumentServiceProxy : ServiceAgentManagedProxy<IDocumentService>, IDocumentService
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RelayDocumentServiceProxy"/> class.
		/// </summary>
		/// <param name="configurationSource">The configuration source.</param>
		public RelayDocumentServiceProxy(IProxyConfigurationSource configurationSource) :
			this(
			new WebChannelFactory<IDocumentService>(new WebHttpBinding(), configurationSource.DocumentService))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayDocumentServiceProxy"/> class.
		/// </summary>
		/// <param name="serviceProxyFactory">The service proxy factory.</param>
		private RelayDocumentServiceProxy(WebChannelFactory<IDocumentService> serviceProxyFactory)
			: base(serviceProxyFactory)
		{
		}

		/// <summary>
		/// Creates the specified container identifier.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.NotSupportedException"></exception>
		public string Create(string containerId, IDictionary<string, string> metaData)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		public void Update(string id, IDictionary<string, string> metaData)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
		/// <exception cref="System.NotSupportedException"></exception>
		public IDictionary<string, string> FetchById(string id)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Deletes the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		public void Delete(string id)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Fetches the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentDto.</returns>
		/// <exception cref="System.NotSupportedException"></exception>
		public DocumentDto FetchDocumentById(string id)
		{
			DocumentDto document;
			var documentService = ClientProxy;
			// ReSharper disable once SuspiciousTypeConversion.Global
			using (new OperationContextScope((IContextChannel) documentService))
			{
				document = documentService.FetchDocumentById(id);
				return document;
			}
		}

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		public string Ping()
		{
			var documentService = ClientProxy;
			// ReSharper disable once SuspiciousTypeConversion.Global
			using (new OperationContextScope((IContextChannel)documentService))
			{
				return documentService.Ping();
			}
		}

		/// <summary>
		/// Locks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		public void Lock(string id)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Unlocks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <exception cref="System.NotSupportedException"></exception>
		public void Unlock(string id)
		{
			throw new NotSupportedException();
		}
	}
}