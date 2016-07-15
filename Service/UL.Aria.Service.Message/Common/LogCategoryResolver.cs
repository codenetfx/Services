using System;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service.Logging;

namespace UL.Aria.Service.Message.Common
{
    /// <summary>
    ///  Resolves a log category based on entity which is the focus of the log message being created.
    /// </summary>
    public class LogCategoryResolver:ILogCategoryResolver
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="entity">The entity to use to resolve the log entry category.</param>
        /// <returns></returns>
        public LogCategory GetCategory(Type entity)
        {
            return LogCategory.InboundOrderMessageService;
        }
    }
}