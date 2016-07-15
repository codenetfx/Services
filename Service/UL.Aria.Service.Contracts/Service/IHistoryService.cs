using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Contracts.Service
{
    /// <summary>
    ///     defines contract for manipulating history items
    /// </summary>
    [ServiceContract]
    public interface IHistoryService
    {
        /// <summary>
        ///     Gets history items by entity id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>
        ///     IEnumerable{HistoryDto}
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "/{id}", Method = "GET", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<HistoryDto> FetchHistoryByEntityId(string id);

        /// <summary>
        ///     Creates new history item.
        /// </summary>
        /// <param name="history">The new history item.</param>
        /// <returns>
        ///     HistoryId
        /// </returns>
        [OperationContract]
        [WebInvoke(
            UriTemplate = "/",
            Method = "POST",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Guid Create(HistoryDto history);

		/// <summary>
		/// Downloads the history by entity identifier.
		/// </summary>
		/// <param name="id">The entity identifier.</param>
		/// <returns>Stream.</returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "/{id}/Download",
			Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
		Stream DownloadHistoryByEntityId(string id);


		/// <summary>
		/// Downloads the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		[OperationContract]
		[WebInvoke(
			UriTemplate = "/{id}/Container/{containerId}/Download",
			Method = "GET",
			RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json,
			BodyStyle = WebMessageBodyStyle.Wrapped)]
	    Stream DownloadTaskHistory(string id, string containerId);

		/// <summary>
		/// Fetches the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		/// 
		[OperationContract]
		[WebInvoke(UriTemplate = "/{id}/Container/{containerId}/TaskHistory", Method = "GET", RequestFormat = WebMessageFormat.Json,
			ResponseFormat = WebMessageFormat.Json)]
		IEnumerable<HistoryDto> FetchTaskHistory(string id, string containerId);

    }
}