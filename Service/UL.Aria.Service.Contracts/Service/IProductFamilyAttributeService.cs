using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Defines the Product Family Service interface.
    /// </summary>
    [ServiceContract]
    public interface IProductFamilyAttributeService
    {
        /// <summary>
        /// Creates the product family attribute.
        /// </summary>
        /// <param name="productFamilyAttributeDto">The product family attribute.</param>
        /// <returns>Product family attribute id.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyAttribute",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Create(ProductFamilyAttributeDto productFamilyAttributeDto);

        /// <summary>
        /// Updates the product family attribute.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="productFamilyAttributeDto">The product family attribute.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyAttribute/{id}",
            Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(string id, ProductFamilyAttributeDto productFamilyAttributeDto);

        /// <summary>
        /// Gets the product family attribute by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Product family attribute data transfer object.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyAttribute/{id}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductFamilyAttributeDto GetById(string id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyAttribute",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<ProductFamilyAttributeDto> GetAll();

        /// <summary>
        /// Gets attributes by scope.
        /// </summary>
        /// <param name="scopeids">The scopeids.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyAttribute/Scope/{scopeids}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IList<ProductFamilyAttributeDto> GetByScope(string scopeids);

        /// <summary>
        /// Removes the specified unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/ProductFamilyAttribute/{id}",
            Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Remove(string id);
    }
}