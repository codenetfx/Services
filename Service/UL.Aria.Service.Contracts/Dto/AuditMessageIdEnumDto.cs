using System.Runtime.Serialization;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Enum AuditMessageIdEnum
    /// </summary>
    [DataContract]
    public enum AuditMessageIdEnumDto
    {
        /// <summary>
        ///     The unknown, needed for serialization
        /// </summary>
        [EnumMember(Value = "Unknown")] Unknown = 0,

        /// <summary>
        ///     The update product status
        /// </summary>
        [EnumMember(Value = "UpdateProductStatus")] UpdateProductStatus = 301,

        /// <summary>
        ///     The product deleted
        /// </summary>
        [EnumMember(Value = "ProductDeleted")] ProductDeleted = 311,

        /// <summary>
        ///     The contact us
        /// </summary>
        [EnumMember(Value = "ContactUs")] ContactUs = 400,

        /// <summary>
        ///     The send email
        /// </summary>
        [EnumMember(Value = "SendEmail")] SendEmail = 401,

        /// <summary>
        ///     The accept terms and conditions
        /// </summary>
        [EnumMember(Value = "AcceptTermsAndConditions")] AcceptTermsAndConditions = 500,

        /// <summary>
        ///     The add user claim
        /// </summary>
        [EnumMember(Value = "AddUserClaim")] AddUserClaim = 1001,

        /// <summary>
        ///     The remove user claim
        /// </summary>
        [EnumMember(Value = "RemoveUserClaim")] RemoveUserClaim = 1002,
        /// <summary>
        /// The remove all user claims
        /// </summary>
        [EnumMember(Value = "RemoveAllUserClaims")] RemoveAllUserClaims = 1002,

        /// <summary>
        ///     The demand access denied
        /// </summary>
        [EnumMember(Value = "DemandAccessDenied")] DemandAccessDenied = 2001,

        /// <summary>
        ///     The delete user
        /// </summary>
        [EnumMember(Value = "DeleteUser")] DeleteUser = 3001,

        /// <summary>
        ///     The create user
        /// </summary>
        [EnumMember(Value = "CreateUser")] CreateUser = 3002,

        /// <summary>
        ///     The update user
        /// </summary>
        [EnumMember(Value = "UpdateUser")] UpdateUser = 3003,

        /// <summary>
        ///     The incoming request validation error
        /// </summary>
        [EnumMember(Value = "IncomingRequestValidationError")] IncomingRequestValidationError = 4006,

		/// <summary>
		///     The incoming request exception
		/// </summary>
		[EnumMember(Value = "IncomingRequestException")]
		IncomingRequestException = 4007,

        /// <summary>
        ///     The buy message received
        /// </summary>
        [EnumMember(Value = "BuyMessageReceived")] BuyMessageReceived = 10001,

        /// <summary>
        ///     The buy message not formed correctly
        /// </summary>
        [EnumMember(Value = "BuyMessageNotFormedCorrectly")] BuyMessageNotFormedCorrectly = 10002,

        /// <summary>
        ///     The buy message enqueued
        /// </summary>
        [EnumMember(Value = "BuyMessageEnqueued")] BuyMessageEnqueued = 10003,

        /// <summary>
        ///     The buy message processed
        /// </summary>
        [EnumMember(Value = "BuyMessageProcessed")] BuyMessageProcessed = 10004,

        /// <summary>
        ///     The project initiated
        /// </summary>
        [EnumMember(Value = "ProjectInitiated")] ProjectInitiated = 10005,

        /// <summary>
        ///     The buy message dequeued
        /// </summary>
        [EnumMember(Value = "BuyMessageDequeued")] BuyMessageDequeued = 10007,

        /// <summary>
        ///     The recover offline messages
        /// </summary>
        [EnumMember(Value = "RecoverOfflineMessages")] RecoverOfflineMessages = 12001,

        /// <summary>
        ///     The handle message
        /// </summary>
        [EnumMember(Value = "HandleMessage")] HandleMessage = 12002,


        /// <summary>
        ///     The delete company
        /// </summary>
        [EnumMember(Value = "DeleteCompany")]
        DeleteCompany = 3051,

        /// <summary>
        ///     The create company
        /// </summary>
        [EnumMember(Value = "CreateCompany")]
        CreateCompany = 3052,

        /// <summary>
        ///     The update company
        /// </summary>
        [EnumMember(Value = "UpdateCompany")]
        UpdateCompany = 3053,

		/// <summary>
		/// The document message dequeued
		/// </summary>
		[EnumMember(Value = "DocumentMessageDequeued")]
		DocumentMessageDequeued = 13000,

		/// <summary>
		/// The contact order message dequeueds
		/// </summary>
		[EnumMember(Value = "ContactOrderMessageDequeued")]
		ContactOrderMessageDequeued = 14000,

		/// <summary>
		/// The task template created
		/// </summary>
		[EnumMember(Value = "TaskTemplateCreated")]
		TaskTemplateCreated = 15000,
		/// <summary>
		/// The task template updated
		/// </summary>
		[EnumMember(Value = "TaskTemplateUpdated")]
		TaskTemplateUpdated = 15001,
		/// <summary>
		/// The task template deleted
		/// </summary>
		[EnumMember(Value = "TaskTemplateDeleted")]
		TaskTemplateDeleted = 15002,
	}
}