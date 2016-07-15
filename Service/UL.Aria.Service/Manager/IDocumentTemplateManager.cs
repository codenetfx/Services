using System.Collections.Generic;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Lookup;

namespace UL.Aria.Service.Manager
{
	/// <summary>
	/// Interface IDocumentTemplateManager
	/// </summary>
	public interface IDocumentTemplateManager : IManagerBase<DocumentTemplate>, ISearchManagerBase<DocumentTemplate>
	{
        /// <summary>
        /// Fetches all.
        /// </summary>
        /// <returns></returns>
	    IEnumerable<DocumentTemplate> FetchAll();
	}
}