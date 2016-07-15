using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.InboundOrderProcessing.Logging
{
	/// <summary>
	/// Class MessageIds.
	/// </summary>
    public class MessageIds
    {
		/// <summary>
		/// The inbound order processing message received
		/// </summary>
        public const int InboundOrderProcessingMessageReceived = 14001;
		/// <summary>
		/// The inbound order processing message exception
		/// </summary>
        public const int InboundOrderProcessingMessageException = 14002;
		/// <summary>
		/// The inbound order processing process exception
		/// </summary>
        public const int InboundOrderProcessingProcessException = 14003;
		/// <summary>
		/// The inbound order processing process exception_ unable_ to_ add_ service line
		/// </summary>
        public const int InboundOrderProcessingProcessException_Unable_To_Add_ServiceLine = 14004;
		/// <summary>
		/// The inbound order processing process exception_ unable_ to_ add_ location code
		/// </summary>
        public const int InboundOrderProcessingProcessException_Unable_To_Add_LocationCode = 14005;
		/// <summary>
		/// The inbound order processing process exception_ unable_ to_ add_ industry code
		/// </summary>
        public const int InboundOrderProcessingProcessException_Unable_To_Add_IndustryCode = 14006;

	    /// <summary>
	    ///     The buy message request parsed
	    /// </summary>
		public const int InboundOrderProcessingBuyMessageRequestParsed = 14008;

	    /// <summary>
	    ///     The buy message request company found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestCompanyFound = 14009;

	    /// <summary>
	    ///     The buy message request company not found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestCompanyNotFound = 14010;

	    /// <summary>
	    ///     The buy message request company created
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestCompanyCreated = 14011;

	    /// <summary>
	    ///     The buy message request industry code found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestIndustryCodeFound = 14012;

	    /// <summary>
	    ///     The buy message request industry code not found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestIndustryCodeNotFound = 14013;

	    /// <summary>
	    ///     The buy message request industry code created
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestIndustryCodeCreated = 14014;

	    /// <summary>
	    ///     The buy message request industry code updated
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestIndustryCodeUpdated = 14015;

	    /// <summary>
	    ///     The buy message request service code found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestServiceCodeFound = 14016;

	    /// <summary>
	    ///     The buy message request service code not found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestServiceCodeNotFound = 14017;

	    /// <summary>
	    ///     The buy message request service code created
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestServiceCodeCreated = 14018;

	    /// <summary>
	    ///     The buy message request service code updated
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestServiceCodeUpdated = 14019;

	    /// <summary>
	    ///     The buy message request location code found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestLocationCodeFound = 14020;

	    /// <summary>
	    ///     The buy message request location code not found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestLocationCodeNotFound = 14021;

	    /// <summary>
	    ///     The buy message request location code created
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestLocationCodeCreated = 14022;

	    /// <summary>
	    ///     The buy message request location code updated
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestLocationCodeUpdated = 14023;

	    /// <summary>
	    ///     The buy message request validation passed
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestValidationPassed = 14024;

	    /// <summary>
	    ///     The buy message request validation failed
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestValidationFailed = 14025;

	    /// <summary>
	    ///     The buy message request found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestFound = 14026;

	    /// <summary>
	    ///     The buy message request not found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageRequestNotFound = 14027;

	    /// <summary>
	    ///     The buy message order parsed
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderParsed = 14028;

	    /// <summary>
	    ///     The buy message order company found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderCompanyFound = 14029;

	    /// <summary>
	    ///     The buy message order company not found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderCompanyNotFound = 14030;

	    /// <summary>
	    ///     The buy message order validation passed
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderValidationPassed = 14031;

	    /// <summary>
	    ///     The buy message order validation failed
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderValidationFailed = 14032;

	    /// <summary>
	    ///     The buy message order found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderFound = 14033;

	    /// <summary>
	    ///     The buy message order not found
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderNotFound = 14034;

	    /// <summary>
	    ///     The buy message order created
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderCreated = 14035;

	    /// <summary>
	    ///     The buy message order updated
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderUpdated = 14036;

	    /// <summary>
	    ///     The buy message customer parsed
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageCustomerParsed = 14037;

	    /// <summary>
	    ///     The buy message order service line detail parsed
	    /// </summary>
	    public const int InboundOrderProcessingBuyMessageOrderServiceLineDetailParsed = 14038;

		/// <summary>
		/// The inbound order processing dequeue count exceeded
		/// </summary>
		public const int InboundOrderProcessingDequeueCountExceeded = 14039;

		/// <summary>
		/// The inbound order processing process order message exception
		/// </summary>
		public const int InboundOrderProcessingProcessOrderMessageException = 14040;

		/// <summary>
		/// The inbound order processing cleanup new exception
		/// </summary>
		public const int InboundOrderProcessingCleanupNewException = 14041;

		/// <summary>
		/// The inbound order processing cleanup failed exception
		/// </summary>
		public const int InboundOrderProcessingCleanupFailedException = 14042;

		/// <summary>
		/// The inbound order processing cleanup order messages exception
		/// </summary>
		public const int InboundOrderProcessingCleanupOrderMessagesException = 14043;

		/// <summary>
		/// The inbound order processing buy message order missing company
		/// </summary>
		public const int InboundOrderProcessingBuyMessageOrderMissingCompany = 14044;

		/// <summary>
		/// The inbound order processing worker role exception
		/// </summary>
		public const int InboundOrderProcessingWorkerRoleException = 14045;
    }
}
