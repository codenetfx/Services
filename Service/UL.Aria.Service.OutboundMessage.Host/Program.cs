using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Service.Host;

namespace UL.Aria.Service.OutboundMessage.Host
{
    [ExcludeFromCodeCoverage]
    static class Program
    {
        private static UnityInstanceProvider _instanceProvider;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            _instanceProvider = UnityInstanceProvider.Create();
            new ServiceExecutionManager(_instanceProvider.Container, Assembly.GetExecutingAssembly().Location).Execute(args);
        }
    }
}
