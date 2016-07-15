namespace UL.Aria.Service.InboundOrderProcessing.MessageProcessor
{
    /// <summary>
    /// Message Ids for this assembly.
    /// </summary>
    public static class MessagesIds
    {
        /// <summary>
        /// The incoming order database item not found exception
        /// </summary>
        public const int ContactProcessorIncomingOrderDatabaseItemNotFoundException = 512;
        /// <summary>
        /// The projects database item not found exception
        /// </summary>
        public const int ContactProcessorProjectsDatabaseItemNotFoundException = 513;
        
        /// <summary>
        /// The parties not found
        /// </summary>
        public const int ContactProcessorPartiesEndpointNotFoundException = 514;

        /// <summary>
        /// The parties database item not found exception
        /// </summary>
        public const int ContactProcessorPartiesDatabaseItemNotFoundException = 515;

        /// <summary>
        /// The contact processor parties not found
        /// </summary>
        public const int ContactProcessorPartiesNotFound = 516;

        /// <summary>
        /// The contact processor unable to fill customer
        /// </summary>
        public const int ContactProcessorUnableToFillCustomer = 517;

        /// <summary>
        /// The contact processor unable to fill agent
        /// </summary>
        public const int ContactProcessorUnableToFillAgent = 518;

        /// <summary>
        /// The contact processor parties fault exception
        /// </summary>
        public const int ContactProcessorPartiesFaultException = 519;
        /// <summary>
        /// The contact processor parties exception
        /// </summary>
        public const int ContactProcessorPartiesGeneralException = 520;
    }
}