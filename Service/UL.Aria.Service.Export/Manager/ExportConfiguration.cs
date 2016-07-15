using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;

namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    ///     Implements configuration properties.
    /// </summary>
    public class ExportConfiguration : IExportConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportConfiguration"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ExportConfiguration()
            : this(ConfigurationManager.AppSettings)
        {
        }

        internal ExportConfiguration(NameValueCollection settings)
        {
            ProjectExportFile = settings.GetValue("UL.Aria.Service.Export.ProjectExportFile", @"export_{0}.txt");
            TaskExportFile = settings.GetValue("UL.Aria.Service.Export.TaskExportFile", @"exporttask_{0}.txt");
            StorageConnectionString = settings.GetValue("UL.Aria.Service.Export.StorageConnectionString", null);
            StorageContainer = settings.GetValue("UL.Aria.Service.Export.StorageContainer", null);
            ProjectExportStorageDirectory = settings.GetValue("UL.Aria.Service.Export.ProjectExportStorageDirectory", null);
        }

        /// <summary>
        /// Gets or sets the project export path.
        /// </summary>
        /// <value>
        /// The project export path.
        /// </value>
        public string ProjectExportFile { get; private set; }


        /// <summary>
        ///     Gets or sets the task export path.
        /// </summary>
        /// <value>
        ///     The task export path.
        /// </value>
        public string TaskExportFile { get; private set; }

        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        /// <value>
        /// The storage connection string.
        /// </value>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the storage container.
        /// </summary>
        /// <value>
        /// The storage container.
        /// </value>
        public string StorageContainer { get; set; }

        /// <summary>
        /// Gets or sets the storage directory for projects.
        /// </summary>
        /// <value>
        /// The storage directory.
        /// </value>
        public string ProjectExportStorageDirectory { get; set; }
    }
}