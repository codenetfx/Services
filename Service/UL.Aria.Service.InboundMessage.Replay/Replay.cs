using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.InboundMessage.Replay.Common;

namespace UL.Aria.Service.InboundMessage.Replay
{
	/// <summary>
	/// Class Replay.
	/// </summary>
	public class Replay
	{
		private readonly string _serviceBusConnectionString;
		private readonly string _serviceBusQueueName;
		private readonly string _storageAccountConnectionString;
		private readonly string _storageAccountContainerFailed;
		private readonly string _storageAccountContainerNew;

		/// <summary>
		/// Initializes a new instance of the <see cref="Replay"/> class.
		/// </summary>
		public Replay()
		{
			Choices = new CachedChoices();
			Options = ConfigureOptionSet();
			_serviceBusConnectionString =
				ConfigurationManager.ConnectionStrings["InboundMessageServicebus"].ConnectionString;
			_serviceBusQueueName = ConfigurationManager.AppSettings["InboundMessage.QueueName"];
			_storageAccountConnectionString =
				ConfigurationManager.ConnectionStrings["InboundMessageStorage"].ConnectionString;
			_storageAccountContainerNew = ConfigurationManager.AppSettings["InboundMessage.Container"];
			_storageAccountContainerFailed = ConfigurationManager.AppSettings["InboundMessage.Container.Failed"];
		}

		public CachedChoices Choices { get; set; }
		public OptionSet Options { get; set; }

		/// <summary>
		/// Starts the specified arguments.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public void Start(string[] args)
		{
			try
			{
				args = FixArgsForQuotedValues(args);
				Options.Parse(args);
				StartProgram();
			}
			catch (OptionException ex)
			{
				PrintException(ex);
			}
			catch (ConfigurationErrorsException ex)
			{
				PrintException(ex);
			}
			catch (Exception ex)
			{
				PrintException(ex, true);
			}
		}

		private static string[] FixArgsForQuotedValues(string[] args)
		{
			var fixedArgs = new List<string>();

			for (var i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("\""))
				{
					fixedArgs.Add(args[i].Remove(0, 1));
					var pos = fixedArgs.Count - 1;

					while (!args[i].EndsWith("\""))
					{
						i++;

						if (args.Length >= i)
							break;

						var temp = args[i].Trim();

						if (temp.EndsWith("\""))
							temp = temp.Remove(temp.Length - 1, 1);

						fixedArgs[pos] += " " + temp;
					}

					if (fixedArgs[fixedArgs.Count - 1].EndsWith("\""))
						fixedArgs[fixedArgs.Count - 1] = fixedArgs[fixedArgs.Count - 1]
							.Remove(fixedArgs[fixedArgs.Count - 1].Length - 1, 1);
				}
				else
				{
					fixedArgs.Add(args[i]);
				}
			}

			return fixedArgs.ToArray();
		}

		private void StartProgram()
		{
			var line = string.Empty;

			try
			{
				do
				{
					try
					{
						PrintHelp();
						Console.WriteLine();
						Console.Write("\tSelect Command: ");
						line = Console.ReadLine();
						if (!string.IsNullOrWhiteSpace(line))
						{
							var args = line.Split(' ');
							args = FixArgsForQuotedValues(args);
							Options.Parse(args);
						}
					}
					catch (ConfigurationErrorsException ex)
					{
						PrintException(ex);
					}
					catch (Exception ex)
					{
						PrintException(ex, true);
					}
				} while (line != "quit");
			}
			catch (Exception ex)
			{
				PrintException(ex);
			}
		}

		private OptionSet ConfigureOptionSet()
		{
			return new OptionSet
			{
				{"dt3|delete-t3", "Delete all t3 failed inbound messages", _ => DeleteT3()},
				{"tra|test-replay-all", "Test Replay all failed inbound messages list blob time", _ => TestReplayAll()},
				{"ra|replay-all", "Replay all failed inbound messages", _ => ReplayAll()},
				{"rs|replay-selected=", "Replay selected inbound messages by message id", ReplaySelected},
				{"h|help", "Displays help information.", _ => PrintHelp()},
				{"s|show", "Displays currently selected configuration choices.", _ => PrintChoices()},
				{"cls|clear", "Clears the screen and re-displays the menu", _ => PrintHelp()},
				{"q|quit", "Exists the program.", _ => Environment.Exit(0)}
			};
		}

