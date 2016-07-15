using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;

using UL.Aria.Common.Authorization;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

using Task = System.Threading.Tasks.Task;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Implements operations for importing <see cref="Product" /> entities from <see cref="ProductUpload" /> objects.
    /// </summary>
    public class ProductUploadImportManager : IProductUploadImportManager
    {
        private readonly ILogManager _logManager;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IProductImportManager _productImportManager;
        private readonly IProductProvider _productProvider;
        private readonly IProductService _productService;
        private readonly IScratchSpaceService _scratchSpaceService;
        private readonly IProfileService _profileService;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductUploadImportManager" /> class.
        /// </summary>
        /// <param name="productProvider">The product provider.</param>
        /// <param name="productService">The product service.</param>
        /// <param name="productImportManager">The product import manager.</param>
        /// <param name="logManager">The log manager.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="scratchSpaceService">The scratch space repository.</param>
        /// <param name="profileService">The profile service.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public ProductUploadImportManager(IProductProvider productProvider, IProductService productService,
                                          IProductImportManager productImportManager, ILogManager logManager,
                                          ITransactionFactory transactionFactory,
                                          IScratchSpaceService scratchSpaceService, IProfileService profileService,IMapperRegistry mapperRegistry)
        {
            _productProvider = productProvider;
            _productService = productService;
            _productImportManager = productImportManager;
            _logManager = logManager;
            _transactionFactory = transactionFactory;
            _scratchSpaceService = scratchSpaceService;
            _profileService = profileService;
            _mapperRegistry = mapperRegistry;
            MilliSecondsTimeout = 10*1000;
            ErrorCount = 0;
        }

        internal int MilliSecondsTimeout { get; set; }

        /// <summary>
        /// Gets or sets the error count.
        /// </summary>
        /// <value>The error count.</value>
        public int ErrorCount { get; set; }

        /// <summary>
        ///     Processes this instance.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="isContinuous">
        ///     if set to <c>true</c> [is continuous].
        /// </param>
        /// <returns></returns>
        public Task Process(CancellationToken token, bool isContinuous = true)
        {
            return new Task(() =>
                {
                    do
                    {

                        if (ErrorCount > 2)
                        {
                            Thread.Sleep(29 * 1000 + new Random().Next(1, 1000));
                            ErrorCount = 0;
                        }

                        ProductUpload productUpload = null;
                        try
                        {
                            Trace.CorrelationManager.ActivityId = Guid.NewGuid();
                            using (var scope = _transactionFactory.Create())
                            {
                                productUpload = _productProvider.FetchProductUploadNextForProcessing();

                                if (productUpload == null)
                                {
                                    new ManualResetEvent(false).WaitOne(MilliSecondsTimeout);
                                    continue;
                                }

                                productUpload.Status = ProductUploadStatusEnumDto.Processing;
                                _productProvider.Update(productUpload);
                                scope.Complete();
                            }
                            // yes, this is outside the transaciton scope as each product must be its own
                            // transaction.
                            Import(productUpload);
                            ErrorCount = 0;
                        }
                        catch (Exception ex)
                        {
                            ErrorCount++;

                            // don't fail, just log.
                            var logMessage = ex.ToLogMessage(MessageIds.ProductUploadImportProcessException, LogCategory.ProductUploadImportManager,
                                                             LogPriority.Critical, TraceEventType.Critical);
                            if (null != productUpload)
                                logMessage.Data.Add("ID", ((productUpload).Id ?? Guid.Empty).ToString());
                            _logManager.Log(logMessage);
                        }

                        if (token.IsCancellationRequested)
                            throw new OperationCanceledException(token);
                    } while (isContinuous);
                }, token);
        }

        /// <summary>
        ///     Imports the specified product upload.
        /// </summary>
        /// <param name="productUpload">The product upload.</param>
        public void Import(ProductUpload productUpload)
        {
            var jobstatus = ProductUploadStatusEnumDto.ProcessedSuccessfully;
            
            SetPrincipal(productUpload);

            try
            {
                using (var stream = new MemoryStream(productUpload.FileContent))
                {
                    foreach (var productUploadResult in _productImportManager.Import(stream))
                        jobstatus = ProcessResult(productUpload, productUploadResult, jobstatus);
                }
            }
            catch (Exception ex)
            {
                jobstatus = ProductUploadStatusEnumDto.ProcessedWithErrors;
                // don't fail, just log.
                var logMessage = ex.ToLogMessage(MessageIds.ProductUploadImportException, LogCategory.ProductUploadImportManager, LogPriority.Critical,
                                                 TraceEventType.Critical);
                if (null != productUpload)
                    logMessage.Data.Add("ID", ((productUpload).Id ?? Guid.Empty).ToString());
                _logManager.Log(logMessage);
            }
            productUpload.Status = jobstatus;
            _productProvider.Update(productUpload);
        }

        internal void SetPrincipal(ProductUpload productUpload)
        {
            var profile = _profileService.FetchByIdOrUserName(productUpload.CreatedById.ToString());
            productUpload.CreatedByUserLoginId = profile.LoginId;
            _productProvider.Update(productUpload);

            var claimsIdentity = new ClaimsIdentity(
                profile.Claims.Select(x => new System.Security.Claims.Claim(x.EntityClaim, x.Value ?? x.EntityId.ToString()))
                );
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name, profile.LoginId));
            claimsIdentity.AddClaim(new System.Security.Claims.Claim(SecuredClaims.UserId, profile.Id.Value.ToString("N")));
            if (claimsIdentity.HasClaim(x => x.Type == SecuredClaims.CompanyAccess && x.Value.ToUpperInvariant() == "46F65EA8-913D-4F36-9E28-89951E7CE8EF"))
            {
                claimsIdentity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Role, "UL-Employee"));
            }
            Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);
        }

        private ProductUploadStatusEnumDto ProcessResult(ProductUpload productUpload,
                                                         ProductUploadResult productUploadResult,
                                                         ProductUploadStatusEnumDto jobstatus)
        {
            productUploadResult.ProductUploadId = productUpload.Id.Value;
            productUploadResult.CreatedByUserLoginId = productUpload.CreatedByUserLoginId;

            try
            {
                productUploadResult.IsValid = PersistProduct(productUpload, productUploadResult);
            }
            catch (Exception ex)
            {
                productUploadResult.IsValid = false;
                productUploadResult.Product = null;
                jobstatus = ProductUploadStatusEnumDto.ProcessedWithErrors;
                _logManager.Log(ex.ToLogMessage(MessageIds.ProductUploadPersistException, LogCategory.ProductUploadImportManager, LogPriority.Medium,
                                                TraceEventType.Error));
            }

            if (!productUploadResult.IsValid)
                jobstatus = ProductUploadStatusEnumDto.ProcessedWithErrors;

            return jobstatus;
        }

        private bool PersistProduct(ProductUpload productUpload, ProductUploadResult productUploadResult)
        {
            productUploadResult.CreatedById = productUpload.CreatedById;
            productUploadResult.Product.CompanyId = productUpload.CompanyId;
            productUploadResult.UpdatedById = productUpload.UpdatedById;
            productUploadResult.Product.CompanyId = productUpload.CompanyId;
            var productUploadResultDto = _mapperRegistry.Map<ProductUploadResultDto>(productUploadResult);
            productUploadResultDto.Product.Characteristics = productUploadResult.Product.Characteristics.Select(x => _mapperRegistry.Map<ProductCharacteristicDto>(x)).ToList();

            if (productUploadResult.Product.Id.HasValue)
            {
                return _productService.UpdateFromUpload(productUploadResult.Product.Id.ToString(), productUploadResultDto, productUploadResult.ProductUploadId.ToString());
            }

            return _productService.CreateFromUpload(productUploadResultDto, productUploadResult.ProductUploadId.ToString());
        }
    }
}