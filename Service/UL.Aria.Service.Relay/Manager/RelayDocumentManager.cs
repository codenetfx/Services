using System;
using System.IO;

using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Relay.Manager
{
	/// <summary>
	/// Class RelayDocumentManager. This class cannot be inherited.
	/// </summary>
	public sealed class RelayDocumentManager : IRelayDocumentManager
	{
		private readonly IRelayDocumentContentServiceProxy _documentContentService;
		private readonly IDocumentService _documentService;
		private readonly IMapperRegistry _mapperRegistry;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayDocumentManager" /> class.
		/// </summary>
		/// <param name="documentService">The document service.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="documentContentService">The document content service.</param>
		public RelayDocumentManager(IDocumentService documentService, IMapperRegistry mapperRegistry,
			IRelayDocumentContentServiceProxy documentContentService)
		{
			_documentService = documentService;
			_mapperRegistry = mapperRegistry;
			_documentContentService = documentContentService;
		}

		/// <summary>
		/// Saves the specified identifier.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="id">The identifier.</param>
		/// <param name="contentType">Type of the content.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>Document.</returns>
		public Document Save(string metadata, Guid id, string contentType, Stream stream)
		{
			_documentContentService.Save(metadata, stream);
			var documentDto = _documentService.FetchDocumentById(id.ToString());
			return _mapperRegistry.Map<Document>(documentDto);
		}

		/// <summary>
		/// Gets the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Document.</returns>
		public Document GetDocumentById(Guid id)
		{
			var documentDto = _documentService.FetchDocumentById(id.ToString());
			return _mapperRegistry.Map<Document>(documentDto);
		}

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		public string Ping()
		{
			return _documentService.Ping();
		}
	}
}