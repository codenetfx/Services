using System;
using System.Configuration;


namespace UL.Aria.Service.Caching
{
    /// <summary>
    /// Attribute that designates a method to use a cache behavior.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CacheResourceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheResourceAttribute" /> class.
        /// </summary>
        /// <param name="behaviorType">Type of the behavior.</param>        
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public CacheResourceAttribute(Type behaviorType)
        {

            this.CacheBehavior = Activator.CreateInstance(behaviorType) as ICacheBehavior;

            if (this.CacheBehavior == null)
                throw new ArgumentOutOfRangeException(string.Format("The type {0} is required to implement ICacheBehavior", behaviorType.AssemblyQualifiedName));

        }

        /// <summary>
        /// Gets or sets the cache target.
        /// </summary>
        /// <value>
        /// The cache target.
        /// </value>
        public string CacheTarget { get; set; }

        /// <summary>
        /// Gets or sets the name of the pramamters to be used as Cross Reference keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public String[] Keys { get; set; }

        /// <summary>
        /// Whe the Cache Operation is set to Custom, the is the item that is the 
        /// strategy that will be executed.
        /// </summary>
        /// <value>
        /// The type of the cache behavior.
        /// </value>
        public ICacheBehavior CacheBehavior { get; internal set; }


        /// <summary>
        /// Gets the type of the target associated with the generic type of the interceptor
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType { get; internal set; }


    }
}