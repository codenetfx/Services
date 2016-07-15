using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;

using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;

using UL.Aria.Service.InboundOrderProcessing;
using UL.Aria.Service.InboundOrderProcessing.Logging;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.InboundMessageProcessor.WebJob
{
	[ExcludeFromCodeCoverage]
	public class WorkerRole : RoleEntryPoint, IDisposable
	{
		// ReSharper disable once InconsistentNaming
		public static readonly string _domain;
		// ReSharper disable once InconsistentNaming
		public static readonly string _user;
		// ReSharper disable once InconsistentNaming
		public static readonly string _pwd;
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
		private Cleanup _cleanupFailed;
		private Cleanup _cleanupNew;
		private Cleanup _cleanupOrderMessages;
		private bool _isDisposed;

		static WorkerRole()
		{
			_domain = Environment.UserDomainName;
			_user = ConfigurationManager.AppSettings.GetValue("InboundMessage.Impersonation.User", "web_aria");
			_pwd = ConfigurationManager.AppSettings.GetValue("InboundMessage.Impersonation.Password", "th3_f0rce");
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public override void Run()
		{
			Trace.TraceInformation("UL.Aria.Service.InboundMessageProcessor.WebJob is running");
			ILogManager logManager = null;

			try
			{
				ContainerLocator.Initialize();
				logManager =
					(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
				var inboundMessageProvider =
					(IInboundMessageProvider) ContainerLocator.Container.Resolve(typeof (IInboundMessageProvider));
				inboundMessageProvider.CreateNewQueue();
				StartCleanupTimers();

				var config = new JobHostConfiguration
				{
					NameResolver = new ConfigurationResolver(),
					TypeLocator = new InboundMessageWebJobsTypeLocator()
				};
				config.Queues.BatchSize = 1;
				config.DashboardConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
				config.ServiceBusConnectionString =
					ConfigurationManager.ConnectionStrings["InboundMessageServicebus"].ConnectionString;

				var host = new JobHost(config);

				host.RunAndBlock();
			}
			catch (Exception ex)
			{
				if (logManager != null)
				{
					var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingWorkerRoleException,
						LogCategory.InboundProcessor,
						LogPriority.Critical,
						TraceEventType.Critical);
					logManager.Log(logMessage);
				}
				throw;
			}
			finally
			{
				_runCompleteEvent.Set();
			}
		}

		private void StartCleanupTimers()
		{
			_cleanupNew = new Cleanup("InboundMessage.New.Cleanup.Time");
			_cleanupNew.Timer = new Timer(_cleanupNew.RunNew);
			_cleanupNew.Reset();

			_cleanupFailed = new Cleanup("InboundMessage.Failed.Cleanup.Time");
			_cleanupFailed.Timer = new Timer(_cleanupFailed.RunFailed);
			_cleanupFailed.Reset();

			_cleanupOrderMessages = new Cleanup("InboundMessage.OrderMessages.Cleanup.Time");
			_cleanupOrderMessages.Timer = new Timer(_cleanupOrderMessages.RunOrderMessages);
			_cleanupOrderMessages.Reset();
		}

		public override bool OnStart()
		{
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			var result = base.OnStart();

			Trace.TraceInformation("UL.Aria.Service.InboundMessageProcessor.WebJob has been started");

			return result;
		}

		public override void OnStop()
		{
			Trace.TraceInformation("UL.Aria.Service.InboundMessageProcessor.WebJob is stopping");

			if (_cleanupNew != null && _cleanupNew.Timer != null)
			{
				_cleanupNew.Timer.Dispose();
			}
			if (_cleanupFailed != null && _cleanupFailed.Timer != null)
			{
				_cleanupFailed.Timer.Dispose();
			}
			if (_cleanupOrderMessages != null && _cleanupOrderMessages.Timer != null)
			{
				_cleanupOrderMessages.Timer.Dispose();
			}

			_cancellationTokenSource.Cancel();
			_runCompleteEvent.WaitOne();

			base.OnStop();

			Trace.TraceInformation("UL.Aria.Service.InboundMessageProcessor.WebJob has stopped");
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="WorkerRole"/> class.
		/// </summary>
		~WorkerRole()
		{
			Dispose(false);
		}

		/// <summary>
		/// Checks the is disposed.
		/// </summary>
		/// <exception cref="System.ObjectDisposedException">Operations are not allowed on a disposed object.</exception>
		protected void CheckIsDisposed()
		{
			if (_isDisposed)
			{
				throw new ObjectDisposedException(string.Format("{0}:{1}", GetType().FullName, GetHashCode()),
					"Operations are not allowed on a disposed object.");
			}
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				using (_cancellationTokenSource)
				{
				}
				using (_runCompleteEvent)
				{
				}
				//
				// Free the state of managed objects.
				//
			}

			//
			// Free the state of unmanaged objects (for example, set large fields to null).
			//

			//
			// We are now disposed.
			//
			_isDisposed = true;
		}

		private class Cleanup
		{
			private readonly string _appSettingsCleanupTime;

			public Cleanup(string appSettingsCleanupTime)
			{
				_appSettingsCleanupTime = appSettingsCleanupTime;
			}

			public Timer Timer { get; set; }

			public void RunNew(object stateInfo)
			{
				using (new NativeMethods.Impersonation(_domain, _user, _pwd))
				{
					var logManager =
						(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
					var inboundMessageProvider =
						(IInboundMessageProvider) ContainerLocator.Container.Resolve(typeof (IInboundMessageProvider));

					try
					{
						inboundMessageProvider.CleanupNewMessages();
					}
					catch (Exception ex)
					{
						var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingCleanupNewException,
							LogCategory.InboundProcessor,
							LogPriority.High,
							TraceEventType.Error);

						logManager.Log(logMessage);
					}
					finally
					{
						Thread.Sleep(3000);
						Reset();
					}
				}
			}

			public void RunFailed(object stateInfo)
			{
				using (new NativeMethods.Impersonation(_domain, _user, _pwd))
				{
					var logManager =
						(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
					var inboundMessageProvider =
						(IInboundMessageProvider) ContainerLocator.Container.Resolve(typeof (IInboundMessageProvider));

					try
					{
						inboundMessageProvider.CleanupFailedMessages();
					}
					catch (Exception ex)
					{
						var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingCleanupFailedException,
							LogCategory.InboundProcessor,
							LogPriority.High,
							TraceEventType.Error);

						logManager.Log(logMessage);
					}
					finally
					{
						Thread.Sleep(3000);
						Reset();
					}
				}
			}

			public void RunOrderMessages(object stateInfo)
			{
				using (new NativeMethods.Impersonation(_domain, _user, _pwd))
				{
					var logManager =
						(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
					var inboundMessageProvider =
						(IInboundMessageProvider) ContainerLocator.Container.Resolve(typeof (IInboundMessageProvider));

					try
					{
						inboundMessageProvider.CleanupOrderMessages();
					}
					catch (Exception ex)
					{
						var logMessage = ex.ToLogMessage(MessageIds.InboundOrderProcessingCleanupOrderMessagesException,
							LogCategory.InboundProcessor,
							LogPriority.High,
							TraceEventType.Error);

						logManager.Log(logMessage);
					}
					finally
					{
						Thread.Sleep(3000);
						Reset();
					}
				}
			}

			private static int GetMillisecondsToUtcTime(DateTime current, double hours, double minutes = 0)
			{
				var desiredTimeToday = DateTime.UtcNow.AddHours(hours).AddMinutes(minutes);

				// If it's already past desired time today, wait until desired time tomorrow    
				if (current > desiredTimeToday)
				{
					desiredTimeToday = desiredTimeToday.AddDays(1.0);
				}

				return (int) ((desiredTimeToday - current).TotalMilliseconds);
			}

			public void Reset()
			{
				var hoursMinutesString = ConfigurationManager.AppSettings.GetValue(_appSettingsCleanupTime, "2:00");
				var hoursMinutes = hoursMinutesString.Split(':');
				double hours;
				double minutes;
				double.TryParse(hoursMinutes[0], out hours);
				double.TryParse(hoursMinutes[1], out minutes);

				var millisecondsUntilTime = GetMillisecondsToUtcTime(DateTime.UtcNow, hours, minutes);

				Timer.Change(millisecondsUntilTime, Timeout.Infinite);
			}
		}
	}

	[ExcludeFromCodeCoverage]
	public static class ContainerLocator
	{
		public static IUnityContainer Container { get; private set; }

		public static void Initialize()
		{
			var unityInstanceProvider = UnityInstanceProvider.Create();

			Container = unityInstanceProvider.Container;
		}
	}

	/// <summary>
	///     Locates types which may be used for defining web jobs
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class InboundMessageWebJobsTypeLocator : ITypeLocator
	{
		private readonly List<Type> _getTypes;

		public InboundMessageWebJobsTypeLocator()
		{
			_getTypes = new List<Type> {typeof (Functions)};
		}

		public IReadOnlyList<Type> GetTypes()
		{
			return _getTypes;
		}
	}

	/// <summary>
	/// Class ConfigurationResolver.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ConfigurationResolver : INameResolver
	{
		/// <summary>
		/// Resolve a %name% to a value. Resolution is not recursive.
		/// </summary>
		/// <param name="name">The name to resolve (without the %... %)</param>
		/// <returns>The value to which the name resolves, if the name is supported; otherwise <see langword="null" />.</returns>
		public string Resolve(string name)
		{
			return ConfigurationManager.AppSettings[name];
		}
	}
}