		private void DeleteT3()
		{
			Console.WriteLine("Searching inbound message failed container...");
			var blobStatistics = new BlobStatistics();
			blobStatistics.Stopwatch.Start();
			var blobContainer = GetBlobContainer(_storageAccountContainerFailed);
			var failedBlobs =
				blobContainer.ListBlobs(blobListingDetails: BlobListingDetails.Metadata)
					.OfType<CloudBlockBlob>()
					.Where(x => x.Metadata["Receiver"] == "t3").ToList();
			blobStatistics.TotalBlobCount = failedBlobs.Count();
			blobStatistics.Stopwatch.Stop();
			Console.WriteLine("Searched inbound message failed container elapsed time {0}", blobStatistics.ElapsedTime);

			foreach (var failedBlob in failedBlobs)
			{
				var messageId = failedBlob.Metadata["MessageId"];
				StartProcessBlob(messageId, blobStatistics);

				try
				{
					failedBlob.DeleteIfExists();
					blobStatistics.Stopwatch.Stop();
					blobStatistics.Successful++;
					Console.WriteLine("Processed MessageId: {0}, elapsed time: {1}.", messageId, blobStatistics.ElapsedTime);
				}
				catch (Exception ex)
				{
					ProcessFailedBlobException(messageId, blobStatistics, ex);
				}

				blobStatistics.TotalElapsedTimeTicks += blobStatistics.TimeSpan.Ticks;
				Console.SetCursorPosition(0, Console.CursorTop - 3);
			}

			EndProcessBlobs(blobStatistics);
		}

		private void TestReplayAll()
		{
			Console.WriteLine("Searching inbound message failed container...");
			var blobStatistics = new BlobStatistics();
			blobStatistics.Stopwatch.Start();
			var blobContainer = GetBlobContainer(_storageAccountContainerFailed);
			var failedBlobs =
				blobContainer.ListBlobs(blobListingDetails: BlobListingDetails.Metadata)
					.OfType<CloudBlockBlob>()
					.Where(x => x.Metadata["Receiver"] != "t3")
					.OrderBy(x => x.Properties.LastModified)
					.ThenByDescending(x => x.Metadata["Receiver"]);
			blobStatistics.TotalBlobCount = failedBlobs.Count();
			blobStatistics.Stopwatch.Stop();
			Console.WriteLine("Searched inbound message failed container elapsed time {0}, total blobs to process: {1}", blobStatistics.ElapsedTime, blobStatistics.TotalBlobCount);
			Console.WriteLine();
			Console.WriteLine("\tPress any key to continue...");
			Console.ReadKey();
		}

		private void ReplayAll()
		{
			Console.WriteLine("Searching inbound message failed container...");
			var blobStatistics = new BlobStatistics();
			blobStatistics.Stopwatch.Start();
			var blobContainer = GetBlobContainer(_storageAccountContainerFailed);
			var failedBlobs =
				blobContainer.ListBlobs(blobListingDetails: BlobListingDetails.Metadata)
					.OfType<CloudBlockBlob>()
					.Where(x => x.Metadata["Receiver"] != "t3")
					.OrderBy(x => x.Properties.LastModified)
					.ThenByDescending(x => x.Metadata["Receiver"]);
			blobStatistics.TotalBlobCount = failedBlobs.Count();
			blobStatistics.Stopwatch.Stop();
			Console.WriteLine("Searched inbound message failed container elapsed time {0}", blobStatistics.ElapsedTime);
			var queueClient =
				QueueClient.CreateFromConnectionString(_serviceBusConnectionString, _serviceBusQueueName);

			foreach (var failedBlob in failedBlobs)
			{
				var messageId = failedBlob.Metadata["MessageId"];
				StartProcessBlob(messageId, blobStatistics);

				try
				{
					ProcessBlob(messageId, failedBlob, queueClient, blobStatistics);
				}
				catch (Exception ex)
				{
					ProcessFailedBlobException(messageId, blobStatistics, ex);
				}

				blobStatistics.TotalElapsedTimeTicks += blobStatistics.TimeSpan.Ticks;
				Console.SetCursorPosition(0, Console.CursorTop - 3);
			}

			EndProcessBlobs(blobStatistics);
		}

