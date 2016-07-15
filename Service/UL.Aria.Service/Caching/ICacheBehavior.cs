using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Caching
{
    /// <summary>
    /// Provides an interface for a Cache Behavior strategy
    /// </summary>
    public interface ICacheBehavior
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
        IMethodReturn Execute(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext, ICacheManager cacheManager, CacheResourceAttribute attribute, ILogManager logManager);

    }
}
