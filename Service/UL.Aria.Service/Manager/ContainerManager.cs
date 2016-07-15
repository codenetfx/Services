using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using UL.Aria.Common.Authorization;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Parser;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	///     Class ContainerManager
	/// </summary>
	public sealed class ContainerManager : IContainerManager
	{
		private readonly IAssetFieldMetadata _assetFieldMetadata;
		private readonly IContainerDefinitionBuilder _containerDefinitionBuilder;
		private readonly IAriaMetaDataRepository _ariaMetaDataRepository;
		private readonly IContainerRepository _containerRepository;
		private readonly IXmlParser _incomingOrderParser;
		private readonly IOrderRepository _orderRepository;
		private readonly IProductRepository _productRepository;
		private readonly ISearchProvider _searchProvider;
		private readonly ITransactionFactory _transactionFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="ContainerManager" /> class.
		/// </summary>
		/// <param name="searchProvider">The search provider.</param>
		/// <param name="containerRepository">The container repository.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="assetFieldMetadata">The asset field metadata.</param>
		/// <param name="productRepository">The product provider.</param>
		/// <param name="orderRepository">The order repository.</param>
		/// <param name="incomingOrderParser">The incoming order parser.</param>
		/// <param name="containerDefinitionBuilder">The container definition builder.</param>
		/// <param name="ariaMetaDataRepository">The aria meta data repository.</param>
		public ContainerManager(ISearchProvider searchProvider, IContainerRepository containerRepository,
			ITransactionFactory transactionFactory,
			IAssetFieldMetadata assetFieldMetadata,
			IProductRepository productRepository, IOrderRepository orderRepository,
			IXmlParser incomingOrderParser,
			IContainerDefinitionBuilder containerDefinitionBuilder, IAriaMetaDataRepository ariaMetaDataRepository)
		{
			_searchProvider = searchProvider;
			_containerRepository = containerRepository;
			_transactionFactory = transactionFactory;
			_assetFieldMetadata = assetFieldMetadata;
			_productRepository = productRepository;
			_orderRepository = orderRepository;
			_containerDefinitionBuilder = containerDefinitionBuilder;
			_ariaMetaDataRepository = ariaMetaDataRepository;
			_incomingOrderParser = incomingOrderParser;
		}

		/// <summary>
		///     Deletes the specified container by id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Delete(Guid containerId)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_containerRepository.Remove(containerId);
				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Creates entity metadata.
		/// </summary>
		/// <param name="primarySearchEntityBase">The container.</param>
		/// <returns>The created content id.</returns>
		public Guid Create(PrimarySearchEntityBase primarySearchEntityBase)
		{
			return Create(primarySearchEntityBase, Guid.NewGuid());
		}

		/// <summary>
		///     Creates entity metadata.
		/// </summary>
		/// <param name="primarySearchEntityBase">The container.</param>
		/// <param name="containerId"></param>
		/// <returns>The created content id.</returns>
		public Guid Create(PrimarySearchEntityBase primarySearchEntityBase, Guid? containerId)
		{
			Guid id;

			using (var transactionScope = _transactionFactory.Create())
			{
				var container = _containerDefinitionBuilder.Create(primarySearchEntityBase,
					containerId ?? Guid.NewGuid());
				primarySearchEntityBase.ContainerId = container.Id;
				_containerRepository.Create(container);
				var containerMetadata = primarySearchEntityBase.GetContainerMetadata(_assetFieldMetadata);
				id = _searchProvider.Create(container, containerMetadata);
				transactionScope.Complete();
			}

			return id;
		}

		/// <summary>
		///     Fetches the specified container by id.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns>Container.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public Container FindById(Guid containerId)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				var container = _containerRepository.GetById(containerId);
				transactionScope.Complete();
				return container;
			}
		}

		/// <summary>
		///     Updates the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Update(Container container)
		{
			using (var transactionScope = _transactionFactory.Create())
			{
				_containerRepository.Update(container);
				transactionScope.Complete();
			}
		}

		/// <summary>
		///     Gets all assignable user claims.
		/// </summary>
		/// <param name="containerId">The container id.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public IList<System.Security.Claims.Claim> GetAvailableUserClaims(Guid containerId)
		{
			//todo  : change to strategy pattern
			var container = _containerRepository.GetById(containerId);
			var list = new List<System.Security.Claims.Claim>
			{
				new System.Security.Claims.Claim(SecuredClaims.UlSystemAuditor, "true"),
				new System.Security.Claims.Claim(SecuredClaims.UlSystemOperations, "true"),
				new System.Security.Claims.Claim(SecuredClaims.UlAdministrator, "true"),
				new System.Security.Claims.Claim(SecuredClaims.ContainerPrivate, containerId.ToString()),
				new System.Security.Claims.Claim(SecuredClaims.ContainerView, containerId.ToString()),
				new System.Security.Claims.Claim(SecuredClaims.CompanyAdmin, container.CompanyId.ToString())
			};

			switch (container.PrimarySearchEntityType)
			{
				case "Order":
					list.Add(new System.Security.Claims.Claim(SecuredClaims.UlOrderAdministrator, "true"));
					list.Add(new System.Security.Claims.Claim(SecuredClaims.CompanyOrderAccess,
						container.CompanyId.ToString()));
					break;
				case "Project":
					list.Add(new System.Security.Claims.Claim(SecuredClaims.UlProjectAdministrator, "true"));
					list.Add(new System.Security.Claims.Claim(SecuredClaims.CompanyProjectAccess,
						container.CompanyId.ToString()));
					list.Add(new System.Security.Claims.Claim(SecuredClaims.ContainerEdit, containerId.ToString()));
					break;
				case "Product":
					list.Add(new System.Security.Claims.Claim(SecuredClaims.UlProductAdministrator, "true"));
					list.Add(new System.Security.Claims.Claim(SecuredClaims.ContainerEdit, containerId.ToString()));
					list.Add(new System.Security.Claims.Claim(SecuredClaims.CompanyProductAdmin,
						container.CompanyId.ToString()));
					break;
			}

			return list;
		}

		/// <summary>
		///     Get all containers by company id
		/// </summary>
		/// <param name="searchCriteria">The search criteria.</param>
		/// <returns></returns>
		public SearchResultSet GetByCompanyId(SearchCriteria searchCriteria)
		{
			var resultset = new SearchResultSet {RefinerResults = new Dictionary<string, List<IRefinementItem>>()};

			//
			// get everything
			//
// ReSharper disable once PossibleInvalidOperationException
			var containers = _containerRepository.GetByCompanyId(searchCriteria.CompanyId.Value);

			//
			// apply filters, if exist (only supported is asset type)
			//
			List<string> assetTypeFilter;
			if (searchCriteria.Filters.TryGetValue(AssetFieldNames.AriaAssetType, out assetTypeFilter) &&
			    assetTypeFilter.Count > 0)
			{
				var entityType = assetTypeFilter[0];
				containers = containers.Where(x => x.PrimarySearchEntityType == entityType);
			}

			//
			// convert to models so we can filter on keywords
			// NOTE: performance implications.  This will do containers.Length number of fetches and 
			// should be merged with the following two linq queries so that only the number of results
			// that match keywords && limited to page size are fetched.  
			//
			// ENSURE: This change does not interfere with the refiner counts...it shouldn't because 
			// asset type is already available before fetch
			//
			resultset.Results = containers.Select(GetSearchResult).Where(x => x != null).ToList();

			//
			// filter any available metadata by keywords
			//
			if (!string.IsNullOrEmpty(searchCriteria.Keyword))
			{
				var keywords = searchCriteria.Keyword.ToUpperInvariant();
				resultset.Results =
					resultset.Results.Where(x => x.Metadata.Any(m => m.Value.ToUpper().Contains(keywords))).ToList();
			}

			//
			// calculate totals & refiners
			//
			var totalResults = resultset.Results.Count;
			var refiners = resultset.Results
				.GroupBy(x => x.EntityType + "")
				.Select(x => new RefinementItem {Count = x.Count(), Name = x.Key, Value = x.Key}).Cast<IRefinementItem>();
			resultset.RefinerResults.Add(AssetFieldNames.AriaAssetType, refiners.ToList());

			//
			// apply paging
			//
			resultset.Results = resultset.Results
				.Skip((int) searchCriteria.StartIndex)
				.Take((int) searchCriteria.EndIndex - (int) searchCriteria.StartIndex + 1)
				.ToList();

			//
			// build remaining object
			//
			resultset.SearchCriteria = searchCriteria;
			resultset.Summary = new SearchSummary
			{
				StartIndex = searchCriteria.StartIndex,
				EndIndex = searchCriteria.StartIndex + resultset.Results.Count() - 1,
				TotalResults = totalResults
			};
			return resultset;
		}

		/// <summary>
		///     Updates the specified primary search entity base.
		/// </summary>
		/// <param name="primarySearchEntityBase">The primary search entity base.</param>
		/// <param name="containerId">The container id.</param>
		/// <returns>Guid.</returns>
		public void Update(PrimarySearchEntityBase primarySearchEntityBase, Guid? containerId = null)
		{
			var container = _containerDefinitionBuilder.Create(primarySearchEntityBase, containerId ?? Guid.NewGuid());
			primarySearchEntityBase.ContainerId = container.Id;
			var containerMetadata = primarySearchEntityBase.GetContainerMetadata(_assetFieldMetadata);
			using (var transactionScope = _transactionFactory.Create())
			{
				_containerRepository.Update(container);
				_searchProvider.Update(container, containerMetadata);
				transactionScope.Complete();
			}
		}

		private SearchResult GetSearchResult(Container container)
		{
			var searchResult = new SearchResult
			{
				EntityType = container.PrimarySearchEntityType.ParseOrDefault(EntityTypeEnumDto.Container),
				Metadata = new Dictionary<string, string>
				{
// ReSharper disable once PossibleInvalidOperationException
					{AssetFieldNames.AriaContainerId, container.Id.Value.ToString()},
					{AssetFieldNames.SharePointAssetId, container.PrimarySearchEntityId.ToString()}
				}
			};

			try
			{
				switch (searchResult.EntityType)
				{
					case EntityTypeEnumDto.Product:
						var product = _productRepository.GetProductForStatusOnly(container.PrimarySearchEntityId);
						if (product != null)
						{
							searchResult.Metadata.Add(AssetFieldNames.AriaName, product.Name);
							searchResult.Metadata.Add(AssetFieldNames.AriaLastModifiedOn,
								product.UpdatedDateTime.ToString(CultureInfo.InvariantCulture));
						}
						break;
					case EntityTypeEnumDto.Order:
						var order = _orderRepository.FindById(container.PrimarySearchEntityId);
						var orderDto =
							_incomingOrderParser.Parse(order.OriginalXmlParsed) as IncomingOrderDto;

// ReSharper disable once PossibleNullReferenceException
						searchResult.Metadata.Add(AssetFieldNames.AriaName, orderDto.OrderNumber);
						searchResult.Metadata.Add(AssetFieldNames.AriaLastModifiedOn,
							orderDto.UpdatedDateTime.ToString(CultureInfo.InvariantCulture));
						break;
					case EntityTypeEnumDto.Project:
						var ariaMetaDataProject = _ariaMetaDataRepository.FetchById(container.Id.GetValueOrDefault());
						var project = ariaMetaDataProject.MetaData.ToIDictionaryFromAriaSharepointXml();
						foreach (var kvp in project)
							searchResult.Metadata.Add(kvp);

						searchResult.Metadata.Add(AssetFieldNames.AriaName,
							project.GetValue(AssetFieldNames.AriaProjectName, (string) null));
						break;
					default:
						var ariaMetaDataResult = _ariaMetaDataRepository.FetchById(container.Id.GetValueOrDefault());
						var result = ariaMetaDataResult.MetaData.ToIDictionaryFromAriaSharepointXml();
						foreach (var kvp in result)
							searchResult.Metadata.Add(kvp);
						break;
				}

				return searchResult;
			}
			catch (DatabaseItemNotFoundException)
			{
				return null;
			}
		}
	}
}