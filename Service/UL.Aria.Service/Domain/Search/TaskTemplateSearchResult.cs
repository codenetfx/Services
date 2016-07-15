using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Domain.Search
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskTemplateSearchResult : SearchResult
	{
		/// <summary>
		/// Gets or sets the task template.
		/// </summary>
		/// <value>
		/// The task template.
		/// </value>
		public TaskTemplate TaskTemplate { get; set; }
	}
}
