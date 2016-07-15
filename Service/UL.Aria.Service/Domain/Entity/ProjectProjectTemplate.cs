using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    ///     Project ProjectTemplate class.
    /// </summary>
    public class ProjectProjectTemplate : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

        /// <summary>
        ///     Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        ///     The parent identifier.
        /// </value>
        public Guid ParentId
        {
            get { return ProjectId; }
            set { ProjectId = value; }
        }

        /// <summary>
        ///     Gets or sets the project identifier.
        /// </summary>
        /// <value>
        ///     The project identifier.
        /// </value>
        public Guid ProjectId { get; set; }

        /// <summary>
        ///     Gets or sets the project template identifier.
        /// </summary>
        /// <value>
        ///     The project template identifier.
        /// </value>
        public Guid ProjectTemplateId { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is part of the templates originally used when the project was
        ///     created.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is original; otherwise, <c>false</c>.
        /// </value>
        public bool IsOriginal { get; set; }

        /// <summary>
        ///     Gets or sets the Record Version.
        /// </summary>
        /// <value>
        ///     Timestamp.
        /// </value>
        public byte[] RecordVersion { get; set; }

        /// <summary>
        ///     Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        ///     The type of the entity.
        /// </value>
        public EntityTypeEnumDto? EntityType
        {
            get { return EntityTypeEnumDto.ProjectProjectTemplate; }
        }

        /// <summary>
        ///     Gets or sets the change date.
        /// </summary>
        /// <value>
        ///     The change date.
        /// </value>
        DateTime ISearchResult.ChangeDate
        {
            get { return UpdatedDateTime; }
        }

        /// <summary>
        ///     Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        ///     The type of the entity.
        /// </value>
        string ISearchResult.EntityType
        {
            get { return EntityType.ToString(); }
        }

        /// <summary>
        ///     Gets or sets the metadata dictionary.
        /// </summary>
        /// <value>
        ///     The metadata.
        /// </value>
        IDictionary<string, string> ISearchResult.Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        string ISearchResult.Name
        {
            get { return Id.GetValueOrDefault().ToString(); }
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        string ISearchResult.Title
        {
            get { return Id.GetValueOrDefault().ToString(); }
        }
    }
}