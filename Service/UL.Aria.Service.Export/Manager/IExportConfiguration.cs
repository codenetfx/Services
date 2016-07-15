namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    ///     Defines configuration properties.
    /// </summary>
    public interface IExportConfiguration
    {
        /// <summary>
        ///     Gets or sets the project export path.
        /// </summary>
        /// <value>
        ///     The project export path.
        /// </value>
        string ProjectExportFile { get; }

        /// <summary>
        ///     Gets or sets the task export path.
        /// </summary>
        /// <value>
        ///     The task export path.
        /// </value>
        string TaskExportFile { get; }

        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        /// <value>
        /// The storage connection string.
        /// </value>
        string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the storage container.
        /// </summary>
        /// <value>
        /// The storage container.
        /// </value>
        string StorageContainer { get; set; }

        /// <summary>
        /// Gets or sets the project storage directory.
        /// </summary>
        /// <value>
        /// The storage directory.
        /// </value>
        string ProjectExportStorageDirectory { get; set; }
    }
}