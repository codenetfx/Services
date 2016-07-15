using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Common.Framework;
using UL.Enterprise.Foundation.Logging;
using UL.Aria.Service.Update.Configuration;

namespace UL.Aria.Service.Update.Configuration
{
    /// <summary>
    /// Provides a implemetation class to get a filename from the config file using the "FileLogger_LogFileName"
    /// setting name.
    /// </summary>
    public class FileLocator : IFileLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileLocator"/> class.
        /// </summary>
        public FileLocator(IUpdateConfigurationSource config)
        {  
            this.Filename = config.LogFilename;
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <value>
        /// The filename.
        /// </value>
        public string Filename { get; set; }
     
    }
}
