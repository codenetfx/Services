using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Domain.Entity;
using System.Data.SqlClient;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class OrderRepository
    /// </summary>
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderRepository" /> class.
        /// </summary>
        public OrderRepository()
            : this("OrderId")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderRepository" /> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the db id field.</param>
        protected OrderRepository(string dbIdFieldName)
            : base(dbIdFieldName)
        {
        }

        /// <summary>
        ///     Creates the specified order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Order id.</returns>
        public Guid Create(Order order)
        {
            var id = Guid.Empty;

            try
            {
                ExecuteCommand(InitializeAddCommandOrder, null, order,
                    cmd => { id = (Guid)cmd.Parameters["@OrderId"].Value; });
            }
            catch (SqlException exception)
            {
                if (exception.Message.Contains("Violation of UNIQUE KEY constraint"))
                    throw new DatabaseItemExistsException();
                throw;
            }

            return id;
        }

        /// <summary>
        ///     Finds the order by id.
        /// </summary>
        /// <param name="entityId">The order id.</param>
        /// <returns></returns>
        public override Order FindById(Guid entityId)
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFetchById(db, entityId))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    return ConstructOrder(reader);
                }
            }
        }

        /// <summary>
        ///     Finds the order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>Order.</returns>
        public Order FindByOrderNumber(string orderNumber)
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFetchById(db, orderNumber))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    return ConstructOrder(reader);
                }
            }
        }

        /// <summary>
        ///     Update the specified order.
        /// </summary>
        /// <param name="order">Order to update from the repository</param>
        /// <returns></returns>
        public override int Update(Order order)
        {
            var orderExisting = FindByOrderNumber(order.OrderNumber);
            return ExecuteCommand(InitializeUpdateCommandOrder, orderExisting.Id, order);
        }

        /// <summary>
        ///     Deletes the specified <see cref="Order" />.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
            Guard.IsNotEmptyGuid(id, "id");
            var count = ExecuteCommand(InitializeRemoveCommand, null, id);
            if (count == 0)
                throw new DatabaseItemNotFoundException();
        }

        private DbCommand InitializeAddCommandOrder(Guid? id, Order order, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pOrder_Insert]");

            db.AddOutParameter(command, "OrderId", DbType.Guid, 128);
            db.AddInParameter(command, "DateBooked", DbType.DateTime2, order.DateBooked);
            db.AddInParameter(command, "DateOrdered", DbType.DateTime2, order.DateOrdered);
            db.AddInParameter(command, "OrderNumber", DbType.String, order.OrderNumber);
            db.AddInParameter(command, "OriginalXmlParsed", DbType.String, order.OriginalXmlParsed);
            db.AddInParameter(command, "Status", DbType.String, order.Status);
            db.AddInParameter(command, "CompanyId", DbType.Guid, order.CompanyId);
            db.AddInParameter(command, "MessageId", DbType.String, order.MessageId);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, order.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, order.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, order.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, order.UpdatedDateTime);

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

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns></returns>
        public override IList<Order> FindAll()
        {
            throw new NotImplementedException();
        }

        private static DbCommand InitializeFetchById(Database db, Guid entityId)
        {
            var command = db.GetStoredProcCommand("[dbo].[pOrder_GetById]");
            db.AddInParameter(command, "OrderId", DbType.Guid, entityId);
            return command;
        }

        private DbCommand InitializeFetchById(Database db, string orderNumber)
        {
            var command = db.GetStoredProcCommand("[dbo].[pOrder_GetByOrderNumber]");
            db.AddInParameter(command, "OrderNumber", DbType.String, orderNumber);
            return command;
        }

        /// <summary>
        ///     Adds the specified order.
        /// </summary>
        /// <param name="entity">Order to add to the repository</param>
        /// <returns>Order's system ID</returns>
        public override void Add(Order entity)
        {
            throw new NotImplementedException();
        }

        private Order ConstructOrder(IDataReader reader)
        {
            Order result;
            if (reader.Read())
            {
                result = new Order
                {
                    Id = reader.GetValue<Guid>("OrderId"),
                    OriginalXmlParsed = reader.GetValue<string>("OriginalXmlParsed"),
                    DateBooked = reader.GetValue<DateTime?>("DateBooked"),
                    DateOrdered = reader.GetValue<DateTime?>("DateOrdered"),
                    OrderNumber = reader.GetValue<string>("OrderNumber"),
                    Status = reader.GetValue<string>("Status"),
                    CompanyId = reader.GetValue<Guid>("CompanyId"),
                    MessageId = reader.GetValue<string>("MessageId"),
                    CreatedById = reader.GetValue<Guid>("CreatedBy"),
                    CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                    UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                    UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
                };
            }
            else
            {
                throw new DatabaseItemNotFoundException("Incoming Order Not Found");
            }

            return result;
        }

        private static DbCommand InitializeRemoveCommand(Guid? unsedId, Guid id, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pOrder_Delete]");
            db.AddInParameter(command, "OrderId", DbType.Guid, id);
            return command;
        }

        private static DbCommand InitializeUpdateCommandOrder(Guid? id, Order order, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pOrder_Update]");

            db.AddInParameter(command, "OrderId", DbType.Guid, id);
            db.AddInParameter(command, "DateBooked", DbType.DateTime2, order.DateBooked);
            db.AddInParameter(command, "DateOrdered", DbType.DateTime2, order.DateOrdered);
            db.AddInParameter(command, "OrderNumber", DbType.String, order.OrderNumber);
            db.AddInParameter(command, "OriginalXmlParsed", DbType.String, order.OriginalXmlParsed);
            db.AddInParameter(command, "Status", DbType.String, order.Status);
            db.AddInParameter(command, "CompanyId", DbType.Guid, order.CompanyId);
            db.AddInParameter(command, "MessageId", DbType.String, order.MessageId);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, order.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, order.UpdatedDateTime);

            return command;
        }

        /// <summary>
        ///     Removes the specified order.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            Delete(entityId);
        }


        /// <summary>
        ///     Fetches the project lookups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lookup> FindOrderLookups()
        {
            var orders = new List<Lookup>();

            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeLookupCommandOrder(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        var lookup = ConstructOrderLookups(reader);
                        orders.Add(lookup);
                    }
                }
            }

            return orders;
        }

        private Lookup ConstructOrderLookups(IDataReader reader)
        {
            return new Lookup
            {
                Id = reader.GetValue<Guid>("OrderId"),
                Name = reader.GetValue<string>("OrderNumber")
            };
        }

        private static DbCommand InitializeLookupCommandOrder(Database db)
        {
            return db.GetStoredProcCommand("dbo.pOrder_GetOrderLookups");
        }

        /// <summary>
        /// Fetches the order lookups.
        /// </summary>
        /// <returns>
        /// A list of order lookups
        /// </returns>
        public IEnumerable<Lookup> FetchOrderLookups()
        {
            return FindOrderLookups();
        }
        
    }
}