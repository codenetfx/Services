using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Client;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;

namespace UL.Aria.Service.Relay.Manager
{
    /// <summary>
    /// Implements a proxy for <see cref="IProductService"/>
    /// </summary>
    public class RelayProductServiceProxy : ServiceAgentManagedProxy<IProductService>, IProductService
    {
        private WebChannelFactory<IProductService> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayProductServiceProxy" /> class.
        /// </summary>
        /// <param name="configurationSource">The configuration source.</param>
        public RelayProductServiceProxy(IProxyConfigurationSource configurationSource) :
            this(
            new WebChannelFactory<IProductService>(new WebHttpBinding(), configurationSource.ProductService))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ServiceAgentManagedProxy{T}" /> class.
        /// </summary>
        /// <param name="serviceProxyFactory">The service proxy factory.</param>
        private RelayProductServiceProxy(WebChannelFactory<IProductService> serviceProxyFactory)
            : base(serviceProxyFactory)
        {
            _factory = serviceProxyFactory;
        }

        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        public string Create(ProductDto product)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="uploadId">The upload id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public bool CreateFromUpload(ProductUploadResultDto product, string uploadId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Updates the specified product.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="product">The product.</param>
        public void Update(string id, ProductDto product)
        {
            throw new NotSupportedException();
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
        /// <exception cref="System.NotSupportedException"></exception>
        public bool UpdateFromUpload(string id, ProductUploadResultDto product, string uploadId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(string id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ProductDto GetProductById(string id)
        {
            ProductDto product = null;
            IProductService productService = ClientProxy;
            using (new OperationContextScope((IContextChannel)productService))
            {
                product = productService.GetProductById(id);
                return product;
            }
        }


        /// <summary>
        ///     Gets the product's download by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Stream GetProductDownloadById(string id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the product's download by id.
        /// </summary>
        /// <param name="familyId">The id.</param>
        /// <returns></returns>
        public Stream GetProductDownloadByProductFamilyId(string familyId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Uploads the update.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productUploadDto">The product upload dto.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public void UploadUpdate(string id, ProductUploadDto productUploadDto)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Uploads the specified products for a specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="fileName"></param>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        public string Upload(string userId, string companyId, string fileName, Stream file)
        {
            throw new NotSupportedException();
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
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSetDto.</returns>
        public ProductUploadResultSearchResultSetDto GetByProductUploadId(string productUploadId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadDto.</returns>
        public ProductUploadDto GetById(string productUploadId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Gets the product download by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns></returns>
        public Stream GetProductDownloadByProductUploadId(string productUploadId)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Changes the status.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public IList<string> ChangeStatus(string id, string value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     Changes the status.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public void ChangeStatusOverride(string id, string value)
        {
            throw new NotSupportedException();
        }
    }
}
