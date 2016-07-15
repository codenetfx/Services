using System;
using System.Collections.Generic;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Notifications;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Provides the default implementation for a Notification Manager.
	/// </summary>
	public class NotificationManager : INotificationManager
	{
		private readonly INotificationProvider _notificationProvider;
		private readonly ITransactionFactory _transactionFactory;
		private readonly INotificationStrategyFactory _notificationFactory;

		/// <summary>
		/// Initializes a new instance of the <see cref="NotificationManager" /> class.
		/// </summary>
		/// <param name="notificationProvider">The notification provider.</param>
		/// <param name="notificationFactory">The notification factory.</param>
		/// <param name="transactionFactory">The transaction factory.</param>
		public NotificationManager(INotificationProvider notificationProvider,
			INotificationStrategyFactory notificationFactory, ITransactionFactory transactionFactory)
		{
			_notificationProvider = notificationProvider;
			_notificationFactory = notificationFactory;
			_transactionFactory = transactionFactory;
		}

		/// <summary>
		/// Fetches the notification matching the specified id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public Notification FetchById(Guid id)
		{
			return _notificationProvider.FetchById(id);
		}

		/// <summary>
		/// Fetches all active notifications associated with the specified userId.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		public IEnumerable<Notification> FetchNotificationsByUser(Guid userId)
		{
			return _notificationProvider.FetchNotificationsByUser(userId);
		}


		/// <summary>
		/// Fetches the notifications by entity.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns></returns>
		public IEnumerable<Notification> FetchNotificationsByEntity(Guid entityId)
		{
			return _notificationProvider.FetchNotificationsByEntity(entityId);
		}

		/// <summary>
		/// Deletes a notification with the specified notificationId.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Delete(Guid id)
		{
			using (var trans = _transactionFactory.Create())
			{
				_notificationProvider.Delete(id);
				trans.Complete();
			}
		}


		/// <summary>
		/// Deletes the notifications for the specified entityId.
		/// </summary>
		/// <param name="entityId"></param>
		public void DeleteNotificationsForEntity(Guid entityId)
		{
			using (var trans = _transactionFactory.Create())
			{
				_notificationProvider.DeleteNotificationsForEntity(entityId);
				trans.Complete();
			}
		}

		/// <summary>
		/// Creates the specified notification.
		/// </summary>
		/// <param name="notification">The notification.</param>
		/// <returns></returns>
		public Notification Create(Notification notification)
		{
			return _notificationProvider.Create(notification);
		}

		/// <summary>
		/// Updates the specified notification.
		/// </summary>
		/// <param name="notification">The notification.</param>
		public void Update(Notification notification)
		{
			using (var trans = _transactionFactory.Create())
			{
				_notificationProvider.Update(notification);
				trans.Complete();
			}
		}


		/// <summary>
		/// Updates the list of notifications in the bulk for the specified entityId.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <param name="entityId">The entity identifier.</param>
		public void UpdateBulk(IEnumerable<Notification> notifications, Guid entityId)
		{
			using (var trans = _transactionFactory.Create())
			{
				_notificationProvider.UpdateBulk(notifications, entityId);
				trans.Complete();
			}
		}

		/// <summary>
		/// Deletes the specified entities.
		/// </summary>
		/// <param name="entities">The entities.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void Delete(IEnumerable<Notification> entities)
		{
			using (var trans = _transactionFactory.Create())
			{
				_notificationProvider.Delete(entities);
				trans.Complete();
			}
		}

		/// <summary>
		/// Processes the notification strategies.
		/// </summary>
		/// <param name="notifications">The notifications.</param>
		/// <param name="entity">The entity.</param>
		public void ProcessNotifications(List<NotificationTypeDto> notifications, DomainEntity entity)
		{
			var strategies = _notificationFactory.GetStrategies(notifications);
			strategies.ForEach(x => x.Run(entity));
		}
	}
}