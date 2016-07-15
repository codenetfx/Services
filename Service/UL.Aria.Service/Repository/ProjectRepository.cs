using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Description;
using Microsoft.Practices.EnterpriseLibrary.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class ProjectRepository
    /// </summary>
    public sealed class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        private const string ProjectNotFoundIdMessage = "Project Not Found Id: '{0}'";
        private const string ProjectNotFoundOrderNumberMessage = "Project Not Found OrderNumber: '{0}'";
        private const string ProjectsNotFoundIdMessage = "Missing Project from Ids: '{0}'";
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectRepository" /> class.
        /// </summary>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ProjectRepository(ITransactionFactory transactionFactory)
            : base("ProjectId")
        {
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Fetches the project lookups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lookup> FetchProjectLookups()
        {
            return FindProjectLookups();
        }

        /// <summary>
        ///     Fetches the projects.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>IList{Project}.</returns>
        IEnumerable<Project> IProjectRepository.FetchProjects(IEnumerable<Guid> ids)
        {
            IList<Project> projects;

            using (var transactionScope = _transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                var idsList = ids as IList<Guid> ?? ids.ToList();
                using (var command = InitializeFetchProjects(db, idsList))
                {
                    using (var reader = db.ExecuteReader(command))
                    {
                        projects = ConstructCompleteProjects(reader);

                        if (projects.Count != idsList.Count())
                            throw new DatabaseItemNotFoundException(string.Format(ProjectsNotFoundIdMessage,
                                string.Join(",", idsList.ToArray())));
                    }
                }

                transactionScope.Complete();
            }

            return projects;
        }

        /// <summary>
        ///     Fetches the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Project.</returns>
        Project IProjectRepository.FetchById(Guid id)
        {
            return FindById(id);
        }

        /// <summary>
        ///     Fetches the by order number.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>IList{Project}.</returns>
        IList<Project> IProjectRepository.FetchByOrderNumber(string orderNumber)
        {
            IList<Project> projects;

            using (var transactionScope = _transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                using (var command = InitializeFetchByOrderNumber(db, orderNumber))
                {
                    using (var reader = db.ExecuteReader(command))
                    {
                        projects = ConstructCompleteProjects(reader);
                    }
                }
                transactionScope.Complete();
                if (projects.Count == 0)
                    throw new DatabaseItemNotFoundException(string.Format(ProjectNotFoundOrderNumberMessage, orderNumber));
            }

            return projects;
        }

        /// <summary>
        ///     Creates the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>Guid.</returns>
        Guid IProjectRepository.Create(Project project)
        {
            return Insert(project);
        }

        /// <summary>
        ///     Updates the specified project.
        /// </summary>
        /// <param name="project">The project.</param>
        void IProjectRepository.Update(Project project)
        {
            Update(project);
        }

        /// <summary>
        ///     Updates Project Status and LineItems Statuses only.
        /// </summary>
        /// <param name="project">The project.</param>
        void IProjectRepository.UpdateStatusFromOrder(Project project)
        {
            UpdateStatusFromOrder(project);
        }

        /// <summary>
        /// Updates the contact.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="contact">The contact.</param>
        public void UpdateContact(Guid projectId, IncomingOrderContact contact)
        {
            var db = DatabaseFactory.CreateDatabase();
            var command = InitializeUpdateCommandProjectContact(contact.Id, contact, db);

            using (command)
            {
                db.ExecuteNonQuery(command);
            }
        }

		/// <summary>
		/// Creates the contact.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="contact">The contact.</param>
		/// <returns>Guid.</returns>
		public Guid CreateContact(Guid projectId, IncomingOrderContact contact)
		{
			try
			{
				ExecuteCommand(InitializeInsertCommandProjectContact, contact.Id, contact);
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
        ///     Updates the contact.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="contact">The contact.</param>
        public void UpdateAllContactsForExternalId(string externalId, IncomingOrderContact contact)
        {
            var db = DatabaseFactory.CreateDatabase();
            var command = InitializeUpdateCommandProjectContactByExternalId(externalId, contact, db);

            using (command)
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///     Updates all customers for external identifier.
        /// </summary>
        /// <param name="externalId">The external identifier.</param>
        /// <param name="customer">The contact.</param>
        public void UpdateAllCustomersForExternalId(string externalId, IncomingOrderCustomer customer)
        {
            var db = DatabaseFactory.CreateDatabase();
            var command = InitializeUpdateCommandProjectCustomerByExternalId(externalId, customer, db);

            using (command)
            {
                db.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        ///     Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        void IProjectRepository.Delete(Guid id)
        {
            Remove(id);
        }

        /// <summary>
        ///     Fetches all headers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Guid> FetchAllIds()
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = InitializeFetchProjectIds(db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        var id = reader.GetValue<Guid>("ProjectId");
                        //projectIds.Add(id);
                        yield return id;
                    }
                }
            }
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <returns>IList{Project}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        IList<Project> IProjectRepository.FetchAll()
        {
            return FindAll();
        }

        /// <summary>
        ///     Fetches the project lookups.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lookup> FindProjectLookups()
        {
            var projects = new List<Lookup>();

            using (_transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                using (var command = InitializeFetchProjectsLookUp(db))
                {
                    using (var reader = db.ExecuteReader(command))
                    {
                        while (reader.Read())
                        {
                            var vProject = ConstructProjectLookups(reader);
                            projects.Add(vProject);
                        }
                    }
                }
            }

            return projects;
        }

        /// <summary>
        ///     Constructs the project lookups.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static Lookup ConstructProjectLookups(IDataReader reader)
        {
            return new Lookup
            {
                Id = reader.GetValue<Guid>("ProjectId"),
                Name = reader.GetValue<string>("Name"),
                ContainerId = reader.GetValue<Guid?>("ContainerId")
            };
        }

        private static DbCommand InitializeFetchProjectsLookUp(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_GetProjectLookups]");

            return command;
        }

        /// <summary>
        ///     Updates from incoming order.
        /// </summary>
        /// <param name="project">The entity.</param>
        /// <exception cref="DatabaseItemNotFoundException"></exception>
        public void UpdateFromIncomingOrder(Project project)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                var count = ExecuteCommand(InitializeUpdateFromIncomingOrderCommandProject, project.Id, project);

                if (count == 0)
                    throw new DatabaseItemNotFoundException(string.Format(ProjectNotFoundIdMessage, project.Id));

                ExecuteCommand(InitializeUpdateCommandProjectContact, project.IncomingOrderContact.Id,
                    project.IncomingOrderContact);

                ExecuteCommand(InitializeUpdateCommandProjectContact, project.BillToContact.Id,
                    project.BillToContact);

                ExecuteCommand(InitializeUpdateCommandProjectContact, project.ShipToContact.Id,
                    project.ShipToContact);

                ExecuteCommand(InitializeUpdateCommandProjectCustomer, project.IncomingOrderCustomer.Id,
                    project.IncomingOrderCustomer);

                ExecuteCommand(InitializeDeleteCommandProjectServiceLineByProjectId, project.Id, project);

                foreach (var incomingOrderServiceLine in project.ServiceLines)
                {
// ReSharper disable once PossibleInvalidOperationException
                    incomingOrderServiceLine.IncomingOrderId = project.Id.Value;
                    ExecuteCommand(InitializeInsertCommandProjectServiceLine, incomingOrderServiceLine.Id,
                        incomingOrderServiceLine);
                }


                transactionScope.Complete();
            }
        }

        private static DbCommand InitializeFetchProjects(Database db, IEnumerable<Guid> ids)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_GetProjects]");
            var dataTable = CreateDataTable();
            foreach (var id in ids)
            {
                var dr = CreateTableRow(dataTable, id);
                dataTable.Rows.Add(dr);
            }
            var sqlParameter = new SqlParameter("@ProjectIds", SqlDbType.Structured) {Value = dataTable};
            command.Parameters.Add(sqlParameter);
            return command;
        }

        private static DbCommand InitializeFetchProjects(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_GetAllProjects]");

            return command;
        }
        
        private static DbCommand InitializeFetchProjectIds(Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_GetAllIds]");

            return command;
        }

        private static void AddTableRowFields(Guid id, DataRow dr)
        {
            dr["ProjectId"] = id;
        }

        private static DataRow CreateTableRow(DataTable dataTable, Guid id)
        {
            var dr = dataTable.NewRow();

            AddTableRowFields(id, dr);

            return dr;
        }

        private static DataTable CreateDataTable()
        {
            var columnList = new List<KeyValuePair<string, Type>>
            {
                new KeyValuePair<string, Type>("ProjectId", typeof (Guid))
            };

            var dataTable = new DataTable();

            foreach (var keyValuePair in columnList)
            {
                dataTable.Columns.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return dataTable;
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns>IList{Project}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IList<Project> FindAll()
        {
            IList<Project> projects;

            using (var transactionScope = _transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                using (var command = InitializeFetchProjects(db))
                {
                    using (var reader = db.ExecuteReader(command))
                    {
                        projects = ConstructCompleteProjects(reader);
                    }
                }

                transactionScope.Complete();
            }

            return projects;
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>Project.</returns>
        public override Project FindById(Guid entityId)
        {
            Project project;

            using (var transactionScope = _transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                using (var command = InitializeFetchById(db, entityId))
                {
                    using (var reader = db.ExecuteReader(command))
                    {
                        project = ConstructCompleteProject(reader, string.Format(ProjectNotFoundIdMessage, entityId));
                    }
                }

                transactionScope.Complete();
            }

            return project;
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public override void Add(Project entity)
        {
            Insert(entity);
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Int32.</returns>
        public override int Update(Project entity)
        {
            int count;

            using (var transactionScope = _transactionFactory.Create())
            {
                count = ExecuteCommand(InitializeUpdateCommandProject, entity.Id, entity);

                if (count == 0)
                    throw new DatabaseItemNotFoundException(string.Format(ProjectNotFoundIdMessage, entity.Id));

// ReSharper disable once PossibleInvalidOperationException
                entity.IncomingOrderContact.IncomingOrderId = entity.Id.Value;
                ExecuteCommand(InitializeUpdateCommandProjectContact, entity.IncomingOrderContact.Id,
                    entity.IncomingOrderContact);

                entity.BillToContact.IncomingOrderId = entity.Id.Value;
                ExecuteCommand(InitializeUpdateCommandProjectContact, entity.BillToContact.Id,
                    entity.BillToContact);

                entity.ShipToContact.IncomingOrderId = entity.Id.Value;
                ExecuteCommand(InitializeUpdateCommandProjectContact, entity.ShipToContact.Id,
                    entity.ShipToContact);

                entity.IncomingOrderCustomer.IncomingOrderId = entity.Id.Value;
                ExecuteCommand(InitializeUpdateCommandProjectCustomer, entity.IncomingOrderCustomer.Id,
                    entity.IncomingOrderCustomer);

                ExecuteCommand(InitializeDeleteCommandProjectServiceLineByProjectId, null, entity);

                foreach (var incomingOrderServiceLine in entity.ServiceLines)
                {
                    incomingOrderServiceLine.IncomingOrderId = entity.Id.Value;
                    ExecuteCommand(InitializeInsertCommandProjectServiceLine, incomingOrderServiceLine.Id,
                        incomingOrderServiceLine);
                }

                transactionScope.Complete();
            }

            return count;
        }

        /// <summary>
        ///     Updates Project Status and LineItems Statuses only.
        /// </summary>
        /// <param name="entity">The Project entity.</param>
        /// <returns>System.Int32.</returns>
        public int UpdateStatusFromOrder(Project entity)
        {
            int count;

            using (var transactionScope = _transactionFactory.Create())
            {
                count = ExecuteCommand(InitializeUpdateProjectStatusFromOrder, entity.Id, entity);

                if (count == 0)
                    throw new DatabaseItemNotFoundException(string.Format(ProjectNotFoundIdMessage, entity.Id));

                // ReSharper disable once PossibleInvalidOperationException

                foreach (var incomingOrderServiceLine in entity.ServiceLines)
                {
                    incomingOrderServiceLine.IncomingOrderId = entity.Id.Value;
                    ExecuteCommand(InitializeUpdateProjectServiceLineStatusFromOrder, incomingOrderServiceLine.Id,
                        incomingOrderServiceLine);
                }

                transactionScope.Complete();
            }

            return count;
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                var count = ExecuteCommand(InitializeRemoveCommand, null, entityId);

                if (count == 0)
                    throw new DatabaseItemNotFoundException(string.Format(ProjectNotFoundIdMessage, entityId));

                transactionScope.Complete();
            }
        }

        private static IList<Project> ConstructCompleteProjects(IDataReader reader)
        {
            var projects = new List<Project>();
            var projectsDictionary = new Dictionary<Guid, Project>();

            while (reader.Read())
            {
                var project = ConstructProject(reader);
                projects.Add(project);
// ReSharper disable once AssignNullToNotNullAttribute
// ReSharper disable once PossibleInvalidOperationException
                projectsDictionary.Add(project.Id.Value, project);
            }

            reader.NextResult();
            while (reader.Read())
            {
                var incomingOrderContact = ConstructIncomingOrderContact(reader);
                var project = projectsDictionary[incomingOrderContact.IncomingOrderId];
                project.IncomingOrderContact = incomingOrderContact;
            }

            reader.NextResult();
            while (reader.Read())
            {
                var incomingOrderCustomer = ConstructIncomingOrderCustomer(reader);
                var project = projectsDictionary[incomingOrderCustomer.IncomingOrderId];
                project.IncomingOrderCustomer = incomingOrderCustomer;
            }

            reader.NextResult();
            while (reader.Read())
            {
                var incomingOrderServiceLine = ConstructIncomingOrderServiceLine(reader);
                var project = projectsDictionary[incomingOrderServiceLine.IncomingOrderId];
                project.ServiceLines.Add(incomingOrderServiceLine);
            }

            reader.NextResult();
            while (reader.Read())
            {
                var incomingOrderContact = ConstructIncomingOrderContact(reader);
                var project = projectsDictionary[incomingOrderContact.IncomingOrderId];
                project.BillToContact = incomingOrderContact;
            }

            reader.NextResult();
            while (reader.Read())
            {
                var incomingOrderContact = ConstructIncomingOrderContact(reader);
                var project = projectsDictionary[incomingOrderContact.IncomingOrderId];
                project.ShipToContact = incomingOrderContact;
            }

            return projects;
        }

        private static Project ConstructCompleteProject(IDataReader reader, string notFoundMessage)
        {
            if (!reader.Read()) throw new DatabaseItemNotFoundException(notFoundMessage);
            var project = ConstructProject(reader);

            reader.NextResult();
            if (reader.Read())
                project.IncomingOrderContact = ConstructIncomingOrderContact(reader);

            reader.NextResult();
            if (reader.Read())
                project.IncomingOrderCustomer = ConstructIncomingOrderCustomer(reader);

            reader.NextResult();
            while (reader.Read())
                project.ServiceLines.Add(ConstructIncomingOrderServiceLine(reader));

            reader.NextResult();
            if (reader.Read())
                project.BillToContact = ConstructIncomingOrderContact(reader);

            reader.NextResult();
            if (reader.Read())
                project.ShipToContact = ConstructIncomingOrderContact(reader);

            return project;
        }

        private static Project ConstructProject(IDataReader reader)
        {
            return new Project
            {
                Id = reader.GetValue<Guid>("ProjectId"),
                Description = reader.GetValue<string>("Description"),
                StartDate = reader.GetValue<DateTime?>("StartDate"),
                EstimatedTATDate = reader.GetValue<DateTime?>("EstimatedTATDate"),
                EndDate = reader.GetValue<DateTime?>("EndDate"),
                ProjectHandler = reader.GetValue<string>("ProjectHandler"),
                EstimatedLabEffort = reader.GetValue<decimal?>("EstimatedLabEffort"),
                EstimateEngineeringEffort = reader.GetValue<decimal?>("EstimateEngineeringEffort"),
                Scope = reader.GetValue<string>("Scope"),
                Assumptions = reader.GetValue<string>("Assumptions"),
                EngineeringOfficeLimitations = reader.GetValue<string>("EngineeringOfficeLimitations"),
                LaboratoryLimitations = reader.GetValue<string>("LaboratoryLimitations"),
                Complexity = reader.GetValue<string>("Complexity"),
                AdditionalCriteria = reader.GetValue<string>("AdditionalCriteria"),
				Industry = reader.GetValue<string>("IndustryDesc"),
                IndustryCode = reader.GetValue<string>("IndustryCode"),
                IndustryCategory = reader.GetValue<string>("IndustryCategory"),
                IndustrySubcategory = reader.GetValue<string>("IndustrySubcategory"),
                Location = reader.GetValue<string>("Location"),
                ProductGroup = reader.GetValue<string>("ProductGroup"),
                StatusDescription = reader.GetValue<string>("StatusDescription"),
                ProjectStatus =
                    (ProjectStatusEnumDto)
                        Enum.Parse(typeof (ProjectStatusEnumDto), reader.GetValue<string>("ProjectStatus")),
                BusinessUnit = reader.GetValue<string>("BusinessUnit"),
                ProjectHeaderStatus = reader.GetValue<string>("ProjectHeaderStatus"),
                CreationDate = reader.GetValue<DateTime?>("CreationDate"),
                CustomerRequestedDate = reader.GetValue<DateTime?>("CustomerRequestedDate"),
                DateBooked = reader.GetValue<DateTime?>("DateBooked"),
                DateOrdered = reader.GetValue<DateTime?>("DateOrdered"),
                LastUpdateDate = reader.GetValue<DateTime?>("LastUpdateDate"),
                ExternalProjectId = reader.GetValue<string>("ExternalProjectId"),
                Name = reader.GetValue<string>("Name"),
                ProjectNumber = reader.GetValue<string>("ProjectNumber"),
                ProjectName = reader.GetValue<string>("ProjectName"),
                CompanyId = reader.GetValue<Guid?>("CompanyId"),
                OrderNumber = reader.GetValue<string>("OrderNumber"),
                OrderType = reader.GetValue<string>("OrderType"),
                CustomerPo = reader.GetValue<string>("CustomerPo"),
                Status = reader.GetValue<string>("Status"),
                ContainerId = reader.GetValue<Guid?>("ContainerId"),
                ProjectTemplateId = reader.GetValue<Guid>("ProjectTemplateId"),
                OriginalXmlParsed = reader.GetValue<string>("OriginalXmlParsed"),
                WorkOrderBusinessComponentId = reader.GetValue<string>("WorkOrderBusinessComponentId"),
                WorkOrderId = reader.GetValue<string>("WorkOrderId"),
                CompletionDate = reader.GetValue<DateTime?>("CompletionDate"),
                DaysInCurrentPhase = reader.GetValue<int>("DaysInCurrentPhase"),
                EstimatedReviewerEffort = reader.GetValue<decimal?>("EstimatedReviewerEffort"),
                NumberOfSamples = reader.GetValue<int?>("NumberOfSamples"),
                SampleReferenceNumbers = reader.GetValue<string>("SampleReferenceNumbers"),
                CCN = reader.GetValue<string>("CCN"),
                FileNo = reader.GetValue<string>("FileNo"),
                StatusNotes = reader.GetValue<string>("StatusNotes"),
                InventoryItemCatalogNumbers = reader.GetValue<string>("InventoryItemCatalogNumbers"),
                InventoryItemNumbersDescriptions = reader.GetValue<string>("InventoryItemNumbersDescriptions"),
                QuoteNo = reader.GetValue<string>("QuoteNo"),
                Expedited = reader.GetValue<bool>("Expedited"),
                Price = reader.GetValue<string>("Price"),
                Standards = reader.GetValue<string>("Standards"),
                ProjectType = reader.GetValue<string>("ProjectType"),
                ServiceCode = reader.GetValue<string>("ServiceCode"),
                ServiceDescription = reader.GetValue<string>("ServiceDesc"),
                ProjectTemplateName = reader.GetValue<string>("ProjectTemplateName"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn"),
                HideFromCustomer = reader.GetValue<bool>("HideFromCustomer"),
				MessageId = reader.GetValue<string>("MessageId"),
				TaskMinimumDueDate = reader.GetValue<DateTime?>("TaskMinimumDueDate"),
				MinimumDueDateTaskId = reader.GetValue<Guid?>("MinimumDueDateTaskId"),
				TotalOrderPrice = reader.GetValue<decimal?>("TotalOrderPrice"),
                Currency = reader.GetValue<string>("Currency"),
                ServiceRequestNumber = reader.GetValue<string>("ServiceRequestNumber"),
                OrderOwner = reader.GetValue<string>("OrderOwner"),
				OverrideAutoComplete = reader.GetValue<bool>("OverrideAutoComplete"),
				HasAutoComplete = reader.GetValue<bool>("AutoCompleteProject")
			};
        }

        private static IncomingOrderContact ConstructIncomingOrderContact(IDataReader reader)
        {
            return new IncomingOrderContact
            {
                Id = reader.GetValue<Guid>("ProjectContactId"),
                IncomingOrderId = reader.GetValue<Guid>("ProjectId"),
                FullName = reader.GetValue<string>("FullName"),
                Title = reader.GetValue<string>("Title"),
                Email = reader.GetValue<string>("Email"),
                Address = reader.GetValue<string>("Address"),
                Phone = reader.GetValue<string>("Phone"),
                State = reader.GetValue<string>("State"),
                Country = reader.GetValue<string>("Country"),
                ContactRoleId = (ContactRoleEnum) reader.GetValue<Int16>("ContactRoleId"),
                City = reader.GetValue<string>("City"),
                Province = reader.GetValue<string>("Province"),
                PostalCode = reader.GetValue<string>("PostalCode"),
                CompanyName = reader.GetValue<string>("CompanyName"),
                ExternalId = reader.GetValue<string>("ExternalId"),
                PartySiteNumber = reader.GetValue<string>("PartySiteNumber"),
                SubscriberNumber = reader.GetValue<string>("SubscriberNumber"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
               
            };
        }

        private static IncomingOrderCustomer ConstructIncomingOrderCustomer(IDataReader reader)
        {
            return new IncomingOrderCustomer
            {
                Id = reader.GetValue<Guid>("ProjectCustomerId"),
                IncomingOrderId = reader.GetValue<Guid>("ProjectId"),
                ProjectName = reader.GetValue<string>("ProjectName"),
                DUNS = reader.GetValue<string>("DUNS"),
                Name = reader.GetValue<string>("Name"),
                ExternalId = reader.GetValue<string>("ExternalId"),
                AgentDetails = reader.GetValue<string>("AgentDetails"),
                SubscriberNumber = reader.GetValue<string>("SubscriberNumber"),
                CreatedById = reader.GetValue<Guid>("CreatedBy"),
                CreatedDateTime = reader.GetValue<DateTime>("CreatedOn"),
                UpdatedById = reader.GetValue<Guid>("UpdatedBy"),
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn")
            };
        }

        private static IncomingOrderServiceLine ConstructIncomingOrderServiceLine(IDataReader reader)
        {
            return new IncomingOrderServiceLine
            {
                Id = reader.GetValue<Guid>("ProjectServiceLineId"),
                IncomingOrderId = reader.GetValue<Guid>("ProjectId"),
                Quantity = reader.GetValue<int>("Quantity"),
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
                PromiseDate = reader.GetValue<DateTime>("PromiseDate"),
                RequestDate = reader.GetValue<DateTime>("RequestDate"),
                StartDate = reader.GetValue<DateTime>("StartDate"),
                Billable = reader.GetValue<string>("Billable"),
                Program = reader.GetValue<string>("Program"),
                Category = reader.GetValue<string>("Category"),
                SubCategory = reader.GetValue<string>("SubCategory"),
                Segment = reader.GetValue<string>("Segment"),
                Description = reader.GetValue<string>("Description"),
                ClientDetailService = reader.GetValue<string>("ClientDetailService"),
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
                UpdatedDateTime = reader.GetValue<DateTime>("UpdatedOn"),
                Hold = reader.GetValue<string>("Hold")
            };
        }

        private Guid Insert(Project project)
        {
            var id = Guid.Empty;

            using (var transactionScope = _transactionFactory.Create())
            {
                ExecuteCommand(InitializeInsertCommandProject, null, project,
                    cmd => { id = (Guid) cmd.Parameters["@ProjectId"].Value; });

                project.IncomingOrderContact.IncomingOrderId = id;
                project.IncomingOrderContact.ContactRoleId = ContactRoleEnum.Customer;
                ExecuteCommand(InitializeInsertCommandProjectContact, project.IncomingOrderContact.Id,
                    project.IncomingOrderContact);

                project.BillToContact.IncomingOrderId = id;
                project.BillToContact.ContactRoleId = ContactRoleEnum.BillTo;
                ExecuteCommand(InitializeInsertCommandProjectContact, project.BillToContact.Id, project.BillToContact);

                project.ShipToContact.IncomingOrderId = id;
                project.ShipToContact.ContactRoleId = ContactRoleEnum.ShipTo;
                ExecuteCommand(InitializeInsertCommandProjectContact, project.ShipToContact.Id, project.ShipToContact);

                project.IncomingOrderCustomer.IncomingOrderId = id;
                ExecuteCommand(InitializeInsertCommandProjectCustomer, project.IncomingOrderCustomer.Id,
                    project.IncomingOrderCustomer);

                foreach (var incomingOrderServiceLine in project.ServiceLines)
                {
                    incomingOrderServiceLine.IncomingOrderId = id;
                    ExecuteCommand(InitializeInsertCommandProjectServiceLine, incomingOrderServiceLine.Id,
                        incomingOrderServiceLine);
                }
                transactionScope.Complete();
            }

            return id;
        }

        private static DbCommand InitializeInsertCommandProject(Guid? id, Project project, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_Insert]");

            db.AddInParameter(command, "ProjectId", DbType.Guid, project.Id);
            db.AddInParameter(command, "Description", DbType.String, project.Description);
            db.AddInParameter(command, "StartDate", DbType.DateTime2, project.StartDate);
            db.AddInParameter(command, "EstimatedTATDate", DbType.DateTime2, project.EstimatedTATDate);
            db.AddInParameter(command, "EndDate", DbType.DateTime2, project.EndDate);
            db.AddInParameter(command, "ProjectHandler", DbType.String, project.ProjectHandler);
            db.AddInParameter(command, "EstimatedLabEffort", DbType.Double, project.EstimatedLabEffort);
            db.AddInParameter(command, "EstimateEngineeringEffort", DbType.Decimal, project.EstimateEngineeringEffort);
            db.AddInParameter(command, "Scope", DbType.String, project.Scope);
            db.AddInParameter(command, "Assumptions", DbType.String, project.Assumptions);
            db.AddInParameter(command, "EngineeringOfficeLimitations", DbType.String,
                project.EngineeringOfficeLimitations);
            db.AddInParameter(command, "LaboratoryLimitations", DbType.String, project.LaboratoryLimitations);
            db.AddInParameter(command, "Complexity", DbType.String, project.Complexity);
            db.AddInParameter(command, "AdditionalCriteria", DbType.String, project.AdditionalCriteria);
            db.AddInParameter(command, "Industry", DbType.String, project.Industry);
            db.AddInParameter(command, "IndustryCode", DbType.String, project.IndustryCode);
            db.AddInParameter(command, "IndustryCategory", DbType.String, project.IndustryCategory);
            db.AddInParameter(command, "IndustrySubcategory", DbType.String, project.IndustrySubcategory);
            db.AddInParameter(command, "Location", DbType.String, project.Location);
            db.AddInParameter(command, "ProductGroup", DbType.String, project.ProductGroup);
            db.AddInParameter(command, "StatusDescription", DbType.String, project.StatusDescription);
            db.AddInParameter(command, "ProjectStatus", DbType.String, project.ProjectStatus.ToString());
            db.AddInParameter(command, "BusinessUnit", DbType.String, project.BusinessUnit);
            db.AddInParameter(command, "ProjectHeaderStatus", DbType.String, project.ProjectHeaderStatus);
            db.AddInParameter(command, "CreationDate", DbType.DateTime2, project.CreationDate);
            db.AddInParameter(command, "CustomerRequestedDate", DbType.DateTime2, project.CustomerRequestedDate);
            db.AddInParameter(command, "DateBooked", DbType.DateTime2, project.DateBooked);
            db.AddInParameter(command, "DateOrdered", DbType.DateTime2, project.DateOrdered);
            db.AddInParameter(command, "LastUpdateDate", DbType.DateTime2, project.LastUpdateDate);
            db.AddInParameter(command, "ExternalProjectId", DbType.String, project.ExternalProjectId);
            db.AddInParameter(command, "Name", DbType.String, project.Name);
            db.AddInParameter(command, "ProjectName", DbType.String, project.ProjectName);
            db.AddInParameter(command, "ProjectNumber", DbType.String, project.ProjectNumber);
            db.AddInParameter(command, "CompanyId", DbType.Guid, project.CompanyId);
            db.AddInParameter(command, "OrderNumber", DbType.String, project.OrderNumber);
            db.AddInParameter(command, "OrderType", DbType.String, project.OrderType);
            db.AddInParameter(command, "CustomerPo", DbType.String, project.CustomerPo);
            db.AddInParameter(command, "Status", DbType.String, project.Status);
            db.AddInParameter(command, "ContainerId", DbType.Guid, project.ContainerId);            
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, project.IncomingOrderId);
            db.AddInParameter(command, "OriginalXmlParsed", DbType.String, project.OriginalXmlParsed);
            db.AddInParameter(command, "WorkOrderBusinessComponentId", DbType.String,
                project.WorkOrderBusinessComponentId);
            db.AddInParameter(command, "WorkOrderId", DbType.String, project.WorkOrderId);
            db.AddInParameter(command, "CompletionDate", DbType.DateTime2, project.CompletionDate);
            db.AddInParameter(command, "DaysInCurrentPhase", DbType.Int32, project.DaysInCurrentPhase);
            db.AddInParameter(command, "EstimatedReviewerEffort", DbType.Decimal, project.EstimatedReviewerEffort);
            db.AddInParameter(command, "NumberOfSamples", DbType.Int32, project.NumberOfSamples);
            db.AddInParameter(command, "SampleReferenceNumbers", DbType.String, project.SampleReferenceNumbers);
            db.AddInParameter(command, "CCN", DbType.String, project.CCN);
            db.AddInParameter(command, "FileNo", DbType.String, project.FileNo);
            db.AddInParameter(command, "StatusNotes", DbType.String, project.StatusNotes);
            db.AddInParameter(command, "InventoryItemCatalogNumbers", DbType.String, project.InventoryItemCatalogNumbers);
            db.AddInParameter(command, "InventoryItemNumbersDescriptions", DbType.String,
                project.InventoryItemNumbersDescriptions);
            db.AddInParameter(command, "QuoteNo", DbType.String, project.QuoteNo);
            db.AddInParameter(command, "Expedited", DbType.Boolean, project.Expedited);
            db.AddInParameter(command, "Price", DbType.String, project.Price);
            db.AddInParameter(command, "Standards", DbType.String, project.Standards);
            db.AddInParameter(command, "ProjectType", DbType.String, project.ProjectType);
            db.AddInParameter(command, "ServiceCode", DbType.String, project.ServiceCode);
            db.AddInParameter(command, "ServiceDescription", DbType.String, project.ServiceDescription);
			db.AddInParameter(command, "MessageId", DbType.String, project.MessageId);
			db.AddInParameter(command, "TaskMinimumDueDate", DbType.DateTime2, project.TaskMinimumDueDate);
			db.AddInParameter(command, "MinimumDueDateTaskId", DbType.Guid, project.MinimumDueDateTaskId);
			db.AddInParameter(command, "TotalOrderPrice", DbType.Decimal, project.TotalOrderPrice);
            db.AddInParameter(command, "Currency", DbType.String, project.Currency);
			db.AddInParameter(command, "CreatedBy", DbType.Guid, project.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, project.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, project.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, project.UpdatedDateTime);
            db.AddInParameter(command, "ServiceRequestNumber", DbType.String, project.ServiceRequestNumber);
            db.AddInParameter(command, "OrderOwner", DbType.String, project.OrderOwner);
			db.AddInParameter(command, "OverrideAutoComplete", DbType.Boolean, project.OverrideAutoComplete);

            return command;
        }

        private DbCommand InitializeInsertCommandProjectContact(Guid? id, IncomingOrderContact projectContact,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectContact_Insert]");

            db.AddInParameter(command, "ProjectContactId", DbType.Guid, projectContact.Id);
            db.AddInParameter(command, "ProjectId", DbType.Guid, projectContact.IncomingOrderId);
            db.AddInParameter(command, "FullName", DbType.String, projectContact.FullName);
            db.AddInParameter(command, "Title", DbType.String, projectContact.Title);
            db.AddInParameter(command, "Email", DbType.String, projectContact.Email);
            db.AddInParameter(command, "Address", DbType.String, projectContact.Address);
            db.AddInParameter(command, "Phone", DbType.String, projectContact.Phone);
            db.AddInParameter(command, "State", DbType.String, projectContact.State);
            db.AddInParameter(command, "Country", DbType.String, projectContact.Country);
            db.AddInParameter(command, "ContactRoleId", DbType.Int16, (Int16) projectContact.ContactRoleId);
            db.AddInParameter(command, "City", DbType.String, projectContact.City);
            db.AddInParameter(command, "Province", DbType.String, projectContact.Province);
            db.AddInParameter(command, "PostalCode", DbType.String, projectContact.PostalCode);
            db.AddInParameter(command, "CompanyName", DbType.String, projectContact.CompanyName);
            db.AddInParameter(command, "ExternalId", DbType.String, projectContact.ExternalId);
            db.AddInParameter(command, "SubscriberNumber", DbType.String, projectContact.SubscriberNumber);
            db.AddInParameter(command, "PartySiteNumber", DbType.String, projectContact.PartySiteNumber);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, projectContact.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, projectContact.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, projectContact.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, projectContact.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializeInsertCommandProjectCustomer(Guid? id, IncomingOrderCustomer projectCustomer,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectCustomer_Insert]");

            db.AddInParameter(command, "ProjectCustomerId", DbType.Guid, projectCustomer.Id);
            db.AddInParameter(command, "ProjectId", DbType.Guid, projectCustomer.IncomingOrderId);
            db.AddInParameter(command, "ProjectName", DbType.String, projectCustomer.ProjectName);
            db.AddInParameter(command, "DUNS", DbType.String, projectCustomer.DUNS);
            db.AddInParameter(command, "Name", DbType.String, projectCustomer.Name);
            db.AddInParameter(command, "ExternalId", DbType.String, projectCustomer.ExternalId);
            db.AddInParameter(command, "AgentDetails", DbType.String, projectCustomer.SubscriberNumber);
            db.AddInParameter(command, "SubscriberNumber", DbType.String, projectCustomer.Name);
            db.AddInParameter(command, "CreatedBy", DbType.Guid, projectCustomer.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, projectCustomer.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, projectCustomer.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, projectCustomer.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializeInsertCommandProjectServiceLine(Guid? id, IncomingOrderServiceLine serviceLine,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectServiceLine_Insert]");

            db.AddInParameter(command, "ProjectServiceLineId", DbType.Guid, serviceLine.Id);
            db.AddInParameter(command, "ProjectId", DbType.Guid, serviceLine.IncomingOrderId);
            db.AddInParameter(command, "Quantity", DbType.Int32, serviceLine.Quantity);
            db.AddInParameter(command, "AllowChargesFromOtherOperatingUnits", DbType.String,
                serviceLine.AllowChargesFromOtherOperatingUnits);
            db.AddInParameter(command, "BillableExpenses", DbType.String, serviceLine.BillableExpenses);
            db.AddInParameter(command, "CustomerModelNumber", DbType.String, serviceLine.CustomerModelNumber);
            db.AddInParameter(command, "ExternalId", DbType.String, serviceLine.ExternalId);
            db.AddInParameter(command, "LineNumber", DbType.String, serviceLine.LineNumber);
            db.AddInParameter(command, "ParentExternalId", DbType.String, serviceLine.ParentExternalId);
            db.AddInParameter(command, "Name", DbType.String, serviceLine.Name);
            db.AddInParameter(command, "TypeCode", DbType.String, serviceLine.TypeCode);
            db.AddInParameter(command, "ItemCategories", DbType.String, serviceLine.ItemCategories);
            db.AddInParameter(command, "FulfillmentMethodCode", DbType.String, serviceLine.FulfillmentMethodCode);
            db.AddInParameter(command, "FulfillmentSet", DbType.String, serviceLine.FulfillmentSet);
            db.AddInParameter(command, "ConfigurationId", DbType.String, serviceLine.ConfigurationId);
            db.AddInParameter(command, "Status", DbType.String, serviceLine.Status);
            db.AddInParameter(command, "PromiseDate", DbType.DateTime2, serviceLine.PromiseDate);
            db.AddInParameter(command, "RequestDate", DbType.DateTime2, serviceLine.RequestDate);
            db.AddInParameter(command, "StartDate", DbType.DateTime2, serviceLine.StartDate);
            db.AddInParameter(command, "Billable", DbType.String, serviceLine.Billable);
            db.AddInParameter(command, "Program", DbType.String, serviceLine.Program);
            db.AddInParameter(command, "Category", DbType.String, serviceLine.Category);
            db.AddInParameter(command, "SubCategory", DbType.String, serviceLine.SubCategory);
            db.AddInParameter(command, "Segment", DbType.String, serviceLine.Segment);
            db.AddInParameter(command, "Description", DbType.String, serviceLine.Description);
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
			db.AddInParameter(command, "CreatedBy", DbType.Guid, serviceLine.CreatedById);
            db.AddInParameter(command, "CreatedOn", DbType.DateTime2, serviceLine.CreatedDateTime);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, serviceLine.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, serviceLine.UpdatedDateTime);

            return command;
        }

        private DbCommand InitializeUpdateProjectServiceLineStatusFromOrder(Guid? id,
            IncomingOrderServiceLine serviceLine,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectServiceLine_UpdateStatusFromOrder]");

            db.AddInParameter(command, "ProjectServiceLineId", DbType.Guid, serviceLine.Id);
            db.AddInParameter(command, "Status", DbType.String, serviceLine.Status);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, serviceLine.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, serviceLine.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeFetchById(Database db, Guid entityId)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_GetById]");
            db.AddInParameter(command, "ProjectId", DbType.Guid, entityId);
            return command;
        }

        private DbCommand InitializeFetchByOrderNumber(Database db, string orderNumber)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_GetByOrderNumber]");
            db.AddInParameter(command, "OrderNumber", DbType.String, orderNumber);
            return command;
        }

        private static DbCommand InitializeRemoveCommand(Guid? unsedId, Guid id, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_Delete]");
            db.AddInParameter(command, "ProjectId", DbType.Guid, id);
            return command;
        }

        private static DbCommand InitializeUpdateCommandProject(Guid? id, Project project, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_Update]");

            db.AddInParameter(command, "ProjectId", DbType.Guid, id);
            db.AddInParameter(command, "Description", DbType.String, project.Description);
            db.AddInParameter(command, "StartDate", DbType.DateTime2, project.StartDate);
            db.AddInParameter(command, "EstimatedTATDate", DbType.DateTime2, project.EstimatedTATDate);
            db.AddInParameter(command, "EndDate", DbType.DateTime2, project.EndDate);
            db.AddInParameter(command, "ProjectHandler", DbType.String, project.ProjectHandler);
            db.AddInParameter(command, "EstimatedLabEffort", DbType.Double, project.EstimatedLabEffort);
            db.AddInParameter(command, "EstimateEngineeringEffort", DbType.Decimal, project.EstimateEngineeringEffort);
            db.AddInParameter(command, "Scope", DbType.String, project.Scope);
            db.AddInParameter(command, "Assumptions", DbType.String, project.Assumptions);
            db.AddInParameter(command, "EngineeringOfficeLimitations", DbType.String,
                project.EngineeringOfficeLimitations);
            db.AddInParameter(command, "LaboratoryLimitations", DbType.String, project.LaboratoryLimitations);
            db.AddInParameter(command, "Complexity", DbType.String, project.Complexity);
            db.AddInParameter(command, "AdditionalCriteria", DbType.String, project.AdditionalCriteria);
            db.AddInParameter(command, "Industry", DbType.String, project.Industry);
            db.AddInParameter(command, "IndustryCode", DbType.String, project.IndustryCode);
            db.AddInParameter(command, "IndustryCategory", DbType.String, project.IndustryCategory);
            db.AddInParameter(command, "IndustrySubcategory", DbType.String, project.IndustrySubcategory);
            db.AddInParameter(command, "Location", DbType.String, project.Location);
            db.AddInParameter(command, "ProductGroup", DbType.String, project.ProductGroup);
            db.AddInParameter(command, "StatusDescription", DbType.String, project.StatusDescription);
            db.AddInParameter(command, "ProjectStatus", DbType.String, project.ProjectStatus.ToString());
            db.AddInParameter(command, "BusinessUnit", DbType.String, project.BusinessUnit);
            db.AddInParameter(command, "ProjectHeaderStatus", DbType.String, project.ProjectHeaderStatus);
            db.AddInParameter(command, "CreationDate", DbType.DateTime2, project.CreationDate);
            db.AddInParameter(command, "CustomerRequestedDate", DbType.DateTime2, project.CustomerRequestedDate);
            db.AddInParameter(command, "DateBooked", DbType.DateTime2, project.DateBooked);
            db.AddInParameter(command, "DateOrdered", DbType.DateTime2, project.DateOrdered);
            db.AddInParameter(command, "LastUpdateDate", DbType.DateTime2, project.LastUpdateDate);
            db.AddInParameter(command, "ExternalProjectId", DbType.String, project.ExternalProjectId);
            db.AddInParameter(command, "Name", DbType.String, project.Name);
            db.AddInParameter(command, "ProjectName", DbType.String, project.ProjectName);
            db.AddInParameter(command, "ProjectNumber", DbType.String, project.ProjectNumber);
            db.AddInParameter(command, "CompanyId", DbType.Guid, project.CompanyId);
            db.AddInParameter(command, "OrderNumber", DbType.String, project.OrderNumber);
            db.AddInParameter(command, "OrderType", DbType.String, project.OrderType);
            db.AddInParameter(command, "CustomerPo", DbType.String, project.CustomerPo);
            db.AddInParameter(command, "Status", DbType.String, project.Status);
            db.AddInParameter(command, "ContainerId", DbType.Guid, project.ContainerId);           
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, project.IncomingOrderId);
            db.AddInParameter(command, "OriginalXmlParsed", DbType.String, project.OriginalXmlParsed);
            db.AddInParameter(command, "WorkOrderBusinessComponentId", DbType.String,
                project.WorkOrderBusinessComponentId);
            db.AddInParameter(command, "WorkOrderId", DbType.String, project.WorkOrderId);
            db.AddInParameter(command, "CompletionDate", DbType.DateTime2, project.CompletionDate);
            db.AddInParameter(command, "DaysInCurrentPhase", DbType.Int32, project.DaysInCurrentPhase);
            db.AddInParameter(command, "EstimatedReviewerEffort", DbType.Decimal, project.EstimatedReviewerEffort);
            db.AddInParameter(command, "NumberOfSamples", DbType.Int32, project.NumberOfSamples);
            db.AddInParameter(command, "SampleReferenceNumbers", DbType.String, project.SampleReferenceNumbers);
            db.AddInParameter(command, "CCN", DbType.String, project.CCN);
            db.AddInParameter(command, "FileNo", DbType.String, project.FileNo);
            db.AddInParameter(command, "StatusNotes", DbType.String, project.StatusNotes);
            db.AddInParameter(command, "InventoryItemCatalogNumbers", DbType.String, project.InventoryItemCatalogNumbers);
            db.AddInParameter(command, "InventoryItemNumbersDescriptions", DbType.String,
                project.InventoryItemNumbersDescriptions);
            db.AddInParameter(command, "QuoteNo", DbType.String, project.QuoteNo);
            db.AddInParameter(command, "Expedited", DbType.Boolean, project.Expedited);
            db.AddInParameter(command, "Price", DbType.String, project.Price);

            db.AddInParameter(command, "Standards", DbType.String, project.Standards);
            db.AddInParameter(command, "ProjectType", DbType.String, project.ProjectType);
            db.AddInParameter(command, "ServiceCode", DbType.String, project.ServiceCode);
			db.AddInParameter(command, "ServiceDescription", DbType.String, project.ServiceDescription);
			db.AddInParameter(command, "TaskMinimumDueDate", DbType.DateTime2, project.TaskMinimumDueDate);
			db.AddInParameter(command, "MinimumDueDateTaskId", DbType.Guid, project.MinimumDueDateTaskId);
			db.AddInParameter(command, "TotalOrderPrice", DbType.Decimal, project.TotalOrderPrice);
            db.AddInParameter(command, "Currency", DbType.String, project.Currency);
			db.AddInParameter(command, "UpdatedBy", DbType.Guid, project.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, project.UpdatedDateTime);
            db.AddInParameter(command, "HideFromCustomer", DbType.Boolean, project.HideFromCustomer);
            db.AddInParameter(command, "ServiceRequestNumber", DbType.String, project.ServiceRequestNumber);
            db.AddInParameter(command, "OrderOwner", DbType.String, project.OrderOwner);
			db.AddInParameter(command, "OverrideAutoComplete", DbType.Boolean, project.OverrideAutoComplete);

            return command;
        }

        private static DbCommand InitializeUpdateProjectStatusFromOrder(Guid? id, Project project, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_UpdateStatusFromOrder]");

            db.AddInParameter(command, "ProjectId", DbType.Guid, id);
            db.AddInParameter(command, "Status", DbType.String, project.Status);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, project.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, project.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeUpdateFromIncomingOrderCommandProject(Guid? id, Project project, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProject_UpdateFromIncomingOrder]");

            db.AddInParameter(command, "ProjectId", DbType.Guid, id);
            db.AddInParameter(command, "Description", DbType.String, project.Description);
            db.AddInParameter(command, "StartDate", DbType.DateTime2, project.StartDate);
            db.AddInParameter(command, "EstimatedTATDate", DbType.DateTime2, project.EstimatedTATDate);
            db.AddInParameter(command, "EndDate", DbType.DateTime2, project.EndDate);
            db.AddInParameter(command, "ProjectHandler", DbType.String, project.ProjectHandler);
            db.AddInParameter(command, "EstimatedLabEffort", DbType.Double, project.EstimatedLabEffort);
            db.AddInParameter(command, "EstimateEngineeringEffort", DbType.Decimal, project.EstimateEngineeringEffort);
            db.AddInParameter(command, "Scope", DbType.String, project.Scope);
            db.AddInParameter(command, "Assumptions", DbType.String, project.Assumptions);
            db.AddInParameter(command, "EngineeringOfficeLimitations", DbType.String,
                project.EngineeringOfficeLimitations);
            db.AddInParameter(command, "LaboratoryLimitations", DbType.String, project.LaboratoryLimitations);
            db.AddInParameter(command, "Complexity", DbType.String, project.Complexity);
            db.AddInParameter(command, "AdditionalCriteria", DbType.String, project.AdditionalCriteria);
            db.AddInParameter(command, "Industry", DbType.String, project.Industry);
            db.AddInParameter(command, "IndustryCode", DbType.String, project.IndustryCode);
            db.AddInParameter(command, "IndustryCategory", DbType.String, project.IndustryCategory);
            db.AddInParameter(command, "IndustrySubcategory", DbType.String, project.IndustrySubcategory);
            db.AddInParameter(command, "Location", DbType.String, project.Location);
            db.AddInParameter(command, "ProductGroup", DbType.String, project.ProductGroup);
            db.AddInParameter(command, "StatusDescription", DbType.String, project.StatusDescription);
            db.AddInParameter(command, "ProjectStatus", DbType.String, project.ProjectStatus.ToString());
            db.AddInParameter(command, "BusinessUnit", DbType.String, project.BusinessUnit);
            db.AddInParameter(command, "ProjectHeaderStatus", DbType.String, project.ProjectHeaderStatus);
            db.AddInParameter(command, "CreationDate", DbType.DateTime2, project.CreationDate);
            db.AddInParameter(command, "CustomerRequestedDate", DbType.DateTime2, project.CustomerRequestedDate);
            db.AddInParameter(command, "DateBooked", DbType.DateTime2, project.DateBooked);
            db.AddInParameter(command, "DateOrdered", DbType.DateTime2, project.DateOrdered);
            db.AddInParameter(command, "LastUpdateDate", DbType.DateTime2, project.LastUpdateDate);
            db.AddInParameter(command, "ExternalProjectId", DbType.String, project.ExternalProjectId);
            db.AddInParameter(command, "Name", DbType.String, project.Name);
            db.AddInParameter(command, "ProjectName", DbType.String, project.ProjectName);
            db.AddInParameter(command, "ProjectNumber", DbType.String, project.ProjectNumber);
            db.AddInParameter(command, "CompanyId", DbType.Guid, project.CompanyId);
            db.AddInParameter(command, "OrderNumber", DbType.String, project.OrderNumber);
            db.AddInParameter(command, "OrderType", DbType.String, project.OrderType);
            db.AddInParameter(command, "CustomerPo", DbType.String, project.CustomerPo);
            db.AddInParameter(command, "Status", DbType.String, project.Status);
            db.AddInParameter(command, "ContainerId", DbType.Guid, project.ContainerId);            
            db.AddInParameter(command, "IncomingOrderId", DbType.Guid, project.IncomingOrderId);
            db.AddInParameter(command, "OriginalXmlParsed", DbType.String, project.OriginalXmlParsed);
            db.AddInParameter(command, "WorkOrderBusinessComponentId", DbType.String,
                project.WorkOrderBusinessComponentId);
            db.AddInParameter(command, "WorkOrderId", DbType.String, project.WorkOrderId);
            db.AddInParameter(command, "CompletionDate", DbType.DateTime2, project.CompletionDate);
            db.AddInParameter(command, "DaysInCurrentPhase", DbType.Int32, project.DaysInCurrentPhase);
            db.AddInParameter(command, "EstimatedReviewerEffort", DbType.Decimal, project.EstimatedReviewerEffort);
            db.AddInParameter(command, "NumberOfSamples", DbType.Int32, project.NumberOfSamples);
            db.AddInParameter(command, "SampleReferenceNumbers", DbType.String, project.SampleReferenceNumbers);
            db.AddInParameter(command, "CCN", DbType.String, project.CCN);
            db.AddInParameter(command, "FileNo", DbType.String, project.FileNo);
            db.AddInParameter(command, "StatusNotes", DbType.String, project.StatusNotes);
            db.AddInParameter(command, "InventoryItemCatalogNumbers", DbType.String, project.InventoryItemCatalogNumbers);
            db.AddInParameter(command, "InventoryItemNumbersDescriptions", DbType.String,
                project.InventoryItemNumbersDescriptions);
            db.AddInParameter(command, "QuoteNo", DbType.String, project.QuoteNo);
            db.AddInParameter(command, "Expedited", DbType.Boolean, project.Expedited);
            db.AddInParameter(command, "Price", DbType.String, project.Price);
            db.AddInParameter(command, "Standards", DbType.String, project.Standards);
            db.AddInParameter(command, "ProjectType", DbType.String, project.ProjectType);
            db.AddInParameter(command, "ServiceCode", DbType.String, project.ServiceCode);
			db.AddInParameter(command, "ServiceDescription", DbType.String, project.ServiceDescription);
			db.AddInParameter(command, "MessageId", DbType.String, project.MessageId);
			db.AddInParameter(command, "TotalOrderPrice", DbType.Decimal, project.TotalOrderPrice);
            db.AddInParameter(command, "Currency", DbType.String, project.Currency);
			db.AddInParameter(command, "UpdatedBy", DbType.Guid, project.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, project.UpdatedDateTime);
            db.AddInParameter(command, "ServiceRequestNumber", DbType.String, project.ServiceRequestNumber);
            db.AddInParameter(command, "OrderOwner", DbType.String, project.OrderOwner);

            return command;
        }

        private static DbCommand InitializeUpdateCommandProjectContact(Guid? id, IncomingOrderContact projectContact,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectContact_Update]");

            db.AddInParameter(command, "ProjectContactId", DbType.Guid, projectContact.Id);
            db.AddInParameter(command, "ProjectId", DbType.Guid, projectContact.IncomingOrderId);
            db.AddInParameter(command, "FullName", DbType.String, projectContact.FullName);
            db.AddInParameter(command, "Title", DbType.String, projectContact.Title);
            db.AddInParameter(command, "Email", DbType.String, projectContact.Email);
            db.AddInParameter(command, "Address", DbType.String, projectContact.Address);
            db.AddInParameter(command, "Phone", DbType.String, projectContact.Phone);
            db.AddInParameter(command, "State", DbType.String, projectContact.State);
            db.AddInParameter(command, "Country", DbType.String, projectContact.Country);
            db.AddInParameter(command, "ContactRoleId", DbType.Int16, (Int16) projectContact.ContactRoleId);
            db.AddInParameter(command, "City", DbType.String, projectContact.City);
            db.AddInParameter(command, "Province", DbType.String, projectContact.Province);
            db.AddInParameter(command, "PostalCode", DbType.String, projectContact.PostalCode);
            db.AddInParameter(command, "CompanyName", DbType.String, projectContact.CompanyName);
            db.AddInParameter(command, "ExternalId", DbType.String, projectContact.ExternalId);
            db.AddInParameter(command, "SubscriberNumber", DbType.String, projectContact.SubscriberNumber);
            db.AddInParameter(command, "PartySiteNumber", DbType.String, projectContact.PartySiteNumber);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, projectContact.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, projectContact.UpdatedDateTime);

            return command;
        }

        private static DbCommand InitializeUpdateCommandProjectContactByExternalId(string externalId,
            IncomingOrderContact projectContact,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectContact_UpdateByExternalId]");

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

        private static DbCommand InitializeUpdateCommandProjectCustomer(Guid? id, IncomingOrderCustomer projectCustomer,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectCustomer_Update]");

            db.AddInParameter(command, "ProjectCustomerId", DbType.Guid, projectCustomer.Id);
            db.AddInParameter(command, "ProjectId", DbType.Guid, projectCustomer.IncomingOrderId);
            db.AddInParameter(command, "ProjectName", DbType.String, projectCustomer.ProjectName);
            db.AddInParameter(command, "DUNS", DbType.String, projectCustomer.DUNS);
            db.AddInParameter(command, "Name", DbType.String, projectCustomer.Name);
            db.AddInParameter(command, "ExternalId", DbType.String, projectCustomer.ExternalId);
            db.AddInParameter(command, "AgentDetails", DbType.String, projectCustomer.SubscriberNumber);
            db.AddInParameter(command, "SubscriberNumber", DbType.String, projectCustomer.Name);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, projectCustomer.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, projectCustomer.UpdatedDateTime);

            return command;
        }


        private static DbCommand InitializeUpdateCommandProjectCustomerByExternalId(string externalId,
            IncomingOrderCustomer projectCustomer,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectCustomer_UpdateByExternalId]");

            db.AddInParameter(command, "DUNS", DbType.String, projectCustomer.DUNS);
            db.AddInParameter(command, "Name", DbType.String, projectCustomer.Name);
            db.AddInParameter(command, "ExternalId", DbType.String, externalId);
            db.AddInParameter(command, "UpdatedBy", DbType.Guid, projectCustomer.UpdatedById);
            db.AddInParameter(command, "UpdatedOn", DbType.DateTime2, projectCustomer.UpdatedDateTime);

            return command;
        }


        private static DbCommand InitializeDeleteCommandProjectServiceLineByProjectId(Guid? id, IncomingOrder project,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pProjectServiceLine_DeleteByProjectId]");

            db.AddInParameter(command, "ProjectId", DbType.Guid, project.Id);

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