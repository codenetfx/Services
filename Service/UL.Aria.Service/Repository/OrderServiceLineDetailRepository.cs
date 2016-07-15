using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class OrderServiceLineDetailRepository.
    /// </summary>
    public class OrderServiceLineDetailRepository : RepositoryBase<OrderServiceLineDetail>,
        IOrderServiceLineDetailRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderServiceLineDetailRepository" /> class.
        /// </summary>
        public OrderServiceLineDetailRepository()
            : this("OrderServiceLineDetailId")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OrderServiceLineDetailRepository" /> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the database identifier field.</param>
        protected OrderServiceLineDetailRepository(string dbIdFieldName)
            : base(dbIdFieldName)
        {
        }

        /// <summary>
        ///     Creates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        public void Create(OrderServiceLineDetail orderServiceLineDetail)
        {
            Add(orderServiceLineDetail);
        }

        /// <summary>
        ///     Adds the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        public override void Add(OrderServiceLineDetail orderServiceLineDetail)
        {
            ExecuteCommand(InitializeAddCommandOrderServiceLineDetail, null, orderServiceLineDetail);
        }

        /// <summary>
        ///     Finds all order service line details.
        /// </summary>
        /// <returns>IList{OrderServiceLineDetail}.</returns>
        public override IList<OrderServiceLineDetail> FindAll()
        {
            return GetByCommandAll(InitializeFetchAllCommand);
        }

        /// <summary>
        ///     Finds the by identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>OrderServiceLineDetail.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override OrderServiceLineDetail FindById(Guid entityId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Finds a order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <returns>OrderServiceLineDetail.</returns>
        public OrderServiceLineDetail FindByIds(Guid orderId, string lineId)
        {
            var database = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFetchByIds(database, orderId, lineId))
            {
                using (var reader = database.ExecuteReader(command))
                {
                    if (reader.Read())
                        return ConstructOrderServiceLineDetail(reader);

                    throw new DatabaseItemNotFoundException("orderservicelinedetail not found", new Exception());
                }
            }
        }

        /// <summary>
        ///     Searches for order service line details by the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>OrderServiceLineDetailSearchResultSet.</returns>
        public OrderServiceLineDetailSearchResultSet Search(SearchCriteria searchCriteria)
        {
            var database = DatabaseFactory.CreateDatabase();
            var searchResultSet = new OrderServiceLineDetailSearchResultSet { SearchCriteria = searchCriteria };
            
            using (var command = InitializeSearchCommand(database, searchCriteria))
            {
                using (var reader = database.ExecuteReader(command))
                {
                    ConstructSearchResultSet(reader, searchResultSet);
                }
            }
            
            return searchResultSet;
        }

        /// <summary>
        ///     Updates the specified order service line detail.
        /// </summary>
        /// <param name="orderServiceLineDetail">The order service line detail.</param>
        /// <returns>System.Int32.</returns>
        public override int Update(OrderServiceLineDetail orderServiceLineDetail)
        {
            return ExecuteCommand(InitializeUpdateCommandOrderServiceLineDetail, null, orderServiceLineDetail);
        }

        /// <summary>
        ///     Deletes the order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="lineId">The line identifier.</param>
        /// <exception cref="DatabaseItemNotFoundException"></exception>
        public void Delete(Guid orderId, string lineId)
        {
            var count = ExecuteCommand(InitializeRemoveCommand, orderId, lineId);
            
            if (count == 0)
                throw new DatabaseItemNotFoundException();
        }

        /// <summary>
        ///     Removes the order service line detail by the specified order identifier and line identifier.
        /// </summary>
        /// <param name="entityId">The entity identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Remove(Guid entityId)
        {
            throw new NotImplementedException();
        }

        private DbCommand InitializeAddCommandOrderServiceLineDetail(Guid? entityId,
            OrderServiceLineDetail orderServiceLineDetail, Database database)
        {
            var command = database.GetStoredProcCommand("[dbo].[pOrderServiceLineDetail_Insert]");

            database.AddInParameter(command, "OrderId", DbType.Guid, orderServiceLineDetail.OrderId);
            database.AddInParameter(command, "LineId", DbType.String, orderServiceLineDetail.LineId);
            database.AddInParameter(command, "OriginalXml", DbType.String, orderServiceLineDetail.OriginalXml);
            database.AddInParameter(command, "LineXml", DbType.String, orderServiceLineDetail.LineXml);
            database.AddInParameter(command, "SenderId", DbType.Int16, orderServiceLineDetail.SenderId);
            database.AddInParameter(command, "CreatedBy", DbType.Guid, orderServiceLineDetail.CreatedById);
            database.AddInParameter(command, "UpdatedBy", DbType.Guid, orderServiceLineDetail.UpdatedById);
            database.AddInParameter(command, "CreatedDT", DbType.DateTime2, orderServiceLineDetail.CreatedDateTime);
            database.AddInParameter(command, "UpdatedDT", DbType.DateTime2, orderServiceLineDetail.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeFetchAllCommand(Database database)
        {
            var command = database.GetStoredProcCommand("[dbo].[pOrderServiceLineDetail_GetAll]");
            
            return command;
        }

        private static DbCommand InitializeFetchByIds(Database database, Guid orderId, string lineId)
        {
            var command = database.GetStoredProcCommand("[dbo].[pOrderServiceLineDetail_GetByIds]");
            
            database.AddInParameter(command, "OrderId", DbType.Guid, orderId);
            database.AddInParameter(command, "LineId", DbType.String, lineId);
            
            return command;
        }

        private static DbCommand InitializeSearchCommand(Database database, SearchCriteria searchCriteria)
        {
            var command = database.GetStoredProcCommand("[dbo].[pOrderServiceLineDetail_GetByOrderId]");

			//add first OrderId guid if exists in criteria
			List<string> values;
	        if (searchCriteria.Filters.TryGetValue(AssetFieldNames.AriaOrderId, out values))
	        {
		        if (values.Count > 0)
		        {
			        database.AddInParameter(command, "OrderId", DbType.Guid, values[0].ToGuid());
		        }
	        }
	        database.AddInParameter(command, "StartIndex", DbType.Int64, searchCriteria.StartIndex);
            database.AddInParameter(command, "EndIndex", DbType.Int64, searchCriteria.EndIndex);
            
            return command;
        }

        private static DbCommand InitializeUpdateCommandOrderServiceLineDetail(Guid? entityId,
            OrderServiceLineDetail orderServiceLineDetail, Database database)
        {
            var command = database.GetStoredProcCommand("[dbo].[pOrderServiceLineDetail_Update]");

            database.AddInParameter(command, "OrderId", DbType.Guid, orderServiceLineDetail.OrderId);
            database.AddInParameter(command, "LineId", DbType.String, orderServiceLineDetail.LineId);
            database.AddInParameter(command, "OriginalXml", DbType.String, orderServiceLineDetail.OriginalXml);
            database.AddInParameter(command, "LineXml", DbType.String, orderServiceLineDetail.LineXml);
            database.AddInParameter(command, "SenderId", DbType.Int16, orderServiceLineDetail.SenderId);
            database.AddInParameter(command, "UpdatedBy", DbType.Guid, orderServiceLineDetail.UpdatedById);
            database.AddInParameter(command, "UpdatedDT", DbType.DateTime2, orderServiceLineDetail.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeRemoveCommand(Guid? orderId, string lineId, Database database)
        {
            var command = database.GetStoredProcCommand("[dbo].[pOrderServiceLineDetail_Delete]");
            
            database.AddInParameter(command, "OrderId", DbType.Guid, orderId);
            database.AddInParameter(command, "LineId", DbType.String, lineId);
            
            return command;
        }

        private static OrderServiceLineDetailSearchResult ConstructSearchResult(IDataReader reader)
        {
            return new OrderServiceLineDetailSearchResult
            {
                OrderServiceLineDetail = ConstructOrderServiceLineDetail(reader)
            };
        }

        private static OrderServiceLineDetail ConstructOrderServiceLineDetail(IDataReader reader)
        {
            return new OrderServiceLineDetail
            {
                OrderId = reader.GetValue<Guid>("OrderId"),
                LineId = reader.GetValue<string>("LineId"),
                GroupName = reader.GetValue<string>("GroupName"),
                OriginalXml = reader.GetValue<string>("OriginalXml"),
                LineXml = reader.GetValue<string>("LineXml"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedDT"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedDT")
            };
        }

        private static int ExecuteCommand<TEntity>(Func<Guid?, TEntity, Database, DbCommand> commandInitializer,
            Guid? id, TEntity entity,
            Action<DbCommand> afterExecute = null)
        {
            int count;
            var database = DatabaseFactory.CreateDatabase();
            var command = commandInitializer(id, entity, database);

            using (command)
            {
                count = database.ExecuteNonQuery(command);
            }

            if (afterExecute != null)
                afterExecute(command);

            return count;
        }

        private static List<OrderServiceLineDetail> GetByCommandAll(Func<Database, DbCommand> initializeCommand)
        {
            var database = DatabaseFactory.CreateDatabase();
            var results = new List<OrderServiceLineDetail>();

            using (var command = initializeCommand(database))
            {
                using (var reader = database.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructOrderServiceLineDetail(reader));
                    }
                }
            }
            
            return results;
        }

        private void ConstructSearchResultSet(IDataReader reader, SearchResultSetBase<OrderServiceLineDetailSearchResult> searchResultSet)
        {
            searchResultSet.Summary = new SearchSummary();
            long? start = null;
            long? end = null;
            searchResultSet.Summary.TotalResults = 0;
            
            while (reader.Read())
            {
                searchResultSet.Summary.TotalResults = reader.GetValue<long>("TotalRows");
                var row = reader.GetValue<long>("RowNumber") - 1;
                start = start ?? row;
                end = end ?? row;
                end = row > end ? row : end;
                start = row < start ? row : start;
                searchResultSet.Results.Add(ConstructSearchResult(reader));
            }
            
            searchResultSet.Summary.StartIndex = start ?? -1;
            searchResultSet.Summary.EndIndex = end ?? -1;
        }
    }
}