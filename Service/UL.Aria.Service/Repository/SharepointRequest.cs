using System.Collections.Generic;
using System.Net.Http;

namespace UL.Aria.Service.Repository
{
    /// <summary>
    /// Sharepoint request info.
    /// </summary>
    public sealed class SharepointRequest
    {
        /// <summary>
        /// Inializes a new instance of the class.
        /// </summary>
        public SharepointRequest()
        {
            Method = HttpMethod.Get;
            ContentType = "application/json";
            CustomHeaders = new Dictionary<string, string>();
            ResponseDataType = "JSON";
        }

        /// <summary>
        /// Gets or sets the Method.
        /// </summary>
        /// <value>
        /// The Method.
        /// </value>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        /// <value>
        /// The content type.
        /// </value>
        public string ContentType { get; set; }
        
        /// <summary>
        /// Gets or sets the accept.
        /// </summary>
        /// <value>
        /// The accept.
        /// </value>
        public string Accept { get; set; }
        
        /// <summary>
        /// Gets or sets the request Uri.
        /// </summary>
        /// <value>
        /// The request Uri.
        /// </value>
        public string RequestUri { get; set; }
        
        /// <summary>
        /// Gets or sets custom headers.
        /// </summary>
        /// <value>
        /// Custom headers.
        /// </value>
        public Dictionary<string, string> CustomHeaders { get; set; }
        
        /// <summary>
        /// Gets or sets the is stream flag.
        /// </summary>
        /// <value>
        /// The is stream flag.
        /// </value>
        public bool IsStream { get; set; }
        
        /// <summary>
        /// Gets or sets the response data type.
        /// </summary>
        /// <value>
        /// The response data type.
        /// </value>
        public string ResponseDataType { get; set; }
    }
}
