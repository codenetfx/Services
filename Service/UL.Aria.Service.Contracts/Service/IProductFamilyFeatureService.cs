using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Defines the ProductFamilyFeature Service interface.
    /// </summary>
    [ServiceContract]
    public interface IProductFamilyFeatureService
    {
        /// <summary>
        ///     Creates the ProductFamilyFeature.
        /// </summary>
        /// <param name="productFamilyFeature">The product family.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyFeature",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Create(ProductFamilyFeatureDto productFamilyFeature);

        /// <summary>
        ///     Updates the ProductFamilyFeature.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyFeature">The product family.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyFeature/{id}",
            Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(string id, ProductFamilyFeatureDto productFamilyFeature);

        /// <summary>
        ///     Gets the ProductFamilyFeature by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyFeature/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductFamilyFeatureDto GetById(string id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyFeature",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<ProductFamilyFeatureDto> GetAll();

        /// <summary>
        /// Gets features by scope.
        /// </summary>
        /// <param name="scopeids">The scopeids.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyFeature/Scope/{scopeids}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<ProductFamilyFeatureDto> GetByScope(string scopeids);

        /// <summary>
        /// Removes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyFeature/{id}",
            Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Remove(string id);
    }
}