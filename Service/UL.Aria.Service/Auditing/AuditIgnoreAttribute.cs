using System;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// Indicates that a property should be ignored by the Audit delta identification process.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
	public sealed class AuditIgnoreAttribute : Attribute
	{
	}
}