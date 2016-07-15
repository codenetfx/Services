using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class AssetLinkDto.
	/// </summary>
	[DataContract]
	public class AssetLinkDto
	{
		/// <summary>
		/// Gets or sets the parent asset identifier.
		/// </summary>
		/// <value>The parent asset identifier.</value>
		[DataMember]
		public Guid ParentAssetId { get; set; }

		/// <summary>
		/// Gets or sets the asset identifier.
		/// </summary>
		/// <value>The asset identifier.</value>
		[DataMember]
		public Guid AssetId { get; set; }

		/// <summary>
		/// Gets or sets the type of the parent asset.
		/// </summary>
		/// <value>The type of the parent asset.</value>
		[DataMember]
		public string ParentAssetType { get; set; }

		/// <summary>
		/// Gets or sets the type of the asset.
		/// </summary>
		/// <value>The type of the asset.</value>
		[DataMember]
		public string AssetType { get; set; }

		/// <summary>
		/// Gets or sets the name of the asset.
		/// </summary>
		/// <value>The name of the asset.</value>
		[DataMember]
		public string AssetName { get; set; }

		/// <summary>
		///     Gets or sets the user it was created by.
		/// </summary>
		/// <value>
		///     The created by.
		/// </value>
		[DataMember]
		public Guid CreatedById { get; set; }

		/// <summary>
		///     Gets or sets the created on.
		/// </summary>
		/// <value>
		///     The created on.
		/// </value>
		[DataMember]
		public DateTime CreatedDateTime { get; set; }

		/// <summary>
		///     Gets or sets the updated on.
		/// </summary>
		/// <value>
		///     The updated on.
		/// </value>
		[DataMember]
		public DateTime UpdatedDateTime { get; set; }

		/// <summary>
		///     Gets or sets who it was updated by.
		/// </summary>
		/// <value>
		///     The updated by person.
		/// </value>
		[DataMember]
		public Guid UpdatedById { get; set; }
	}
}
