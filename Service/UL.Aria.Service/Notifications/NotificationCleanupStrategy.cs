using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Domain;
using Ntype = UL.Aria.Service.Contracts.Dto.NotificationTypeDto;

namespace UL.Aria.Service.Notifications
{
    /// <summary>
    /// 
    /// </summary>
    [NotificationType(Ntype.EntityCleanup)]
    public class NotificationCleanupStrategy: INotificationStrategy
    {
        private readonly INotificationManager _notificationManaager;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationCleanupStrategy"/> class.
        /// </summary>
        /// <param name="notificationManaager">The notification manaager.</param>
        public NotificationCleanupStrategy(INotificationManager notificationManaager)
        {
            _notificationManaager = notificationManaager;
        }

        /// <summary>
        /// When implmented in derived classes, it manages 
        /// notifications using its particular strategy.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Run(DomainEntity entity)
        {
            _notificationManaager.DeleteNotificationsForEntity(entity.Id.Value);
        }
    }
}
