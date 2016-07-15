using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Logging;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Caching.Behaviors
{
    /// <summary>
    /// Provides a behavior for creating a cache index entry for resolvoing the Entity.Id based 
    /// on the Unique Key specified as a parameter in the decorated method.
    /// </summary>
    public class CacheIndexIdByUniqueKey : CacheBehaviorBase
    {

        /// <summary>
        /// Executes the cache behavior
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="getNext">The get next.</param>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="logManager">The log manager.</param>
        /// <returns></returns>
        public override IMethodReturn Execute(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext, ICacheManager cacheManager, CacheResourceAttribute attribute, ILogManager logManager)
        {
            IMethodReturn methodReturn = null;

            try
            {
                var key = GetLookupKey(input, attribute);

                string instanceId = null;
                try
                {
                    instanceId = cacheManager.GetIndex(key);
                }
                catch (TimeoutException ex)
                {
                    base.LogCacheException(logManager, ex, MessageIds.CacheGetIndexTimeoutException, key);

                    // If we're having timeout problems retrieving from the cache, just execute 
                    // the delegate and return rather than also trying to update the cache.
                    return base.Execute(input, getNext, cacheManager, attribute, logManager);
                }

                if (!string.IsNullOrWhiteSpace(instanceId))
                {
                    object instance = null;
                    try
                    {
                        instance = cacheManager.GetItem(instanceId);
                    }
                    catch (TimeoutException ex)
                    {
                        base.LogCacheException(logManager, ex, MessageIds.CacheGetItemTimeoutException, key);

                        return base.Execute(input, getNext, cacheManager, attribute, logManager);
                    }

                    if (instance != null)
                    {
                        return input.CreateMethodReturn(instance);
                    }
                    else
                    {
                        cacheManager.RemoveIndex(key);
                    }
                }

                methodReturn = base.Execute(input, getNext, cacheManager, attribute, logManager);

                if (methodReturn.ReturnValue != null)
                {
                    var entity = methodReturn.ReturnValue as TrackedDomainEntity;
                    if (entity != null && entity.Id.GetValueOrDefault() != Guid.Empty)
                    {
                        cacheManager.StoreItem(entity.Id.ToString(), entity);
                        cacheManager.StoreIndex(key, entity.Id.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                methodReturn = input.CreateExceptionMethodReturn(e);
            }

            return methodReturn;
        }
    }
}
