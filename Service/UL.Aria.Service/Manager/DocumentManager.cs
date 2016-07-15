using System;
using System.Collections.Generic;
using System.Globalization;

using UL.Aria.Common.Authorization;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Class DocumentManager. This class cannot be inherited.
	/// </summary>
	public sealed class DocumentManager : IDocumentManager
	{
		private readonly IAssetProvider _assetProvider;
		private readonly IPrincipalResolver _principalResolver;
		private readonly IAuthorizationManager _authorizationManager;
		private readonly IServiceConfiguration _serviceConfiguration;
		private readonly IDocumentProvider _documentProvider;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="DocumentManager" /> class.
		/// </summary>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="documentProvider">The document provider.</param>
		/// <param name="assetProvider">The asset provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="authorizationManager">The authorization manager.</param>
		/// <param name="serviceConfiguration">The service configuration.</param>
		public DocumentManager(ITransactionFactory transactionFactory, IDocumentProvider documentProvider,
			IAssetProvider assetProvider, IPrincipalResolver principalResolver, IAuthorizationManager authorizationManager, IServiceConfiguration serviceConfiguration)
		{
			_transactionFactory = transactionFactory;
			_documentProvider = documentProvider;
			_assetProvider = assetProvider;
			_principalResolver = principalResolver;
			_authorizationManager = authorizationManager;
			_serviceConfiguration = serviceConfiguration;
		}

		/// <summary>
		/// Creates the specified container identifier.
		/// </summary>
		/// <param name="containerId">The container identifier.</param>
		/// <param name="metaData">The meta data.</param>
		/// <returns>Guid.</returns>
		public Guid Create(Guid containerId, IDictionary<string, string> metaData)
		{
			var id = Guid.NewGuid();
			_assetProvider.Create(containerId, metaData, id);
			return id;
		}

		/// <summary>
		/// Updates the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="metaData">The meta data.</param>
		public void Update(Guid id, IDictionary<string, string> metaData)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var assetMetadata = _assetProvider.Fetch(id);
				DocumentContentProvider.CheckDocumentLocked(assetMetadata);
				_assetProvider.Update(id, metaData);

				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Fetches the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>IDictionary&lt;System.String, System.String&gt;.</returns>
		public IDictionary<string, string> FetchById(Guid id)
		{
			return _assetProvider.Fetch(id);
		}

		/// <summary>
		/// Deletes the document with the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete(Guid id)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var assetMetadata = _assetProvider.Fetch(id);
				DocumentContentProvider.CheckDocumentLocked(assetMetadata);
				_documentProvider.Delete(id);
				_assetProvider.DeleteContent(id);

				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Fetches the document by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Document.</returns>
		public Document FetchDocumentById(Guid id)
		{
			return _documentProvider.FetchById(id);
		}

		/// <summary>
		/// Locks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Lock(Guid id)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
                var utcNow = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
				var metaData = _assetProvider.Fetch(id);
			    var userName = _principalResolver.Current.Identity.Name;
			    if (metaData.ContainsKey(AssetFieldNames.AriaLockedBy))
				{
					if (!string.IsNullOrWhiteSpace(metaData[AssetFieldNames.AriaLockedBy]) &&
					    metaData[AssetFieldNames.AriaLockedBy] != userName)
					{
						throw new InvalidOperationException(string.Format("Document is locked by {0}",
							metaData[AssetFieldNames.AriaLockedBy]));
					}
					metaData[AssetFieldNames.AriaLockedBy] = userName;
				}
				else
				{
					metaData.Add(AssetFieldNames.AriaLockedBy, userName);
				}
				if (metaData.ContainsKey(AssetFieldNames.AriaLockedDateTime))
				{
					metaData[AssetFieldNames.AriaLockedDateTime] = utcNow;
				}
				else
				{
					metaData.Add(AssetFieldNames.AriaLockedDateTime, utcNow);
				}

                metaData[AssetFieldNames.AriaLastModifiedBy] = userName;
                metaData[AssetFieldNames.AriaLastModifiedOn] = utcNow;
                
                _assetProvider.Update(id, metaData);
				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Unlocks the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="verify">if set to <c>true</c> [verify].</param>
		/// <exception cref="System.InvalidOperationException"></exception>
		public void Unlock(Guid id, bool verify)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var metaData = _assetProvider.Fetch(id);

			    var userName = _principalResolver.Current.Identity.Name;
			    if (verify && metaData.ContainsKey(AssetFieldNames.AriaLockedBy))
				{
					var claimsPrincipal = _principalResolver.Current;
					var resourceClaim = new System.Security.Claims.Claim(SecuredResources.AriaAdministration, "");
					var actionClaim = new System.Security.Claims.Claim(SecuredActions.Update, id.ToString());
					var resourceClaimCompanyAdmin = new System.Security.Claims.Claim(SecuredResources.CompanyAdministration, _serviceConfiguration.UlCompanyId.ToString());
					if (!_authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim) &&
						!_authorizationManager.Authorize(claimsPrincipal, resourceClaimCompanyAdmin, actionClaim) &&
					    metaData[AssetFieldNames.AriaLockedBy] != userName)
					{
						throw new InvalidOperationException(string.Format("Document is locked by {0}",
							metaData[AssetFieldNames.AriaLockedBy]));
					}
				}

				if (metaData.ContainsKey(AssetFieldNames.AriaLockedBy))
				{
					metaData[AssetFieldNames.AriaLockedBy] = "";
				}
				if (metaData.ContainsKey(AssetFieldNames.AriaLockedDateTime))
				{
					metaData[AssetFieldNames.AriaLockedDateTime] = "";
				}

                metaData[AssetFieldNames.AriaLastModifiedBy] = userName;
                metaData[AssetFieldNames.AriaLastModifiedOn] = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

				_assetProvider.Update(id, metaData);
				transactionScope.Complete();
			}
		}
	}
}