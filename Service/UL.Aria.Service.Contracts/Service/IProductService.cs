using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Product service interface.
    /// </summary>
    [ServiceContract]
    public interface IProductService
    {
        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Create(ProductDto product);

        /// <summary>
        ///     Creates the specified product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <param name="uploadId">The upload id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{uploadId}",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool CreateFromUpload(ProductUploadResultDto product, string uploadId);

        /// <summary>
        ///     Updates the specified product.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="product">The product.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}",
            Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(string id, ProductDto product);

        /// <summary>
        ///     Updates the specified product.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="product">The product.</param>
        /// <param name="uploadId">The upload id.</param>
        /// <returns>
        ///     <c>true</c> if XXXX, <c>false</c> otherwise
        /// </returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}/{uploadId}",
            Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        bool UpdateFromUpload(string id, ProductUploadResultDto product, string uploadId);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Delete(string id);

        /// <summary>
        ///     Gets the product by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductDto GetProductById(string id);

        /// <summary>
        ///     Gets the product's download by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}/Download",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetProductDownloadById(string id);

        /// <summary>
        ///     Gets the product's download by id.
        /// </summary>
        /// <param name="familyId">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Family/{familyId}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetProductDownloadByProductFamilyId(string familyId);

        /// <summary>
        ///     Uploads the update.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productUploadDto">The product upload dto.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Upload/{id}",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void UploadUpdate(string id, ProductUploadDto productUploadDto);

        /// <summary>
        ///     Uploads the specified products for a specified user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="fileName"></param>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Company/{companyId}?fileName={fileName}&userId={userId}",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Upload(string userId, string companyId, string fileName, Stream file);

        /// <summary>
        ///     Fetches the product uploads by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="rowStartIndex">Start index of the row.</param>
        /// <param name="rowEndIndex">End index of the row.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/User/{userId}?rowStartIndex={rowStartIndex}&rowEndIndex={rowEndIndex}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductUploadSearchResultSetDto FetchByUserId(string userId, long rowStartIndex, long rowEndIndex);

        /// <summary>
        ///     Gets the by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadResultSearchResultSetDto.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Upload/{productUploadId}/List",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductUploadResultSearchResultSetDto GetByProductUploadId(string productUploadId);

        /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns>ProductUploadDto.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Upload/{productUploadId}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductUploadDto GetById(string productUploadId);

        /// <summary>
        ///     Gets the product download by product upload id.
        /// </summary>
        /// <param name="productUploadId">The product upload id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Upload/{productUploadId}/Download",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetProductDownloadByProductUploadId(string productUploadId);

        /// <summary>
        ///     Changes the status.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "Status/{id}/{value}",
            Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void ChangeStatusOverride(string id, string value);


        /// <summary>
        ///     Changes the status.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "StatusOverride/{id}/{value}",
            Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<string> ChangeStatus(string id, string value);
    }
}