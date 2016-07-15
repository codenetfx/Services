using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Microsoft.Practices.Unity.InterceptionExtension;

using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Manager;
using UL.Aria.Service.Provider;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Authorization;

namespace UL.Aria.Service.Auditing
{
	/// <summary>
	/// Class AuditInterceptionBehaviorBase.
	/// </summary>
	public abstract class AuditInterceptionBehaviorBase<T, TDto> : IInterceptionBehavior where T : class
	{
		private readonly IHistoryProvider _historyProvider;
		private readonly IPrincipalResolver _principalResolver;
		private readonly IProfileManager _profileManager;


		/// <summary>
		/// Initializes a new instance of the <see cref="AuditInterceptionBehaviorBase{T, TDto}"/> class.
		/// </summary>
		/// <param name="historyProvider">The history provider.</param>
		/// <param name="principalResolver">The principal resolver.</param>
		/// <param name="profileManager">The profile manager.</param>
		protected AuditInterceptionBehaviorBase(IHistoryProvider historyProvider, IPrincipalResolver principalResolver,
			IProfileManager profileManager)
		{
			_historyProvider = historyProvider;
			_principalResolver = principalResolver;
			_profileManager = profileManager;
		}

		/// <summary>
		/// Implement this method to execute your behavior processing.
		/// </summary>
		/// <param name="input">Inputs to the current call to the target.</param>
		/// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
		/// <returns>
		/// Return value from the target.
		/// </returns>
		public IMethodReturn Invoke(IMethodInvocation input,
			GetNextInterceptionBehaviorDelegate getNext)
		{
			IEnumerable<T> entities = null;
			var isAuditable = input.MethodBase.DeclaringType.GetCustomAttribute<AuditAttribute>() != null;
			AuditResourceAttribute attribute = null;

			if (isAuditable)
			{
				attribute = input.MethodBase.GetCustomAttribute<AuditResourceAttribute>();
				if (attribute != null)
				{
					if (input.Arguments.ContainsParameter(attribute.Target))
					{
						var entity = input.Arguments[attribute.Target] as T;
						if (entity == null)
						{
							var entityId = input.Arguments[attribute.Target] as Guid?;
							if (entityId == null)
							{
								entities = input.Arguments[attribute.Target] as IEnumerable<T>;
							}
							else
							{
								entity = GetEntity(entityId.GetValueOrDefault());
								entities = new List<T> { entity };
							}
						}
						else
						{
							entities = new List<T> {entity};
						}
					}
					else
					{
						//find argument by strategy
						var entityParameters = input.Arguments.OfType<T>().ToList();

						if (entityParameters.Count() == 1)
						{
							var entity = entityParameters.First();
							entities = new List<T> {entity};
						}
						else
						{
							var entitiesParameters = input.Arguments.OfType<IEnumerable<T>>().ToList();

							if (entitiesParameters.Count() == 1)
							{
								entities = entitiesParameters.First();
							}
						}
					}
				}
				else if (input.MethodBase.GetCustomAttribute<AuditIgnoreAttribute>() == null)
				{
					var exMessage = string.Format(
						"The Method {0}.{1} belongs to an interfaces that requires auditing. Either the AuditResource or AuditIgnore attribute needs to be added explicitly to the method.",
						input.MethodBase.DeclaringType, input.MethodBase.Name);
					throw new AuditConfigurationException(exMessage);
				}
			}

			// Invoke the next behavior in the chain.
			var result = getNext()(input, getNext);

			if (result.Exception == null && entities != null)
			{
				var userName = _principalResolver.Current.Identity.Name;
				var userProfile = _profileManager.FetchByUserName(userName);
				foreach (var entity in entities)
				{
					var dto = ConvertToDto(entity);
					_historyProvider.Create(new History
					{
						HistoryId = Guid.NewGuid(),
						EntityId = GetEntityId(entity),
						ActionUserId = userProfile.Id.GetValueOrDefault(),
						ActionDate = DateTime.UtcNow,
						EntityType = GetEntityType(dto),
						ActionDetail = XmlSerialize(dto),
						ActionType = attribute != null && !string.IsNullOrWhiteSpace(attribute.ActionType) ? attribute.ActionType : string.Format("{0}.{1}", input.MethodBase.DeclaringType, input.MethodBase.Name),
						ActionDetailEntityType = dto.GetType().GetAssemblyQualifiedTypeName()
					});
				}
			}

			return result;
		}

		/// <summary>
		/// Gets the type of the entity.
		/// </summary>
		/// <param name="dto">The dto.</param>
		/// <returns>System.String.</returns>
		protected virtual string GetEntityType(TDto dto)
		{
			return dto.GetType().GetAssemblyQualifiedTypeName();
		}

	    /// <summary>
	    /// Gets the entity identifier.
	    /// </summary>
	    /// <param name="entity">The entity.</param>
	    /// <returns>Guid.</returns>
	    protected abstract Guid GetEntityId(T entity);

		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <param name="entityId">The entity identifier.</param>
		/// <returns>T.</returns>
		protected virtual T GetEntity(Guid entityId)
		{
			return null;
		}

		/// <summary>
		/// when implemented in a derived class, returns a DataContract Serializable
		/// object to be stored as the audit details.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		protected abstract TDto ConvertToDto(T entity);

        /// <summary>
        /// XMLs the serialize.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
		protected virtual string XmlSerialize(TDto dto)
		{
            return XmlSerializeInternal(dto);
		}

        /// <summary>
        /// XMLs the serialize internal.
        /// </summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        public static string XmlSerializeInternal(TDto dto)
        {
            string result;
            var serializer = new DataContractSerializer(dto.GetType());

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, dto);
                stream.Flush();
                stream.Position = 0;
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }

            return result;
        }

		/// <summary>
		/// Returns the interfaces required by the behavior for the objects it intercepts.
		/// </summary>
		/// <returns>
		/// The required interfaces.
		/// </returns>
		public IEnumerable<Type> GetRequiredInterfaces()
		{
			return Type.EmptyTypes;
		}

		/// <summary>
		/// Returns a flag indicating if this behavior will actually do anything when invoked.
		/// </summary>
		/// <remarks>
		/// This is used to optimize interception. If the behaviors won't actually
		///             do anything (for example, PIAB where no policies match) then the interception
		///             mechanism can be skipped completely.
		/// </remarks>
		public bool WillExecute
		{
			get { return true; }
		}
	}
}