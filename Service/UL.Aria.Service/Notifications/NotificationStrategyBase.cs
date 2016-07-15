using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Domain;

namespace UL.Aria.Service.Notifications
{
	/// <summary>
	/// Class OrderNotificationStrategyBase.
	/// </summary>
	public abstract class NotificationStrategyBase<T> : INotificationStrategy<T> where T : DomainEntity
	{
		/// <summary>
		/// The _notification manager
		/// </summary>
// ReSharper disable once InconsistentNaming
		protected readonly INotificationManager _notificationManager;

		/// <summary>
		/// The _principal resolver
		/// </summary>
// ReSharper disable once InconsistentNaming
		protected readonly IPrincipalResolver _principalResolver;

		/// <summary>
		/// The _profile manager
		/// </summary>
// ReSharper disable once InconsistentNaming
		protected readonly IProfileManager _profileManager;

		/// <summary>
		/// The _current user identifier
		/// </summary>
// ReSharper disable once InconsistentNaming
		protected readonly Guid _currentUserId;

		/// <summary>
		/// Initializes a new instance of the <see cref="NotificationStrategyBase{T}"/> class.
		/// </summary>
		/// <param name="notificationManager">The notification manager.</param>
		/// <param name="profileManager">The profile manager.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		protected NotificationStrategyBase(INotificationManager notificationManager,
			IProfileManager profileManager, IPrincipalResolver principalResolver)
		{
			_notificationManager = notificationManager;
			_principalResolver = principalResolver;
			_profileManager = profileManager;
			if (_principalResolver != null)
			{
				_currentUserId = _principalResolver.UserId;
			}
		}

		/// <summary>
		/// Gets the notification body.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="comment">The comment.</param>
		/// <returns>System.String.</returns>
		protected string GetNotificationBody(string title, string comment = null)
		{
			return string.Format("{{\"Title\": \"{0}\", \"Details\": {{\"Comment\": \"{1}\" }} }}", title, comment);
		}

		/// <summary>
		/// Gets the notification body.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.String.</returns>
		protected internal abstract string GetNotificationBody(T entity);

		/// <summary>
		/// Gets the container identifier.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>System.Nullable&lt;Guid&gt;.</returns>
		protected internal abstract Guid? GetContainerId(T entity);

		/// <summary>
		/// Gets the start date.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>DateTime.</returns>
		protected virtual DateTime? GetStartDate(T entity)
		{
			return DateTime.UtcNow;
		}

		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		/// <value>The type of the entity.</value>
		protected internal abstract EntityTypeEnumDto EntityType { get; }

		/// <summary>
		/// When implemented in derived classes, it creates
		/// a notification using its particular strategy.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void Run(DomainEntity entity)
		{
			Run(entity as T);
		}

		/// <summary>
		/// Deletes the notifications.
		/// </summary>
		/// <param name="allNotifications">All notifications.</param>
		/// <param name="notificationType">Type of the notification.</param>
		protected virtual void DeleteNotifications(IEnumerable<Notification> allNotifications,
			NotificationTypeDto notificationType)
		{
			var notificationsToDelete = allNotifications.Where(x => x.NotificationType == notificationType);
			_notificationManager.Delete(notificationsToDelete);
		}

		/// <summary>
		/// When implemented in derived classes, it creates
		/// a notification using its particular strategy.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public virtual void Run(T entity)
		{
			//delete existing canceled notifications.
			var notificationType = GetType().GetCustomAttribute<NotificationTypeAttribute>().NotificationType;
			var allNotifications = _notificationManager.FetchNotificationsByEntity(entity.Id.GetValueOrDefault());
			DeleteNotifications(allNotifications, notificationType);

			foreach (var userId in GetUserList(entity).Distinct())
			{
				_notificationManager.Create(new Notification(Guid.NewGuid())
				{
					Body = GetNotificationBody(entity),
					StartDate = GetStartDate(entity),
					EntityId = entity.Id.GetValueOrDefault(),
					EntityType = EntityType,
					ContainerId = GetContainerId(entity),
					NotificationType = notificationType,
					UserId = userId,
					CreatedById = _currentUserId,
					UpdatedById = _currentUserId,
					CreatedDateTime = DateTime.UtcNow,
					UpdatedDateTime = DateTime.UtcNow
				});
			}
		}

		/// <summary>
		/// Gets the user list.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns>IList&lt;Guid&gt;.</returns>
		protected internal abstract IList<Guid> GetUserList(T entity);
	}
}