using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Relay.Common;
using UL.Enterprise.Foundation.Service.Configuration;
using UL.Enterprise.Foundation.Service.Host;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Implementation;
using UL.Aria.Service.Relay.Service;
using UL.Enterprise.Foundation.Unity;

namespace UL.Aria.Service.Relay.Host
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            var serviceConfiguration = ConfigurationManager.GetSection("AzureWcfProcessConfiguration") as AzureWcfProcessConfigurationSection;
         

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;


            if (serviceConfiguration != null && serviceConfiguration.Services != null)
            {
                WcfProcessHost.ConfigureServiceHost(UnityModuleManager.CurrentContext.Container, serviceConfiguration);

                try
                {
                    new ServiceExecutionManager(UnityModuleManager.CurrentContext.Container, Assembly.GetExecutingAssembly().Location).Execute(args);
                }
                finally
                {
                    UnityModuleManager.DisposeSingleton();
                }
            }

        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            EventLog.WriteEvent(Assembly.GetEntryAssembly().GetName().Name, new EventInstance(1, 0), e.ExceptionObject.ToString());
            var logManager = UnityModuleManager.CurrentContext.Resolve<ILogManager>();
            if (null != logManager)
            {
                var exception = (e.ExceptionObject as Exception);
                if (null != exception)
                {
                    logManager.Log(exception.ToLogMessage(MessageIds.RelayHostUnhandledException, LogCategory.InboundOrderListener, LogPriority.Critical,
                                                          TraceEventType.Error));
                }
            }
        }
    }
}
