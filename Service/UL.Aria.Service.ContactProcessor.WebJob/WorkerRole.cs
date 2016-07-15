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

using UL.Aria.Service.ContactProcessor.WebJob.Logging;
using UL.Aria.Service.InboundOrderProcessing;
using UL.Aria.Service.InboundOrderProcessing.Provider;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.ContactProcessor.WebJob
{
	[SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), ExcludeFromCodeCoverage]
	public class WorkerRole : RoleEntryPoint, IDisposable
	{
		// ReSharper disable once InconsistentNaming
		public static readonly string _domain;
		// ReSharper disable once InconsistentNaming
		public static readonly string _user;
		// ReSharper disable once InconsistentNaming
		public static readonly string _pwd;
		// ReSharper disable once InconsistentNaming
		private static readonly int _batchSize;
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);
		private bool _isDisposed;

		static WorkerRole()
		{
			_domain = Environment.UserDomainName;
			_user = ConfigurationManager.AppSettings.GetValue("ContactOrder.Impersonation.User", "web_aria");
			_pwd = ConfigurationManager.AppSettings.GetValue("ContactOrder.Impersonation.Password", "th3_f0rce");
			_batchSize = ConfigurationManager.AppSettings.GetValue("ContactOrder.WebJob.BatchSize", 1);
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
			Trace.TraceInformation("UL.Aria.Service.ContactProcessor.WebJob is running");
			ILogManager logManager = null;

			try
			{
				ContainerLocator.Initialize();
				logManager =
					(ILogManager) ContainerLocator.Container.Resolve(typeof (ILogManager));
				var contactProvider =
					(IContactOrderProvider) ContainerLocator.Container.Resolve(typeof (IContactOrderProvider));
				contactProvider.CreateContactOrderQueue();

				var config = new JobHostConfiguration
				{
					NameResolver = new ConfigurationResolver(),
					TypeLocator = new ContactOrderWebJobsTypeLocator()
				};
				config.Queues.BatchSize = _batchSize;
				config.DashboardConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString;
				config.ServiceBusConnectionString =
					ConfigurationManager.ConnectionStrings["ContactOrderServicebus"].ConnectionString;

				var host = new JobHost(config);

				host.RunAndBlock();
			}
			catch (Exception ex)
			{
				if (logManager != null)
				{
					var logMessage = ex.ToLogMessage(MessageIds.ContactProcessingWorkerRoleException,
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

		public override bool OnStart()
		{
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			var result = base.OnStart();

			Trace.TraceInformation("UL.Aria.Service.ContactProcessor.WebJob has been started");

			return result;
		}

		public override void OnStop()
		{
			Trace.TraceInformation("UL.Aria.Service.ContactProcessor.WebJob is stopping");

			_cancellationTokenSource.Cancel();
			_runCompleteEvent.WaitOne();

			base.OnStop();

			Trace.TraceInformation("UL.Aria.Service.ContactProcessor.WebJob has stopped");
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
	/// Class ContactOrderWebJobsTypeLocator.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class ContactOrderWebJobsTypeLocator : ITypeLocator
	{
		private readonly List<Type> _getTypes;

		public ContactOrderWebJobsTypeLocator()
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