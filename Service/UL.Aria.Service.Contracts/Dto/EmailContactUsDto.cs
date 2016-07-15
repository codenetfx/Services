using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// 
	/// </summary>
    [DataContract]
    public class EmailContactUsDto
    {
		/// <summary>
		/// Gets or sets the contact email.
		/// </summary>
		/// <value>
		/// The contact email.
		/// </value>
		[DataMember]
		public string ContactEmail { get; set; }
		/// <summary>
		/// Gets or sets the contact copy email.
		/// </summary>
		/// <value>
		/// The contact copy email.
		/// </value>
		[DataMember]
		public string ContactCopyEmail { get; set; }
		/// <summary>
		/// Gets or sets the name of the industry.
		/// </summary>
		/// <value>
		/// The name of the industry.
		/// </value>
		[DataMember]
		public string IndustryName { get; set; }
		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		/// <value>
		/// The subject.
		/// </value>
		[DataMember]
		public string Subject { get; set; }
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		[DataMember]
		public string Message { get; set; }
	}
}