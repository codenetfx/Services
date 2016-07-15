using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Update.Results
{
    /// <summary>
    /// Provides a classifier for indicating the result of a task execution.
    /// </summary>
    public class TaskExecutionResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskExecutionResult"/> class.
        /// </summary>
        /// <param name="lookup">The lookup.</param>
        /// <param name="successful">if set to <c>true</c> [successful].</param>
        /// <param name="errorMessage">The error message.</param>
        public TaskExecutionResult(Lookup lookup, bool successful, string errorMessage = "")
        {
            this.Lookup = lookup;
            this.Successful = successful;
            this.ErrorMessage = errorMessage;
        }

     

        /// <summary>
        /// Gets the lookup.
        /// </summary>
        /// <value>
        /// The lookup.
        /// </value>
        public Lookup Lookup { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TaskExecutionResult"/> is successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if successful; otherwise, <c>false</c>.
        /// </value>
        public bool Successful { get; private set; }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
           string format = "{0}\t{1}\t{2}\t{3}";
           return String.Format(format, Lookup.Id.GetValueOrDefault().ToString(), 
               Lookup.Name, 
               this.Successful.ToString(), 
               this.ErrorMessage);
        }

    }
}
