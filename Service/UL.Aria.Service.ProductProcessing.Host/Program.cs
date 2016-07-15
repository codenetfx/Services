using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Dispatcher;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service.Host;
using UL.Aria.Service.Manager;

namespace UL.Aria.Service.ProductProcessing.Host
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static UnityInstanceProvider _instanceProvider;

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            var lic = new Aspose.Cells.License();
            lic.SetLicense("Aspose.Total.lic");
            _instanceProvider = UnityInstanceProvider.Create();
            new ServiceExecutionManager(_instanceProvider.Container, Assembly.GetExecutingAssembly().Location).Execute(args);
        }
    }
}
