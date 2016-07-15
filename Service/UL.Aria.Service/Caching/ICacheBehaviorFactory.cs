using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Caching
{
    /// <summary>
    /// Provides an interface for a Cache Behavior Factory
    /// </summary>
    public interface ICacheBehaviorFactory
    {

        /// <summary>
        /// Gets the cache behavior based on the configuration in the specified attribute.
        /// </summary>
        /// <param name="attr">The attribute.</param>
        /// <returns></returns>
        ICacheBehavior GetCacheBehavior(CacheResourceAttribute attr);      

    }
}
