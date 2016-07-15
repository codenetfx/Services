using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Transactions;

using UL.Aria.Common;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation;
using UL.Aria.Common.Authorization;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Manager.Validation;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     Product Service implementation.
    /// </summary>
    [AutoRegisterRestServiceAttribute("ProductDetail")]
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall
        )]
    public class ProductService : IProductService
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ILogManager _logManager;
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IPrincipalResolver _principalResolver;
        private readonly IProductManager _productManager;
        private readonly IProductProvider _productProvider;
        private readonly IProductUploadProductInsertManager _productUploadProductInsertManager;
        private readonly IProductUploadProductUpdateManager _productUploadProductUpdateManager;
        private readonly IProductValidationManager _productValidationManager;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService" /> class.
        /// </summary>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="productManager">The product manager.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="productValidationManager">The product validation manager.</param>
        /// <param name="productUploadProductUpdateManager">The product upload product update manager.</param>
        /// <param name="productUploadProductInsertManager">The product upload product insert manager.</param>
        /// <param name="productProvider">The product provider.</param>
        /// <param name="assetProvider">The asset provider.</param>
        /// <param name="logManager">The log manager.</param>
        public ProductService(IMapperRegistry mapperRegistry, IProductManager productManager,
            ITransactionFactory transactionFactory,
            IAuthorizationManager authorizationManager,
            IPrincipalResolver principalResolver,
            IProductValidationManager productValidationManager,
            IProductUploadProductUpdateManager productUploadProductUpdateManager,
            IProductUploadProductInsertManager productUploadProductInsertManager,
            IProductProvider productProvider,
            IAssetProvider assetProvider,
            ILogManager logManager
            )
        {
            _mapperRegistry = mapperRegistry;
            _productManager = productManager;
            _transactionFactory = transactionFactory;
            _authorizationManager = authorizationManager;
            _principalResolver = principalResolver;
            _productValidationManager = productValidationManager;
            _productUploadProductUpdateManager = productUploadProductUpdateManager;
            _productUploadProductInsertManager = productUploadProductInsertManager;
            _productProvider = productProvider;
            _assetProvider = assetProvider;
            _logManager = logManager;
        }

        /// <summary>
        ///     Creates the specified product family.
        /// </summary>
        /// <param name="productFamily">The product family.</param>
        /// <returns></returns>
        public string Create(ProductDto productFamily)
        {
            Guard.IsNotNull(productFamily, "product");
            ValidateAccessRights(string.Empty, SecuredActions.Create);

            var product = _mapperRegistry.Map<Product>(productFamily);
            using (var transaction = _transactionFactory.Create())
            {
                var id = _productManager.Create(product).ToString();
                transaction.Complete();
                return id;
            }
        }

        /// <summary>
        ///     Updates the specified product family.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="product">The product family.</param>
        public void Update(string id, ProductDto product)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            Guard.IsNotNull(product, "product");
            // ReSharper disable PossibleInvalidOperationException
            Guard.AreEqual(convertedId, product.Id.Value, "Product.Id");
            // ReSharper restore PossibleInvalidOperationException
            var productMetadata = _assetProvider.Fetch(convertedId);
            var containerId = productMetadata.GetValue(AssetFieldNames.AriaContainerId, default(Guid)).ToString();
            ValidateAccessRights(containerId, SecuredActions.Update);

            var family = _mapperRegistry.Map<Product>(product);
            using (var transaction = _transactionFactory.Create())
            {
                _productManager.Update(family);
                transaction.Complete();
            }
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var productId = Guid.Parse(id);
            var productMetadata = _assetProvider.Fetch(productId);
            var containerId = productMetadata.GetValue(AssetFieldNames.AriaContainerId, default(Guid)).ToString();
            ValidateAccessRights(containerId, SecuredActions.Update);
            _productManager.Delete(productId);
        }

        /// <summary>
        ///     Gets the product family by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ProductDto GetProductById(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            var productMetadata = _assetProvider.Fetch(convertedId);
            var containerId = productMetadata.GetValue(AssetFieldNames.AriaContainerId, default(Guid)).ToString();
            ValidateAccessRights(containerId, SecuredActions.View);

            Product product;

            using (var transaction = _transactionFactory.Create())
            {
                product = _productManager.Get(convertedId);
                transaction.Complete();
            }

            return _mapperRegistry.Map<ProductDto>(product);
        }

        /// <summary>
        ///     Gets the product's download by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Stream GetProductDownloadById(string id)
        {
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "id");
            var context = WebOperationContext.Current;
            Stream productDocument;
            using (var transaction = _transactionFactory.Create())
            {
                productDocument = _productManager.GetDownload(convertedId);
                if (null != context)
                {
                    context.OutgoingResponse.Headers["Content-Disposition"] = "attachment; filename=" + id + ".xlsx";
                    context.OutgoingResponse.ContentLength = productDocument.Length;
                    context.OutgoingResponse.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                transaction.Complete();
            }
            return productDocument;
        }

        /// <summary>
        ///     Gets the product's download by id.
        /// </summary>
        /// <param name="familyId">The id.</param>
        /// <returns></returns>
        public Stream GetProductDownloadByProductFamilyId(string familyId)
        {
            var convertedId = Guid.Parse(familyId);
            Guard.IsNotEmptyGuid(convertedId, "familyId");
            var context = WebOperationContext.Current;
            Stream productDocument;
            using (var transaction = _transactionFactory.Create())
            {
                productDocument = _productManager.GetProductDownloadByProductFamilyId(convertedId);
                if (null != context)
                {
                    context.OutgoingResponse.Headers["Content-Disposition"] = "attachment; filename=" + familyId +
                                                                              ".xlsx";
                    context.OutgoingResponse.ContentLength = productDocument.Length;
                    context.OutgoingResponse.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                transaction.Complete();
            }
            return productDocument;
        }

        /// <summary>
        ///     Gets the product download by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns></returns>
        public Stream GetProductDownloadByProductUploadId(string productUploadId)
        {
            var convertedId = Guid.Parse(productUploadId);
            Guard.IsNotEmptyGuid(convertedId, "familyId");
            var context = WebOperationContext.Current;
            Stream productDocument;
            using (var transaction = _transactionFactory.Create())
            {
                productDocument = _productManager.GetProductDownloadByProductUploadId(convertedId);
                if (null != context)
                {
                    context.OutgoingResponse.Headers["Content-Disposition"] = "attachment; filename=" + productUploadId +
                                                                              ".xlsx";
                    context.OutgoingResponse.ContentLength = productDocument.Length;
                    context.OutgoingResponse.ContentType =
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                }
                transaction.Complete();
            }
            return productDocument;
        }

        /// <summary>
        ///     Uploads the specified file.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="fileName"></param>
        /// <param name="file">The file.</param>
        /// <returns>Confirmation Guid.</returns>
        public string Upload(string userId, string companyId, string fileName, Stream file)
        {
            Guard.IsNotNull(userId, "UserId");
            var userIdGuid = userId.ToGuid();
            Guard.IsNotEmptyGuid(userIdGuid, "UserId");

            Guard.IsNotNull(companyId, "CompanyId");
            var companyIdGuid = companyId.ToGuid();
            Guard.IsNotEmptyGuid(companyIdGuid, "CompanyId");

            Guard.IsNotNullOrEmptyTrimmed(fileName, "FileName");

            Guard.IsNotNull(file, "File");

            var productUpload = new ProductUpload
            {
                CompanyId = companyIdGuid,
                Status = ProductUploadStatusEnumDto.ReadyToProcess,
                FileContent = ReadToEnd(file),
                FileName = fileName.Substring(0, Math.Min(fileName.Length, 255)),
                CreatedById = userIdGuid,
                UpdatedById = userIdGuid,
                CreatedDateTime = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };


            return _productManager.Upload(productUpload).ToString();
        }

        /// <summary>
        ///     Fetches the product uploads by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex">Start index of the row.</param>
        /// <param name="rowEndIndex">End index of the row.</param>
        /// <returns>System.String.</returns>
        public ProductUploadSearchResultSetDto FetchByUserId(string userId, long rowStartIndex, long rowEndIndex)
        {
            Guard.IsNotNull(userId, "UserId");
            var userIdGuid = userId.ToGuid();
            Guard.IsNotEmptyGuid(userIdGuid, "UserId");
            var searchResultSet = _productManager.FetchByUserId(userIdGuid, rowStartIndex, rowEndIndex);
            return _mapperRegistry.Map<ProductUploadSearchResultSetDto>(searchResultSet);
        }

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSetDto.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ProductUploadResultSearchResultSetDto GetByProductUploadId(string productUploadId)
        {
            Guard.IsNotNull(productUploadId, "ProductUploadId");
            var productUploadIdGuid = productUploadId.ToGuid();
            Guard.IsNotEmptyGuid(productUploadIdGuid, "ProductUploadId");
            
            var searchResultSet = _productManager.GetByProductUploadId(productUploadIdGuid);
            return _mapperRegistry.Map<ProductUploadResultSearchResultSetDto>(searchResultSet);
            
        }

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadDto.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ProductUploadDto GetById(string productUploadId)
        {
            Guard.IsNotNull(productUploadId, "ProductUploadId");
            var productUploadIdGuid = productUploadId.ToGuid();
            Guard.IsNotEmptyGuid(productUploadIdGuid, "ProductUploadId");
            using (var transaction = _transactionFactory.Create())
            {
                var productUpload = _productManager.GetById(productUploadIdGuid);
                transaction.Complete();
                return _mapperRegistry.Map<ProductUploadDto>(productUpload);
            }
        }

        /// <summary>
        ///     Changes the status.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public void ChangeStatusOverride(string id, string value)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            Guard.IsNotNullOrEmpty(value, "value");

            ProductStatus status;
            if (!Enum.TryParse(value, out status))
                throw new ArgumentException("Invalid value", "value");
            if (status != ProductStatus.Submitted)
                throw new ArgumentException("New Status must be Submitted", "value");
            
            //
            // perform the update
            //
            using (TransactionScope t = _transactionFactory.Create())
            {
                Product product = _productManager.Get(new Guid(id));
                //
                // check for correct access rights
                //
                ClaimsPrincipal claimsPrincipal = _principalResolver.Current;
                var resourceClaim = new System.Security.Claims.Claim(SecuredResources.ProductStatusOverride, product.ContainerId.ToString());
                var actionClaim = new System.Security.Claims.Claim(SecuredActions.Update, product.ContainerId.ToString());
                bool authorize = _authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim);

                if (!authorize)
                    throw new UnauthorizedAccessException("You are not authorized to change this product's status");
                product.Status = status;
                product.SubmittedDateTime = DateTime.UtcNow;

                _productManager.UpdateStatus(product);
                t.Complete();
            }
        }

        /// <summary>
        ///     Changes the status.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public IList<string> ChangeStatus(string id, string value)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            Guard.IsNotNullOrEmpty(value, "value");

            ProductStatus status;
            if (!Enum.TryParse(value, out status))
                throw new ArgumentException("Invalid value", "value");
            if (status != ProductStatus.Submitted)
                throw new ArgumentException("New Status must be Submitted", "value");

            //
            // perform the update
            //
            using (TransactionScope t = _transactionFactory.Create())
            {
                Product product = _productManager.Get(new Guid(id));

				//
				// check for correct access rights
				//
                ClaimsPrincipal claimsPrincipal = _principalResolver.Current;
                var resourceClaim = new System.Security.Claims.Claim(SecuredResources.ProductStatus, product.ContainerId.ToString());
                var actionClaim = new System.Security.Claims.Claim(SecuredActions.Update, product.ContainerId.ToString());
                bool authorize = _authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim);

                if (!authorize)
                    throw new UnauthorizedAccessException("You are not authorized to change this product's status");

                var errors = _productValidationManager.Validate(product);

                if (null != errors && errors.Any())
                    return errors;

                product.Status = status;
                product.SubmittedDateTime = DateTime.UtcNow;

                _productManager.UpdateStatus(product);
                t.Complete();
            }
            return Enumerable.Empty<string>().ToList();
        }

        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="uploadId">The upload id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        public bool CreateFromUpload(ProductUploadResultDto product, string uploadId)
        {
            Guard.IsNotNull(product, "product");
            Guard.IsNotNullOrEmpty(uploadId, "uploadId");
            var uploadIdGuid = uploadId.ToGuid();
            Guard.IsNotEmptyGuid(uploadIdGuid, "uploadId");
            var companyId = _productManager.GetProductUploadHeader(uploadIdGuid).CompanyId;
            ValidateAccessRightsForCompany(companyId);
            var productUploadResult = _mapperRegistry.Map<ProductUploadResult>(product);
            productUploadResult.Product.Characteristics =
                product.Product.Characteristics.Select(x => _mapperRegistry.Map<ProductCharacteristic>(x)).ToList();
            using (var transaction = _transactionFactory.Create())
            {
                _productUploadProductInsertManager.Persist(productUploadResult, uploadIdGuid);
                transaction.Complete();
            }
            return productUploadResult.IsValid;
        }

        /// <summary>
        ///     Updates the specified product.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="product">The product.</param>
        /// <param name="uploadId">The upload id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        public bool UpdateFromUpload(string id, ProductUploadResultDto product, string uploadId)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            Guard.IsNotNull(product, "product");
            Guard.IsNotNullOrEmpty(uploadId, "uploadId");
            var uploadIdGuid = uploadId.ToGuid();
            Guard.IsNotEmptyGuid(uploadIdGuid, "uploadId");

            var productUploadResult = _mapperRegistry.Map<ProductUploadResult>(product);
            IDictionary<string, string> productMetadata = null;

            try
            {
                productMetadata = _assetProvider.Fetch(product.Product.Id.Value);
            }
            catch (Exception ex)
            {
                productUploadResult.IsValid = false;
                productUploadResult.Product = null;
                productUploadResult.Messages.Add(new ProductUploadMessage
                {
                    MessageType = ProductUploadMessageTypeEnumDto.Error,
                    Title = "Unable to find Product for Update.",
                    Detail =
                        String.Format(
                            "Product does not exist. The product id was {0}",
                            product.Product.Id),
                    CreatedDateTime = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                });
                ProductUploadProductInsertManager.PersistProductUploadResult(_transactionFactory, _productProvider, _logManager, productUploadResult);
                _logManager.Log(ex.ToLogMessage(MessageIds.ProductServiceUpdateFromUploadException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                TraceEventType.Error));
                throw;
            }

            var containerId = productMetadata.GetValue(AssetFieldNames.AriaContainerId, default(Guid)).ToString();
            ValidateAccessRights(containerId, SecuredActions.Update);

            productUploadResult.Product.Characteristics =
                product.Product.Characteristics.Select(x => _mapperRegistry.Map<ProductCharacteristic>(x)).ToList();
            using (var transaction = _transactionFactory.Create())
            {
                _productUploadProductUpdateManager.Persist(idGuid, productUploadResult, uploadIdGuid);
                transaction.Complete();
            }
            return productUploadResult.IsValid;
        }

        /// <summary>
        ///     Uploads the update.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productUploadDto">The product upload dto.</param>
        public void UploadUpdate(string id, ProductUploadDto productUploadDto)
        {
            Guard.IsNotNull(id, "id");
            var idGuid = id.ToGuid();
            Guard.IsNotEmptyGuid(idGuid, "id");
            Guard.IsNotNull(productUploadDto, "productUpload");
            var productUpload = _mapperRegistry.Map<ProductUpload>(productUploadDto);
            _productProvider.Update(productUpload);
        }

        internal static byte[] ReadToEnd(Stream stream)
        {
            var readBuffer = new byte[4096];

            var totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        var temp = new byte[readBuffer.Length*2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte) nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }

        internal void ValidateAccessRightsForCompany(Guid companyId)
        {
            var claimsPrincipal = _principalResolver.Current;
            var resourceClaim = new System.Security.Claims.Claim(SecuredResources.ProductInstance, companyId.ToString());
            var actionClaim = new System.Security.Claims.Claim(SecuredActions.Create, companyId.ToString());
            var authorized = _authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim);

            if (!authorized)
                throw new UnauthorizedAccessException("You are not authorized to create products for this company.");
        }

        internal void ValidateAccessRights(string containerId, string securedAction)
        {
            var claimsPrincipal = _principalResolver.Current;
            var resourceClaim = new System.Security.Claims.Claim(SecuredResources.ProductInstance, containerId);
            var actionClaim = new System.Security.Claims.Claim(securedAction, containerId);
            var authorized = _authorizationManager.Authorize(claimsPrincipal, resourceClaim, actionClaim);

            if (!authorized)
                throw new UnauthorizedAccessException("You are not authorized to access this product");
        }
    }
}