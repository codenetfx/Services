using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;


namespace UL.Aria.Service.Host
{
	/// <summary>
	/// 
	/// </summary>
    [ExcludeFromCodeCoverage] 
	public class WebRole : RoleEntryPoint
	{

		/// <summary>
		/// Called when [start].
		/// </summary>
		/// <returns></returns>
		public override bool OnStart()
		{
			var env = CloudConfigurationManager.GetSetting("EnvironmentName");
			var info = new ProcessStartInfo("setup.cmd", env + " Services") { UseShellExecute = false };
			var process = System.Diagnostics.Process.Start(info);
			 if (process != null)
				 process.WaitForExit();

			return base.OnStart();
		}
	}
}