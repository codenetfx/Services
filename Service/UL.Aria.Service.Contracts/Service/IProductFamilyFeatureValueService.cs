using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IProductFamilyFeatureValueService
    /// </summary>
    [ServiceContract]
    public interface IProductFamilyFeatureValueService
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{ProductFamilyFeatureValueDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<ProductFamilyFeatureValueDto> FetchAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamilyFeatureValueDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        ProductFamilyFeatureValueDto Fetch(string id);

        /// <summary>
        ///     Fetches the by product family id.
        /// </summary>
        /// <param name="productFeatureId">The product feature id.</param>
        /// <returns>IList{ProductFamilyFeatureValueDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/ProductFeatureId/{productFeatureId}", Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<ProductFamilyFeatureValueDto> FetchByProductFeatureId(string productFeatureId);

        /// <summary>
        ///     Creates the specified product family feature value.
        /// </summary>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Create(ProductFamilyFeatureValueDto productFamilyFeatureValue);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyFeatureValue">The product family feature value.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, ProductFamilyFeatureValueDto productFamilyFeatureValue);

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Delete(string id);
    }
}