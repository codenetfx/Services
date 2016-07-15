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
    /// Provides a behavior for Caching a returned object againsed the Key(s) specified as parameters in decorated method signature.
    /// </summary>
    public class CacheReturnValueBehavior : CacheBehaviorBase
    {
        /// <summary>
        /// Executes the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="getNext">The get next.</param>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="attr">The attribute.</param>
        /// <param name="logManager">The log manager.</param>
        /// <returns></returns>
        public override IMethodReturn Execute(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext, ICacheManager cacheManager, CacheResourceAttribute attr, ILogManager logManager)
        {
            IMethodReturn methodReturn = null;

            var key = this.GetLookupKey(input, attr);

            object cachedObject = null;
            try
            {
                cachedObject = cacheManager.GetItem(key);
            }
            catch (TimeoutException ex)
            {
                base.LogCacheException(logManager, ex, MessageIds.CacheGetItemTimeoutException, key);

                // If we're having timeout problems retrieving from the cache, just execute 
                // the delegate and return rather than also trying to update the cache.
                return base.Execute(input, getNext, cacheManager, attr, logManager);
            }

            if (cachedObject != null)
            {
                methodReturn = input.CreateMethodReturn(cachedObject);
                return methodReturn;
            }

            methodReturn = base.Execute(input, getNext, cacheManager, attr, logManager);

            if (methodReturn.ReturnValue != null)
            {
                if (key != string.Empty)
                {
                    cacheManager.StoreItem(key, methodReturn.ReturnValue);
                }
            }

            return methodReturn;

        }
    }
}
