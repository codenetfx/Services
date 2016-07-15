using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Product Family service interface.
    /// </summary>
    [ServiceContract]
    public interface IProductFamilyService
    {
        /// <summary>
        ///     Creates the specified product family from the create request.
        /// </summary>
        /// <param name="productFamilyCreateUpdateRequest">The product family create request.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Create(ProductFamilyDetailDto productFamilyCreateUpdateRequest);

        /// <summary>
        ///     Updates the specified product family from the update request.
        /// </summary>
        /// <param name="productFamilyCreateUpdateRequest">The product family update request.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/",
            Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Update(ProductFamilyDetailDto productFamilyCreateUpdateRequest);

        /// <summary>
        ///     Gets the product family detail by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>ProductFamilyDetailDto.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}/Detail",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductFamilyDetailDto GetProductFamilyDetailById(string id);

        /// <summary>
        ///     Gets the product family by id.
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
        ProductFamilyDto GetProductFamilyById(string id);


        /// <summary>
        ///     Gets the product family template.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="template"></param>
        /// <returns>A <see cref="Stream" /> with the template contents.</returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{id}/{template}",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        Stream GetProductFamilyTemplate(string id, string template);

        /// <summary>
        ///     Gets the product families by business unit.
        /// </summary>
        /// <param name="businessUnitId">The business unit id.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/BusinessUnit/{businessUnitId}/ProductFamily",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProductFamiliesDto GetProductFamiliesByBusinessUnit(string businessUnitId);

        /// <summary>
        ///     Gets the product families.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/List",
            Method = "GET",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<ProductFamilyDto> GetProductFamilies();

        /// <summary>
        ///     Saves the product family attribute associations.
        /// </summary>
        /// <param name="productFamilyId">The product family id.</param>
        /// <param name="productFamilyAttributeAssociationIds">The product family attribute association ids.</param>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/{productFamilyId}/AttributeAssociation",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void SaveProductFamilyAttributeAssociations(string productFamilyId,
            IList<Guid> productFamilyAttributeAssociationIds);

        /// <summary>
        ///     Uploads the specified file to upload.
        /// </summary>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/Upload",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IEnumerable<string> Upload(Stream fileToUpload);
    }
}