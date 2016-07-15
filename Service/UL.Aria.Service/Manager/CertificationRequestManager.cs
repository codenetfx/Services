using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Provider;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Implements operations for working with <see cref="CertificationRequestTaskProperty"/> objects
    /// </summary>
    public class CertificationRequestManager : ICertificationRequestManager
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskPropertyProvider _taskPropertyProvider;
        private readonly IProjectProvider _projectProvider;
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly ICertificationRequestService _certificationRequestService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificationRequestManager" /> class.
        /// </summary>
        /// <param name="taskPropertyProvider">The task property provider.</param>
        /// <param name="projectProvider">The project provider.</param>
        /// <param name="taskRepository">The task repository.</param>
        /// <param name="certificationRequestService">The certification request service.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        public CertificationRequestManager(ITaskPropertyProvider taskPropertyProvider, IProjectProvider projectProvider, ITaskRepository taskRepository, ICertificationRequestService certificationRequestService, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory)
        {
            _taskRepository = taskRepository;
            _taskPropertyProvider = taskPropertyProvider;
            _projectProvider = projectProvider;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _certificationRequestService = certificationRequestService;
        }

        /// <summary>
        /// Publishes the request.
        /// </summary>
        /// <param name="certificationRequest">The certification request.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string PublishRequest(CertificationRequestTaskProperty certificationRequest)
        {
            using (var scope = _transactionFactory.Create())
            {
                var project = _projectProvider.FetchById(certificationRequest.ProjectId);
                certificationRequest.ProjectEndDate = project.EndDate;
                certificationRequest.CCN = project.CCN;
                certificationRequest.ContactEmail = project.ShipToContact.Email;
                certificationRequest.ContactName = project.ShipToContact.FullName;
                certificationRequest.FileNo = project.FileNo;
                certificationRequest.ProjectId = project.Id.Value;
                certificationRequest.ProjectHandler = project.ProjectHandler;
                certificationRequest.SubscriberNumber = project.ShipToContact.SubscriberNumber;
                certificationRequest.ProjectNumber = project.ProjectNumber;

                _taskPropertyProvider.Create(certificationRequest);
                var dto = _mapperRegistry.Map<CertificationRequestDto>(certificationRequest);
                var publishCertificationRequest = _certificationRequestService.PublishCertificationRequest(dto);
                scope.Complete();
                return publishCertificationRequest;
            }
        }

        /// <summary>
        /// Gets the requests.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<CertificationRequestTaskProperty> FetchRequests(Guid taskId)
        {
            return _taskPropertyProvider.FindByTaskPropertyTypeId<CertificationRequestTaskProperty>(taskId,
                CertificationRequestTaskProperty.CertificationRequestTaskPropertyTypeId);
        }
    }
}