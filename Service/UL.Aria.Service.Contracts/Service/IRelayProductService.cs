using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IRelayProductService
    {
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
    }
}