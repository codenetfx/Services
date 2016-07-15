using System.Collections.Generic;
using System.ServiceModel;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
	/// <summary>
	/// Class DocumentsService.
	/// </summary>
	// Had to name DocumentsService plural due to issue with MS Rest not working with DocumentService
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
		InstanceContextMode = InstanceContextMode.PerCall)]
	public class DocumentsService : IDocumentService
	{
		private readonly IDocumentManager _documentManager;
		private readonly IMapperRegistry _mapperRegistry;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentsService" /> class.
		/// </summary>
		/// <param name="documentManager">The document manager.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public DocumentsService(IDocumentManager documentManager, IMapperRegistry mapperRegistry)
		{
			_documentManager = documentManager;
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// Creates the specified container identifier.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>System.String.</returns>
		public string Create(string containerId, IDictionary<string, string> metaData)
		{
			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var convertedContainerId = containerId.ToGuid();
			Guard.IsNotEmptyGuid(convertedContainerId, "containerId");
			Guard.IsNotNull(metaData, "metaData");

			return _documentManager.Create(convertedContainerId, metaData).ToString();
		}

		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="metaData">The meta data.</param>
		public void Update(string id, IDictionary<string, string> metaData)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");
			Guard.IsNotNull(metaData, "metaData");

			_documentManager.Update(convertedId, metaData);
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
		public IDictionary<string, string> FetchById(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");

			return _documentManager.FetchById(convertedId);
		}

		/// <summary>
		/// Deletes the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");

			_documentManager.Delete(convertedId);
		}

		/// <summary>
		/// Fetches the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>DocumentDto.</returns>
		public DocumentDto FetchDocumentById(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");

			var document = _documentManager.FetchDocumentById(convertedId);
			return _mapperRegistry.Map<DocumentDto>(document);
		}

		/// <summary>
		/// Pings this instance.
		/// </summary>
		/// <returns>System.String.</returns>
		public string Ping()
		{
			return "Verified";
		}

		/// <summary>
		/// Locks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Lock(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");
			
			_documentManager.Lock(convertedId);
		}

		/// <summary>
		/// Unlocks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Unlock(string id)
		{
			Guard.IsNotNullOrEmpty(id, "id");
			var convertedId = id.ToGuid();
			Guard.IsNotEmptyGuid(convertedId, "id");
			
			_documentManager.Unlock(convertedId, true);
		}
	}
}