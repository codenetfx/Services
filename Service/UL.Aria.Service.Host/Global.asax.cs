using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

using Aspose.Cells;

using Microsoft.Practices.Unity;

using UL.Aria.Service.Claim.Implementation;
using UL.Aria.Service.Host.Common;
using UL.Aria.Service.Implementation;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Host
{
    /// <summary>
    ///     Global application class.
    /// </summary>
    [ExcludeFromCodeCoverage] // Approved by Jim L.
    public class Global : HttpApplication
    {
        /// <summary>
        ///     Handles the Start event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            var lic = new License();
            lic.SetLicense("Aspose.Total.lic");
            RegisterServiceRoutes();
        }

        /// <summary>
        ///     Handles the Start event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Session_Start(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the BeginRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the AuthenticateRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the Error event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the End event of the Session control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        ///     Handles the LogRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_LogRequest(object sender, EventArgs e)
        {
            //TODO Add Log for requests. This should be verbose.
            //HttpApplication app = (HttpApplication)sender;


            //var ex = app.Server.GetLastError();

            //if (ex != null)
            //    System.Diagnostics.EventLog.WriteEntry("service", ex.ToString());
            //else
            //{
            //    StringBuilder message = new StringBuilder();
            //    message.AppendFormat("MachineName:{3}, StatusCode:{0}, StatusDescription:{1}, ContentType:{2}", 
            //        app.Context.Response.StatusCode, app.Context.Response.StatusDescription, app.Context.Response.ContentType, Environment.MachineName);
            //    var request = app.Context.Request;
            //    var referrer = app.Context.Request.UrlReferrer;
            //    message.AppendLine();
            //    message.Append("Url:\t\t\t");
            //    message.Append(request.Url);
            //    foreach (var header in request.Headers)
            //    {
            //        message.AppendLine();
            //        message.AppendFormat("Header:{0}:{1}",header, request.Headers[header.ToString()]);
            //    }
            //    message.AppendLine();
            //    message.Append(request.Params);
            //    if (referrer != null)
            //    {
            //        message.AppendLine();
            //        message.Append("Referrer:\t\t");
            //        message.Append(referrer.ToString());
            //    }


            //    if (Thread.CurrentPrincipal != null)
            //    {
            //        message.AppendLine();
            //        message.Append("Thread Principal:\t");
            //        message.Append(Thread.CurrentPrincipal.Identity.Name);
            //    }


            //    System.Diagnostics.EventLog.WriteEntry("service", message.ToString());
            //}
        }

        /// <summary>
        ///     Handles the End event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void Application_End(object sender, EventArgs e)
        {
        }

        private void RegisterServiceRoutes()
        {
            var serviceHostFactory = new WcfServiceFactory(new UnityContainer());
            serviceHostFactory.ConfigureContainer();

            RestServiceRegistar.AutoRegisterRestServices(serviceHostFactory, typeof(ServiceMapperRegistry).Assembly,
                RouteTable.Routes,
                 new RouteManager(),
                x => x.Namespace == "UL.Aria.Service.Implementation",
                serviceRegistrationOverrides: new Dictionary<Type, string>()
                {

                });

            //last
            RouteTable.Routes.Add(new ServiceRoute("", serviceHostFactory, typeof(AriaService)));

        }
    }
}