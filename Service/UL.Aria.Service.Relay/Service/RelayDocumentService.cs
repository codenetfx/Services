using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using UL.Aria.Service.Contracts;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Relay.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Relay.Service
{
	/// <summary>
	/// Class RelayDocumentService.
	/// </summary>
	[ServiceBehavior(
		ConcurrencyMode = ConcurrencyMode.Multiple,
		IncludeExceptionDetailInFaults = true,
		InstanceContextMode = InstanceContextMode.PerCall,
		Namespace = @"http://aria.ul.com/Relay/DocumentDetail"
		)]
	public class RelayDocumentService : IRelayDocumentService
	{
		private readonly IMapperRegistry _mapperRegistry;
		private readonly IRelayDocumentManager _relayDocumentManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayDocumentService"/> class.
		/// </summary>
		/// <param name="relayDocumentManager">The relay document manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public RelayDocumentService(IRelayDocumentManager relayDocumentManager, IMapperRegistry mapperRegistry)
		{
			_relayDocumentManager = relayDocumentManager;
			_mapperRegistry = mapperRegistry;
		}

		internal string _testMetaData = null;

		/// <summary>
		/// Saves the specified identifier.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>DocumentDto.</returns>
		public DocumentDto Save(Stream stream)
		{
			System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
			var metadata = WebOperationContext.Current == null ? _testMetaData : WebOperationContext.Current.IncomingRequest.Headers["metadata"];
			Guard.IsNotNullOrEmpty(metadata, "metadata");
			Guard.IsNotNull(stream, "stream");

			var convertedMetadata = Encoding.UTF8.GetString(Convert.FromBase64String(metadata));
			var inboundDocument = OutboundDocumentMetadata.ParseDocumentMetadata<InboundDocumentDto>(convertedMetadata);

			Guard.IsNotNullOrEmpty(inboundDocument.MessageId, "MessageId");
			Guard.IsNotEmptyGuid(inboundDocument.DocumentId, "DocumentId");

			var document = _relayDocumentManager.Save(metadata, inboundDocument.DocumentId, inboundDocument.ContentType, stream);

			return _mapperRegistry.Map<DocumentDto>(document);
		}

		/// <summary>
		/// Gets the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentDto.</returns>
		public DocumentDto GetDocumentById(string id)
		{
			System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = Guid.Parse(id);
			Guard.IsNotEmptyGuid(convertedId, "id");

			var document = _relayDocumentManager.GetDocumentById(convertedId);

			return _mapperRegistry.Map<DocumentDto>(document);
		}

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		public string Ping()
		{
			System.Diagnostics.Trace.CorrelationManager.ActivityId = Guid.NewGuid();

			return _relayDocumentManager.Ping();
		}
	}
}