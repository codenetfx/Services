using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Utility;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;
using Guard = UL.Enterprise.Foundation.Framework.Guard;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// 
    /// </summary>
    [AutoRegisterRestService]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class CertificationManagementService:ICertificationManagementService
    {
        private readonly ICertificationRequestManager _certificationRequestManager;
        private readonly IMapperRegistry _mapperRegistry;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificationManagementService"/> class.
        /// </summary>
        /// <param name="certificationRequestManager">The certification request manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public CertificationManagementService(ICertificationRequestManager certificationRequestManager, IMapperRegistry mapperRegistry)
        {
            _certificationRequestManager = certificationRequestManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Submits a certification request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public string PublishCertificationRequest(CertificationManagementDto request)
        {
            Guard.IsNotNull(request, "request");
            var entity = _mapperRegistry.Map<CertificationRequestTaskProperty>(request);

            return _certificationRequestManager.PublishRequest(entity);
        }

        /// <summary>
        /// Fetches the certification requests.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">taskId</exception>
        public IEnumerable<CertificationManagementDto> FetchCertificationRequests(string taskId)
        {
            Guard.IsNotNullOrEmpty(taskId, "taskId");
            Guid id;
            if (!Guid.TryParse(taskId, out id))
                throw new ArgumentException("taskId");
            var results = _certificationRequestManager.FetchRequests(id);
            return _mapperRegistry.Map<IEnumerable<CertificationManagementDto>>(results);
        }
    }
}
