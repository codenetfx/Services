using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UL.Enterprise.Foundation.Logging;

namespace UL.Aria.Service.Caching
{
    /// <summary>
    /// Provides an abstract class for handling a cache action.
    /// </summary>
    public abstract class CacheBehaviorBase : ICacheBehavior
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
        public virtual IMethodReturn Execute(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext, ICacheManager cacheManager, CacheResourceAttribute attribute, ILogManager logManager)
        {
            return getNext()(input, getNext);
        }

        /// <summary>
        /// Gets the lookup key by concation of all paramater values where the parameter name matches 
        /// the specified key in the attribute's Keys array. 
        /// If no array is supplied, Concats all paramters that are of a suppored CacheKey type.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        internal protected string GetLookupKey(IMethodInvocation input, CacheResourceAttribute attribute)
        {
            var returnKey = string.Empty;

            if (attribute.Keys != null && attribute.Keys.Length > 0)
            {
                foreach (var key in attribute.Keys)
                {
                    if (input.Arguments.ContainsParameter(key))
                    {
                        var arg = input.Arguments[key];

                        if (CacheKeySupport.IsSupported(arg.GetType()))
                        {
                            returnKey += arg.ToString();
                        }
                    }
                }
            }
            else
            {
                //any argument passed that is suppored is part of the key
                foreach (var arg in input.Arguments)
                {
                    if (CacheKeySupport.IsSupported(arg.GetType()))
                    {
                        returnKey += arg.ToString();
                    }
                }
            }

            //throw exception of no key is found
            return returnKey;
        }

        /// <summary>
        /// Gets the entity(s), pased on the attribute's specified CacheTarget.
        /// If CacheTarget value is not supplied looks for an a method paramenter of type T or generic IEnumerable of T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The input.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns></returns>
        internal protected IEnumerable<T> GetEntities<T>(IMethodInvocation input, CacheResourceAttribute attribute) where T : class
        {
            IEnumerable<T> entities = null;

            if (attribute != null)
            {              
                
                if(string.IsNullOrWhiteSpace( attribute.CacheTarget))
                {                    

                    //find argument by strategy
                    if (input.Arguments.Count > 0)
                    {
                        var entityParameters = input.Arguments.OfType<T>();
                  

                        if (entityParameters.Count() == 1)
                        {
                            var entity = entityParameters.First();
                            entities = new List<T> { entity };
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
                else
                {
                    if (input.Arguments.ContainsParameter(attribute.CacheTarget))
                    {
                        var entity = input.Arguments[attribute.CacheTarget] as T;
                        if (entity == null)
                        {
                            entities = input.Arguments[attribute.CacheTarget] as IEnumerable<T>;
                        }
                        else
                        {
                            entities = new List<T> { entity };
                        }
                    }                  
                }

            }

            
            return entities;
        }

        /// <summary>
        /// Logs the cache exception.
        /// </summary>
        /// <param name="logManager">The log manager.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="messageId">The message identifier.</param>
        /// <param name="cacheKey">The cache key.</param>
        internal protected void LogCacheException(ILogManager logManager, Exception ex, int messageId, string cacheKey)
        {
            var logMessage = ex.ToLogMessage(messageId, LogCategory.System, LogPriority.Medium, TraceEventType.Error);
            logMessage.Data.Add("cache-key", cacheKey);
            logManager.Log(logMessage);
        }
    }
}
