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
    /// Provides a Caching behavior, to cache entity object being passed into the target method.
    /// </summary>
    public class CacheIncomingTargetByIdBehavior : CacheBehaviorBase
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
                methodReturn = base.Execute(input, getNext, cacheManager, attribute, logManager);

                var entities = this.GetEntities<TrackedDomainEntity>(input, attribute);
                if (entities != null && entities.Count() > 0)
                {
                    foreach (var entity in entities)
                    {
                        if (entity != null && entity.Id.GetValueOrDefault() != Guid.Empty)
                            cacheManager.StoreItem(entity.Id.ToString(), entity);
                    }
                }
                else
                {
                    throw new InvalidOperationException(
                        String.Format("Explicit CacheTarget parameter named {3} not found during execution of CacheBehavior {0} decorating method {1}.{2}",
                        this.GetType().FullName, input.MethodBase.DeclaringType.FullName, input.MethodBase.Name, attribute.CacheTarget));
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
