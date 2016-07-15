using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    ///     class that represents a dto on the wire
    /// </summary>
    [DataContract]
    public class HistoryDto
    {
        /// <summary>
        /// Primary guid
        /// PK guid
        /// </summary>
        [DataMember]
        public Guid HistoryId { get; set; }

        /// <summary>
        /// Entity reference guid
        /// FK guid for project,task,order,product,request,company
        /// </summary>
        [DataMember]
        public Guid EntityId { get; set; }

        /// <summary>
        /// String for Entity Type
        /// </summary>
        [DataMember]
        public string EntityType { get; set; }

        /// <summary>
        /// DateTime.UtcNow
        /// </summary>
        [DataMember]
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Guid for UserContext.CurrentUser
        /// </summary>
        [DataMember]
        public Guid ActionUserId { get; set; }

        /// <summary>
        /// String for UserContext.CurrentUser
        /// </summary>
        [DataMember]
        public string ActionUserText { get; set; }

        /// <summary>
        /// String for Created,Changed,Deleted
        /// </summary>
        [DataMember]
        public string ActionType { get; set; }

        /// <summary>
        /// String for now, maybe a sub-model based on Type
        /// </summary>
        [DataMember]
        public string ActionDetail { get; set; }

        /// <summary>
        /// Gets or sets the tracked information.
        /// </summary>
        /// <value>
        /// The tracked information.
        /// </value>
        [DataMember]
        public List<NameValuePairDto> TrackedInfo { get; set; }

		/// <summary>
		/// Gets or sets the type of the action detail entity.
		/// </summary>
		/// <value>The type of the action detail entity.</value>
		[DataMember]
		public string ActionDetailEntityType { get; set; }
	}
}