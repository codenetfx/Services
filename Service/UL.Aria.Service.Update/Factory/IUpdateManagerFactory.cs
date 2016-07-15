using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Update.Managers;

namespace UL.Aria.Service.Update.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUpdateManagerFactory
    {
        /// <summary>
        /// Gets the manager.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns></returns>
        IUpdateManager GetManager(UL.Aria.Service.Contracts.Dto.EntityTypeEnumDto entityType);
    }
}
