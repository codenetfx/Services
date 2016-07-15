using System.Collections.Generic;
using System.IO;

using UL.Aria.Service.Domain;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    /// Defines operations to build download documents for entity histories.
    /// </summary>
    public interface IHistoryDocumentBuilder
    {
        /// <summary>
        /// Builds a document for the specified entity histories.
        /// </summary>
		/// <param name="histories">The entity histories.</param>
        /// <returns></returns>
        Stream Build(IEnumerable<History> histories);
    }
}