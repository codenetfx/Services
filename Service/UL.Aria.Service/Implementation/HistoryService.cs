using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    ///     fulfills operations for
    /// </summary>
    [AutoRegisterRestServiceAttribute]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class HistoryService : IHistoryService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly IHistoryManager _historyManager;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HistoryService" /> class.
        /// </summary>
        /// <param name="historyManager">The history manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public HistoryService(IHistoryManager historyManager, IMapperRegistry mapperRegistry)
        {
            _historyManager = historyManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        ///     Gets history items by entity id.
        /// </summary>
        /// <param name="id">The entity id.</param>
        /// <returns>
        ///     IEnumerable{HistoryDto}
        /// </returns>
        public IEnumerable<HistoryDto> FetchHistoryByEntityId(string id)
        {
            Guard.IsNotNullOrEmpty(id, "id");
            var historyItems = _historyManager.GetHistoryByEntityId(new Guid(id));
            return historyItems.Select(historyItem => _mapperRegistry.Map<HistoryDto>(historyItem)).ToList();
        }

        /// <summary>
        ///     Creates a new history item.
        /// </summary>
        /// <param name="historyDto">The new history item.</param>
        /// <returns>historyId</returns>
        public Guid Create(HistoryDto historyDto)
        {
            Guard.IsNotNull(historyDto, "history");
            var history = _mapperRegistry.Map<History>(historyDto);
            var historyId = _historyManager.Create(history);
            return historyId;
        }

        /// <summary>
        /// Downloads the history by entity identifier.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        /// <returns>Stream.</returns>
        public Stream DownloadHistoryByEntityId(string id)
        {
            Guard.IsNotNullOrEmpty(id, "entityId");
            var convertedId = Guid.Parse(id);
            Guard.IsNotEmptyGuid(convertedId, "entityId");

            var context = WebOperationContext.Current;

            var historyDocument = _historyManager.DownloadHistoryByEntityId(convertedId);
            if (null != context)
            {
                context.OutgoingResponse.Headers["Content-Disposition"] =
                    "attachment; filename=Project History Export - " +
                    DateTime.UtcNow.ToShortDateString() + " " +
                    DateTime.UtcNow.ToShortTimeString() + " " +
                    id +
                    ".xlsx";
                context.OutgoingResponse.ContentLength = historyDocument.Length;
                context.OutgoingResponse.ContentType =
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            return historyDocument;
        }

		/// <summary>
		/// Downloads the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
	    public Stream DownloadTaskHistory(string id, string containerId)
	    {
			Guard.IsNotNullOrEmpty(id, "entityId");
			var convertedId = Guid.Parse(id);
			Guard.IsNotEmptyGuid(convertedId, "entityId");

			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var convertedContainerId = Guid.Parse(containerId);
			Guard.IsNotEmptyGuid(convertedContainerId, "containerId");


			var context = WebOperationContext.Current;

			var historyDocument = _historyManager.DownloadTaskHistory(convertedId, convertedContainerId);
			if (null != context)
			{
				context.OutgoingResponse.Headers["Content-Disposition"] =
					"attachment; filename=Task History - " +
					DateTime.UtcNow.ToShortDateString() + " " +
					DateTime.UtcNow.ToShortTimeString() + " " +
					id +
					".xlsx";
				context.OutgoingResponse.ContentLength = historyDocument.Length;
				context.OutgoingResponse.ContentType =
					"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			}
			return historyDocument;
	    }


		/// <summary>
		/// Fetches the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
		public IEnumerable<HistoryDto> FetchTaskHistory(string id, string containerId)
		{
			Guard.IsNotNullOrEmpty(id, "entityId");
			var convertedId = Guid.Parse(id);
			Guard.IsNotEmptyGuid(convertedId, "entityId");

			Guard.IsNotNullOrEmpty(containerId, "containerId");
			var convertedContainerId = Guid.Parse(containerId);
			Guard.IsNotEmptyGuid(convertedContainerId, "containerId");

			var historyItems = _historyManager.FetchTaskHistory(convertedId, convertedContainerId);
			return historyItems.Select(historyItem => _mapperRegistry.Map<HistoryDto>(historyItem)).ToList();
		}
	}
}