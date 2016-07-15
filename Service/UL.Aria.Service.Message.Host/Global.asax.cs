using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel.Activation;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using UL.Enterprise.Foundation.Service.Unity;
using UL.Aria.Service.Message.Host.Common;
using UL.Aria.Service.Message.Implementation;

namespace UL.Aria.Service.Message.Host
{
    /// <summary>
    /// Global application class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Global : System.Web.HttpApplication
    {

        /// <summary>
        /// Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected internal void Application_Start(object sender, EventArgs e)
        {
            var serviceHostFactory = new WcfServiceFactory(new UnityContainer());
            serviceHostFactory.ConfigureContainer();
            RegisterResources(RouteTable.Routes, serviceHostFactory);
        }

        internal static void RegisterResources(RouteCollection routes, UnityServiceHostFactory serviceHostFactory)
        {
            routes.Add(new ServiceRoute("OrderMessageService", serviceHostFactory, typeof (OrderMessageService)));
        }

    }
}