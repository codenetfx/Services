using System;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Practices.Unity;
using UL.Aria.Common;
using UL.Aria.Service.Export.Logging;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Export.Manager;
using Local =UL.Aria.Service.Export.Common;
using UL.Aria.Service.Manager;

namespace UL.Aria.Service.Export
{
    /// <summary>
    /// Main for Export exe.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var provider = Local.UnityInstanceProvider.Create())
            {
                try
                {
                    provider.Resolve<IProjectExportManager>().ExportProjects();
                }
                catch (System.Exception ex)
                {
                   
                    try
                    {
                        provider.Resolve<ILogManager>().Log(ex.ToLogMessage(MessageIds.ExportUnhandledException, LogCategory.Export, LogPriority.High,
                            TraceEventType.Error));
                    }
                    catch
                    {
                        EventLog.WriteEntry("UL.Aria.Service.Export", ex + Environment.NewLine + ex.StackTrace);
                    }
                    throw;
                }
            }
        }
    }

}
