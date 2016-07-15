using System;
using System.Collections.Generic;
using Microsoft.ServiceBus.Tracing;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain
{
	/// <summary>
	/// History Domain Model
	/// </summary>
	public class History
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class.
        /// </summary>
	    public History()
	    {
	        TrackedInfo = new List<NameValuePair>();
	    }

		/// <summary>
		/// Primary guid
		/// PK guid
		/// </summary>
		public Guid HistoryId { get; set; }

		/// <summary>
		/// Entity reference guid
		/// FK guid for project,task,order,product,request,company
		/// </summary>
		public Guid EntityId { get; set; }

		/// <summary>
		/// DateTime.UtcNow
		/// </summary>
		public DateTime ActionDate { get; set; }

		/// <summary>
		/// Guid for UserContext.CurrentUser
		/// </summary>
		public Guid ActionUserId { get; set; }

        /// <summary>
        /// String for UserContext.CurrentUser
        /// </summary>
        public string ActionUserText { get; set; }

		/// <summary>
		/// String for Created,Changed,Deleted
		/// </summary>
		public string ActionType { get; set; }

		/// <summary>
		/// String for now, maybe a sub-model based on Type
		/// </summary>
		public string ActionDetail { get; set; }

		/// <summary>
		/// Gets or sets the type of the entity.
		/// </summary>
		/// <value>
		/// The type of the entity.
		/// </value>
		public string EntityType { get; set; }

		/// <summary>
		/// Gets or sets the tracked items.
		/// </summary>
		/// <value>
		/// The tracked items.
		/// </value>
		public List<NameValuePair> TrackedInfo { get; set; }

		/// <summary>
		/// Gets or sets the type of the action detail entity.
		/// </summary>
		/// <value>The type of the action detail entity.</value>
		public string ActionDetailEntityType { get; set; }
	}
}