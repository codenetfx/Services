using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Domain.SharePoint;
using UL.Aria.Service.Logging;
using UL.Aria.Service.Provider.SearchCoordinator;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Provider
{
	/// <summary>
	///     Search provider
	/// </summary>
	public class SearchProvider : ISearchProvider
	{
		private readonly IAriaMetaDataRepository _ariaMetaDataRepository;
		private readonly ITransactionFactory _transactionFactory;
		private readonly IAssetFieldMetadata _assetFieldMetadata;
		private readonly IContainerSerializer _containerSerializer;
		private readonly ILogManager _logManager;
		private readonly IPrincipalResolver _principalResoler;
		private readonly ISearchCoordinatorFactory _searchCoordinatorFactory;
		private readonly ISharePointQuery _sharepointQuery;

		/// <summary>
		/// Initializes a new instance of the <see cref="SearchProvider" /> class.
		/// </summary>
		/// <param name="sharepointQuery">The sharepoint query.</param>
		/// <param name="assetFieldMetadata">The asset field metadata.</param>
		/// <param name="containerSerializer">The container serializer.</param>
		/// <param name="logManager">The log manager.</param>
		/// <param name="principalResoler">The principal resoler.</param>
		/// <param name="searchCoordinatorFactory">The search coordinator factory.</param>
		/// <param name="ariaMetaDataRepository">The aria meta data repository.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		public SearchProvider(ISharePointQuery sharepointQuery, IAssetFieldMetadata assetFieldMetadata,
			IContainerSerializer containerSerializer,
			ILogManager logManager,
			IPrincipalResolver principalResoler,
			ISearchCoordinatorFactory searchCoordinatorFactory,
			IAriaMetaDataRepository ariaMetaDataRepository,
			ITransactionFactory transactionFactory)
		{
			_sharepointQuery = sharepointQuery;
			_assetFieldMetadata = assetFieldMetadata;
			_containerSerializer = containerSerializer;
			_logManager = logManager;
			_principalResoler = principalResoler;
			_searchCoordinatorFactory = searchCoordinatorFactory;
			_ariaMetaDataRepository = ariaMetaDataRepository;
			_transactionFactory = transactionFactory;
		}

		/// <summary>
		///     Searches the specified specification.
		/// </summary>
		/// <param name="searchCriteria">The specification.</param>
		/// <returns></returns>
		public SearchResultSet Search(SearchCriteria searchCriteria)
		{
			var currentPrincipal = _principalResoler.Current;
			var isCustomerUser = !currentPrincipal.HasClaim(SecuredClaims.UlEmployee, SecuredActions.Role);

			var selectProperties = _assetFieldMetadata.GetSelectProperties(searchCriteria.EntityType);
			var query = new StringBuilder();

			if (searchCriteria.EntityType != null)
			{
				query.AppendFormat("{0}:{1} ", AssetFieldNames.AriaAssetType, searchCriteria.EntityType);
			}

			if (!searchCriteria.IncludeDeletedRecords)
			{
				if (query.Length > 0)
					query.Append(" AND ");

				query.AppendFormat(" (NOT {0}:{1}) ", AssetFieldNames.IsDeleted, true);
			}

			if (isCustomerUser)
			{
				if (query.Length > 0)
					query.Append(" AND ");

				query.AppendFormat(" (NOT {0}:{1}) ", AssetFieldNames.AriaHideFromCustomer, true);
			}

			if (searchCriteria.FilterContainers)
			{
				if (query.Length > 0)
					query.Append(" AND ");

				query.AppendFormat(" ((NOT {0}:{1}) AND IsContainer:False) ", AssetFieldNames.AriaParentAssetId, Guid.Empty);
			}

			if (!string.IsNullOrWhiteSpace(searchCriteria.Keyword))
			{
				if (query.Length > 0)
					query.Append(" AND ");

				query.Append("(");
				query.Append(searchCriteria.Keyword);
				query.Append(")");
			}
			else if (query.Length == 0)
			{
				query.Append("*");
			}

			var sharePointQueryResult = new SharePointQueryResult();

			try
			{
				var coordinators = _searchCoordinatorFactory.GetCoordinators(searchCriteria.EntityType.GetValueOrDefault());
				coordinators.ForEach(x => x.ApplyCoordination(searchCriteria));

				sharePointQueryResult = _sharepointQuery.SubmitQuery(
					query.ToString(),
					selectProperties,
					searchCriteria.Refiners, searchCriteria.Filters,
					searchCriteria.StartIndex, searchCriteria.EndIndex - searchCriteria.StartIndex + 1, searchCriteria.Sorts, searchCriteria.AdditionalFilterString);

				for (var i = 0; i < searchCriteria.SearchCoordinators.Count; i++)
				{
					searchCriteria.SearchCoordinators[i].RetractCoordination(searchCriteria);
					i--;
				}
			}
			catch (Exception ex)
			{
				var logMessageEx = new LogMessage(MessageIds.SearchProviderSearchFailed, LogPriority.High, TraceEventType.Error,
					string.Format("Search failed, {0}", ex), LogCategory.Search);
				logMessageEx.Data.Add("CorrelationId", Trace.CorrelationManager.ActivityId.ToString());
				logMessageEx.Data.Add("JsonRequest", sharePointQueryResult.JsonRequest ?? "");
				logMessageEx.Data.Add("JsonResponse",
					sharePointQueryResult.JsonResult == null
						? ""
						: sharePointQueryResult.JsonResult.Substring(0,
							sharePointQueryResult.JsonResult.Length > 8196 ? 8196 : sharePointQueryResult.JsonResult.Length));
				logMessageEx.Data.Add("GetUri", sharePointQueryResult.GetUri ?? "");
				logMessageEx.LogCategories.Add(LogCategory.Search);
				_logManager.Log(logMessageEx);
				throw;
			}

			var spSearchQueryResult = sharePointQueryResult.SearchResults ?? new List<IDictionary<string, string>>();
			var totalResults = sharePointQueryResult.TotalRows;

			var searchResultList = spSearchQueryResult.Select(metadata => new SearchResult
			{
				EntityType = metadata.GetValue(AssetFieldNames.AriaAssetType, (EntityTypeEnumDto?) null),
				Metadata = metadata
			}).ToList();

			// Log search results
			var logMessage = new LogMessage(MessageIds.SearchProviderSearchComplete, LogPriority.Low, TraceEventType.Information,
				string.Format(
					"Search results, returned row count: {0}, total row count: {1} (Check extended properties for details)",
					searchResultList.Count, totalResults), LogCategory.Search);
			logMessage.Data.Add("CorrelationId", Trace.CorrelationManager.ActivityId.ToString());
			logMessage.Data.Add("JsonRequest", sharePointQueryResult.JsonRequest ?? "");
			logMessage.Data.Add("JsonResponse",
				sharePointQueryResult.JsonResult == null
					? ""
					: sharePointQueryResult.JsonResult.Substring(0,
						sharePointQueryResult.JsonResult.Length > 8196 ? 8196 : sharePointQueryResult.JsonResult.Length));
			logMessage.Data.Add("GetUri", sharePointQueryResult.GetUri ?? "");
			logMessage.LogCategories.Add(LogCategory.Search);
			_logManager.Log(logMessage);

			return new SearchResultSet
			{
				Summary = new SearchSummary
				{
					TotalResults = totalResults,
					StartIndex = searchCriteria.StartIndex,
					EndIndex = searchCriteria.StartIndex + searchResultList.Count - 1,
					LastCommand = sharePointQueryResult.GetUri
				},
				SearchCriteria = searchCriteria,
				Results = searchResultList,
				RefinerResults = sharePointQueryResult.RefinerResults
			};
		}

		/// <summary>
		///     Creates the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="containerMetadata">The container metadata.</param>
		/// <returns>The created entity id</returns>
		public Guid Create(Container container, string containerMetadata)
		{
			var containerSerialized = _containerSerializer.Serialize(container);
			var sharePointContainer = SharePointContainer.Parse(containerSerialized);
			var metadataDictionary = AssetProvider.ParseAllMetadataToDictionary(containerMetadata);

			if (sharePointContainer.Id == Guid.Empty)
			{
				sharePointContainer.Id = Guid.NewGuid();
			}

			sharePointContainer.MetaData = metadataDictionary;

			var claims = AssetProvider.CompoundClaimStringForSearch(AssetProvider.ClaimSchema, AssetProvider.ClaimProvider,
				metadataDictionary);
			var ariaMetaData = new AriaMetaData
			{
				Id = sharePointContainer.Id,
				ParentAssetId = Guid.Empty,
				Claims = claims,
				AssetName = string.Empty,
				MetaData = containerMetadata,
				SecurityDescriptor =
					null,
				Uri = string.Empty,
				Version = string.Empty,
				LastModifiedTime = DateTime.UtcNow,
				IsParsed = false,
				IsDeleted = false,
				AvailableClaims = sharePointContainer.GetListsWithClaims()
			};

			_ariaMetaDataRepository.Create(ariaMetaData);
			return sharePointContainer.Id;
		}

		/// <summary>
		///     Updates the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="containerMetadata">The container metadata.</param>
		public void Update(Container container, string containerMetadata)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var ariaMetaData = _ariaMetaDataRepository.FetchById(container.Id.GetValueOrDefault());
				ariaMetaData.MetaData = containerMetadata;
				AssetProvider.UpdateAsset(_ariaMetaDataRepository, ariaMetaData);

				transactionScope.Complete();
			}
		}

		/// <summary>
		/// Gets the products for the specified project.
		/// </summary>
		/// <param name="projectId">The project unique identifier.</param>
		/// <returns></returns>
		public IList<Guid> FetchProductsByProjectId(Guid projectId)
		{
			var criteria = new SearchCriteria {EntityType = EntityTypeEnumDto.Product};
			criteria.Filters.Add(AssetFieldNames.AriaProjectId, new List<string> {projectId.ToString()});
			criteria.StartIndex = 0;
			criteria.EndIndex = 250;

			var results = Search(criteria);

			if (null != results && null != results.Results)
				return results.Results
					.Select(x => x.Metadata.GetValue(AssetFieldNames.AriaProductId, default(Guid)))
					.Where(x => x != default(Guid))
					.ToList();

			return new Guid[0];
		}
	}
}