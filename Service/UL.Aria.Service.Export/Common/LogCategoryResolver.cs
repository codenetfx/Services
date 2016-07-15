using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service.Logging;

namespace UL.Aria.Service.Export.Common
{
    /// <summary>
    /// Class that implements the <see cref="ILogCategoryResolver"/> for this assembly.
    /// </summary>
    public class LogCategoryResolver:ILogCategoryResolver
    {
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="entity">The entity to use to resolve the log entry category.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public LogCategory GetCategory(Type entity)
        {
            return LogCategory.Export;
        }
    }
}
