using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    ///     Implements Azure based storage for scratch space
    /// </summary>
    [ExcludeFromCodeCoverage] //JML Mostly API calls to an azure API that does not have Interfaces, so is not testable.
    public class AzureScratchSpaceRepository : IScratchSpaceRepository
    {
        internal static readonly Regex IsGuid = new Regex(@"(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}", RegexOptions.Compiled);
        private readonly IServiceConfiguration _serviceConfiguration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureScratchSpaceRepository" /> class.
        /// </summary>
        /// <param name="serviceConfiguration">The service configuration.</param>
        public AzureScratchSpaceRepository(IServiceConfiguration serviceConfiguration)
        {
            _serviceConfiguration = serviceConfiguration;
        }

        /// <summary>
        ///     Fetches all.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public IEnumerable<ScratchFileInfo> FetchAll(Guid userId)
        {
            List<ScratchFileInfo> scratchFileInfos = FetchAllWithCleanup(userId);

            return scratchFileInfos;
        }

        /// <summary>
        ///     Fetches the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileId">The file id.</param>
        /// <returns></returns>
        public Stream FetchContent(Guid userId, Guid fileId)
        {
            CloudBlobDirectory userStorage = GetUserStorage(userId);
            CloudBlockBlob blob = userStorage.GetBlockBlobReference(fileId.ToString());
            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        ///     Publishes the content.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="contentStream">The content stream.</param>
        /// <returns></returns>
        public Guid PublishContent(Guid userId, string fileName, Stream contentStream)
        {
            CloudBlobDirectory container = GetUserStorage(userId);
            Guid id = Guid.NewGuid();

            CloudBlockBlob blob = container.GetBlockBlobReference(id.ToString());
            blob.UploadFromStream(contentStream);
            CreateNewMetaData(blob, fileName);
            return id;
        }

        /// <summary>
        ///     Purges the specified user's files
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="forceDelete">True to delete all files regardless of age</param>
        public void Purge(Guid userid, bool forceDelete)
        {
            FetchAllWithCleanup(userid, forceDelete);
        }

        private List<ScratchFileInfo> FetchAllWithCleanup(Guid userId, bool purgeAll = false)
        {
            CloudBlobDirectory userStorage = GetUserStorage(userId);
            IEnumerable<IListBlobItem> blobs = userStorage.ListBlobs();

            var scratchFileInfos = new List<ScratchFileInfo>();
            foreach (IListBlobItem blobItem in blobs)
            {
                var uri = blobItem.Uri.ToString();
                var blobName = uri.Substring(uri.LastIndexOf('/') + 1);
                CloudBlockBlob blob = userStorage.GetBlockBlobReference(blobName);
                blob.FetchAttributes();
                if (!DeleteIfExpired(blob, purgeAll))
                {
                    scratchFileInfos.Add(ConstructScratchFileInfo(blob));
                }
            }
            return scratchFileInfos;
        }

        private static ScratchFileInfo ConstructScratchFileInfo(ICloudBlob blob)
        {
            var fileName = new Guid(Path.GetFileName(blob.Name));
            var scratchFileInfo = new ScratchFileInfo
            {
                Extension = blob.Metadata.GetValue("Extension", string.Empty),
                Name = blob.Metadata.GetValue("LocalName", fileName.ToString()),
                Size = blob.Properties.Length,
                Id = fileName
            };
            if (!blob.Properties.LastModified.HasValue)
            {
                return scratchFileInfo;
            }

            scratchFileInfo.CreationTime = blob.Properties.LastModified.Value.LocalDateTime;
            scratchFileInfo.CreationTimeUtc = blob.Properties.LastModified.Value.UtcDateTime;
            scratchFileInfo.LastWriteTime = blob.Properties.LastModified.Value.LocalDateTime;
            scratchFileInfo.LastWriteTimeUtc = blob.Properties.LastModified.Value.UtcDateTime;
            scratchFileInfo.LastAccessTime = blob.Properties.LastModified.Value.LocalDateTime;
            scratchFileInfo.LastAccessTimeUtc = blob.Properties.LastModified.Value.UtcDateTime;
            return scratchFileInfo;
        }

        private static void CreateNewMetaData(ICloudBlob blob, string fileName)
        {
            blob.Metadata.Add("LocalName", fileName);
            blob.Metadata.Add("Extension", Path.GetExtension(fileName));
            blob.SetMetadata();
        }

        private bool DeleteIfExpired(CloudBlockBlob blob, bool purgeAll)
        {
            if (purgeAll || DateTime.UtcNow.Subtract(blob.Properties.LastModified.Value.UtcDateTime).TotalMinutes > _serviceConfiguration.ScratchSpaceExpiration)
            {
                blob.DeleteIfExistsAsync();
                return true;
            }
            return false;
        }

        private CloudBlobDirectory GetUserStorage(Guid userId)
        {
            CloudBlobContainer container = GetContainer();

            CloudBlobDirectory directory = container.GetDirectoryReference(userId.ToString());

            return directory;
        }

        private CloudBlobContainer GetContainer()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(_serviceConfiguration.StorageConnectionString);

            CloudBlobClient client = account.CreateCloudBlobClient();

            CloudBlobContainer container = client.GetContainerReference(_serviceConfiguration.ScratchSpaceRootPath.ToLower());
            container.CreateIfNotExists();
            return container;
        }
    }
}