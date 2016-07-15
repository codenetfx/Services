using System;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	internal sealed class AuditResourceAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AuditResourceAttribute"/> class.
		/// </summary>
		public AuditResourceAttribute()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuditResourceAttribute"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		public AuditResourceAttribute(string target)
		{
			Target = target;
		}

		/// <summary>
		/// Gets the target entity parameter name.
		/// </summary>
		/// <value>
		/// The target.
		/// </value>
		public string Target { get; set; }

		/// <summary>
		/// Gets or sets the type of the action.
		/// </summary>
		/// <value>The type of the action.</value>
		public string ActionType { get; set; }
	}
}