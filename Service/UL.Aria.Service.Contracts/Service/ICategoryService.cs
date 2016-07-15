using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface ICategoryService
    /// </summary>
    [ServiceContract]
    public interface ICategoryService
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{CategoryDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<CategoryDto> FetchAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>CategoryDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        CategoryDto Fetch(string id);

        /// <summary>
        ///     Creates the specified category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>System.String.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        string Create(CategoryDto category);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="company">The company.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, CategoryDto company);

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