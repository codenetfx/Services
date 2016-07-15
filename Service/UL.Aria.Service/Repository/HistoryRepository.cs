using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using UL.Aria.Service.Domain;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// History Repository
    /// </summary>
    public class HistoryRepository : IHistoryRepository
    {
        private const string HistoryNotFoundIdMessage = "History Item Not Found Id: '{0}'";

        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="HistoryRepository" /> class.
        /// </summary>
        /// <param name="transactionFactory">The transaction factory.</param>
        public HistoryRepository(ITransactionFactory transactionFactory)
        {
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Finds the history item by id.
        /// </summary>
        /// <param name="historyId">The history id.</param>
        /// <returns>History.</returns>
        public History FindById(Guid historyId)
        {
            History history;
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFetchById(db, historyId))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    history = ConstructHistory(reader, string.Format(HistoryNotFoundIdMessage, historyId));
                }
            }
            return history;
        }

        private static DbCommand InitializeFetchById(Database db, Guid historyId)
        {
            var command = db.GetStoredProcCommand("[dbo].[pHistory_GetById]");
            db.AddInParameter(command, "HistoryId", DbType.Guid, historyId);
            return command;
        }

        
        /// <summary>
        ///     Finds all history items for a given Entity, i.e. Project History, Order History, etc
        /// </summary>
        /// <returns>IList{History}.</returns>
        public IEnumerable<History> FindAllByEntityId(Guid entityId)
        {
            IList<History> historyList;
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFetchByEntityId(db, entityId))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    historyList = ConstructCompleteHistoryList(reader);
                }
            }
            return historyList;
        }

        private static DbCommand InitializeFetchByEntityId(Database db, Guid entityId)
        {
            var command = db.GetStoredProcCommand("[dbo].[pHistory_GetByEntityId]");
            db.AddInParameter(command, "EntityId", DbType.Guid, entityId);
            return command;
        }

        private static IList<History> ConstructCompleteHistoryList(IDataReader reader)
        {
            var historyList = new List<History>();
            while (reader.Read())
            {
                var history = ConstructHistory(reader);
                historyList.Add(history);
            }
            return historyList;
        }

        private static History ConstructHistory(IDataReader reader, string notFoundMessage)
        {
            if (!reader.Read()) throw new DatabaseItemNotFoundException(notFoundMessage);
            var history = ConstructHistory(reader);
            return history;
        }

        private static History ConstructHistory(IDataReader reader)
        {
            return new History
            {
                HistoryId = reader.GetValue<Guid>("HistoryId"),
                EntityId = reader.GetValue<Guid>("EntityId"),
                EntityType = reader.GetValue<string>("EntityType"),
                ActionDate = reader.GetValue<DateTime>("ActionDate"),
                ActionUserId = reader.GetValue<Guid>("ActionUserId"),
                ActionType = reader.GetValue<string>("ActionType"),
                ActionDetail = reader.GetValue<string>("ActionDetail"),
                ActionDetailEntityType = reader.GetValue<string>("ActionDetailEntityType"),
            };
        }

        /// <summary>
        ///     Creates the specified history item.
        /// </summary>
        /// <param name="history">The new history item.</param>
        /// <returns>HistoryId.</returns>
        public Guid Create(History history)
        {
            var id = Guid.Empty;
            using (var transactionScope = _transactionFactory.Create())
            {
                ExecuteCommand(InitializeInsertCommandHistory, null, history,
                    cmd => { id = (Guid)cmd.Parameters["@HistoryId"].Value; });
                transactionScope.Complete();
            }
            return id;
        }

        private static DbCommand InitializeInsertCommandHistory(Guid? id, History history, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pHistory_Insert]");
            db.AddInParameter(command, "HistoryId", DbType.Guid, history.HistoryId);
            db.AddInParameter(command, "EntityId", DbType.Guid, history.EntityId);
            db.AddInParameter(command, "EntityType", DbType.String, history.EntityType);
            db.AddInParameter(command, "ActionDate", DbType.DateTime2, history.ActionDate);
            db.AddInParameter(command, "ActionUserId", DbType.Guid, history.ActionUserId);
            db.AddInParameter(command, "ActionType", DbType.String, history.ActionType);
            db.AddInParameter(command, "ActionDetail", DbType.String, history.ActionDetail);
			db.AddInParameter(command, "ActionDetailEntityType", DbType.String, history.ActionDetailEntityType);
			return command;
        }


        /// <summary>
        ///     Updates the specified history item.
        /// </summary>
        /// <param name="history">The history item.</param>
        public int Update(History history)
        {
            int count;
            using (var transactionScope = _transactionFactory.Create())
            {
                count = ExecuteCommand(InitializeUpdateCommandHistory, history.HistoryId, history);
                if (count == 0)
                    throw new DatabaseItemNotFoundException(string.Format(HistoryNotFoundIdMessage, history.HistoryId));
                transactionScope.Complete();
            }
            return count;
        }

        private static DbCommand InitializeUpdateCommandHistory(Guid? id, History history, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pHistory_Update]");

            db.AddInParameter(command, "HistoryId", DbType.Guid, id);
            db.AddInParameter(command, "EntityId", DbType.Guid, history.EntityId);
            db.AddInParameter(command, "EntityType", DbType.String, history.EntityType);
            db.AddInParameter(command, "ActionDate", DbType.DateTime2, history.ActionDate);
            db.AddInParameter(command, "ActionUserId", DbType.Guid, history.ActionUserId);
            db.AddInParameter(command, "ActionType", DbType.String, history.ActionType);
            db.AddInParameter(command, "ActionDetail", DbType.String, history.ActionDetail);
			db.AddInParameter(command, "ActionDetailEntityType", DbType.String, history.ActionDetailEntityType);
			return command;
        }

        /// <summary>
        ///     Deletes the specified history item.
        /// </summary>
        /// <param name="historyId">The history id.</param>
        public void Delete(Guid historyId)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                var count = ExecuteCommand(InitializeRemoveCommand, null, historyId);

                if (count == 0)
                    throw new DatabaseItemNotFoundException(string.Format(HistoryNotFoundIdMessage, historyId));

                transactionScope.Complete();
            }
        }

        private static DbCommand InitializeRemoveCommand(Guid? unsedId, Guid id, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pHistory_Delete]");
            db.AddInParameter(command, "HistoryId", DbType.Guid, id);
            return command;
        }

        private static int ExecuteCommand<TEntity>(Func<Guid?, TEntity, Database, DbCommand> commandInitializer,
            Guid? id, TEntity entity,
            Action<DbCommand> afterExecute = null)
        {
            int count;
            var db = DatabaseFactory.CreateDatabase();
            var command = commandInitializer(id, entity, db);

            using (command)
            {
                count = db.ExecuteNonQuery(command);
            }
            if (afterExecute != null)
                afterExecute(command);

            return count;
        }

    }
}
