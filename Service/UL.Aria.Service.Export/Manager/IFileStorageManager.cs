using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Export.Manager
{
    /// <summary>
    /// Defines operations for storing files.
    /// </summary>
    public interface IFileStorageManager
    {
        /// <summary>
        /// Saves the specified stream.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        void Save(string path, string name, Stream content);

        /// <summary>
        /// Gets the specified stream.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Stream Get(string path, string name);

        /// <summary>
        /// Removes the specified file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name">The name.</param>
        void Remove(string path, string name);
    }
}
