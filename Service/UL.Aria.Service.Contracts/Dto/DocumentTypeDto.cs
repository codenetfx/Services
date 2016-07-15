using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Document type enumeration data transfer object.
    /// </summary>
    [DataContract]
    public class DocumentTypeDto
    {
        private readonly static List<DocumentTypeDto> DocumentTypesList= new List<DocumentTypeDto>
                {
                    new DocumentTypeDto {Id = Guid.Parse("12981bac-5d2f-43e6-9bf8-1404fecf3701"), Name = "Deliverables and Outputs"},
                    new DocumentTypeDto {Id = Guid.Parse("dc055d4f-0835-4972-8231-2aeea611ec49"), Name = "Reference Materials"},
                    new DocumentTypeDto
                    {
                        Id = Guid.Parse("a19b4b06-32a8-45d7-9083-961e7d92e242"),
                        Name = "Communications and Correspondence"
                    },
                    new DocumentTypeDto
                    {
                        Id = Guid.Parse("A98C8961-7385-4933-A90B-9AB9885C654B"),
                        Name = "Datasheets and Test Records"
                    },
                    new DocumentTypeDto
                    {
                        Id = Guid.Parse("738429FF-2D0D-48BA-891C-168499DC9555"),
                        Name = "Facility Assessment"
                    },
                    new DocumentTypeDto {Id = Guid.Parse("c3da7c2b-2350-4306-964b-3ea5b8846abc"), Name = "Unspecified"}
                };

        /// <summary>
		/// Gets or sets the id.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		[DataMember]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		[DataMember]
		public string Name { get; set; }

        /// <summary>
        /// Gets the document types.
        /// </summary>
        /// <value>
        /// The document types.
        /// </value>
        [IgnoreDataMember]
        public static List<DocumentTypeDto> DocumentTypes
        {
            get
            {
                return DocumentTypesList;
            }
        }
    }
}