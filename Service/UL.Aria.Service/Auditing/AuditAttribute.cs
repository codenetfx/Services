using System;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// Indicates that an interface, when intercepted, should have audit behavior.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
	public sealed class AuditAttribute : Attribute
	{
	}
}