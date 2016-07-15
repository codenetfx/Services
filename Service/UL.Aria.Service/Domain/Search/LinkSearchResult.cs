using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
    /// <summary>
    /// Link Search Result.
    /// </summary>
    public class LinkSearchResult:SearchResult
    {
        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public Link Link { get; set; }
    }
}
