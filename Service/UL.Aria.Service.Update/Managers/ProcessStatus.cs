using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Update.Managers
{
    /// <summary>
    /// Provides a type to indicate the statis of a long runing process.
    /// </summary>
    public enum ProcessStatus
    {
        /// <summary>
        /// The processing
        /// </summary>
        Processing = 1,

        /// <summary>
        /// The completed
        /// </summary>
        Completed = 2,

        /// <summary>
        /// The interupted
        /// </summary>
        Interupted = 3,

        /// <summary>
        /// The cancelled
        /// </summary>
        Cancelled = 4
    }
}
