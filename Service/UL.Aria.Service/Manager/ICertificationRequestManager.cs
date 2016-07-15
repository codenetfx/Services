using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for working with <see cref="CertificationRequestTaskProperty"/> objects
    /// </summary>
    public interface ICertificationRequestManager
    {
        /// <summary>
        /// Publishes the request.
        /// </summary>
        /// <param name="certificationRequest">The certification request.</param>
        /// <returns></returns>
        string PublishRequest(CertificationRequestTaskProperty certificationRequest );

        /// <summary>
        /// Gets the requests.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IEnumerable<CertificationRequestTaskProperty> FetchRequests(Guid taskId);
    }
}
