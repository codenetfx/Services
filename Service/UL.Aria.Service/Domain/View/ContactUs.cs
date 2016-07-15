using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Domain.View
{
	/// <summary>
	/// The view model for the contact us email
	/// </summary>
	public class ContactUs
	{
		/// <summary>
		/// Gets or sets the contact email.
		/// </summary>
		/// <value>
		/// The contact email.
		/// </value>
		public string ContactEmail { get; set; }

		/// <summary>
		/// Gets or sets the industry.
		/// </summary>
		/// <value>
		/// The industry.
		/// </value>
		public string IndustryName { get; set; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		/// <value>
		/// The subject.
		/// </value>
		public string Subject { get; set; }

		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string Message { get; set; }

		/// <summary>
		/// Gets or sets the contact copy email.
		/// </summary>
		/// <value>
		/// The contact copy email.
		/// </value>
		public string ContactCopyEmail { get; set; }
	}
}