using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using Guard = UL.Enterprise.Foundation.Framework.Guard;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class IncomingOrderRepository
    /// </summary>
    public class IncomingOrderRepository : RepositoryBase<IncomingOrder>, IIncomingOrderRepository
    {

        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IncomingOrderRepository" /> class.
        /// </summary>
        public IncomingOrderRepository(ITransactionFactory transactionFactory)
            : this("IncomingOrderId")
        {
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="IncomingOrderRepository" /> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the db id field.</param>
        protected IncomingOrderRepository(string dbIdFieldName)
            : base(dbIdFieldName)
        {
        }

        /// <summary>
        ///     Creates the specified incoming order.
        /// </summary>
        /// <param name="incomingOrder">The incoming order.</param>
        /// <returns>Incoming order id.</returns>
        public Guid Create(IncomingOrder incomingOrder)
        {
	        // ReSharper disable once PossibleInvalidOperationException
            Guid id = incomingOrder.Id.Value;

            try
            {
                ExecuteCommand(InitializeAddCommandIncomingOrder, null, incomingOrder);
            }
            catch (SqlException exception)
            {
                if (exception.Message.Contains("Violation of UNIQUE KEY constraint"))
                    throw new DatabaseItemExistsException();
                throw;
            }

            incomingOrder.IncomingOrderContact.IncomingOrderId = id;
            incomingOrder.IncomingOrderContact.ContactRoleId = ContactRoleEnum.Customer;
            ExecuteCommand(InitializeAddCommandIncomingOrderContact, incomingOrder.IncomingOrderContact.Id,
                incomingOrder.IncomingOrderContact);

            incomingOrder.BillToContact.IncomingOrderId = id;
            incomingOrder.BillToContact.ContactRoleId = ContactRoleEnum.BillTo;
            ExecuteCommand(InitializeAddCommandIncomingOrderContact, incomingOrder.BillToContact.Id,
                incomingOrder.BillToContact);

            incomingOrder.ShipToContact.IncomingOrderId = id;
            incomingOrder.ShipToContact.ContactRoleId = ContactRoleEnum.ShipTo;
            ExecuteCommand(InitializeAddCommandIncomingOrderContact, incomingOrder.ShipToContact.Id,
                incomingOrder.ShipToContact);

            incomingOrder.IncomingOrderCustomer.IncomingOrderId = id;
            ExecuteCommand(InitializeAddCommandIncomingOrderCustomer, incomingOrder.IncomingOrderCustomer.Id,
                incomingOrder.IncomingOrderCustomer);

            foreach (IncomingOrderServiceLine incomingOrderServiceLine in incomingOrder.ServiceLines)
            {
                incomingOrderServiceLine.IncomingOrderId = id;
                ExecuteCommand(InitializeAddCommandIncomingOrderServiceLine, incomingOrderServiceLine.Id,
                    incomingOrderServiceLine);
            }

            return id;
        }

        /// <summary>
        ///     Finds the incoming order by id.
        /// </summary>
        /// <param name="entityId">The incoming order id.</param>
        /// <returns></returns>
        public override IncomingOrder FindById(Guid entityId)
        {
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand command = InitializeFetchById(db, entityId))
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    return ConstructCompleteIncomingOrder(reader);
                }
            }
        }

        /// <summary>
        ///     Finds the incoming order by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>IncomingOrder.</returns>
        public IncomingOrder FindByOrderNumber(string orderNumber)
        {
            Database db = DatabaseFactory.CreateDatabase();

            using (DbCommand command = InitializeFetchById(db, orderNumber))
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    return ConstructCompleteIncomingOrder(reader);
                }
            }
        }

        /// <summary>
        ///     Finds the by service line id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns></returns>
        public IncomingOrder FindByServiceLineId(Guid entityId)
        {
            Database db = DatabaseFactory.CreateDatabase();
            Guid incomingOrderId;
            using (DbCommand command1 = InitializeFetchByIdForServiceLine(db, entityId))
            {
                using (IDataReader reader1 = db.ExecuteReader(command1))
                {
                    if (!reader1.Read())
                    {
                        throw new DatabaseItemNotFoundException(string.Format("entityId = {0}", entityId));
                    }

                    incomingOrderId = reader1.GetValue<Guid>("IncomingOrderId");
                }
            }

            using (DbCommand command2 = InitializeFetchById(db, incomingOrderId))
            {
                using (IDataReader reader2 = db.ExecuteReader(command2))
                {
                    return ConstructCompleteIncomingOrder(reader2);
                }
            }
        }

        /// <summary>
        ///     Update the specified incoming order.
        /// </summary>
        /// <param name="incomingOrder">Incoming order to update from the repository</param>
        /// <returns></returns>
        public override int Update(IncomingOrder incomingOrder)
        {
            IncomingOrder incomingOrderExisting = FindByOrderNumber(incomingOrder.OrderNumber);
            ExecuteCommand(InitializeUpdateCommandIncomingOrderContact, incomingOrderExisting.IncomingOrderContact.Id,
                incomingOrder.IncomingOrderContact);
            ExecuteCommand(InitializeUpdateCommandIncomingOrderContact, incomingOrderExisting.BillToContact.Id,
                incomingOrder.BillToContact);
            ExecuteCommand(InitializeUpdateCommandIncomingOrderContact, incomingOrderExisting.ShipToContact.Id,
                incomingOrder.ShipToContact);
            ExecuteCommand(InitializeUpdateCommandIncomingOrderCustomer, incomingOrderExisting.IncomingOrderCustomer.Id,
                incomingOrder.IncomingOrderCustomer);
            ExecuteCommand(InitializeDeleteCommandIncomingOrderServiceLineByOrderId, incomingOrderExisting.Id,
                incomingOrderExisting);

            foreach (IncomingOrderServiceLine incomingOrderServiceLine in incomingOrder.ServiceLines)
            {
	            // ReSharper disable once PossibleInvalidOperationException
                incomingOrderServiceLine.IncomingOrderId = incomingOrderExisting.Id.Value;
                ExecuteCommand(InitializeAddCommandIncomingOrderServiceLine, incomingOrderServiceLine.Id,
                    incomingOrderServiceLine);
            }

            return ExecuteCommand(InitializeUpdateCommandIncomingOrder, incomingOrderExisting.Id, incomingOrder);
        }

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="contact">The contact.</param>
        public void UpdateAllContactsForExternalId(string externalId, IncomingOrderContact contact)
        {
            var db = DatabaseFactory.CreateDatabase();
            var command = InitializeUpdateCommandContactByExternalId(externalId, contact, db);

            using (command)
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        public void UpdateContact(IncomingOrderContact contact)
        {
            var db = DatabaseFactory.CreateDatabase();
            var command = InitializeUpdateCommandIncomingOrderContact(contact.Id, contact, db);

            using (command)
            {
                db.ExecuteNonQuery(command);
            }
        }

		/// <summary>
		/// Creates the contact.
		/// </summary>
		/// <param name="contact">The contact.</param>
		/// <returns>Guid.</returns>
		public Guid CreateContact(IncomingOrderContact contact)
		{
			try
			{
				ExecuteCommand(InitializeAddCommandIncomingOrderContact, contact.Id,
					contact);
			}
			catch (SqlException exception)
			{
				if (exception.Message.Contains("Violation of UNIQUE KEY constraint"))
					throw new DatabaseItemExistsException();
				throw;
			}

			return contact.Id.GetValueOrDefault();
		}

        /// <summary>
        /// Updates all customers for external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="customer">The customer.</param>
        public void UpdateAllCustomersForExternalId(string externalId, IncomingOrderCustomer customer)
        {
            var db = DatabaseFactory.CreateDatabase();
            var command = InitializeUpdateCommandCustomerByExternalId(externalId, customer, db);

            using (command)
            {
                db.ExecuteNonQuery(command);
            }
        }

        private static DbCommand InitializeUpdateCommandContactByExternalId(string externalId, IncomingOrderContact projectContact,
           Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pIncomingOrderContact_UpdateByExternalId]");

            db.AddInParameter(command, "FullName", DbType.String, projectContact.FullName);
            db.AddInParameter(command, "Title", DbType.String, projectContact.Title);
            db.AddInParameter(command, "Email", DbType.String, projectContact.Email);
            db.AddInParameter(command, "Address", DbType.String, projectContact.Address);
            db.AddInParameter(command, "Phone", DbType.String, projectContact.Phone);
            db.AddInParameter(command, "State", DbType.String, projectContact.State);
            db.AddInParameter(command, "Country", DbType.String, projectContact.Country);
            db.AddInParameter(command, "City", DbType.String, projectContact.City);
            db.AddInParameter(command, "Province", DbType.String, projectContact.Province);
            db.AddInParameter(command, "PostalCode", DbType.String, projectContact.PostalCode);
            db.AddInParameter(command, "ExternalId", DbType.String, externalId);
            db.AddInParameter(command, "CompanyName", DbType.String, projectContact.CompanyName);
            db.AddInParameter(command, "SubscriberNumber", DbType.String, projectContact.SubscriberNumber);
            db.AddInParameter(command, "PartySiteNumber", DbType.String, projectContact.PartySiteNumber);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, projectContact.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, projectContact.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeUpdateCommandCustomerByExternalId(string externalId, IncomingOrderCustomer projectCustomer,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pIncomingOrderCustomer_UpdateByExternalId]");

            db.AddInParameter(command, "DUNS", DbType.String, projectCustomer.DUNS);
            db.AddInParameter(command, "Name", DbType.String, projectCustomer.Name);
            db.AddInParameter(command, "ExternalId", DbType.String, externalId);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, projectCustomer.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, projectCustomer.UpdatedDateTime);

            return command;
        }

        /// <summary>
        ///     Deletes the specified <see cref="IncomingOrder" />.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
            Guard.IsNotEmptyGuid(id, "id");
            int count = ExecuteCommand(InitializeRemoveCommand, null, id);
            if (count == 0)
                throw new DatabaseItemNotFoundException();
        }

        /// <summary>
        ///     Deletes the service line <see cref="IncomingOrderServiceLine" />.
        /// </summary>
        /// <param name="id">The service line id.</param>
        public void DeleteServiceLine(Guid id)
        {
            Guard.IsNotEmptyGuid(id, "id");
            int count = ExecuteCommand(InitializeRemoveServiceLineCommand, null, id);
            if (count == 0)
                throw new DatabaseItemNotFoundException();
        }

        /// <summary>
        ///     Searches for <see cref="IncomingOrder" /> objects using the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns></returns>
        public IncomingOrderSearchResultSet Search(SearchCriteria searchCriteria)
        {
            Database db = DatabaseFactory.CreateDatabase();

            var set = new IncomingOrderSearchResultSet {SearchCriteria = searchCriteria};
            using (DbCommand command = InitializeSearchCommand(db, searchCriteria))
            {
                using (IDataReader reader = db.ExecuteReader(command))
                {
                    ConstructSearchResultSet(reader, set);
                    ConstructServiceLineRefinementItemSource(reader, searchCriteria, set,
                        AssetFieldNames.AriaProjectIndustryCode);
                    ConstructServiceLineRefinementItemSource(reader, searchCriteria, set,
                        AssetFieldNames.AriaProjectServiceCode);
                    ConstructServiceLineRefinementItemSource(reader, searchCriteria, set,
                        AssetFieldNames.AriaProjectLocationName);
                    ConstructServiceLineRefinementItemSource(reader, searchCriteria, set,
                        AssetFieldNames.AriaCustomerCountry);
                    ConstructServiceLineRefinementItemSource(reader, searchCriteria, set,
                        AssetFieldNames.AriaCustomerState);
                }
            }
            return set;
        }

        /// <summary>
        ///     Adds the service line.
        /// </summary>
        /// <param name="serviceLine">The service line.</param>
        public void AddServiceLine(IncomingOrderServiceLine serviceLine)
        {
            ExecuteCommand(InitializeAddCommandIncomingOrderServiceLine, null, serviceLine);
        }

        private static void ConstructServiceLineRefinementItemSource(IDataReader rdr, SearchCriteria searchCriteria,
            IncomingOrderSearchResultSet set, string refinerFieldName)
        {
            rdr.NextResult();
            if (searchCriteria.Refiners.Contains(refinerFieldName))
            {
                var items = new List<IRefinementItem>();
                while (rdr.Read())
                {
                    var item = new RefinementItem {Count = rdr.GetInt32(0), Value = rdr.GetValue<string>("FilterValue")};
                    if (string.IsNullOrWhiteSpace(item.Value))
                        continue;
                    item.Token = item.Value;
                    item.Name = item.Value;
                    items.Add(item);
                }
                if (items.Count > 0)
                    set.RefinerResults.Add(refinerFieldName, items);
            }
        }

        private DbCommand InitializeAddCommandIncomingOrder(Guid? id, IncomingOrder incomingOrder, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrder_Insert]");

            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, incomingOrder.Id);
            db.AddInParameter(command, "BusinessUnit", DbType.String, incomingOrder.BusinessUnit);
            db.AddInParameter(command, "CreationDate", DbType.DateTime2, incomingOrder.CreationDate);
            db.AddInParameter(command, "CustomerPo", DbType.String, incomingOrder.CustomerPo);
            db.AddInParameter(command, "CustomerRequestedDate", DbType.DateTime2, incomingOrder.CustomerRequestedDate);
            db.AddInParameter(command, "DateBooked", DbType.DateTime2, incomingOrder.DateBooked);
            db.AddInParameter(command, "DateOrdered", DbType.DateTime2, incomingOrder.DateOrdered);
            db.AddInParameter(command, "ExternalProjectId", DbType.String, incomingOrder.ExternalProjectId);
            db.AddInParameter(command, "LastUpdateDate", DbType.DateTime2, incomingOrder.LastUpdateDate);
            db.AddInParameter(command, "OrderNumber", DbType.String, incomingOrder.OrderNumber);
            db.AddInParameter(command, "OrderType", DbType.String, incomingOrder.OrderType);
            db.AddInParameter(command, "OriginalXmlParsed", DbType.String, incomingOrder.OriginalXmlParsed);
            db.AddInParameter(command, "ProjectHeaderStatus", DbType.String, incomingOrder.ProjectHeaderStatus);
            db.AddInParameter(command, "ProjectName", DbType.String, incomingOrder.ProjectName);
            db.AddInParameter(command, "ProjectNumber", DbType.String, incomingOrder.ProjectNumber);
            db.AddInParameter(command, "Status", DbType.String, incomingOrder.Status);
            db.AddInParameter(command, "CompanyId", DbType.Guid, incomingOrder.CompanyId);
            db.AddInParameter(command, "WorkOrderBusinessComponentId", DbType.String,
                incomingOrder.WorkOrderBusinessComponentId);
            db.AddInParameter(command, "WorkOrderId", DbType.String, incomingOrder.WorkOrderId);
			db.AddInParameter(command, "MessageId", DbType.String, incomingOrder.MessageId);
			db.AddInParameter(command, "QuoteNumber", DbType.String, incomingOrder.QuoteNo);
			db.AddInParameter(command, "TotalOrderPrice", DbType.Decimal, incomingOrder.TotalOrderPrice);
			db.AddInParameter(command, "Currency", DbType.String, incomingOrder.Currency);
			db.AddInParameter(command, "CreatedBy", DbType.Guid, incomingOrder.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, incomingOrder.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, incomingOrder.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, incomingOrder.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializeAddCommandIncomingOrderContact(Guid? id, IncomingOrderContact incomingOrderContact,
            Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderContact_Insert]");

            db.AddInParameter(command, "IncomingOrderContactId", DbType.Guid, incomingOrderContact.Id);
            db.AddInParameter(command, "FullName", DbType.String, incomingOrderContact.FullName);
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, incomingOrderContact.IncomingOrderId);
            db.AddInParameter(command, "ContactRoleId", DbType.Int16, (Int16) incomingOrderContact.ContactRoleId);
            db.AddInParameter(command, "Title", DbType.String, incomingOrderContact.Title);
            db.AddInParameter(command, "Email", DbType.String, incomingOrderContact.Email); db.AddInParameter(command, "Address", DbType.String, incomingOrderContact.Address);
            db.AddInParameter(command, "City", DbType.String, incomingOrderContact.City);
            db.AddInParameter(command, "State", DbType.String, incomingOrderContact.State);
            db.AddInParameter(command, "Province", DbType.String, incomingOrderContact.Province);
            db.AddInParameter(command, "Country", DbType.String, incomingOrderContact.Country);
            db.AddInParameter(command, "PostalCode", DbType.String, incomingOrderContact.PostalCode);
            db.AddInParameter(command, "Phone", DbType.String, incomingOrderContact.Phone);
            db.AddInParameter(command, "CompanyName", DbType.String, incomingOrderContact.CompanyName);
            db.AddInParameter(command, "ExternalId", DbType.String, incomingOrderContact.ExternalId);
            db.AddInParameter(command, "SubscriberNumber", DbType.String, incomingOrderContact.SubscriberNumber);
            db.AddInParameter(command, "PartySiteNumber", DbType.String, incomingOrderContact.PartySiteNumber);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, incomingOrderContact.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, incomingOrderContact.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, incomingOrderContact.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, incomingOrderContact.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializeAddCommandIncomingOrderCustomer(Guid? id,
            IncomingOrderCustomer incomingOrderCustomer,
            Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderCustomer_Insert]");

            db.AddInParameter(command, "IncomingOrderCustomerId", DbType.Guid, incomingOrderCustomer.Id);
            db.AddInParameter(command, "DUNS", DbType.String, incomingOrderCustomer.DUNS);
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, incomingOrderCustomer.IncomingOrderId);
            db.AddInParameter(command, "Name", DbType.String, incomingOrderCustomer.Name);
            db.AddInParameter(command, "ProjectName", DbType.String, incomingOrderCustomer.ProjectName);
            db.AddInParameter(command, "ExternalId", DbType.String, incomingOrderCustomer.ExternalId);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, incomingOrderCustomer.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, incomingOrderCustomer.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, incomingOrderCustomer.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, incomingOrderCustomer.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializeAddCommandIncomingOrderServiceLine(Guid? id, IncomingOrderServiceLine serviceLine,
            Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderServiceLine_Insert]");

            db.AddInParameter(command, "IncomingOrderServiceLineId", DbType.Guid, serviceLine.Id);
            db.AddInParameter(command, "AllowChargesFromOtherOperatingUnits", DbType.String,
                serviceLine.AllowChargesFromOtherOperatingUnits);
            db.AddInParameter(command, "BillableExpenses", DbType.String, serviceLine.BillableExpenses);
            db.AddInParameter(command, "ConfigurationId", DbType.String, serviceLine.ConfigurationId);
            db.AddInParameter(command, "CustomerModelNumber", DbType.String, serviceLine.CustomerModelNumber);
            db.AddInParameter(command, "ExternalId", DbType.String, serviceLine.ExternalId);
            db.AddInParameter(command, "FulfillmentMethodCode", DbType.String, serviceLine.FulfillmentMethodCode);
            db.AddInParameter(command, "FulfillmentSet", DbType.String, serviceLine.FulfillmentSet);
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, serviceLine.IncomingOrderId);
            db.AddInParameter(command, "ItemCategories", DbType.String, serviceLine.ItemCategories);
            db.AddInParameter(command, "LineNumber", DbType.String, serviceLine.LineNumber);
            db.AddInParameter(command, "Name", DbType.String, serviceLine.Name);
            db.AddInParameter(command, "ParentExternalId", DbType.String, serviceLine.ParentExternalId);
            db.AddInParameter(command, "PromiseDate", DbType.DateTime2, serviceLine.PromiseDate);
            db.AddInParameter(command, "RequestDate", DbType.DateTime2, serviceLine.RequestDate);
            db.AddInParameter(command, "StartDate", DbType.DateTime2, serviceLine.StartDate);
            db.AddInParameter(command, "Status", DbType.String, serviceLine.Status);
            db.AddInParameter(command, "TypeCode", DbType.String, serviceLine.TypeCode);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, serviceLine.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, serviceLine.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, serviceLine.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, serviceLine.UpdatedDateTime);

            db.AddInParameter(command, "Description", DbType.String, serviceLine.Description);
            db.AddInParameter(command, "Billable", DbType.String, serviceLine.Billable);
            db.AddInParameter(command, "Program", DbType.String, serviceLine.Program);
            db.AddInParameter(command, "Category", DbType.String, serviceLine.Category);
            db.AddInParameter(command, "SubCategory", DbType.String, serviceLine.SubCategory);
            db.AddInParameter(command, "Segment", DbType.String, serviceLine.Segment);
            db.AddInParameter(command, "ClientDetailService", DbType.String, serviceLine.ClientDetailService);

            db.AddInParameter(command, "WorkOrderLineBusinessComponentId", DbType.String,
                serviceLine.WorkOrderLineBusinessComponentId);
            db.AddInParameter(command, "WorkOrderLineId", DbType.String, serviceLine.WorkOrderLineId);
            db.AddInParameter(command, "ApplicationObjectKeyId", DbType.String, serviceLine.ApplicationObjectKeyId);
            db.AddInParameter(command, "ServiceCode", DbType.String, serviceLine.ServiceCode);
            db.AddInParameter(command, "IndustryCode", DbType.String, serviceLine.IndustryCode);
            db.AddInParameter(command, "LocationName", DbType.String, serviceLine.LocationName);
			db.AddInParameter(command, "LocationCode", DbType.String, serviceLine.LocationCode);
			db.AddInParameter(command, "Hold", DbType.String, serviceLine.Hold);
			db.AddInParameter(command, "ServiceSegment", DbType.String, serviceLine.ServiceSegment);
			db.AddInParameter(command, "ServiceSegmentDescription", DbType.String, serviceLine.ServiceSegmentDescription);
			db.AddInParameter(command, "ServiceCategory", DbType.String, serviceLine.ServiceCategory);
			db.AddInParameter(command, "ServiceCategoryDescription", DbType.String, serviceLine.ServiceCategoryDescription);
			db.AddInParameter(command, "ServiceSubCategory", DbType.String, serviceLine.ServiceSubCategory);
			db.AddInParameter(command, "ServiceSubCategoryDescription", DbType.String, serviceLine.ServiceSubCategoryDescription);
			db.AddInParameter(command, "ServiceProgram", DbType.String, serviceLine.ServiceProgram);
			db.AddInParameter(command, "ServiceProgramDescription", DbType.String, serviceLine.ServiceProgramDescription);
			db.AddInParameter(command, "DetailedService", DbType.String, serviceLine.DetailedService);
			db.AddInParameter(command, "DetailedServiceDescription", DbType.String, serviceLine.DetailedServiceDescription);
			db.AddInParameter(command, "Industry", DbType.String, serviceLine.Industry);
			db.AddInParameter(command, "IndustryDescription", DbType.String, serviceLine.IndustryDescription);
			db.AddInParameter(command, "IndustryCategory", DbType.String, serviceLine.IndustryCategory);
			db.AddInParameter(command, "IndustryCategoryDescription", DbType.String, serviceLine.IndustryCategoryDescription);
			db.AddInParameter(command, "IndustrySubCategory", DbType.String, serviceLine.IndustrySubCategory);
			db.AddInParameter(command, "IndustrySubCategoryDescription", DbType.String, serviceLine.IndustrySubCategoryDescription);
			db.AddInParameter(command, "ProductGroup", DbType.String, serviceLine.ProductGroup);
			db.AddInParameter(command, "ProductGroupDescription", DbType.String, serviceLine.ProductGroupDescription);
			db.AddInParameter(command, "ProductType", DbType.String, serviceLine.ProductType);
			db.AddInParameter(command, "ProductTypeDescription", DbType.String, serviceLine.ProductTypeDescription);
			db.AddInParameter(command, "Price", DbType.Decimal, serviceLine.Price);
			db.AddInParameter(command, "Currency", DbType.String, serviceLine.Currency);
			return command;
        }

        private static int ExecuteCommand<TEntity>(Func<Guid?, TEntity, Database, DbCommand> commandInitializer,
            Guid? id, TEntity entity,
            Action<DbCommand> afterExecute = null)
        {
            int count;
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand command = commandInitializer(id, entity, db);

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
        public override IList<IncomingOrder> FindAll()
        {
            throw new NotImplementedException();
        }

        private static DbCommand InitializeFetchById(Database db, Guid entityId)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrder_GetById]");
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, entityId);
            return command;
        }

        private DbCommand InitializeFetchById(Database db, string orderNumber)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrder_GetByOrderNumber]");
            db.AddInParameter(command, "OrderNumber", DbType.String, orderNumber);
            return command;
        }


        private DbCommand InitializeFetchByIdForServiceLine(Database db, Guid entityId)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderServiceLine_Get]");
            db.AddInParameter(command, "IncomingOrderServiceLineId", DbType.Guid, entityId);
            return command;
        }

        /// <summary>
        ///     Adds the specified incoming order.
        /// </summary>
        /// <param name="entity">Incoming order to add to the repository</param>
        /// <returns>Incoming order's system ID</returns>
        public override void Add(IncomingOrder entity)
        {
            throw new NotImplementedException();
        }

        private IncomingOrder ConstructCompleteIncomingOrder(IDataReader reader)
        {
            IncomingOrder result;
            Guid orderId;
            if (reader.Read())
            {
                result = ConstructIncomingOrder(reader);
            }
            else
            {
                throw new DatabaseItemNotFoundException("Incoming Order Not Found");
            }
            reader.NextResult();

            if (reader.Read())
            {
                IncomingOrderCustomer customer = ConstructIncomingCustomer(reader, out orderId);
                result.IncomingOrderCustomer = customer;
            }
            reader.NextResult();
            if (reader.Read())
            {
                IncomingOrderContact contact = ConstructIncomingContact(reader, out orderId);
                result.IncomingOrderContact = contact;
            }
            reader.NextResult();
            while (reader.Read())
            {
                IncomingOrderServiceLine serviceLine = ConstructIncomingOrderServiceLine(reader, out orderId);
                result.ServiceLines.Add(serviceLine);
            }
            reader.NextResult();
            if (reader.Read())
            {
                IncomingOrderContact contact = ConstructIncomingContact(reader, out orderId);
                result.BillToContact = contact;
            }
            reader.NextResult();
            if (reader.Read())
            {
                IncomingOrderContact contact = ConstructIncomingContact(reader, out orderId);
                result.ShipToContact = contact;
            }
            return result;
        }

        private void ConstructSearchResultSet(IDataReader reader, IncomingOrderSearchResultSet set)
        {
            IncomingOrderSearchResult tempOrder;
            set.Summary = new SearchSummary();
            long? start = null;
            long? end = null;
            Guid orderId;
            while (reader.Read())
            {
                set.Summary.TotalResults = reader.GetValue<long>("TotalRows");
                //returned on each row for convenience. will be same value on each.
                long row = reader.GetValue<long>("RowNumber") - 1;
                start = start ?? row;
                end = end ?? row;
                end = row > end ? row : end;
                start = row < start ? row : start;
                set.Results.Add(ConstructSearchResult(reader));
            }
            set.Summary.StartIndex = start ?? 0;
            set.Summary.EndIndex = end ?? 0;
            reader.NextResult();

            while (reader.Read())
            {
                IncomingOrderCustomer customer = ConstructIncomingCustomer(reader, out orderId);
                tempOrder = set.Results.FirstOrDefault(o => o.Id == orderId);
                if (null != tempOrder)
                {
                    tempOrder.IncomingOrder.IncomingOrderCustomer = customer;
                }
            }
            reader.NextResult();
            while (reader.Read())
            {
                IncomingOrderContact contact = ConstructIncomingContact(reader, out orderId);
                tempOrder = set.Results.FirstOrDefault(o => o.Id == orderId);
                if (null != tempOrder)
                {
                    tempOrder.IncomingOrder.IncomingOrderContact = contact;
                }
            }
            reader.NextResult();
            while (reader.Read())
            {
                IncomingOrderServiceLine serviceLine = ConstructIncomingOrderServiceLine(reader, out orderId);
                tempOrder = set.Results.FirstOrDefault(o => o.Id == orderId);
                if (null != tempOrder)
                {
                    tempOrder.IncomingOrder.ServiceLines.Add(serviceLine);
                }
            }
            reader.NextResult();
            while (reader.Read())
            {
                IncomingOrderContact contact = ConstructIncomingContact(reader, out orderId);
                tempOrder = set.Results.FirstOrDefault(o => o.Id == orderId);
                if (null != tempOrder)
                {
                    tempOrder.IncomingOrder.BillToContact = contact;
                }
            }
            reader.NextResult();
            while (reader.Read())
            {
                IncomingOrderContact contact = ConstructIncomingContact(reader, out orderId);
                tempOrder = set.Results.FirstOrDefault(o => o.Id == orderId);
                if (null != tempOrder)
                {
                    tempOrder.IncomingOrder.ShipToContact = contact;
                }
            }
        }

        private static IncomingOrderSearchResult ConstructSearchResult(IDataReader reader)
        {
            var result = new IncomingOrderSearchResult
            {
                Id = reader.GetValue<Guid>("IncomingOrderId"),
                EntityType = EntityTypeEnumDto.IncomingOrder,
                Name = reader.GetValue<string>("OrderNumber"),
                Title = reader.GetValue<string>("ProjectName"),
                ChangeDate = reader.GetValue<DateTime>("UpdatedOn"),
                IncomingOrder = ConstructIncomingOrder(reader)
            };

            // this field is only returned from the search SPROC
            result.IncomingOrder.CompanyName = reader.GetValue<string>(SqlFieldNames.IncomingOrderCompanyName);

            return result;
        }

        private static IncomingOrder ConstructIncomingOrder(IDataReader reader)
        {
            var id = reader.GetValue<Guid>("IncomingOrderId");
            var result = new IncomingOrder
            {
                Id = id,
                BusinessUnit = reader.GetValue<string>("BusinessUnit"),
                CreationDate = reader.GetValue<DateTime>("CreationDate"),
                CustomerPo = reader.GetValue<string>("CustomerPo"),
                CustomerRequestedDate = reader.GetValue<DateTime?>("CustomerRequestedDate"),
                DateBooked = reader.GetValue<DateTime?>(SqlFieldNames.IncomingOrderDateBooked),
                DateOrdered = reader.GetValue<DateTime?>("DateOrdered"),
                ExternalProjectId = reader.GetValue<string>("ExternalProjectId"),
                LastUpdateDate = reader.GetValue<DateTime?>("LastUpdateDate"),
                OrderNumber = reader.GetValue<string>("OrderNumber"),
                OrderType = reader.GetValue<string>("OrderType"),
                ProjectHeaderStatus = reader.GetValue<string>("ProjectHeaderStatus"),
                ProjectName = reader.GetValue<string>("ProjectName"),
                ProjectNumber = reader.GetValue<string>("ProjectNumber"),
                Status = reader.GetValue<string>("Status"),
                CompanyId = reader.GetValue<Guid>("CompanyId"),
                OriginalXmlParsed = reader.GetValue<string>("OriginalXmlParsed"),
                WorkOrderBusinessComponentId = reader.GetValue<string>("WorkOrderBusinessComponentId"),
                WorkOrderId = reader.GetValue<string>("WorkOrderId"),
				MessageId = reader.GetValue<string>("MessageId"),
				QuoteNo = reader.GetValue<string>("QuoteNumber"),
				TotalOrderPrice = reader.GetValue<decimal?>("TotalOrderPrice"),
				Currency = reader.GetValue<string>("Currency"),
				CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
            };

            return result;
        }

        private static IncomingOrderCustomer ConstructIncomingCustomer(IDataReader reader, out Guid incomingOrderId)
        {
            var id = reader.GetValue<Guid>("IncomingOrderCustomerId");
            incomingOrderId = reader.GetValue<Guid>("IncomingOrderId");
            var result = new IncomingOrderCustomer
            {
                Id = id,
                ProjectName = reader.GetValue<string>("ProjectName"),
                DUNS = reader.GetValue<string>("DUNS"),
                Name = reader.GetValue<string>("Name"),
                ExternalId = reader.GetValue<string>("ExternalId"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
            };

            return result;
        }

        private static IncomingOrderContact ConstructIncomingContact(IDataReader reader, out Guid incomingOrderId)
        {
            var id = reader.GetValue<Guid>("IncomingOrderContactId");
            incomingOrderId = reader.GetValue<Guid>("IncomingOrderId");
            var result = new IncomingOrderContact
            {
                Id = id,
                FullName = reader.GetValue<string>("FullName"),
                Title = reader.GetValue<string>("Title"),
                Email = reader.GetValue<string>("Email"),
                ContactRoleId = (ContactRoleEnum) reader.GetValue<Int16>("ContactRoleId"),
                Address = reader.GetValue<string>("Address"),
                City = reader.GetValue<string>("City"),
                State = reader.GetValue<string>("State"),
                Province = reader.GetValue<string>("Province"),
                Country = reader.GetValue<string>("Country"),
                PostalCode = reader.GetValue<string>("PostalCode"),
                Phone = reader.GetValue<string>("Phone"),
                ExternalId = reader.GetValue<string>("ExternalId"),
                PartySiteNumber = reader.GetValue<string>("PartySiteNumber"),
                CompanyName = reader.GetValue<string>("CompanyName"),
                SubscriberNumber = reader.GetValue<string>("SubscriberNumber"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
            };

            return result;
        }

        private IncomingOrderServiceLine ConstructIncomingOrderServiceLine(IDataReader reader, out Guid incomingOrderId)
        {
            var id = reader.GetValue<Guid>("IncomingOrderServiceLineId");
            incomingOrderId = reader.GetValue<Guid>("IncomingOrderId");
            var result = new IncomingOrderServiceLine
            {
                Id = id,
                AllowChargesFromOtherOperatingUnits = reader.GetValue<string>("AllowChargesFromOtherOperatingUnits"),
                BillableExpenses = reader.GetValue<string>("BillableExpenses"),
                CustomerModelNumber = reader.GetValue<string>("CustomerModelNumber"),
                ExternalId = reader.GetValue<string>("ExternalId"),
                LineNumber = reader.GetValue<string>("LineNumber"),
                ParentExternalId = reader.GetValue<string>("ParentExternalId"),
                Name = reader.GetValue<string>("Name"),
                TypeCode = reader.GetValue<string>("TypeCode"),
                ItemCategories = reader.GetValue<string>("ItemCategories"),
                FulfillmentMethodCode = reader.GetValue<string>("FulfillmentMethodCode"),
                FulfillmentSet = reader.GetValue<string>("FulfillmentSet"),
                ConfigurationId = reader.GetValue<string>("ConfigurationId"),
                Status = reader.GetValue<string>("Status"),
                Description = reader.GetValue<string>("Description"),
                Billable = reader.GetValue<string>("Billable"),
                Program = reader.GetValue<string>("Program"),
                Category = reader.GetValue<string>("Category"),
                SubCategory = reader.GetValue<string>("SubCategory"),
                Segment = reader.GetValue<string>("Segment"),
                ClientDetailService = reader.GetValue<string>("ClientDetailService"),
                StartDate = reader.GetValue<DateTime>("StartDate"),
                PromiseDate = reader.GetValue<DateTime>("PromiseDate"),
                RequestDate = reader.GetValue<DateTime>("RequestDate"),
                WorkOrderLineBusinessComponentId = reader.GetValue<string>("WorkOrderLineBusinessComponentId"),
                WorkOrderLineId = reader.GetValue<string>("WorkOrderLineId"),
                ApplicationObjectKeyId = reader.GetValue<string>("ApplicationObjectKeyId"),
                ServiceCode = reader.GetValue<string>("ServiceCode"),
                IndustryCode = reader.GetValue<string>("IndustryCode"),
                LocationName = reader.GetValue<string>("LocationName"),
                LocationCode = reader.GetValue<string>("LocationCode"),
				LocationCodeLabel = reader.GetValue<string>("LocationCodeLabel"),
				ServiceCodeLabel = reader.GetValue<string>("ServiceCodeLabel"),
				IndustryCodeLabel = reader.GetValue<string>("IndustryCodeLabel"),
				Hold = reader.GetValue<string>("Hold"),
				ServiceSegment = reader.GetValue<string>("ServiceSegment"),
				ServiceSegmentDescription = reader.GetValue<string>("ServiceSegmentDescription"),
				ServiceCategory = reader.GetValue<string>("ServiceCategory"),
				ServiceCategoryDescription = reader.GetValue<string>("ServiceCategoryDescription"),
				ServiceSubCategory = reader.GetValue<string>("ServiceSubCategory"),
				ServiceSubCategoryDescription = reader.GetValue<string>("ServiceSubCategoryDescription"),
				ServiceProgram = reader.GetValue<string>("ServiceProgram"),
				ServiceProgramDescription = reader.GetValue<string>("ServiceProgramDescription"),
				DetailedService = reader.GetValue<string>("DetailedService"),
				DetailedServiceDescription = reader.GetValue<string>("DetailedServiceDescription"),
				Industry = reader.GetValue<string>("Industry"),
				IndustryDescription = reader.GetValue<string>("IndustryDescription"),
				IndustryCategory = reader.GetValue<string>("IndustryCategory"),
				IndustryCategoryDescription = reader.GetValue<string>("IndustryCategoryDescription"),
				IndustrySubCategory = reader.GetValue<string>("IndustrySubCategory"),
				IndustrySubCategoryDescription = reader.GetValue<string>("IndustrySubCategoryDescription"),
				ProductGroup = reader.GetValue<string>("ProductGroup"),
				ProductGroupDescription = reader.GetValue<string>("ProductGroupDescription"),
				ProductType = reader.GetValue<string>("ProductType"),
				ProductTypeDescription = reader.GetValue<string>("ProductTypeDescription"),
                Price = reader.GetValue<decimal?>("Price"),
                Currency = reader.GetValue<string>("Currency"),
				CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
            };
            return result;
        }

        private static DbCommand InitializeRemoveCommand(Guid? unsedId, Guid id, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrder_Delete]");
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, id);
            return command;
        }

        private static DbCommand InitializeRemoveServiceLineCommand(Guid? unsedId, Guid id, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderServiceLine_Delete]");
            db.AddInParameter(command, "IncomingOrderServiceLineId", DbType.Guid, id);
            return command;
        }

        private static DbCommand InitializeSearchCommand(Database db, SearchCriteria searchCriteria)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrder_Search]");

            //
            // only use the first Sort specified
            //
            string sortBy = null;
            var sortDirection = SortDirection.Ascending;
            if (searchCriteria.Sorts.Count > 0)
            {
                sortBy = searchCriteria.Sorts[0].FieldName;
                sortDirection = searchCriteria.Sorts[0].Order;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(searchCriteria.SortBy))
                {
                    sortBy = searchCriteria.SortBy;
                    sortDirection = searchCriteria.SortDirection;
                }
            }


            if (!string.IsNullOrWhiteSpace(searchCriteria.Keyword))
            {
                db.AddInParameter(command, "Keyword", DbType.String, searchCriteria.Keyword.Replace("*", "%"));
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                db.AddInParameter(command, "SortBy", DbType.String, sortBy);
            }
            db.AddInParameter(command, "SortDirection", DbType.String,
                sortDirection == SortDirection.Descending ? "DESC" : "ASC");
            db.AddInParameter(command, "StartIndex", DbType.Int64, searchCriteria.StartIndex);
            db.AddInParameter(command, "EndIndex", DbType.Int64, searchCriteria.EndIndex);
            RepositoryHelper.AddFilter(db, command, searchCriteria, AssetFieldNames.AriaProjectIndustryCode, "IndustryCode");
            RepositoryHelper.AddFilter(db, command, searchCriteria, AssetFieldNames.AriaProjectServiceCode, "ServiceCode");
            RepositoryHelper.AddFilter(db, command, searchCriteria, AssetFieldNames.AriaProjectLocationName, "LocationName");
            RepositoryHelper.AddFilter(db, command, searchCriteria, AssetFieldNames.AriaCustomerCountry, "CustomerCountry");
            RepositoryHelper.AddFilter(db, command, searchCriteria, AssetFieldNames.AriaCustomerState, "CustomerState");

            return command;
        }


        private static DbCommand InitializeUpdateCommandIncomingOrder(Guid? id, IncomingOrder incomingOrder, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrder_Update]");

            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, id);
            db.AddInParameter(command, "BusinessUnit", DbType.String, incomingOrder.BusinessUnit);
            db.AddInParameter(command, "CreationDate", DbType.DateTime2, incomingOrder.CreationDate);
            db.AddInParameter(command, "CustomerPo", DbType.String, incomingOrder.CustomerPo);
            db.AddInParameter(command, "CustomerRequestedDate", DbType.DateTime2, incomingOrder.CustomerRequestedDate);
            db.AddInParameter(command, "DateBooked", DbType.DateTime2, incomingOrder.DateBooked);
            db.AddInParameter(command, "DateOrdered", DbType.DateTime2, incomingOrder.DateOrdered);
            db.AddInParameter(command, "ExternalProjectId", DbType.String, incomingOrder.ExternalProjectId);
            db.AddInParameter(command, "LastUpdateDate", DbType.DateTime2, incomingOrder.LastUpdateDate);
            db.AddInParameter(command, "OrderNumber", DbType.String, incomingOrder.OrderNumber);
            db.AddInParameter(command, "OrderType", DbType.String, incomingOrder.OrderType);
            db.AddInParameter(command, "OriginalXmlParsed", DbType.String, incomingOrder.OriginalXmlParsed);
            db.AddInParameter(command, "ProjectHeaderStatus", DbType.String, incomingOrder.ProjectHeaderStatus);
            db.AddInParameter(command, "ProjectName", DbType.String, incomingOrder.ProjectName);
            db.AddInParameter(command, "ProjectNumber", DbType.String, incomingOrder.ProjectNumber);
            db.AddInParameter(command, "Status", DbType.String, incomingOrder.Status);
            db.AddInParameter(command, "CompanyId", DbType.Guid, incomingOrder.CompanyId);
            db.AddInParameter(command, "WorkOrderBusinessComponentId", DbType.String,
                incomingOrder.WorkOrderBusinessComponentId);
			db.AddInParameter(command, "WorkOrderId", DbType.String, incomingOrder.WorkOrderId);
			db.AddInParameter(command, "MessageId", DbType.String, incomingOrder.MessageId);
			db.AddInParameter(command, "QuoteNumber", DbType.String, incomingOrder.QuoteNo);
			db.AddInParameter(command, "TotalOrderPrice", DbType.Decimal, incomingOrder.TotalOrderPrice);
            db.AddInParameter(command, "Currency", DbType.String, incomingOrder.Currency);
			db.AddInParameter(command, "UpdatedBy", DbType.Guid, incomingOrder.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, incomingOrder.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeUpdateCommandIncomingOrderContact(Guid? id,
            IncomingOrderContact incomingOrderContact,
            Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderContact_Update]");

            db.AddInParameter(command, "IncomingOrderContactId", DbType.Guid, id);
            db.AddInParameter(command, "Title", DbType.String, incomingOrderContact.Title);
            db.AddInParameter(command, "Email", DbType.String, incomingOrderContact.Email);
            db.AddInParameter(command, "FullName", DbType.String, incomingOrderContact.FullName);
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, incomingOrderContact.IncomingOrderId);
            db.AddInParameter(command, "ContactRoleId", DbType.Int16, (Int16) incomingOrderContact.ContactRoleId);
            db.AddInParameter(command, "Address", DbType.String, incomingOrderContact.Address);
            db.AddInParameter(command, "City", DbType.String, incomingOrderContact.City);
            db.AddInParameter(command, "State", DbType.String, incomingOrderContact.State);
            db.AddInParameter(command, "Province", DbType.String, incomingOrderContact.Province);
            db.AddInParameter(command, "Country", DbType.String, incomingOrderContact.Country);
            db.AddInParameter(command, "PostalCode", DbType.String, incomingOrderContact.PostalCode);
            db.AddInParameter(command, "Phone", DbType.String, incomingOrderContact.Phone);
            db.AddInParameter(command, "CompanyName", DbType.String, incomingOrderContact.CompanyName);
            db.AddInParameter(command, "ExternalId", DbType.String, incomingOrderContact.ExternalId);
            db.AddInParameter(command, "SubscriberNumber", DbType.String, incomingOrderContact.SubscriberNumber);
            db.AddInParameter(command, "PartySiteNumber", DbType.String, incomingOrderContact.PartySiteNumber);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, incomingOrderContact.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, incomingOrderContact.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeUpdateCommandIncomingOrderCustomer(Guid? id,
            IncomingOrderCustomer
                incomingOrderCustomer, Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderCustomer_Update]");

            db.AddInParameter(command, "IncomingOrderCustomerId", DbType.Guid, id);
            db.AddInParameter(command, "DUNS", DbType.String, incomingOrderCustomer.DUNS);
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, incomingOrderCustomer.IncomingOrderId);
            db.AddInParameter(command, "Name", DbType.String, incomingOrderCustomer.Name);
            db.AddInParameter(command, "ProjectName", DbType.String, incomingOrderCustomer.ProjectName);
            db.AddInParameter(command, "ExternalId", DbType.String, incomingOrderCustomer.ExternalId);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, incomingOrderCustomer.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, incomingOrderCustomer.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeDeleteCommandIncomingOrderServiceLineByOrderId(Guid? id,
            IncomingOrder incomingOrder,
            Database db)
        {
            DbCommand command = db.GetStoredProcCommand("[dbo].[pIncomingOrderServiceLine_DeleteByOrderId]");

            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, incomingOrder.Id);

            return command;
        }

        /// <summary>
        ///     Removes the specified incoming order.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            Delete(entityId);
        }



        /// <summary>
        /// Fetches the Incoming Order lookups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lookup> FetchIncomingOrderLookups()
        {
            List<Lookup> incomingOrders = new List<Lookup>();

            using (_transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                using (var command = InitializeFetchIncomingOrderLookUp(db))
                {
                    using (var reader = db.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            var vProject = ConstructIncomingOrderLookups(reader);
                            incomingOrders.Add(vProject);
                        }
                    }
                }
            }

            return incomingOrders;
        }

        private DbCommand InitializeFetchIncomingOrderLookUp(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pIncomingOrder_GetLookups]");
            return command;
        }

        private Lookup ConstructIncomingOrderLookups(IDataReader reader)
        {
            var id = reader.GetValue<Guid>("IncomingOrderId");
            var result = new Lookup
            {
                Id = id,
                Name = reader.GetValue<string>("Name")
            };

            return result;
        }
    }
}