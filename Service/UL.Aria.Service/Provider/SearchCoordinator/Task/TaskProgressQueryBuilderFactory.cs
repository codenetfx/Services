using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace UL.Aria.Service.Provider.SearchCoordinator.Task
{
	/// <summary>
	/// 
	/// </summary>
	public class TaskProgressQueryBuilderFactory : ITaskProgressQueryBuilderFactory
	{
		private readonly IUnityContainer _unityContainer;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskProgressQueryBuilderFactory"/> class.
		/// </summary>
		/// <param name="unityContainer">The unity container.</param>
		public TaskProgressQueryBuilderFactory(IUnityContainer unityContainer)
		{
			_unityContainer = unityContainer;
		}

		/// <summary>
		/// Gets the query builder.
		/// </summary>
		/// <param name="taskProgress">The task progress.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public IQueryBuilder GetQueryBuilder(string taskProgress)
		{
			return _unityContainer.Resolve<IQueryBuilder>(taskProgress);
		}
	}
}
