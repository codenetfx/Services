using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Provides a Repository interface for Task Notifications.
    /// </summary>
    public interface ITaskNotificationRepository : IPrimaryAssocatedRepository<TaskNotification>
    {
        /// <summary>
        /// Finds the by task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        IEnumerable<TaskNotification> FindByTaskId(Guid taskId);
    }
}
