using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Provider.SearchCoordinator.Task
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class QueryBuilderAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="QueryBuilderAttribute"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public QueryBuilderAttribute(TaskProgressEnumDto name)
		{
			Name = name;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public TaskProgressEnumDto Name { get; set; }
	}
}
