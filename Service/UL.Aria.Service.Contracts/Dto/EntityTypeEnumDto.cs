using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Container type enumeration.
	/// </summary>
	[DataContract]
	public enum EntityTypeEnumDto
	{
		/// <summary>
		/// The container entity types.
		/// </summary>
		[EnumMember(Value = "Container")]
		Container,

		/// <summary>
		/// The order entity types.
		/// </summary>
		[EnumMember(Value = "Order")]
		Order,

		/// <summary>
		/// The product entity types.
		/// </summary>
		[EnumMember(Value = "Product")]
		Product,

		/// <summary>
		/// The project entity types.
		/// </summary>
		[EnumMember(Value = "Project")]
		Project,

		/// <summary>
		/// The document entity types.
		/// </summary>
		[EnumMember(Value = "Document")]
		Document,

		/// <summary>
		/// An entity representing an incoming(new) order.
		/// </summary>
		[EnumMember(Value = "IncomingOrder")]
		IncomingOrder,

		/// <summary>
		/// The product family entity types.
		/// </summary>
		[EnumMember(Value = "ProductFamily")]
		ProductFamily,

		/// <summary>
		/// The product upload
		/// </summary>
		[EnumMember(Value = "ProductUpload")]
		ProductUpload,

		/// <summary>
		/// The task
		/// </summary>
		[EnumMember(Value = "Task")]
		Task,
		/// <summary>
		/// An Entity Representing Order Details
		/// </summary>
		[EnumMember(Value = "OrderDetail")]
		OrderDetail,

		/// <summary>
		/// The customer organization
		/// </summary>
		[EnumMember(Value = "CustomerOrganization")]
		CustomerOrganization,

		/// <summary>
		/// pseudo project entity, just search meta of project
		/// </summary>
		[EnumMember(Value = "ProjectMeta")]
		ProjectMeta,

		/// <summary>
		/// The user
		/// </summary>
		[EnumMember(Value = "User")]
		User,

		/// <summary>
		/// The notification
		/// </summary>
		[EnumMember(Value = "Notification")]
		Notification,

		/// <summary>
		/// The project template
		/// </summary>
		[EnumMember(Value = "ProjectTemplate")]
		ProjectTemplate,

		/// <summary>
		/// The task template
		/// </summary>
		[EnumMember(Value = "TaskCategory")]
		TaskCategory,
		/// <summary>
		/// The task type
		/// </summary>
		[EnumMember(Value = "TaskType")]
		TaskType,
		/// <summary>
		/// The business unit
		/// </summary>
		[EnumMember(Value = "BusinessUnit")]
		BusinessUnit,
		/// <summary>
		/// The link
		/// </summary>
		[EnumMember(Value = "Link")]
		Link,
		/// <summary>
		/// The document template
		/// </summary>
		[EnumMember(Value = "DocumentTemplate")]
		DocumentTemplate,

		/// <summary>
		/// The dashboard
		/// </summary>
		[EnumMember]
		Dashboard,
		/// <summary>
		/// The company
		/// </summary>
		[EnumMember(Value = "Company")]
		Company,

		/// <summary>
		/// The industry code
		/// </summary>
		[EnumMember(Value = "IndustryCode")]
		IndustryCode,

		/// <summary>
		/// The service code
		/// </summary>
		[EnumMember(Value = "ServiceCode")]
		ServiceCode,

        /// <summary>
        /// The user team
        /// </summary>
        [EnumMember(Value = "UserTeam")]
        UserTeam,

        /// <summary>
        /// The user team member
        /// </summary>
        [EnumMember(Value = "UserTeamMember")]
        UserTeamMember,

        /// <summary>
        /// The ProjectProjectTemplate
        /// </summary>
        [EnumMember(Value = "ProjectProjectTemplate")]
        ProjectProjectTemplate,

		/// <summary>
		/// The department code
		/// </summary>
		[EnumMember(Value = "DepartmentCode")]
		DepartmentCode,

        /// <summary>
        /// The task property
        /// </summary>
        [EnumMember(Value = "TaskProperty")]
        TaskProperty,

        /// <summary>
        /// The task property type
        /// </summary>
         [EnumMember(Value = "TaskPropertyType")]
	    TaskPropertyType,

        /// <summary>
        /// The task notification
        /// </summary>
        [EnumMember(Value = "TaskNotification")]
        TaskNotification,

        /// <summary>
        /// The task type notification
        /// </summary>
        [EnumMember(Value = "TaskTypeNotification")]
        TaskTypeNotification
	}

}