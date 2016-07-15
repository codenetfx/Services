using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.InboundOrderProcessing.Domain;

namespace UL.Aria.Service.InboundOrderProcessing.Repository
{
    /// <summary>
    ///     Stores and retrieves <see cref="OrderMessage" /> objects from a SQL database.
    /// </summary>
    public class InboundOrderRepository : IInboundOrderRepository
    {
        /// <summary>
        ///     Creates the specified <see cref="OrderMessage" /> record.
        /// </summary>
        /// <param name="orderMessage">The order message.</param>
        public void Create(OrderMessage orderMessage)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeAddCommand(orderMessage, db))
            {
                db.ExecuteNonQuery(command);
            }
            foreach (var keyValuePair in orderMessage.Properties)
            {
                Create(orderMessage.Id.Value, keyValuePair);
            }
        }

        private void Create(Guid orderMessageId, KeyValuePair<string, object> entity)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeAddCommand(orderMessageId,entity, db))
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///     Fetches an <see cref="OrderMessage" /> by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public OrderMessage FetchById(Guid id)
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeFindById(id, db))
            {
                return ExecuteFetchOrder(db, command);
            }
        }

        private OrderMessage ExecuteFetchOrder(Database db, DbCommand command)
        {
            using (var reader = db.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    var message = ConstructOrderMessage(reader);
                    reader.NextResult();
                    while (reader.Read())
                    {
                        message.Properties.Add(ConstructOrderMessageProperty(reader));
                    }
                    return message;
                }
            }
            return null;
        }

        /// <summary>
        ///     Fetches the next <see cref="OrderMessage" /> for processing.
        /// </summary>
        /// <returns></returns>
        public OrderMessage FetchNextForProcessing()
        {
            var db = DatabaseFactory.CreateDatabase();
            using (var command = InitializeGetNext(db))
            {
                return ExecuteFetchOrder(db, command);
            }
        }

        private KeyValuePair<string, object> ConstructOrderMessageProperty(IDataReader reader)
        {
                var name = reader.GetString(reader.GetOrdinal("Name"));
                var value = reader.GetString(reader.GetOrdinal("Value"));
                return new KeyValuePair<string, object>(name, value);
         }

        private DbCommand InitializeFindById(Guid entityId, Database db)
        {
            DbCommand command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pOrderMessage_Get]";

            DbParameter parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "OrderMessageId";
            parameter.Value = entityId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        private OrderMessage ConstructOrderMessage(IDataReader reader)
        {
            var orderMessageId = reader.GetGuid(reader.GetOrdinal("OrderMessageId"));
            var externalMessageId = reader.GetString(reader.GetOrdinal("ExternalMessageId"));
            var originator = reader.GetString(reader.GetOrdinal("Originator"));
            var receiver = reader.GetString(reader.GetOrdinal("Receiver"));
            var body = reader.GetString(reader.GetOrdinal("Body"));

            return new OrderMessage
                {
                    Id = orderMessageId,
                    ExternalMessageId = externalMessageId,
                    Originator = originator,
                    Receiver = receiver,
                    Body = body
                };
        }

        private DbCommand InitializeAddCommand(OrderMessage entity, Database db)
        {
            DbCommand command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pOrderMessage_Insert]";

            DbParameter parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "OrderMessageId";
            parameter.Value = entity.Id;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "ExternalMessageId";
            parameter.Value = entity.ExternalMessageId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "Originator";
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = entity.Originator;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "Receiver";
            parameter.Value = entity.Receiver;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);


            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "Body";
            parameter.Value = entity.Body;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            return command;
        }

        private DbCommand InitializeAddCommand(Guid orderMessageId, KeyValuePair<string, object> entity, Database db)
        {
            DbCommand command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pOrderMessageProperty_Insert]";

            DbParameter parameter = command.CreateParameter();

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "OrderMessagePropertyId";
            parameter.Value = Guid.NewGuid();
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.Guid;
            parameter.ParameterName = "OrderMessageId";
            parameter.Value = orderMessageId;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "Name";
            parameter.Value = entity.Key;
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);

            parameter = command.CreateParameter();
            parameter.DbType = DbType.String;
            parameter.ParameterName = "Value";
            parameter.Direction = ParameterDirection.Input;
            parameter.Value = entity.Value.ToString();
            command.Parameters.Add(parameter);

            return command;
        }

        private DbCommand InitializeGetNext(Database db)
        {
            DbCommand command = db.DbProviderFactory.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "[dbo].[pOrderMessage_GetNextAndDelete]";

            return command;
        }
    }
}