		private void ReplaySelected(string commaDelimitedMessageIds)
		{
			var messageIds = commaDelimitedMessageIds.Split(',');
			var blobStatistics = new BlobStatistics {TotalBlobCount = messageIds.Count()};
			var queueClient =
				QueueClient.CreateFromConnectionString(_serviceBusConnectionString, _serviceBusQueueName);

			foreach (var messageId in messageIds)
			{
				StartProcessBlob(messageId, blobStatistics);

				try
				{
					var failedBlob = GetBlob(_storageAccountContainerFailed, messageId);
					ProcessBlob(messageId, failedBlob, queueClient, blobStatistics);
				}
				catch (Exception ex)
				{
					ProcessFailedBlobException(messageId, blobStatistics, ex);
				}

				blobStatistics.TotalElapsedTimeTicks += blobStatistics.TimeSpan.Ticks;
				Console.SetCursorPosition(0, Console.CursorTop - 3);
			}

			EndProcessBlobs(blobStatistics);
		}

		private static void StartProcessBlob(string messageId, BlobStatistics blobStatistics)
		{
			var timeSpan = blobStatistics.Successful + blobStatistics.Failed == 0
				? new TimeSpan(0)
				: new TimeSpan(blobStatistics.TotalElapsedTimeTicks/(blobStatistics.Successful + blobStatistics.Failed));
			var expectedTimeSpan = blobStatistics.Successful + blobStatistics.Failed == 0
				? new TimeSpan(0)
				: new TimeSpan((blobStatistics.TotalElapsedTimeTicks / (blobStatistics.Successful + blobStatistics.Failed)) * (blobStatistics.TotalBlobCount - blobStatistics.Failed - blobStatistics.Successful));
			Console.WriteLine("Processing {0} of {1} ({2} successful, {3} failed, avg. elapsed time {4}, expected time left: {5})...",
				blobStatistics.Successful + blobStatistics.Failed + 1, blobStatistics.TotalBlobCount, blobStatistics.Successful,
				blobStatistics.Failed, blobStatistics.GetElapsedTime(timeSpan), blobStatistics.GetElapsedTime(expectedTimeSpan));
			Console.WriteLine("Processing MessageId: {0}...", messageId);
			blobStatistics.Stopwatch.Reset();
			blobStatistics.Stopwatch.Start();
		}

		private void ProcessBlob(string messageId, CloudBlockBlob failedBlob, QueueClient queueClient,
			BlobStatistics blobStatistics)
		{
			var newBlob = GetBlob(_storageAccountContainerNew, messageId);

			using (var stream = failedBlob.OpenRead())
			{
				newBlob.UploadFromStream(stream);
			}

			var queueMessage = new InboundMessageDto
			{
				MessageId = messageId,
				ExternalMessageId = failedBlob.Metadata["ExternalMessageId"],
				Receiver = failedBlob.Metadata["Receiver"],
				Originator = failedBlob.Metadata["Originator"]
			};

			queueClient.Send(new BrokeredMessage(queueMessage));
			failedBlob.DeleteIfExists();
			blobStatistics.Stopwatch.Stop();
			blobStatistics.Successful++;
			Console.WriteLine("Processed MessageId: {0}, elapsed time: {1}.", messageId, blobStatistics.ElapsedTime);
		}

		private static void ProcessFailedBlobException(string messageId, BlobStatistics blobStatistics, Exception ex)
		{
			blobStatistics.Stopwatch.Stop();
			blobStatistics.Failed++;
			Console.WriteLine("Error Processing MessageId: {0}, elapsed time {1}, Error: {2}.", messageId,
				blobStatistics.ElapsedTime, ex.Message);
		}

