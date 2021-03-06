﻿using System;
using System.Collections.Generic;
using System.Linq;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class TaskReminderRemovalStrategy.
	/// </summary>
	[NotificationType(NotificationTypeDto.TaskReminderRemoval)]
	public sealed class TaskReminderRemovalStrategy : NotificationStrategyBase<Task>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TaskReminderRemovalStrategy" /> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		public TaskReminderRemovalStrategy(INotificationManager notificationManager)
			: base(notificationManager, null, null)
		{
		}

		/// <summary>
		/// When implemented in derived classes, it removes
		/// a notification using its particular strategy.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public override void Run(Task entity)
		{
			if (entity.ReminderDate == null)
			{
				var notifications =
					_notificationManager.FetchNotificationsByEntity(entity.Id.GetValueOrDefault())
						.Where(x => x.NotificationType == NotificationTypeDto.TaskReminder);
				_notificationManager.Delete(notifications);
			}
		}

		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		/// <value>The type of the entity.</value>
		/// <exception cref="System.NotImplementedException"></exception>
		protected internal override EntityTypeEnumDto EntityType
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the notification body.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.String.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		protected internal override string GetNotificationBody(Task entity)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the container identifier.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.Nullable&lt;Guid&gt;.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		protected internal override Guid? GetContainerId(Task entity)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets the user list.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>IList&lt;Guid&gt;.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		protected internal override IList<Guid> GetUserList(Task entity)
		{
			throw new NotImplementedException();
		}
	}
}