using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Caching
{
    /// <summary>
    /// Provides a factory for retrieving Cached Behaviors
    /// </summary>
    public class CacheBehaviorFactory:ICacheBehaviorFactory
    {

        /// <summary>
        /// Gets the cache behavior based on the configuration in the specified attribute.
        /// </summary>
        /// <param name="attr">The attribute.</param>
        /// <returns></returns>
        public ICacheBehavior GetCacheBehavior(CacheResourceAttribute attr)
        {
            if (attr == null || attr.CacheBehavior == null)
                return new Behaviors.CachePassthroughBehavior();

            return attr.CacheBehavior;
        }

    }
}
