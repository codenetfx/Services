using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Class DocumentManagementManager.
	/// </summary>
	public class DocumentManagementManager : IDocumentManagementManager
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IDocumentContentManager _documentContentManager;
		private readonly IPrincipalResolver _principalResolver;
		private readonly IDocumentManager _documentManager;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentManagementManager" /> class.
		/// </summary>
		/// <param name="documentContentManager">The document content manager.</param>
		/// <param name="assetProvider">The asset provider.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="documentManager">The document manager.</param>
		public DocumentManagementManager(IDocumentContentManager documentContentManager, IAssetProvider assetProvider,
			ITransactionFactory transactionFactory,
			IPrincipalResolver principalResolver,
			IDocumentManager documentManager)
		{
			_documentContentManager = documentContentManager;
			_assetProvider = assetProvider;
			_transactionFactory = transactionFactory;
			_principalResolver = principalResolver;
			_documentManager = documentManager;
		}

		/// <summary>
		/// Creates the existing document in the container with the primary search entity and links it to the task.
		/// </summary>
		/// <param name="documentId">The document identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>Document Guid.</returns>
		public Guid CreateAndLink(Guid documentId, Guid containerId, Guid taskId, IDictionary<string, string> metaData)
		{
			var newDocumentId = Guid.NewGuid();

			using (var transactionScope = _transactionFactory.Create())
			{
				_assetProvider.Create(containerId, metaData, newDocumentId);

				using (var stream = _documentContentManager.FetchById(documentId))
				{
					_documentContentManager.Create(newDocumentId, metaData[AssetFieldNames.AriaContentType], stream, true);
				}

				var assetLink = new AssetLink
				{
					ParentAssetId = taskId,
					AssetId = newDocumentId,
					ParentAssetType = EntityTypeEnumDto.Task.ToString(),
					AssetType = EntityTypeEnumDto.Document.ToString(),
					AssetName = metaData[AssetFieldNames.AriaName],
					CreatedById = _principalResolver.UserId,
					CreatedDateTime = DateTime.UtcNow,
					UpdatedById = _principalResolver.UserId,
					UpdatedDateTime = DateTime.UtcNow
				};

				_assetProvider.CreateAssetLink(assetLink);
				_documentManager.Lock(newDocumentId);

				transactionScope.Complete();
			}

			return newDocumentId;
		}
	}
}