		private static void EndProcessBlobs(BlobStatistics blobStatistics)
		{
			var timeSpan = blobStatistics.Successful + blobStatistics.Failed == 0
				? new TimeSpan(0)
				: new TimeSpan(blobStatistics.TotalElapsedTimeTicks/(blobStatistics.Successful + blobStatistics.Failed));
			var expectedTimeSpan = new TimeSpan(0);
			Console.WriteLine("Processing {0} of {1} ({2} successful, {3} failed, avg. elapsed time {4}, expected time left: {5})...",
				blobStatistics.Successful + blobStatistics.Failed, blobStatistics.TotalBlobCount, blobStatistics.Successful,
				blobStatistics.Failed, blobStatistics.GetElapsedTime(timeSpan), blobStatistics.GetElapsedTime(expectedTimeSpan));
			Console.SetCursorPosition(0, Console.CursorTop + 2);
			Console.WriteLine();
			timeSpan = blobStatistics.Successful + blobStatistics.Failed == 0
				? new TimeSpan(0)
				: new TimeSpan(blobStatistics.TotalElapsedTimeTicks);
			Console.WriteLine(
				"\tProcessed inbound message failed container blobs, Successful: {0}, Failed: {1}, elapsed time: {2}.",
				blobStatistics.Successful, blobStatistics.Failed, blobStatistics.GetElapsedTime(timeSpan));
			Console.WriteLine("\tPress any key to continue...");
			Console.ReadKey();
		}

		private void PrintChoices()
		{
			const string format = "\t{0,-35} = {1, -5}";
			Console.WriteLine();
			Console.WriteLine("\tCURRENT CHOICES");
			Console.WriteLine();
			Console.WriteLine("\t{0, -35}{1, -3}", "SETTING", "VALUE");
			Console.WriteLine();
			var cType = typeof (CachedChoices);
			var properties = cType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var prop in properties)
			{
				var tempVal = prop.GetValue(Choices);
				var list = tempVal as IEnumerable;
				if (list != null && !(list is string))
				{
					foreach (var item in list)
					{
						Console.WriteLine(format, prop.Name, item);
					}
				}
				else
				{
					Console.WriteLine(format, prop.Name, tempVal);
				}
			}

			Console.WriteLine();
			Console.Write("\tPress any key to continue.");
			Console.ReadKey();
		}

		private static void PrintException(Exception ex, bool showStack = false)
		{
			Console.WriteLine();
			Console.WriteLine("\tAn error has occurred!");
			Console.WriteLine();
			Console.WriteLine("\tMessage: " + ex.Message);

			if (showStack)
			{
				Console.WriteLine("\tStack Trace: " + ex.StackTrace);
			}

			Console.WriteLine();
			Console.Write("\tPress any key to continue.");
			Console.ReadKey();
		}

		private void PrintHelp()
		{
			Console.Clear();
			Console.WriteLine();
			Console.WriteLine("\tDocument Simulator - COMMANDS");
			Console.WriteLine();
			const string format = "\t/{0,-30}{1}";

			foreach (var opt in Options)
			{
				Console.WriteLine(format, string.Join(" | /", opt.Names), opt.Description);
			}
		}

		private CloudBlockBlob GetBlob(string container, string blob)
		{
			var blobContainer = GetBlobContainer(container);
			return blobContainer.GetBlockBlobReference(blob);
		}

		private CloudBlobContainer GetBlobContainer(string container)
		{
			var blobClient = GetStorageAccount()
				.CreateCloudBlobClient();
			return
				blobClient
					.GetContainerReference(container.ToLower());
		}

		private CloudStorageAccount GetStorageAccount()
		{
			return CloudStorageAccount.Parse(_storageAccountConnectionString);
		}

		private class BlobStatistics
		{
			public BlobStatistics()
			{
				Stopwatch = new Stopwatch();
				Successful = 0;
				Failed = 0;
				TotalElapsedTimeTicks = 0;
			}

			public Stopwatch Stopwatch { get; private set; }
			public int Successful { get; set; }
			public int Failed { get; set; }

			public TimeSpan TimeSpan
			{
				get { return Stopwatch.Elapsed; }
			}

			public long TotalElapsedTimeTicks { get; set; }
			public int TotalBlobCount { get; set; }

			public string ElapsedTime
			{
				get { return GetElapsedTime(TimeSpan); }
			}

			public string GetElapsedTime(TimeSpan timeSpan)
			{
				return string.Format("{0:00}:{1:00}:{2:00}.{3:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds,
					timeSpan.Milliseconds/10);
			}
		}
	}
}