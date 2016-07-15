using System;

namespace UL.Aria.Service.Domain.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentType
    {
		/// <summary>
		/// Gets or sets the document type id.
		/// </summary>
		/// <value>
		/// The document id.
		/// </value>
        public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the document type.
		/// </summary>
		/// <value>
		/// The data.
		/// </value>
        public string Name { get; set; }
    }
}