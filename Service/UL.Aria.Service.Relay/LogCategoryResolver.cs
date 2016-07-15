using System;
using System.Diagnostics.CodeAnalysis;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service.Logging;

namespace UL.Aria.Service.Relay
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class LogCategoryResolver:ILogCategoryResolver
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="entity">The entity to use to resolve the log entry category.</param>
        /// <returns></returns>
        public LogCategory GetCategory(Type entity)
        {
            return LogCategory.ProductRelay;
        }
    }
}