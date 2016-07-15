using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Framework;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     The content repository
    /// </summary>
    public sealed class ContainerRepository : RepositoryBase<Container>, IContainerRepository
    {
        private readonly ITransactionFactory _transactionFactory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerRepository" /> class.
        /// </summary>
        /// <param name="transactionFactory">The transaction factory.</param>
        public ContainerRepository(ITransactionFactory transactionFactory) : this("ContainerId")
        {
            _transactionFactory = transactionFactory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ContainerRepository" /> class.
        /// </summary>
        /// <param name="dbIdFieldName">Name of the db id field.</param>
        private ContainerRepository(string dbIdFieldName) : base(dbIdFieldName)
        {
        }


        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <returns>IList{Container}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IList<Container> FindAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Finds the by id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <returns>Container.</returns>
        public override Container FindById(Guid entityId)
        {
            return GetById(entityId);
        }

        /// <summary>
        ///     Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Add(Container entity)
        {
            Create(entity);
        }

        /// <summary>
        ///     Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Guid.</returns>
        public Guid Create(Container entity)
        {
            //Guard.IsNotEmptyGuid(entity.PrimarySearchEntityId, "entityId");
            var id = Guid.Empty;
            using (var transactionScope = _transactionFactory.Create())
            {
                ExecuteNonQueryCommand(db => InitializeInsertCommand(entity, db), entity,
                    cmd => { id = (Guid) cmd.Parameters["@ContainerId"].Value; });
                CreateChildren(id, entity);
                transactionScope.Complete();
            }
            return id;
        }

        /// <summary>
        ///     Removes the specified entity id.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        public override void Remove(Guid entityId)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                using (var command = InitializeRemoveCommand(entityId, db))
                {
                    db.ExecuteNonQuery(command);
                }
                transactionScope.Complete();
            }
        }

        /// <summary>
        ///     Deletes the list.
        /// </summary>
        /// <param name="entityId">The entity id.</param>
        /// <param name="name">The name.</param>
        public void DeleteList(Guid entityId, string name)
        {
            using (var transactionScope = _transactionFactory.Create())
            {
                var db = DatabaseFactory.CreateDatabase();

                using (var command = InitializeDeleteListCommand(entityId, db, name))
                {
                    db.ExecuteNonQuery(command);
                }
                transactionScope.Complete();
            }
        }

		/// <summary>
		/// Gets the by entity identifier.
		/// </summary>
		/// <param name="primarySearchEntityId">The primary search entity identifier.</param>
		/// <returns>Container.</returns>
	    public Container GetByPrimarySearchEntityId(Guid primarySearchEntityId)
	    {
			IList<Container> results;
			using (var transactionScope = _transactionFactory.Create())
			{
				results =
					ExecuteReaderCommand(database => InitializedGetByPrimarySearchEntityIdCommand(primarySearchEntityId, database),
						ConstructEntity);
				transactionScope.Complete();
			}
			return results.Count == 0 ? null : results[0];
		}

	    /// <summary>
        ///     Gets the by id.
        /// </summary>
        /// <param name="containerId">The container id.</param>
        /// <returns>Container.</returns>
        public Container GetById(Guid containerId)
        {
            IList<Container> results;
            using (var transactionScope = _transactionFactory.Create())
            {
                results =
                    ExecuteReaderCommand(database => InitializedGetByIdCommand(containerId, database),
                        ConstructEntity);
                transactionScope.Complete();
            }
            return results.Count == 0 ? null : results[0];
        }

        /// <summary>
        ///     Gets the by company id.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <returns>IEnumerable{Container}.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<Container> GetByCompanyId(Guid companyId)
        {
            IEnumerable<Container> containers;
            using (var transactionScope = _transactionFactory.Create())
            {
                containers = ExecuteReaderCommand(database => InitializedGetByCompanyIdCommand(companyId, database),
                    ConstructEntityCompany);
                transactionScope.Complete();
            }
            return containers;
        }

        /// <summary>
        ///     Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>System.Int32.</returns>
        public override int Update(Container entity)
        {
            Guard.IsNotNull(entity, "entity");
            int count = 1;
			// Removed to address  Production issues PRB0043154/CHG0050564
			// Because claims on containers are immutable we do not ever need nor want to update containers once created.
//			using (var transactionScope = _transactionFactory.Create())
//			{
//// ReSharper disable PossibleInvalidOperationException
//				RemoveChildren(entity.Id.Value, entity.CompanyId.Value);
//				count = ExecuteNonQueryCommand(db => InitializedUpdateCommand(entity, db), entity);
//				CreateChildren(entity.Id.Value, entity);
//				transactionScope.Complete();
//			}
            return count;
        }

        private void CreateChildren(Guid id, Container entity)
        {
            foreach (var availableClaim in entity.AvailableClaims)
            {
                var claim = availableClaim.Claim;
                Int64 claimId = 0;
                ExecuteNonQueryCommand(db => InitializeInsertCommandClaim(claim, db), claim,
                    cmd =>
                    {
                        claimId =
                            (Int64) cmd.Parameters["@ClaimId"].Value;
                    });
                availableClaim.Claim.Id = claimId;
                availableClaim.ContainerId = id;
                Int64 availableClaimId = 0;
// ReSharper disable once AccessToForEachVariableInClosure
                ExecuteNonQueryCommand(db => InitializeInsertCommandAvailableClaim(availableClaim, db), availableClaim,
                    cmd =>
                    {
                        availableClaimId =
                            (Int64) cmd.Parameters["@ContainerAvailableClaimId"].Value;
                    });
                availableClaim.Id = availableClaimId;
            }

            foreach (var containerList in entity.ContainerLists)
            {
                containerList.ContainerId = id;
                Int64 containerListId = 0;
// ReSharper disable once AccessToForEachVariableInClosure
                ExecuteNonQueryCommand(db => InitializeInsertCommandContainerList(containerList, db), containerList,
                    cmd => { containerListId = (Int64) cmd.Parameters["@ContainerListId"].Value; });
                containerList.Id = containerListId;

                foreach (var containerListPermission in containerList.Permissions)
                {
                    containerListPermission.ContainerListId = containerListId;
                    var availableClaim =
                        entity.AvailableClaims.FirstOrDefault(
                            x =>
                                x.Claim.Uri == containerListPermission.Claim.Type &&
                                x.Value == containerListPermission.Claim.Value);
// ReSharper disable once PossibleNullReferenceException
                    containerListPermission.ContainerAvailableClaimId = availableClaim.Id;
                    ExecuteNonQueryCommand(
// ReSharper disable once AccessToForEachVariableInClosure
                        db => InitializeInsertCommandContainerListPermission(containerListPermission, db),
                        containerListPermission);
                }
            }
        }

		//private void RemoveChildren(Guid entityId, Guid companyId)
		//{
		//	var db = DatabaseFactory.CreateDatabase();

		//	using (var command = InitializeRemoveCommandChildren(entityId, companyId, db))
		//	{
		//		db.ExecuteNonQuery(command);
		//	}
		//}

        private DbCommand InitializeRemoveCommand(Guid entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainer_Delete]");

            db.AddInParameter(command, "ContainerId", DbType.Guid, entity);

            return command;
        }

        private DbCommand InitializeDeleteListCommand(Guid entity, Database db, string name)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainerList_Delete]");

            db.AddInParameter(command, "ContainerId", DbType.Guid, entity);
            db.AddInParameter(command, "Name", DbType.String, name);

            return command;
        }

		//private DbCommand InitializeRemoveCommandChildren(Guid entity, Guid companyId, Database db)
		//{
		//	var command = db.GetStoredProcCommand("[dbo].[pContainer_DeleteChildren]");

		//	db.AddInParameter(command, "ContainerId", DbType.Guid, entity);
		//	db.AddInParameter(command, "CompanyId", DbType.Guid, companyId);

		//	return command;
		//}

        private DbCommand InitializeInsertCommand(Container entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainer_Insert]");

            db.AddInParameter(command, "@ContainerId", DbType.Guid, entity.Id);
            db.AddInParameter(command, "@CompanyId", DbType.Guid, entity.CompanyId);
            db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, entity.PrimarySearchEntityId);
            db.AddInParameter(command, "@PrimarySearchEntityType", DbType.String, entity.PrimarySearchEntityType);

            return command;
        }

        private DbCommand InitializeInsertCommandClaim(Domain.Entity.Claim claim,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pClaim_Insert]");

            db.AddOutParameter(command, "@ClaimId", DbType.Int64, 64);
            db.AddInParameter(command, "@Description", DbType.String, claim.Description);
            db.AddInParameter(command, "@Claim", DbType.String, claim.Uri);

            return command;
        }

        private DbCommand InitializeInsertCommandAvailableClaim(ContainerAvailableClaim containerAvailableClaim,
            Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainerAvailableClaims_Insert]");

            db.AddOutParameter(command, "@ContainerAvailableClaimId", DbType.Int64, 64);
            db.AddInParameter(command, "@ContainerId", DbType.Guid, containerAvailableClaim.ContainerId);
            db.AddInParameter(command, "@ClaimId", DbType.Int64, containerAvailableClaim.Claim.Id);
            db.AddInParameter(command, "@Value", DbType.String, containerAvailableClaim.Value);

            return command;
        }

        private DbCommand InitializeInsertCommandContainerList(ContainerList containerList, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainerList_Insert]");

            db.AddOutParameter(command, "@ContainerListId", DbType.Int64, 64);
            db.AddInParameter(command, "@ContainerId", DbType.Guid, containerList.ContainerId);
            db.AddInParameter(command, "@AssetType", DbType.String, containerList.AssetType);
            db.AddInParameter(command, "@Name", DbType.String, containerList.Name);

            return command;
        }

        private DbCommand InitializeInsertCommandContainerListPermission(
            ContainerListPermission containerListPermission, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainerListPermission_Insert]");

            db.AddInParameter(command, "@ContainerListId", DbType.Int64, containerListPermission.ContainerListId);
            db.AddInParameter(command, "@ContainerAvailableClaimId", DbType.Int64,
                containerListPermission.ContainerAvailableClaimId);
            db.AddInParameter(command, "@Permission", DbType.String, containerListPermission.Permission);
            db.AddInParameter(command, "@GroupName", DbType.String, containerListPermission.GroupName);

            return command;
        }

		//private DbCommand InitializedUpdateCommand(Container container, Database db)
		//{
		//	var command = db.GetStoredProcCommand("[dbo].[pContainer_Update]");

		//	db.AddInParameter(command, "@ContainerId", DbType.Guid, container.Id);
		//	db.AddInParameter(command, "@CompanyId", DbType.Guid, container.CompanyId);
		//	db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, container.PrimarySearchEntityId);
		//	db.AddInParameter(command, "@PrimarySearchEntityType", DbType.String, container.PrimarySearchEntityType);

		//	return command;
		//}

        private DbCommand InitializedGetByIdCommand(Guid containerId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainer_GetById]");

            db.AddInParameter(command, "@ContainerId", DbType.Guid, containerId);

            return command;
        }

		private DbCommand InitializedGetByPrimarySearchEntityIdCommand(Guid entityId, Database db)
		{
			var command = db.GetStoredProcCommand("[dbo].[pContainer_GetByPrimarySearchEntityId]");

			db.AddInParameter(command, "@PrimarySearchEntityId", DbType.Guid, entityId);

			return command;
		}

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Container.</returns>
        protected override Container ConstructEntity(IDataReader reader)
        {
            Container entity = base.ConstructEntity(reader);
            entity.CompanyId = reader.GetValue<Guid>("CompanyId");
            entity.PrimarySearchEntityId = reader.GetValue<Guid>("PrimarySearchEntityId");
            entity.PrimarySearchEntityType = reader.GetValue<string>("PrimarySearchEntityType");
            entity.IsDeleted = reader.GetValue<bool>("IsDeleted");

            reader.NextResult();
            while (reader.Read())
            {
                entity.AvailableClaims.Add(ConstructContainerAvailableClaim(reader));
            }

            reader.NextResult();
            while (reader.Read())
            {
                entity.ContainerLists.Add(ConstructContainerList(reader));
            }

            reader.NextResult();
            while (reader.Read())
            {
                ConstructContainerListPermissions(reader, entity);
            }

            return entity;
        }

        /// <summary>
        ///     Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Container.</returns>
        private Container ConstructEntityCompany(IDataReader reader)
        {
            Container entity = base.ConstructEntity(reader);
            entity.CompanyId = reader.GetValue<Guid>("CompanyId");
            entity.PrimarySearchEntityId = reader.GetValue<Guid>("PrimarySearchEntityId");
            entity.PrimarySearchEntityType = reader.GetValue<string>("PrimarySearchEntityType");

            return entity;
        }

        private ContainerAvailableClaim ConstructContainerAvailableClaim(IDataReader reader)
        {
            return new ContainerAvailableClaim
            {
                Id = reader.GetValue<Int64>("ContainerAvailableClaimId"),
                ContainerId = reader.GetValue<Guid>("ContainerId"),
                Claim = new Domain.Entity.Claim
                {
                    Id = reader.GetValue<Int64>("ClaimId"),
                    Description = reader.GetValue<string>("Description"),
                    Uri = reader.GetValue<string>("Claim")
                },
                Value = reader.GetValue<string>("Value")
            };
        }

        private ContainerList ConstructContainerList(IDataReader reader)
        {
            return new ContainerList
            {
                Id = reader.GetValue<Int64>("ContainerListId"),
                ContainerId = reader.GetValue<Guid>("ContainerId"),
                AssetType = reader.GetValue<string>("AssetType"),
                Name = reader.GetValue<string>("Name")
            };
        }

        private void ConstructContainerListPermissions(IDataReader reader, Container entity)
        {
            var containerListId = reader.GetValue<Int64>("ContainerListId");
            var containerList = entity.ContainerLists.FirstOrDefault(x => x.Id == containerListId);
            var containerAvailableClaimId = reader.GetValue<Int64>("ContainerAvailableClaimId");
            var containerAvailableClaim = entity.AvailableClaims.FirstOrDefault(x => x.Id == containerAvailableClaimId);
            var containerListPermission = new ContainerListPermission
            {
                ContainerListId = containerListId,
                ContainerAvailableClaimId = containerAvailableClaimId,
                Claim =
// ReSharper disable once PossibleNullReferenceException
                    new System.Security.Claims.Claim(containerAvailableClaim.Claim.Uri,
                        containerAvailableClaim.Value),
                Permission = reader.GetValue<string>("Permission"),
                GroupName = reader.GetValue<string>("GroupName")
            };
// ReSharper disable once PossibleNullReferenceException
            containerList.Permissions.Add(containerListPermission);
        }

        private DbCommand InitializedGetByCompanyIdCommand(Guid containerId, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pContainer_GetByCompanyId]");

            db.AddInParameter(command, "@CompanyId", DbType.Guid, containerId);

            return command;
        }
    }
}