using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    /// Interface ITaskTypeService
    /// </summary>
    [ServiceContract]
    public interface ITaskTypeAvailableBehaviorService
    {
        /// <summary>
        /// Fetches the fields for the corresponding available behavior by identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns>
        /// TaskTypeDto.
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{taskTypeAvailableBehaviorId}/fields", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IList<TaskTypeAvailableBehaviorFieldDto> FindByTaskTypeAvailableBehaviorId(string taskTypeAvailableBehaviorId);
    }
}