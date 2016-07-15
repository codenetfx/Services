using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Provider.SearchCoordinator.Task
{
	/// <summary>
	/// 
	/// </summary>
	public interface IQueryBuilder
	{
		/// <summary>
		/// Builds the refiner filter query.
		/// </summary>
		/// <returns></returns>
		string BuildRefinerFilterQuery();
	}
}
