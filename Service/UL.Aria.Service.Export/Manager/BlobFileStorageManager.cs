using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    ///     Implements operations for storing files.
    /// </summary>
    [ExcludeFromCodeCoverage] // JML - low level api calls to azure storage, not unit testable.
    public class BlobFileStorageManager : IFileStorageManager
    {
        private readonly IExportConfiguration _exportConfiguration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlobFileStorageManager" /> class.
        /// </summary>
        /// <param name="exportConfiguration">The export configuration.</param>
        public BlobFileStorageManager(IExportConfiguration exportConfiguration)
        {
            _exportConfiguration = exportConfiguration;
        }

        /// <summary>
        ///     Puts the specified stream.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public void Save(string path, string name, Stream content)
        {
            CloudBlobDirectory container = GetUserStorage(path);

            CloudBlockBlob blob = container.GetBlockBlobReference(name);
            blob.UploadFromStream(content);
            blob.Metadata.Add("LocalName", Path.GetFileNameWithoutExtension(name));
            blob.Metadata.Add("Extension", Path.GetExtension(name));
            blob.Metadata.Add("Time", DateTime.UtcNow.ToString());
            blob.SetMetadata();
        }

        /// <summary>
        ///     Gets the specified stream.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Stream Get(string path, string name)
        {
            CloudBlobDirectory userStorage = GetUserStorage(path);
            CloudBlockBlob blob = userStorage.GetBlockBlobReference(name);
            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            stream.Position = 0;
            return stream;
        }

        /// <summary> 
        ///     Removes the specified file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name">The name.</param>
        public void Remove(string path, string name)
        {
            CloudBlobDirectory userStorage = GetUserStorage(path);

            CloudBlockBlob blob = userStorage.GetBlockBlobReference(name);
            blob.Delete();
        }

        private CloudBlobDirectory GetUserStorage(string directoryName)
        {
            CloudBlobContainer container = GetContainer();

            CloudBlobDirectory directory = container.GetDirectoryReference(directoryName);

            return directory;
        }

        private CloudBlobContainer GetContainer()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(_exportConfiguration.StorageConnectionString);

            CloudBlobClient client = account.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(_exportConfiguration.StorageContainer.ToLower());
            container.CreateIfNotExists();
            return container;
        }
    }
}