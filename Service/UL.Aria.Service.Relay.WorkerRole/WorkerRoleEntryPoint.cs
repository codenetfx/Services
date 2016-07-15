using System;
using System.Reflection;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service;

namespace UL.Aria.Service.Relay.WorkerRole
{
	/// <summary>
	/// Class WorkerRoleEntryPoint.
	/// </summary>
	public class WorkerRoleEntryPoint : WorkerRoleEntryPointBase
	{
		/// <summary>
		/// Gets the assembly name.
		/// </summary>
		/// <value>The assembly name.</value>
		public override string AssemblyName
		{
			get { return "UL.Aria.Service.Relay.WorkerRole"; }
		}

		/// <summary>
		/// Gets the message identifier.
		/// </summary>
		/// <value>The message identifier.</value>
		public override int MessageId
		{
			get { return MessageIds.RelayHostUnhandledException; }
		}

		/// <summary>
		/// Gets the log category.
		/// </summary>
		/// <value>The log category.</value>
		public override LogCategory LogCategory
		{
			get { return LogCategory.InboundOrderListener; }
		}

		/// <summary>
		/// Gets the unity relay module.
		/// </summary>
		/// <value>The unity relay module.</value>
		public override Type UnityRelayModule
		{
			get { return typeof (UnityRelayModule); }
		}
	}
}