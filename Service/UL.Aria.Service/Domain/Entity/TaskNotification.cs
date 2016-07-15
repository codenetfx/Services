using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// Task notification class.
    /// </summary>
    [Serializable]
    public class TaskNotification : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public EntityTypeEnumDto? EntityType
        {
            get { return EntityTypeEnumDto.TaskNotification; }
        }

        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        DateTime ISearchResult.ChangeDate
        {
            get { return UpdatedDateTime; }
        }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        string ISearchResult.EntityType
        {
            get { return this.EntityType.ToString(); }
        }

        /// <summary>
        /// Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        IDictionary<string, string> ISearchResult.Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string ISearchResult.Name
        {
            get { return this.Id.GetValueOrDefault().ToString(); }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string ISearchResult.Title
        {
            get { return this.Id.GetValueOrDefault().ToString(); }
        }
    }
}
