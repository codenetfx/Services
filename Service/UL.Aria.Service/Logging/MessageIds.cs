using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Logging
{
    internal static class MessageIds
    {
        public const int ProjectStatusMessagePublished = 30100;
        public const int ProjectStatusMessagePublishError = 30101;
        public const int ProjectStatusMessageDequeued = 30102;
        public const int ProjectStatusMessageDequeueError = 30103;
        public const int OutboundMessageException = 30150;
        public const int OutboundMessageRetrieved = 30151;
        
        public const int ProductUploadDocumentImportMissingDocument = 30200;
        public const int ProductUploadDocumentImportError = 30201;
        public const int ProductUploadDocumentIdentifierInvalidFormat = 30202;
        public const int ProductUploadDocumentExistingDocumentNotFonund = 30203;
        public const int ProductUploadDocumentCharacteristicMissingDocument = 30204;
        public const int ProductUploadImportProcessException = 30250;
        public const int ProductUploadImportSaveSubmittedProductException = 30251;
        public const int ProductUploadImportPersistSubmittedProductException = 30260;
        public const int ProductUploadImportPersistUploadResultException = 30252;
        public const int ProductUploadImportException = 30253;
        public const int ProductUploadScratchSpaceException = 30254;
        public const int ProductUploadScratchSpaceDocumentNotFoundException = 30255;
        public const int ProductUploadPersistCharacteristicsException = 30256;
        public const int ProductUploadPersistException = 30257;
        public const int ProductUploadInsertManagerException = 30258;
        public const int ProductUploadInsertManagerNotFoundException = 30259;
        public const int ProductServiceUpdateFromUploadException = 30270;

        public const int ProductFamilyNotFoundException = 30250;

        public const int SearchProviderSearchFailed = 9990;
        public const int SearchProviderSearchComplete = 9991;
        public const int SearchSharePointErrorWebException = 9992;
        public const int SearchSharePointErrorGeneralException = 9993;

        public const int UnableToSendAccountCreatedEmail = 9801;

        public const int ProjectManangerProjectCannotBeCanceledWithLines = 9700;
        public const int ProjectManangerProjectCannotChangeCompany = 9701;
        public const int IncomingOrderProjectCreate = 9702;
        public const int ProjectProviderStatusChanged = 9703;

        public const int TaskManagerException = 30300;
        public const int TaskManagerDeleteTaskAssetNotFoundException = 30301;
        public const int TaskBehaviorPropertyNotFound = 30302;

	    public const int CustomerManagerStart = 30400;
	    public const int CustomerManagerEnd = 30401;
	    public const int CustomerManagerExteranlServiceStart = 30402;
	    public const int CustomerManagerEndpointNotFoundException = 30403;
	    public const int CustomerManagerDatabaseItemNotFoundException = 30404;


		public const int RequestCreated = 30500;
		public const int RequestUpdated = 30501;
		public const int RequestDeleted = 30502;
		public const int ProjectUpdated = 30503;

        public const int CacheGetIndexTimeoutException = 30600;
        public const int CacheGetItemTimeoutException = 30601;

        public const int ContactManagerContactOrderDtoQueued = 30501;
        public const int ContactManagerContactOrderDtoException = 30502;
    }
}



