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
	public abstract class AuditInterceptionBehaviorEntityBase<T, TDto> : AuditInterceptionBehaviorBase<T, TDto> where T : TrackedDomainEntity
	{
		private readonly IHistoryProvider _historyProvider;
		private readonly IPrincipalResolver _principalResolver;
		private readonly IProfileManager _profileManager;


        /// <summary>
        /// Initializes a new instance of the <see cref="AuditInterceptionBehaviorBase{T, TDto}" /> class.
        /// </summary>
        /// <param name="historyProvider">The history provider.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        /// <param name="profileManager">The profile manager.</param>
		protected AuditInterceptionBehaviorEntityBase(IHistoryProvider historyProvider, IPrincipalResolver principalResolver,
			IProfileManager profileManager)
            : base(historyProvider, principalResolver, profileManager)
		{
			_historyProvider = historyProvider;
			_principalResolver = principalResolver;
			_profileManager = profileManager;
		}

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Guid.
        /// </returns>
	    protected override Guid GetEntityId(T entity)
	    {
	        return entity.Id.GetValueOrDefault();
	    }
	}
}