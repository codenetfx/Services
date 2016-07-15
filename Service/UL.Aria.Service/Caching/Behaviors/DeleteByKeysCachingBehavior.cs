using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Logging;


namespace UL.Aria.Service.Caching.Behaviors
{
    /// <summary>
    /// Provides a Caching behavior for deleting cached items based on the Keys in the specified in the decorated method signature.
    /// </summary>
    public class DeleteByKeysCachingBehavior : CacheBehaviorBase
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
            var methodReturn = base.Execute(input, getNext, cacheManager, attribute, logManager);

            var key = this.GetLookupKey(input, attribute);
            if (!string.IsNullOrWhiteSpace(key))
            {
                cacheManager.RemoveItem(key);
            }

            return methodReturn;
        }
    }
}
