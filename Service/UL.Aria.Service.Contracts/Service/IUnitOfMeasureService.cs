using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     Interface IUnitOfMeasureService
    /// </summary>
    [ServiceContract]
    public interface IUnitOfMeasureService
    {
        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{UnitOfMeasureDto}.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/List", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<UnitOfMeasureDto> FetchAll();

        /// <summary>
        ///     Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>UnitOfMeasureDto.</returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        UnitOfMeasureDto Fetch(string id);

        /// <summary>
        ///     Creates the specified unit of measure.
        /// </summary>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/", Method = "POST", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Create(UnitOfMeasureDto unitOfMeasure);

        /// <summary>
        ///     Updates the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="unitOfMeasure">The unit of measure.</param>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "PUT", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        void Update(string id, UnitOfMeasureDto unitOfMeasure);

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