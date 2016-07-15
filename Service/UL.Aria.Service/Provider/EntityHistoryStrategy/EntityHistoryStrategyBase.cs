using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
    /// <summary>
    /// Class EntityHistoryStrategyBase.
    /// </summary>
    public abstract class EntityHistoryStrategyBase : IEntityHistoryStrategy
    {
        /// <summary>
        /// Creates the entity history.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <returns>EntityHistory.</returns>
        public abstract EntityHistory CreateEntityHistory(History history);

        /// <summary>
        /// Derives the tracked information.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <returns>List&lt;NameValuePair&gt;.</returns>
        public abstract List<NameValuePair> DeriveTrackedInfo(History history);

        /// <summary>
        /// De-serializes the specified history.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <returns>System.Object.</returns>
        protected object Deserialize(History history)
        {
            var type = Type.GetType(history.ActionDetailEntityType);
            var historyBytes = Encoding.ASCII.GetBytes(history.ActionDetail);
            var reader = XmlDictionaryReader.CreateTextReader(historyBytes, new XmlDictionaryReaderQuotas()
            {
                MaxStringContentLength = int.MaxValue
            });

            // ReSharper disable once AssignNullToNotNullAttribute
            var serializer = new DataContractSerializer(type);
            return serializer.ReadObject(reader, true);
        }

        /// <summary>
        /// Processes the specified history.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <param name="historyDeltas">The history deltas.</param>
        /// <param name="entityHistoryPrevious">The entity history previous.</param>
        /// <returns>EntityHistory.</returns>
        public EntityHistory Process(History history, ICollection<TaskDelta> historyDeltas,
            EntityHistory entityHistoryPrevious)
        {
            var entityHistoryCurrent = CreateEntityHistory(history);
            var taskDelta = CreateDeltas(history, entityHistoryPrevious, entityHistoryCurrent);
            if (taskDelta.MetaDeltaList.Count > 0)
            {
                historyDeltas.Add(taskDelta);
                entityHistoryPrevious = GetPreviousEntityHistory(entityHistoryPrevious, entityHistoryCurrent);
            }

            return entityHistoryPrevious;
        }

        /// <summary>
        /// Gets the previous entity history.
        /// </summary>
        /// <param name="entityHistoryPrevious">The entity history previous.</param>
        /// <param name="entityHistoryCurrent">The entity history current.</param>
        /// <returns>EntityHistory.</returns>
        protected virtual EntityHistory GetPreviousEntityHistory(EntityHistory entityHistoryPrevious,
            EntityHistory entityHistoryCurrent)
        {
            return entityHistoryCurrent;
        }

        /// <summary>
        /// Creates the deltas.
        /// </summary>
        /// <param name="history">The history.</param>
        /// <param name="entityHistoryPrevious">The entity history previous.</param>
        /// <param name="entityHistoryCurrent">The entity history current.</param>
        /// <returns>TaskDelta.</returns>
        protected virtual TaskDelta CreateDeltas(History history, EntityHistory entityHistoryPrevious,
            EntityHistory entityHistoryCurrent)
        {
            var taskDelta = new TaskDelta
            {
                Action = BuildAction(history.EntityType, history.ActionType.Split('.').LastOrDefault()),
                CreatedBy = entityHistoryCurrent.CreatedBy,
                CreatedDate = entityHistoryCurrent.CreatedDate,
            };
            ProcessFieldsForDeltas(taskDelta, history, entityHistoryPrevious, entityHistoryCurrent);
            return taskDelta;
        }

        /// <summary>
        /// Builds the action.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="action">The action.</param>
        /// <returns>System.String.</returns>
        public virtual string BuildAction(string entityType, string action)
        {
            return action;
        }

        /// <summary>
        /// Processes the fields for deltas.
        /// </summary>
        /// <param name="taskDelta">The task delta.</param>
        /// <param name="history">The history.</param>
        /// <param name="entityHistoryPrevious">The entity history previous.</param>
        /// <param name="entityHistoryCurrent">The entity history current.</param>
        protected abstract void ProcessFieldsForDeltas(TaskDelta taskDelta, History history, EntityHistory entityHistoryPrevious,
            EntityHistory entityHistoryCurrent);
    }
}