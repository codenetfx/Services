using Microsoft.Practices.Unity.InterceptionExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;
using System.Reflection;
using UL.Enterprise.Foundation.Logging;


namespace UL.Aria.Service.Caching
{

    /// <summary>
    /// Provides a calls for the interception registration for a Caching decorator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheInterceptor<T> : IInterceptionBehavior where T : TrackedDomainEntity
    {
        private readonly ICacheManager _cacheManager;
        private readonly ICacheBehaviorFactory _factory;
        private readonly ILogManager _logManager;


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheInterceptor{T}" /> class.
        /// </summary>
        /// <param name="cacheManager">The cache manager.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="logManager">The log manager.</param>
        public CacheInterceptor(ICacheManager cacheManager, ICacheBehaviorFactory factory, ILogManager logManager)
        {
            _factory = factory;
            _cacheManager = cacheManager;
            _logManager = logManager;
        }

        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts.
        /// </summary>
        /// <returns>
        /// The required interfaces.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// Implement this method to execute your behavior processing.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <returns>
        /// Return value from the target.
        /// </returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            ICacheBehavior behavior = null;
            var attr = this.GetAttribute(input);
            if (attr != null) attr.TargetType = typeof(T);
            behavior = _factory.GetCacheBehavior(attr);
            return behavior.Execute(input, getNext, _cacheManager, attr, _logManager);
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


        /// <summary>
        /// Gets the CacheResouce attribute.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected CacheResourceAttribute GetAttribute(IMethodInvocation input)
        {
            return input.MethodBase.GetCustomAttribute<CacheResourceAttribute>();
        }
              
    }








}
