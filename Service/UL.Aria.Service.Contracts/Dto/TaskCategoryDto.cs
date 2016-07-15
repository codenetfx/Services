using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// 
	/// </summary>
	[DataContract]
	public class TaskCategoryDto
	{
		/// <summary>
		///     Gets or sets the id.
		/// </summary>
		/// <value>
		///     The id.
		/// </value>
		[DataMember]
		public Guid? Id { get; set; }


		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[DataMember]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		[DataMember]
		public string Description { get; set; }

		/// <summary>
		///     Gets or sets the created by.
		/// </summary>
		/// <value>The created by.</value>
		[DataMember]
		public Guid CreatedById { get; set; }

		/// <summary>
		///     Gets or sets the updated by.
		/// </summary>
		/// <value>The updated by.</value>
		[DataMember]
		public Guid UpdatedById { get; set; }

		/// <summary>
		///     Gets or sets the created date time.
		/// </summary>
		/// <value>The created date time.</value>
		[DataMember]
		public DateTime CreatedDateTime { get; set; }

		/// <summary>
		///     Gets or sets the updated date time.
		/// </summary>
		/// <value>The updated date time.</value>
		[DataMember]
		public DateTime UpdatedDateTime { get; set; }

	}
}
