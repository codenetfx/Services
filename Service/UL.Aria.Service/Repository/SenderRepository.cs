using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Data;

using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Class SenderRepository.
    /// </summary>
    public class SenderRepository : ISenderRepository
    {
        /// <summary>
        ///     Fetches the name of the by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Sender.</returns>
        /// <exception cref="DatabaseItemNotFoundException">sender not found;null</exception>
        public Sender FetchByName(string name)
        {
            var sender = GetByCommand(InitializeFindByIdCommand, name).FirstOrDefault();

            if (sender == null)
                throw new DatabaseItemNotFoundException(string.Format("sender not found by name: {0}", name), null);

            return sender;
        }

        /// <summary>
        ///     Creates the specified sender.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public void Create(Sender sender)
        {
            ExecuteNonQueryCommand(db => InitializeInsertCommand(sender, db));
        }

        private static DbCommand InitializeFindByIdCommand(string name, Database database)
        {
            var command = database.GetStoredProcCommand("[dbo].[pSender_GetByName]");

            database.AddInParameter(command, "Name", DbType.String, name);

            return command;
        }
        private static DbCommand InitializeInsertCommand(Sender entity, Database db)
        {
            var command = db.GetStoredProcCommand("[dbo].[pSender_Insert]");

            db.AddInParameter(command, "@SenderId", DbType.Int16, entity.Id);
            db.AddInParameter(command, "@GroupName", DbType.String, entity.GroupName);
            db.AddInParameter(command, "@Name", DbType.String, entity.Name);

            return command;
        }

        private static Sender ConstructSender(IDataReader reader)
        {
            return new Sender
            {
                Id = reader.GetValue<Int16>("SenderId"),
                GroupName = reader.GetValue<string>("GroupName"),
                Name = reader.GetValue<string>("Name")
            };
        }

        private static IEnumerable<Sender> GetByCommand(Func<string, Database, DbCommand> initializeCommand, string name)
        {
            var db = DatabaseFactory.CreateDatabase();
            var results = new List<Sender>();

            using (var command = initializeCommand(name, db))
            {
                using (var reader = db.ExecuteReader(command))
                {
                    while (reader.Read())
                    {
                        results.Add(ConstructSender(reader));
                    }
                }
            }
            return results;
        }

        private static void ExecuteNonQueryCommand(Func<Database, DbCommand> commandInitializer)
        {
            var db = DatabaseFactory.CreateDatabase();

            using (var command = commandInitializer(db))
            {
                db.ExecuteNonQuery(command);
            }
        }
    }
}