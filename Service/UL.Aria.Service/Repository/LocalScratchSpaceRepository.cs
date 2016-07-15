using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using UL.Aria.Common;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Conrete implementation of ScratchSpaceStorage for the Windows File System
	/// </summary>
	public class LocalScratchSpaceRepository : IScratchSpaceRepository, IScratchSpaceConfigurationSource
	{
		private readonly IScratchSpaceConfigurationSource _configurationSource;
		private const string PathNameSetting = "UL.ScratchSpaceRootPath";
		private const string ExpirationPeriodSetting = "UL.ScratchSpaceExpirationPeriod";
		private readonly string _rootPath;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalScratchSpaceRepository"/> class.
		/// </summary>
		/// <param name="configurationSource">The configuration source.</param>
		public LocalScratchSpaceRepository(IScratchSpaceConfigurationSource configurationSource)
		{
			_configurationSource = configurationSource;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalScratchSpaceRepository"/> class.
		/// </summary>
		public LocalScratchSpaceRepository()
			: this(ConfigurationManager.AppSettings)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocalScratchSpaceRepository"/> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		internal LocalScratchSpaceRepository(NameValueCollection settings)
		{
			_rootPath = settings.GetValue(PathNameSetting, "~/ScratchSpaceRoot");
			Expiration = settings.GetValue(ExpirationPeriodSetting, 48 * 60);

			_configurationSource = this;
		}

		/// <summary>
		/// Fetches all.
		/// </summary>
		/// <param name="userId">The user id.</param>
		/// <returns></returns>
		public IEnumerable<ScratchFileInfo> FetchAll(Guid userId)
		{
			var path = Provision(userId);

            if (!Directory.Exists(path))
                throw new DatabaseItemNotFoundException();

			DirectoryInfo info = new DirectoryInfo(path);

			return info.EnumerateFiles().Select(Selector);
		}

		/// <summary>
		/// Selectors the specified file info.
		/// </summary>
		/// <param name="fileInfo">The file info.</param>
		/// <returns></returns>
		internal ScratchFileInfo Selector(FileInfo fileInfo)
		{
			//we store files as {originalName}_{id}, so find last underscore and separate the two pieces
			var idx = fileInfo.Name.LastIndexOf("_", StringComparison.Ordinal);
			var name = fileInfo.Name.Substring(0, idx);
			var id = fileInfo.Name.Substring(idx + 1);

			return new ScratchFileInfo {
				CreationTime = fileInfo.CreationTime,
				Extension = Path.GetExtension(name),
				CreationTimeUtc = fileInfo.CreationTimeUtc,
				LastAccessTime = fileInfo.LastAccessTime,
				LastAccessTimeUtc = fileInfo.LastAccessTimeUtc,
				LastWriteTime = fileInfo.LastWriteTime,
				LastWriteTimeUtc = fileInfo.LastAccessTimeUtc,
				Name = name,
				Size = fileInfo.Length,
				Id = Guid.Parse(id)
			};
		}

		/// <summary>
		/// Fetches the content.
		/// </summary>
		/// <param name="userId">The user id.</param>
		/// <param name="fileId">The file id.</param>
		/// <returns></returns>
		public Stream FetchContent(Guid userId, Guid fileId)
		{
			var path = Provision(userId);

			DirectoryInfo directoryInfo = new DirectoryInfo(path);

			FileInfo fileInfo = directoryInfo.EnumerateFiles(String.Format("*{0}.*", fileId.ToString())).Single();

			return fileInfo.OpenRead();
		}

		/// <summary>
		/// Publishes the content.
		/// </summary>
		/// <param name="userId">The user id.</param>
		/// <param name="fileName"></param>
		/// <param name="contentStream">The content stream.</param>
		/// <returns></returns>
		public Guid PublishContent(Guid userId, string fileName, Stream contentStream)
		{
			var path = Provision(userId);

			var directoryInfo = new DirectoryInfo(path);
			var randomFileName = Path.GetRandomFileName();

			var combinedPath = Path.Combine(directoryInfo.FullName, randomFileName);
			var fileInfo = new FileInfo(combinedPath);

			using (var streamWriter = new StreamWriter(fileInfo.OpenWrite()))
			{
				contentStream.CopyTo(streamWriter.BaseStream);
				contentStream.Dispose();
			}

			var fileId = Guid.NewGuid();
			var destFileName = String.Format("{0}_{1}", fileName, fileId.ToString());
			destFileName = Path.Combine(directoryInfo.FullName, destFileName);

			try
			{
				fileInfo.MoveTo(destFileName);
			}
			catch
			{
				//if error occurs renaming (eg file name too long), then delete the temp file
				fileInfo.Delete();
				throw;
			}

			return fileId;
		}

		/// <summary>
		/// Provisions the specified userid.
		/// </summary>
		/// <param name="userid">The userid.</param>
		/// <returns></returns>
		public string Provision(Guid userid)
		{
			var fullPath = _configurationSource[userid];

			DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);

			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}

			Purge(userid, false);

			return directoryInfo.FullName;
		}

		/// <summary>
		/// Gets the <see cref="System.String"/> with the specified userid.
		/// </summary>
		/// <value>
		/// The <see cref="System.String"/>.
		/// </value>
		/// <param name="userid">The userid.</param>
		/// <returns></returns>
		public string this[Guid userid]
		{
			get
			{
				var rootPath = _rootPath;
				if (rootPath.StartsWith("~"))
					rootPath = Path.Combine(HttpRuntime.AppDomainAppPath, rootPath.Substring(2));

				return Path.Combine(rootPath, userid.ToString());
			}
		}

		/// <summary>
		/// Purges the specified user's files
		/// </summary>
		/// <param name="userid">The userid.</param>
		/// <param name="forceDelete">True to delete all files regardless of age</param>
		public void Purge(Guid userid, bool forceDelete)
		{
			var expiration = _configurationSource.Expiration;
			var fullPath = _configurationSource[userid];

			DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);

			IEnumerable<FileInfo> fileInfos = directoryInfo.EnumerateFiles();

			//filter to files older than Expiration if we are not told to force deletions
			if (!forceDelete)
				fileInfos = fileInfos.Where(i => DateTime.Now.Subtract(i.CreationTime).TotalMinutes > expiration);

			foreach (FileInfo source in fileInfos)
			{
				try
				{
					source.Delete();
				}
				catch (Exception)
				{
					// Eat it ... there could be a process holding onto the file 
				}
			}
		}

		/// <summary>
		/// Gets the expiration in minutes.
		/// </summary>
		/// <value></value>
		public double Expiration { get; private set; }
	}
}