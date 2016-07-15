using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Data;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Defines persistance for <see cref="ProjectStatusMessage"/> objects.
    /// </summary>
    public interface IProjectStatusMessageRepository : IRepositoryBase<ProjectStatusMessage>
    {
        /// <summary>
        /// Gets the next <see cref="ProjectStatusMessage"/>.
        /// </summary>
        /// <returns></returns>
        ProjectStatusMessage GetNext();
    }
}
