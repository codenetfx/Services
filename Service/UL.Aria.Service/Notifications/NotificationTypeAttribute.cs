using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Notifications
{
    /// <summary>
    /// Provides an attribute for tagging strategies with the NotificationTypeDto.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class NotificationTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeAttribute"/> class.
        /// </summary>
        /// <param name="notificationtype">The notificationtype.</param>
        public NotificationTypeAttribute(NotificationTypeDto notificationtype)
        {
            this.NotificationType = notificationtype;
        }

        /// <summary>
        /// Gets the type of the notification.
        /// </summary>
        /// <value>
        /// The type of the notification.
        /// </value>
        public NotificationTypeDto NotificationType { get; private set; }

    }
}
