using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Update.Results
{
    /// <summary>
    /// Provides a class to send progress informaiton for a long running process.
    /// </summary>
    public class ProgressInfo
    {

        /// <summary>
        /// Gets or sets the completed item identifier of the item that 
        /// completed that trigger this progressInfo to be sent.
        /// </summary>
        /// <value>
        /// The completed item identifier.
        /// </value>
        public Guid CompletedItemId { get; set; }
        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        /// <value>
        /// The total items.
        /// </value>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the processed count.
        /// </summary>
        /// <value>
        /// The processed count.
        /// </value>
        public int ProcessedCount { get; set; }

        /// <summary>
        /// Gets the percentage complete.
        /// </summary>
        /// <value>
        /// The percentage complete.
        /// </value>
        public int PercentageComplete
        {
            get
            {
                if (this.TotalItems > 0)
                    return (int)Math.Floor(((double)this.ProcessedCount / (double)this.TotalItems) * 100);

                return 100;                    
            }
        }
    }
}
