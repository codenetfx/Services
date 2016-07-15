using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class LookupDto.
	/// </summary>
   [DataContract]
    public class LookupDto
    {
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
        [DataMember]
        public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        [DataMember]
        public String Name { get; set; }

		/// <summary>
		/// Gets or sets the container identifier.
		/// </summary>
		/// <value>The container identifier.</value>
        [DataMember]
        public Guid? ContainerId { get; set; }
    }
}
