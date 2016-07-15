using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UL.Aria.Common.Authorization;
using UL.Aria.Common.BusinessMessage;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Product Manager
    /// </summary>
    public class ProductManager : IProductManager
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IBusinessMessageProvider _businessMessageProvider;
        private readonly IContainerProvider _containerProvider;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProductDocumentManager _productDocumentManager;
        private readonly IProductProvider _productProvider;
        private readonly ISearchProvider _searchProvider;
        private readonly ISmtpClientManager _smtpClientManager;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductManager" /> class.
        /// </summary>
        /// <param name="productProvider">The product provider.</param>
        /// <param name="productDocumentManager">The product document manager.</param>
        /// <param name="smtpClientManager">The SMTP client manager.</param>
        /// <param name="businessMessageProvider">The business message provider.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="searchProvider">The search provider.</param>
        /// <param name="assetProvider">The asset provider.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="containerProvider">The container provider.</param>
        public ProductManager(IProductProvider productProvider, IProductDocumentManager productDocumentManager,
            ISmtpClientManager smtpClientManager, IBusinessMessageProvider businessMessageProvider,
            IPrincipalResolver principalResolver, ISearchProvider searchProvider, IAssetProvider assetProvider,
            ITransactionFactory transactionFactory, IContainerProvider containerProvider)
        {
            _productProvider = productProvider;
            _productDocumentManager = productDocumentManager;
            _principalResolver = principalResolver;
            _smtpClientManager = smtpClientManager;
            _businessMessageProvider = businessMessageProvider;
            _searchProvider = searchProvider;
            _assetProvider = assetProvider;
            _transactionFactory = transactionFactory;
            _containerProvider = containerProvider;
        }

        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public Guid Create(Product product)
        {
            return _productProvider.Create(product);
        }

        /// <summary>
        ///     Updates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        public void Update(Product product)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                _productProvider.Update(product);
                if (product.Status != ProductStatus.Draft)
                {
// ReSharper disable once PossibleInvalidOperationException
                    var metaData = _assetProvider.Fetch(product.Id.Value);
                    var containerId = metaData[AssetFieldNames.AriaContainerId].ToGuid();
                    _containerProvider.DeleteList(containerId, "Modify");
                }
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Gets the specified product by id.
        /// </summary>
        /// <param name="productId">The product by id.</param>
        /// <returns></returns>
        public Product Get(Guid productId)
        {
            Product product = _productProvider.Get(productId);

            //
            // get the user and compare
            //
            bool isUlEmployee = _principalResolver.Current.Claims.Any(i => i.Type == SecuredClaims.UlEmployee);
            if (isUlEmployee)
            {
                product.CanDelete = true;
            }
            else
            {
                //
                // get the characteristic for Status and see if it equals 'Draft'
                //
                product.CanDelete = product.Status == ProductStatus.Draft;
            }

            for (var i = product.Characteristics.Count - 1; i >= 0; i--)
            {
                var characteristic = product.Characteristics[i];
                if (characteristic.ParentId.HasValue)
                    product.Characteristics.Remove(characteristic);
            }

            return product;
        }

        /// <summary>
        ///     Gets the download.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <returns></returns>
        public Stream GetDownload(Guid productId)
        {
            Product product = Get(productId);
            return _productDocumentManager.Get(product);
        }

        /// <summary>
        ///     Gets the by product family id.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <returns></returns>
        public IEnumerable<Product> GetByProductFamilyId(Guid productFamilyId)
        {
            return _productProvider.GetByProductFamilyId(productFamilyId);
        }

        /// <summary>
        ///     Gets the by family download.
        /// </summary>
        /// <param name="productFamilyId"></param>
        /// <returns></returns>
        public Stream GetProductDownloadByProductFamilyId(Guid productFamilyId)
        {
            IEnumerable<Product> products = GetByProductFamilyId(productFamilyId);

            return _productDocumentManager.Get(products, productFamilyId);
        }

        /// <summary>
        ///     Gets the product download by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        public Stream GetProductDownloadByProductUploadId(Guid productUploadId)
        {
            ProductUploadResultSearchResultSet searchResult = _productProvider.GetByProductUploadId(productUploadId);

            // b/c this is a search result, the repo will not regard an empty result as a Not Found.
            if (searchResult == null || searchResult.Results.Count == 0)
                throw new DatabaseItemNotFoundException();

			// select just the non-null products
            IList<Product> products = searchResult.Results.Where(x => x.ProductUploadResult.Product != null).Select(x => x.ProductUploadResult.Product).ToList();
			
			if (products.Count == 0)
				throw new DatabaseItemNotFoundException();

            return _productDocumentManager.Get(products, products.First().ProductFamilyId);
        }


        /// <summary>
        ///     Uploads the specified file.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        /// <returns>Confirmation Id.</returns>
        public Guid Upload(ProductUpload productUpload)
        {
            return _productProvider.Upload(productUpload);
        }

        /// <summary>
        ///     Fetches the by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex">Start index of the row.</param>
        /// <param name="rowEndIndex">End index of the row.</param>
        /// <returns>ProductUploadSearchResultSet.</returns>
        public ProductUploadSearchResultSet FetchByUserId(Guid userId, long rowStartIndex, long rowEndIndex)
        {
            return _productProvider.FetchByUserId(userId, rowStartIndex, rowEndIndex);
        }

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSet.</returns>
        public ProductUploadResultSearchResultSet GetByProductUploadId(Guid productUploadId)
        {
            return _productProvider.GetByProductUploadId(productUploadId);
        }

        /// <summary>
        /// Gets the product upload header.
        /// </summary>
        /// <param name="productUploadId">The product upload unique identifier.</param>
        /// <returns></returns>
        public ProductUpload GetProductUploadHeader(Guid productUploadId)
        {
            return _productProvider.GetProductUploadHeader(productUploadId);
        }

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUpload.</returns>
        public ProductUpload GetById(Guid productUploadId)
        {
            return _productProvider.GetById(productUploadId);
        }

        /// <summary>
        ///     Deletes the specified product id.
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Delete(Guid productId)
        {
            using (var transaction = _transactionFactory.Create())
            {
                var product = _productProvider.Get(productId);
                var messageBuilder = new StringBuilder()
                    .AppendFormat("Product {0} deleted.", product.Id)
                    .AppendLine()
                    .AppendFormat("Product Name: '{0}'", product.Name)
                    .AppendLine()
                    .AppendFormat("Product Description: '{0}' ", product.Description)
                    .AppendLine();
                _businessMessageProvider.Publish(AuditMessageIdEnumDto.ProductDeleted, messageBuilder.ToString());
                _productProvider.Delete(productId);
// ReSharper disable once PossibleInvalidOperationException
                _containerProvider.Delete(product.ContainerId.Value);
                product.IsDeleted = true;
                _assetProvider.Update(productId, product);
                transaction.Complete();
            }
        }

        /// <summary>
        ///     Updates the status.
        /// </summary>
        /// <param name="product">The product.</param>
        public void UpdateStatus(Product product)
        {
            string message;

            using (var transactionScope = _transactionFactory.Create())
            {
                StringBuilder messageBuilder = new StringBuilder()
                    .AppendFormat("The Status of product {0} has changed to {1}.", product.Id, product.Status)
                    .AppendLine()
                    .AppendFormat("Product Name: '{0}'", product.Name)
                    .AppendLine()
                    .AppendFormat("Product Description: '{0}' ", product.Description)
                    .AppendLine();

                message = messageBuilder.ToString();

                _businessMessageProvider.Publish(AuditMessageIdEnumDto.UpdateProductStatus, message);
// ReSharper disable once PossibleInvalidOperationException
                var prod = _productProvider.Get(product.Id.Value);

                if (Equals(product.Status, prod.Status))
                    throw new Exception("Product has already been in" + product.Status.ToString());


                var metadata = _assetProvider.Fetch(product.Id.Value);

                if (metadata != null && metadata.Count > 0)
                {
                    metadata[AssetFieldNames.AriaProductStatus] = product.Status.ToString();

                    if (product.Status == ProductStatus.Submitted)
                    {
                        product.SubmittedDateTime = DateTime.UtcNow;
                        if (!metadata.ContainsKey(AssetFieldNames.AriaProductSubmittedDate))
                        {
                            metadata.Add(AssetFieldNames.AriaProductSubmittedDate, product.SubmittedDateTime.ToString());
                        }
                        else
                        {
                            metadata[AssetFieldNames.AriaProductSubmittedDate] = product.SubmittedDateTime.ToString();
                        }    
                    }
                   
                    _assetProvider.Update(product.Id.Value, metadata);

                    var containerId = metadata[AssetFieldNames.AriaContainerId].ToGuid();
                    _containerProvider.DeleteList(containerId, "Modify");
                    
                    // name , description, id
                    _productProvider.UpdateStatus(product.Id.Value, product.Status, product.SubmittedDateTime);
                    UpdateDocumentsToReadOnly(containerId);
                    transactionScope.Complete();
                }
            }

            _smtpClientManager.SendProductStatusChanged(product);
        }


        private void UpdateDocumentsToReadOnly(Guid containerId)
        {
            var searchCriteria = new SearchCriteria
            {
                EntityType = EntityTypeEnumDto.Document,
                Keyword =
                    String.Format("{1}:{0}  ", containerId,
                        String.Concat('"', AssetFieldNames.AriaContainerId, '"')),
                StartIndex = 0,
                EndIndex = 100
            };

            var searchResultSet = _searchProvider.Search(searchCriteria);

            IQueryable<SearchResult> searchResults =
                searchResultSet.Results.Where(i => i.Metadata.ContainsKey(AssetFieldNames.AriaPermission))
                    .AsQueryable();

            IQueryable<SearchResult> filteredResult =
                searchResults.Where(
                    i =>
                        String.Compare(i.Metadata[AssetFieldNames.AriaPermission], "Modify",
                            StringComparison.CurrentCultureIgnoreCase) == 0);

            foreach (SearchResult source in filteredResult)
            {
                source.Metadata[AssetFieldNames.AriaPermission] = "ReadOnly";
                string id = source.Metadata[AssetFieldNames.AriaDocumentId];
                _assetProvider.Update(id.ToGuid(), source.Metadata);
            }
        }
    }
}