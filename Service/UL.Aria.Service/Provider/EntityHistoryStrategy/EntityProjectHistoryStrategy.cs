using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
	/// <summary>
	/// Class EntityProjectHistoryStrategy. This class cannot be inherited.
	/// </summary>
	public sealed class EntityProjectHistoryStrategy : EntityHistoryStrategyBase
	{
		private readonly IMapperRegistry _mapperRegistry;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityAssetLinkHistoryStrategy"/> class.
		/// </summary>
		public EntityProjectHistoryStrategy(IMapperRegistry mapperRegistry)
		{
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// Creates the entity history.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>EntityHistory.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public override EntityHistory CreateEntityHistory(History history)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Derives the tracked information.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>List&lt;NameValuePair&gt;.</returns>
		public override List<NameValuePair> DeriveTrackedInfo(History history)
		{
			var projectDto = Deserialize(history) as ProjectDto;
			var project = _mapperRegistry.Map<Project>(projectDto);

			var orderLines = (project.ServiceLines != null)
				? string.Join(",", project.ServiceLines.Select(x => x.LineNumber))
				: string.Empty;

			var trackedItems = new List<NameValuePair>
			{
				new NameValuePair {Name = "Order Number", Value = project.OrderNumber},
				new NameValuePair {Name = "Order Line Items", Value = orderLines},
				new NameValuePair {Name = "Project Status", Value = project.ProjectStatus.ToString()},
				new NameValuePair {Name = "Project Standards", Value = project.Standards},
				new NameValuePair {Name = "Scope", Value = project.Description},
				new NameValuePair {Name = "Project Handler", Value = project.ProjectHandler},
				new NameValuePair
				{
					Name = "Estimated Completion Date",
					Value = project.EndDate.HasValue ? project.EndDate.Value.ToShortDateString() : string.Empty
				},
				new NameValuePair {Name = "Order Owner", Value = project.OrderOwner},
			};

			return trackedItems;
		}

		/// <summary>
		/// Processes the fields for deltas.
		/// </summary>
		/// <param name="taskDelta">The task delta.</param>
		/// <param name="history">The history.</param>
		/// <param name="entityHistoryPrevious">The entity history previous.</param>
		/// <param name="entityHistoryCurrent">The entity history current.</param>
		protected override void ProcessFieldsForDeltas(TaskDelta taskDelta, History history,
			EntityHistory entityHistoryPrevious,
			EntityHistory entityHistoryCurrent)
		{
			throw new NotImplementedException();
		}
	}
}