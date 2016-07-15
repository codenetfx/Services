using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     Implements operations for managing templates for <see cref="ProductFamily" /> objects.
    /// </summary>
    public class ProductFamilyTemplateManager : IProductFamilyTemplateManager
    {
        private readonly ILogManager _logManager;
        private readonly IProductFamilyCharacteristicGroupHelper _productFamilyCharacteristicGroupHelper;
        private readonly IProductFamilyManager _productFamilyManager;
        private readonly IProfileManager _profileManager;
        private readonly IProductFamilyDocumentBuilderLocator _providerLocator;
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProductFamilyTemplateManager" /> class.
        /// </summary>
        /// <param name="productFamilyManager">The product family manager.</param>
        /// <param name="profileManager">The profile manager.</param>
        /// <param name="logManager">The log manager.</param>
        /// <param name="providerLocator">The provider locator.</param>
        /// <param name="productFamilyCharacteristicGroupHelper">The product family characteristic group helper.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProductFamilyTemplateManager(IProductFamilyManager productFamilyManager, IProfileManager profileManager,
            ILogManager logManager, IProductFamilyDocumentBuilderLocator providerLocator,
            IProductFamilyCharacteristicGroupHelper productFamilyCharacteristicGroupHelper,
            ITransactionFactory transactionFactory)
        {
            _productFamilyManager = productFamilyManager;
            _profileManager = profileManager;
            _logManager = logManager;
            _providerLocator = providerLocator;
            _productFamilyCharacteristicGroupHelper = productFamilyCharacteristicGroupHelper;
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches the template whose <see cref="ProductFamily" /> is identified by <paramref name="id" />.
        /// </summary>
        /// <param name="id">The id of the <see cref="ProductFamily" />.</param>
        /// <param name="templateType"></param>
        /// <returns>
        ///     A <see cref="Stream" /> with the contents of the template.
        /// </returns>
        public Stream FetchProductFamilyTemplate(Guid id, ProductFamilyTemplate templateType)
        {
            var productFamily = _productFamilyManager.Get(id);
            var documentBuilder = _providerLocator.Resolve(templateType.ToString());
            IEnumerable<ProductFamilyCharacteristicDomainEntity> baseCharacteristics;
            IEnumerable<ProductFamilyCharacteristicDomainEntity> variableCharacteristics;
            IReadOnlyCollection<ProductFamilyCharacteristicDomainEntity> characteristics;
            using (var scope = _transactionFactory.Create())
            {
                characteristics = _productFamilyManager.GetProductFamilyCharacteristics(productFamily.Id.Value);
                scope.Complete();
            }
            _productFamilyCharacteristicGroupHelper.GroupCharacteristics(characteristics, out baseCharacteristics,
                out variableCharacteristics);

            var creatingUser = new ProfileBo();
            if (Guid.Empty != productFamily.CreatedById)
            {
                try
                {
                    creatingUser = _profileManager.FetchById(productFamily.CreatedById);
                }
                catch (DatabaseItemNotFoundException exception)
                {
                    _logManager.Log(exception.ToLogMessage(MessageIds.ProductFamilyNotFoundException, LogCategory.ProductFamilyTemplateManager, LogPriority.Low,
                        TraceEventType.Verbose));
                }
            }

            var featureAssociations = _productFamilyManager.GetProductFamilyFeatureAssociations(id);
            var dependencyMappings = _productFamilyManager.GetDependencies(id, featureAssociations);

            return documentBuilder.Build(
                productFamily,
                creatingUser,
                baseCharacteristics,
                variableCharacteristics,
                dependencyMappings);
        }
    }
}