using System;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service.Host;

namespace UL.Aria.Service.InboundOrderProcessing.Host
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static UnityInstanceProvider _instanceProvider;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private static void Main(string[] args)
        {
            _instanceProvider = UnityInstanceProvider.Create();
            new ServiceExecutionManager(_instanceProvider.Container, Assembly.GetExecutingAssembly().Location).Execute(args);
        }
    }
}