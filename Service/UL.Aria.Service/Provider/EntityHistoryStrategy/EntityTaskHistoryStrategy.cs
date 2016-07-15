using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Linq;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
	/// <summary>
	/// Class EntityTaskHistoryStrategy. This class cannot be inherited.
	/// </summary>
	public sealed class EntityTaskHistoryStrategy : EntityHistoryStrategyBase
	{
		private readonly IMapperRegistry _mapperRegistry;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityTaskHistoryStrategy"/> class.
		/// </summary>
		/// <param name="mapperRegistry">The mapper registry.</param>
		public EntityTaskHistoryStrategy(IMapperRegistry mapperRegistry)
		{
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// Creates the entity history.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>EntityHistory.</returns>
		public override EntityHistory CreateEntityHistory(History history)
		{
			return CreateEntityTaskHistory(history);
		}

		private EntityTaskHistory CreateEntityTaskHistory(History history)
		{
			var taskDto = Deserialize(history) as TaskDto;
			var task = _mapperRegistry.Map<Task>(taskDto);
			return new EntityTaskHistory {Task = task, CreatedBy = task.ModifiedBy, CreatedDate = task.Modified};
		}

		/// <summary>
		/// Derives the tracked information.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>List&lt;NameValuePair&gt;.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public override List<NameValuePair> DeriveTrackedInfo(History history)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Processes the fields for deltas.
		/// </summary>
		/// <param name="taskDelta">The task delta.</param>
		/// <param name="history">The history.</param>
		/// <param name="entityHistoryPrevious">The entity history previous.</param>
		/// <param name="entityHistoryCurrent">The entity history current.</param>
		protected override void ProcessFieldsForDeltas(TaskDelta taskDelta, History history,
			EntityHistory entityHistoryPrevious,
			EntityHistory entityHistoryCurrent)
		{
			ProcessTaskFieldsForDeltas(taskDelta, ((EntityTaskHistory) entityHistoryPrevious).Task,
				((EntityTaskHistory) entityHistoryCurrent).Task);
		}

		internal static void ProcessTaskFieldsForDeltas(TaskDelta taskDelta, Task taskPrevious,
			Task taskCurrent)
		{
			const double tolerance = 0.001;
			MetaDelta metaDelta;
			if (taskPrevious.IsDeleted != taskCurrent.IsDeleted)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskDeleted",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.IsDeleted.ToString(),
					OrignalValue = taskPrevious.IsDeleted.ToString()
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
				if (taskCurrent.IsDeleted)
				{
					taskDelta.Action = "Deleted";
					return;
				}
			}
			if (String.Compare(taskPrevious.Title, taskCurrent.Title, StringComparison.OrdinalIgnoreCase) != 0)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "Title",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.Title,
					OrignalValue = taskPrevious.Title
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (taskDelta.Action == "Created" || Math.Abs(taskPrevious.PercentComplete - taskCurrent.PercentComplete) > tolerance)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "PercentComplete",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.PercentComplete.ToString(CultureInfo.InvariantCulture),
					OrignalValue = taskPrevious.PercentComplete.ToString(CultureInfo.InvariantCulture)
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (taskPrevious.StartDate != taskCurrent.StartDate)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "StartDate",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.StartDate.ToString(),
					OrignalValue = taskPrevious.StartDate.ToString()
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (taskPrevious.DueDate != taskCurrent.DueDate)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "DueDate",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.DueDate.ToString(),
					OrignalValue = taskPrevious.DueDate.ToString()
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (taskDelta.Action == "Created" || taskPrevious.Progress != taskCurrent.Progress)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskProgress",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.ProgressLabel,
					OrignalValue = taskPrevious.ProgressLabel
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (Math.Abs(taskPrevious.ActualDuration.GetValueOrDefault() - taskCurrent.ActualDuration.GetValueOrDefault()) >
			    tolerance)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskActualDuration",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.ActualDuration.ToString(),
					OrignalValue = taskPrevious.ActualDuration.ToString()
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (String.Compare(taskPrevious.TaskOwner, taskCurrent.TaskOwner, StringComparison.OrdinalIgnoreCase) != 0)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskOwner",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.TaskOwner,
					OrignalValue = taskPrevious.TaskOwner
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (String.Compare(taskPrevious.ModifiedBy, taskCurrent.ModifiedBy, StringComparison.OrdinalIgnoreCase) != 0)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskModifiedBy",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.ModifiedBy,
					OrignalValue = taskPrevious.ModifiedBy
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (String.Compare(taskPrevious.Comment, taskCurrent.Comment, StringComparison.OrdinalIgnoreCase) != 0)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskComments",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.Comment,
					OrignalValue = taskPrevious.Comment
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (
				Math.Abs(taskPrevious.EstimatedDuration.GetValueOrDefault() - taskCurrent.EstimatedDuration.GetValueOrDefault()) >
				tolerance)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskEstimatedDuration",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.EstimatedDuration.ToString(),
					OrignalValue = taskPrevious.EstimatedDuration.ToString()
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (taskDelta.Action == "Created" || taskPrevious.Status != taskCurrent.Status)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskStatus",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.StatusLabel,
					OrignalValue = taskPrevious.StatusLabel
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (taskPrevious.ReminderDate != taskCurrent.ReminderDate)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskReminderDate",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.ReminderDate.ToString(),
					OrignalValue = taskPrevious.ReminderDate.ToString()
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (String.Compare(taskPrevious.Category, taskCurrent.Category, StringComparison.OrdinalIgnoreCase) != 0)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskCategory",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.Category,
					OrignalValue = taskPrevious.Category
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (
				Math.Abs(taskPrevious.ClientBarrierHours.GetValueOrDefault() - taskCurrent.ClientBarrierHours.GetValueOrDefault()) >
				tolerance)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskClientBarrierHours",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.Category,
					OrignalValue = taskPrevious.Category
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (String.Compare(taskPrevious.Group, taskCurrent.Group, StringComparison.OrdinalIgnoreCase) != 0)
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskCategory",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.Group,
					OrignalValue = taskPrevious.Group
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
			if (taskPrevious.EstimatedStartDayNumber.GetValueOrDefault() !=
			    taskCurrent.EstimatedStartDayNumber.GetValueOrDefault())
			{
				metaDelta = new MetaDelta
				{
					AriaFieldName = "AriaTaskEstimatedStartDayNumber",
					Id = Guid.NewGuid(),
					ModifiedValue = taskCurrent.EstimatedStartDayNumber.ToString(),
					OrignalValue = taskPrevious.EstimatedStartDayNumber.ToString()
				};
				taskDelta.MetaDeltaList.Add(metaDelta);
			}
		    if (taskPrevious.ParentTaskNumber != taskCurrent.ParentTaskNumber)
		    {
		        metaDelta = new MetaDelta
		        {
		            AriaFieldName = "ParentTaskNumber",
		            Id = Guid.NewGuid(),
		            ModifiedValue = taskCurrent.ParentTaskNumber.ToString(),
		            OrignalValue = taskPrevious.ParentTaskNumber.ToString()
		        };
		        taskDelta.MetaDeltaList.Add(metaDelta);
		    }
            if (!taskPrevious.Predecessors.OrderBy(x => x.TaskNumber).Select(y => y.TaskNumber).ToList()
                .SequenceEqual(taskCurrent.Predecessors.OrderBy(x => x.TaskNumber).Select(y => y.TaskNumber).ToList()))
		    {
		        metaDelta = new MetaDelta
		        {
                    AriaFieldName = "TaskPredecessors",
                    Id = Guid.NewGuid(),
                    ModifiedValue = string.Join(",", taskCurrent.Predecessors.Select(x => x.TaskNumber).ToArray()),
                    OrignalValue = string.Join(",", taskPrevious.Predecessors.Select(x => x.TaskNumber).ToArray())
		        };
		        taskDelta.MetaDeltaList.Add(metaDelta);
		    }
            if (!taskPrevious.ChildTaskNumbers.OrderBy(x => x).ToList()
                .SequenceEqual(taskCurrent.ChildTaskNumbers.OrderBy(y => y).ToList()))
            {
                var currentTasks = taskCurrent.ChildTaskNumbers.Distinct().ToList();
                metaDelta = new MetaDelta
                {
                    AriaFieldName = "ChildTaskNumbers",
                    Id = Guid.NewGuid(),
                    ModifiedValue = string.Join(",", currentTasks.ToArray()),
                    OrignalValue = string.Join(",", taskPrevious.ChildTaskNumbers.ToArray())
                };
                taskDelta.MetaDeltaList.Add(metaDelta);
            }
            if (!string.IsNullOrEmpty(taskCurrent.LastDocumentAdded))
		    {
		        metaDelta = new MetaDelta
		        {
                    AriaFieldName = "AriaTaskLastDocumentAdded",
                    Id = Guid.NewGuid(),
                    ModifiedValue = taskCurrent.LastDocumentAdded
		        };
		        taskDelta.MetaDeltaList.Add(metaDelta);
		    }
            if (!string.IsNullOrEmpty(taskCurrent.LastDocumentRemoved))
            {
                metaDelta = new MetaDelta
                {
                    AriaFieldName = "AriaTaskLastDocumentRemoved",
                    Id = Guid.NewGuid(),
                    ModifiedValue = taskCurrent.LastDocumentRemoved
                };
                taskDelta.MetaDeltaList.Add(metaDelta);
            }
		}
	}
}