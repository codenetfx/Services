using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides an implemenation of the Notification Repository interface.
    /// </summary>
    public class NotificationRepository : TrackedDomainEntityRepositoryBase<Notification>, INotificationRepository
    {
        private static Dictionary<string, PropertyInfo> NotificationPropertyLookup;
        private static object pad_lock = new object();
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
        /// </summary>
        public NotificationRepository() : base("NotificationId", "Notification") { }


        /// <summary>
        /// Fetches all active notifications associated with the specified userId.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IEnumerable<Notification> FetchNotificationsByUser(Guid userId)
        {
            return ExecuteReaderCommand(
                database =>
                {
                    return InitializeFindAllByCommand(userId, "UserId", database);
                },
                ConstructEntity);
        }

        /// <summary>
        /// Fetches all active notifications associated with the specified entityId.
        /// </summary>
        /// <param name="entiyId">The entiy identifier.</param>
        /// <returns></returns>
        public IEnumerable<Notification> FetchNotificationsByEntity(Guid entiyId)
        {
            return ExecuteReaderCommand(
                database =>
                {
                    return InitializeFindAllByCommand(entiyId, "EntityId", database);
                },
                this.ConstructEntity);
        }

        /// <summary>
        /// Removes the notifications for the specified entityId from the database.
        /// </summary>
        /// <param name="entityId"></param>
        public void RemoveNotificationsForEntity(Guid entityId)
        {
            var hResult = ExecuteNonQueryCommand(
              database =>
              {
                  return InitializeDeleteAllByCommand(entityId, "EntityId", database);
              });
        }

        /// <summary>
        /// Updates the list of notifications in the bulk for the specified entityId.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        /// <param name="entityId">The entity identifier.</param>
        public void UpdateBulk(IEnumerable<Notification> notifications, Guid entityId)
        {
            base.Update(notifications);
        }

        private static DbCommand InitializeFindAllByCommand(Guid relationId, string relationName, Database db)
        {
            var cmd = db.GetStoredProcCommand("[dbo].[pNotification_GetBy]");
            db.AddInParameter(cmd, relationName, DbType.Guid, relationId);
            return cmd;
        }

        private static DbCommand InitializeDeleteAllByCommand(Guid relationId, string relationName, Database db)
        {
            var cmd = db.GetStoredProcCommand("[dbo].[pNotification_DeleteBy]");
            db.AddInParameter(cmd, relationName, DbType.Guid, relationId);
            return cmd;
        }



        /// <summary>
        /// Adds the table row fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="isNew">if set to <c>true</c> [is new].</param>
        /// <param name="isDirty">if set to <c>true</c> [is dirty].</param>
        /// <param name="isDelete">if set to <c>true</c> [is delete].</param>
        /// <param name="dr">The dr.</param>
        protected override void AddTableRowFields(Notification entity, bool isNew, bool isDirty, bool isDelete, DataRow dr)
        {
            AssureTypeLookup();
            foreach (DataColumn col in dr.Table.Columns)
            {
                if (NotificationPropertyLookup.ContainsKey(col.ColumnName))
                {
                    var tempVal =NotificationPropertyLookup[col.ColumnName].GetValue(entity);
                    dr[col.ColumnName] = (tempVal == null) ? DBNull.Value : tempVal;
                }
                 
            }

            base.AddTableRowFields(entity, isNew, isDirty, isDelete, dr);
        }

        private void AssureTypeLookup()
        {
            if (NotificationPropertyLookup == null)
            {
                lock (pad_lock)
                {
                    var flags = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;

                    if (NotificationPropertyLookup == null)
                        NotificationPropertyLookup = typeof(Notification).GetProperties(flags).ToDictionary(x => x.Name);
                }
            }
        }

        /// <summary>
        /// Constructs the entity.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected override Notification ConstructEntity(IDataReader reader)
        {
            var entity = base.ConstructEntity(reader);
            entity.Body = reader.GetValue<string>("Body");
            entity.EndDate = reader.GetValue<DateTime>("EndDate");
            entity.EntityId = reader.GetValue<Guid>("EntityId");
            entity.EntityType = (EntityTypeEnumDto)Enum.Parse(typeof(EntityTypeEnumDto), reader.GetValue<string>("EntityType"));
            entity.NotificationType = (NotificationTypeDto)Enum.Parse(typeof(NotificationTypeDto), reader.GetValue<int>("NotificationType").ToString());
            entity.StartDate = reader.GetValue<DateTime>("StartDate");
            entity.UserId = reader.GetValue<Guid?>("UserId");
            entity.ContainerId = reader.GetValue<Guid?>("ContainerId");
            return entity;
        }

    }
}
