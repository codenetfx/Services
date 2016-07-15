using System;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// Exception for when a class marked to be audited is not configured properly
	/// </summary>
	[Serializable]
	public class AuditConfigurationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AuditConfigurationException"/> class.
		/// </summary>
		public AuditConfigurationException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuditConfigurationException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public AuditConfigurationException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuditConfigurationException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="inner">The inner.</param>
		public AuditConfigurationException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}