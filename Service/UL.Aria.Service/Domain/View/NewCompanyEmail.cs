using System;

namespace UL.Aria.Service.Domain.View
{
    /// <summary>
    ///     Create new company email
    /// </summary>
    public class NewCompanyEmail
    {
        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        /// <value>
        /// Order Number.
        /// </value>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// Company Name.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        /// <value>
        /// Account Number.
        /// </value>
        public string AccountNumber { get; set; }        
    }
}