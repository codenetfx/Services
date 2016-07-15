using System;
using System.Collections.Generic;
using System.Linq;
using UL.Enterprise.Foundation.Logging;
using UL.Enterprise.Foundation.Service.Logging;
using UL.Aria.Service.Claim.Contract;
using UL.Aria.Service.Contracts.Service;

namespace UL.Aria.Service.Host.Common
{
    /// <summary>
    ///  Resolves a log category based on entity which is the focus of the log message being created.
    /// </summary>
    public class LogCategoryResolver:ILogCategoryResolver
    {
        private static readonly Dictionary<string, LogCategory> Declarations = new Dictionary<string, LogCategory>
            {
                {typeof (ICompanyService).Name, LogCategory.Company},
                {typeof (IProfileService).Name, LogCategory.Profile},
                {typeof (IClaimDefinitionService).Name, LogCategory.Claim},
                {typeof (IUserClaimService).Name, LogCategory.Claim}
            };

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="entity">The entity to use to resolve the log entry category.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public LogCategory GetCategory(Type entity)
        {
            var defaultValue = new KeyValuePair<string, LogCategory>(String.Empty, LogCategory.System);
            KeyValuePair<string, LogCategory> kvp =
                    Declarations.Where(i => i.Key == entity.Name)
                                .DefaultIfEmpty(defaultValue)
                                .First();
            return kvp.Value;
        }
    }
}