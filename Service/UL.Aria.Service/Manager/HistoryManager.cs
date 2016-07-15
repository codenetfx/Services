using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Transactions;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Provider.EntityHistoryStrategy;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    ///     implements concrete functionality for
    /// </summary>
    public class HistoryManager : IHistoryManager
    {
        private readonly ITransactionFactory _transactionFactory;
        private readonly IHistoryProvider _historyProvider;
        private readonly IMapperRegistry _mapperRegistry;
	    private readonly IHistoryDocumentBuilder _historyDocumentBuilder;
        private readonly ITaskManager _taskManager;
        private readonly IProjectProvider _projectProvider;
	    private readonly IProfileManager _profileManager;
	    private readonly IEntityHistoryStrategyResolver _entityHistoryStrategyResolver;

	    /// <summary>
		/// Initializes a new instance of the <see cref="HistoryManager" /> class.
		/// </summary>
		/// <param name="transactionFactory">The transaction factory.</param>
		/// <param name="historyProvider">The history provider.</param>
		/// <param name="mapperRegistry">The mapper registry.</param>
		/// <param name="historyDocumentBuilder">The history document builder.</param>
		/// <param name="taskManager">The task provider.</param>
		/// <param name="projectProvider">The project provider.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="entityHistoryStrategyResolver">The entity history strategy resolver.</param>
        public HistoryManager(ITransactionFactory transactionFactory, IHistoryProvider historyProvider, IMapperRegistry mapperRegistry, 
			IHistoryDocumentBuilder historyDocumentBuilder, ITaskManager taskManager, 
			IProjectProvider projectProvider, IProfileManager profileManager,
			IEntityHistoryStrategyResolver entityHistoryStrategyResolver)
        {
            _transactionFactory = transactionFactory;
            _historyProvider = historyProvider;
            _mapperRegistry = mapperRegistry;
			_historyDocumentBuilder = historyDocumentBuilder;
	        _taskManager = taskManager;
            _projectProvider = projectProvider;
	        _profileManager = profileManager;
			_entityHistoryStrategyResolver = entityHistoryStrategyResolver;
        }

        /// <summary>
        ///     Gets history items by entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>IEnumerable{History}</returns>
        public IEnumerable<History> GetHistoryByEntityId(Guid entityId)
        {
            var historyItems = _historyProvider.FetchHistoryByEntityId(entityId).ToList();
            var lastItem = new History { TrackedInfo = new List<NameValuePair>() };
            historyItems.ForEach(historyItem =>
            {
                historyItem.TrackedInfo = DeriveTrackedInfo(historyItem);
                historyItem.ActionUserText = DeriveActionUserText(historyItem);
                // refine ActionType from raw string
                if (historyItem.ActionType != null) historyItem.ActionType = historyItem.ActionType.Split('.').LastOrDefault();
                // serialize the History ActionDetail list
                if (historyItem.TrackedInfo != null)
                    historyItem.ActionDetail =
                        string.Join("<br/>",
                            historyItem.TrackedInfo.Where(x => !lastItem.TrackedInfo.Any(y => y.Name == x.Name && y.Value == x.Value)).Select(x => "<b>" + x.Name + "</b> : " + x.Value).ToArray());
                lastItem = historyItem;
            });

            // For Project, add the Task Created and Deleted items to history list
            var containerId = _projectProvider.FetchById(entityId).ContainerId;
            if (containerId != null)
            {
                var allTasksWithDeleted = _taskManager.FetchAllWithDeleted(containerId.Value);
                foreach (var item in allTasksWithDeleted.GroupBy(x => new { x.CreatedById, x.Created.Year, x.Created.Month, x.Created.Day, x.Created.Hour, x.Created.Minute }))
                {
                    historyItems.Add(new History
                    {
                        ActionDate = item.First().Created.AddSeconds(-item.First().Created.Second),
                        ActionType = "Task(s) Added",
                        ActionDetail = string.Join("<br/>", item.ToList().Select(x => "<b>Name</b> : " + x.Title).ToArray()),
                        ActionUserText = GetProfileDisplayName(item.First().CreatedById)
                    });
                }
                foreach (var item in allTasksWithDeleted.Where(x => x.IsDeleted).GroupBy(x => new { x.CreatedById, x.Created.Year, x.Created.Month, x.Created.Day, x.Created.Hour, x.Created.Minute }))
                {
                  
                    historyItems.Add(new History
                    {
                        ActionDate = item.First().Modified.AddSeconds(-item.First().Modified.Second),
                        ActionType = "Task(s) Deleted",
                        ActionDetail = string.Join("<br/>", item.ToList().Select(x => "<b>Name</b> : " + x.Title).ToArray()),
                        ActionUserText = GetProfileDisplayName(item.First().CreatedById)
                    });
                }
            }

            return historyItems;
        }

        /// <summary>
        /// Creates a new history item.
        /// </summary>
        /// <param name="history">The new history item.</param>
        /// <returns>historyId</returns>
        public Guid Create(History history)
        {
            using (TransactionScope scope = _transactionFactory.Create())
            {
                var historyId = _historyProvider.Create(history);
                scope.Complete();
                return historyId;
            }
        }

        internal List<NameValuePair> DeriveTrackedInfo(History historyItem)
        {
			var entityHistoryStrategy = _entityHistoryStrategyResolver.Resolve(historyItem.ActionDetailEntityType);
			var entityHistoryContext = new EntityHistoryContext(entityHistoryStrategy);
	        historyItem.ActionType = entityHistoryContext.BuildAction(historyItem.EntityType, historyItem.ActionType);
	        return entityHistoryContext.DeriveTrackedInfo(historyItem);
        }

        internal string DeriveActionUserText(History historyItem)
        {
            if (historyItem != null && historyItem.ActionUserId != Guid.Empty)
            {
                var profile = _profileManager.FetchById(historyItem.ActionUserId); //display text for user
                if (profile != null)
                {
                    return profile.DisplayName;
                }
            }
            return null;
        }

		/// <summary>
		/// Downloads the history by entity identifier.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>Stream.</returns>
		public Stream DownloadHistoryByEntityId(Guid entityId)
		{
			var historyItems = GetHistoryByEntityId(entityId).ToList();
            var lastItem = new History { TrackedInfo = new List<NameValuePair>() };
            foreach (var item in historyItems.OrderBy(x=>x.ActionDate))
            {
                if (item.ActionType != null) item.ActionType = item.ActionType.Split('.').LastOrDefault();
                if (item.TrackedInfo != null)
                    item.ActionDetail =
                        string.Join(Environment.NewLine,
                            item.TrackedInfo.Where(x => !lastItem.TrackedInfo.Any(y => y.Name == x.Name && y.Value == x.Value)).Select(x => x.Name + " : " + x.Value).ToArray());
                lastItem = item;
            }

		    var project = _projectProvider.FetchById(entityId);
		    if (null != project && project.ContainerId.HasValue)
		    {
		        var allTasksWithDeleted = _taskManager.FetchAllWithDeleted(project.ContainerId.Value);
		        if (null == allTasksWithDeleted)
		            return _historyDocumentBuilder.Build(historyItems.OrderBy(x => x.ActionDate));
		        // Add the Task Created items to display table
		        foreach (
		            var item in
		                allTasksWithDeleted.GroupBy(
		                    x =>
		                        new
		                        {
		                            x.CreatedById,
		                            x.Created.Year,
		                            x.Created.Month,
		                            x.Created.Day,
		                            x.Created.Hour,
		                            x.Created.Minute
		                        }))
		        {
		            historyItems.Add(new History
		            {
		                ActionDate = item.First().Created.AddSeconds(-item.First().Created.Second),
		                ActionType = "Task(s) Added",
		                ActionDetail =
		                    string.Join(Environment.NewLine, item.ToList().Select(x => "Name : " + x.Title).ToArray()),
		                ActionUserId = item.First().CreatedById
		            });
		        }
		        foreach (
		            var item in
		                allTasksWithDeleted.Where(x => x.IsDeleted)
		                    .GroupBy(
		                        x =>
		                            new
		                            {
		                                x.CreatedById,
		                                x.Created.Year,
		                                x.Created.Month,
		                                x.Created.Day,
		                                x.Created.Hour,
		                                x.Created.Minute
		                            }))
		        {
		            var first = item.First();
		            var history = new History
		            {
		                ActionDate = first.Modified.AddSeconds(-first.Modified.Second),
		                ActionType = "Task(s) Deleted",
		                ActionDetail =
		                    string.Join(Environment.NewLine, item.ToList().Select(x => "Name : " + x.Title).ToArray()),
		            };
		            var actionUser = Guid.Empty;
		            Guid.TryParse(first.ModifiedBy, out actionUser);
		            history.ActionUserId = actionUser;
		            historyItems.Add(history);
		        }
		    }

            return _historyDocumentBuilder.Build(historyItems.Where(x=>!string.IsNullOrEmpty(x.ActionDetail)).OrderBy(x => x.ActionDate));
		}

		/// <summary>
		/// Downloads the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
	    public Stream DownloadTaskHistory(Guid id, Guid containerId)
		{
			var historyItems = GetTaskHistory(id, containerId);
			var lastItem = new History { TrackedInfo = new List<NameValuePair>() };
			var enumerable = historyItems as IList<History> ?? historyItems.ToList();
            foreach (var item in enumerable)
			{
				if (item.ActionType != null) item.ActionType = item.ActionType.Split('.').LastOrDefault();
				item.ActionDetail =
                    string.Join(Environment.NewLine,
					item.TrackedInfo.Where(x => !lastItem.TrackedInfo.Any(y => y.Name == x.Name && y.Value == x.Value)).Select(x => x.Name + " : " + x.Value).ToArray());
				lastItem = item;
			}

			return _historyDocumentBuilder.Build(enumerable.OrderBy(x => x.ActionDate));
	    }

		internal void GetModifyingUser(NameValuePair x)
		{
			Guid profileId;
			if (!Guid.TryParse(x.Value, out profileId))
				return;
			if (profileId != Guid.Empty)
			{
				var profile = _profileManager.FetchById(profileId);
				if (profile != null)
				{
					//display text for userId
					x.Value = profile.DisplayName;
				}
			}
		}

	    private IEnumerable<History> GetTaskHistory(Guid id, Guid containerId)
	    {
			var taskDeltaHistory = _taskManager.FetchDeltaHistory(containerId, id);
			var historyItems = _mapperRegistry.Map<List<History>>(taskDeltaHistory);

			var includeFieldList = new Dictionary<string, string>
            {
                {"Title", "Task Name"},
                {"Body", "Body"},
                {"StartDate", "Start Date"},
                {"DueDate", "Due Date"},
                {"PercentComplete", "Percent Complete"},
                {"AriaTaskStatus", "Task Status"},
                {"AriaTaskComments", "Comments"},
                {"AriaTaskCategory", "Category"},
                {"AriaTaskOwner", "Owner"},
                {"AriaTaskClientBarrierHours", "Client Barrier Hours"},
                {"AriaTaskProgress", "Progress"},
                {"AriaTaskActualDuration", "Actual Duration"},
                {"AriaTaskModifiedBy", "Modified By"},
                {"AriaTaskGroup", "Group"},
                {"AriaTaskEstimatedDuration", "Estimated Duration"},
                {"AriaTaskEstimatedStartDayNumber", "Estimated Start Day Number"},
                {"AriaTaskDeleted", "Deleted"},
                {"AriaTaskReminderDate", "Reminder Date"},
                {"AriaTaskLastDocumentAdded", "Last Document(s) Added"},
                {"AriaTaskLastDocumentRemoved", "Last Document(s) Removed"},
                {"ParentTaskNumber", "Parent Task"},
                {"TaskPredecessors", "Predecessor Tasks"},
                {"ChildTaskNumbers", "Child Tasks"},
            };

            var lastItem = new History() { TrackedInfo = new List<NameValuePair>() };
	        historyItems.ForEach(historyItem =>
	        {
	            historyItem.ActionUserText = DeriveActionUserText(historyItem);
				if (historyItem.TrackedInfo != null)
				{
					historyItem.TrackedInfo = historyItem.TrackedInfo.Where(
						delegate(NameValuePair x)
						{
							if (x.Name == "AriaTaskStatus" && x.Value != null)
							{
								x.Value = x.Value.SpaceIt();
							}
							if (x.Name == "AriaTaskModifiedBy" && x.Value != null)
							{
								GetModifyingUser(x);
							}
                            if ((x.Name == "AriaTaskLastDocumentAdded" || x.Name == "AriaTaskLastDocumentRemoved"))
                            {
                                if (x.Value == null)
                                {
                                    x.Name = "";
                                }
                                else
                                {
                                    x.Value = x.Value.Replace(";", ", ");
                                }
                            }
							return (includeFieldList.ContainsKey(x.Name));
							//return (true);
						})
						.Select(
							x =>
								new NameValuePair
								{
									Name = includeFieldList.FirstOrDefault(y => y.Key.Equals(x.Name)).Value,
									Value = x.Value,
								})
						.ToList();
				}

                if (historyItem.ActionType != null) historyItem.ActionType = historyItem.ActionType.Split('.').LastOrDefault();
	            
                if (historyItem.TrackedInfo != null)
	                historyItem.ActionDetail =
	                    string.Join("<br/>",
	                        historyItem.TrackedInfo.Where(x => !lastItem.TrackedInfo.Any(y => y.Name == x.Name && y.Value == x.Value)).Select(x => "<b>" + x.Name + "</b> : " + x.Value).ToArray());
	            lastItem = historyItem;
	        });

		    return historyItems.Where(x => x.TrackedInfo != null && x.TrackedInfo.Any()).ToList();
	    }


		/// <summary>
		/// Fetches the task history.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <param name="containerId">The container identifier.</param>
		/// <returns></returns>
	    public IEnumerable<History> FetchTaskHistory(Guid id, Guid containerId)
	    {
		    return GetTaskHistory(id, containerId);
	    }

	    internal string GetProfileDisplayName(Guid id)
	    {
		    if (id == Guid.Empty)
			    return null;

			var profle = _profileManager.FetchById(id);
		    if (profle != null)
		    {
			    return profle.DisplayName;
		    }
		    return null;

	    }
	}
}