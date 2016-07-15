using System;
using System.Collections.Generic;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Provider.EntityHistoryStrategy
{
	/// <summary>
	/// Class EntityAssetLinkHistoryStrategy. This class cannot be inherited.
	/// </summary>
	public sealed class EntityAssetLinkHistoryStrategy : EntityHistoryStrategyBase
	{
		private readonly IMapperRegistry _mapperRegistry;

		/// <summary>
		/// Initializes a new instance of the <see cref="EntityAssetLinkHistoryStrategy"/> class.
		/// </summary>
		public EntityAssetLinkHistoryStrategy(IMapperRegistry mapperRegistry)
		{
			_mapperRegistry = mapperRegistry;
		}

		/// <summary>
		/// Creates the entity history.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>EntityHistory.</returns>
		public override EntityHistory CreateEntityHistory(History history)
		{
			return CreateEntityAssetLinkHistory(history);
		}

		private EntityAssetLinkHistory CreateEntityAssetLinkHistory(History history)
		{
			var assetLinkDto = Deserialize(history) as AssetLinkDto;
			var assetLink = _mapperRegistry.Map<AssetLink>(assetLinkDto);
			return new EntityAssetLinkHistory
			{
				AssetLink = assetLink,
				CreatedBy = assetLink.UpdatedById.ToString(),
				CreatedDate = assetLink.UpdatedDateTime
			};
		}

// ReSharper disable once InconsistentNaming
		private static readonly IDictionary<string, string> _actionConversions = new Dictionary<string, string>
		{
			{typeof(TaskDto).GetAssemblyQualifiedTypeName(), "Document"},
			{typeof(ProjectDto).GetAssemblyQualifiedTypeName(), "Product"}
		};

		/// <summary>
		/// Derives the tracked information.
		/// </summary>
		/// <param name="history">The history.</param>
		/// <returns>List&lt;NameValuePair&gt;.</returns>
		public override List<NameValuePair> DeriveTrackedInfo(History history)
		{
			var assetLinkDto = Deserialize(history) as AssetLinkDto;
			var assetLink = _mapperRegistry.Map<AssetLink>(assetLinkDto);

			var trackedItems = new List<NameValuePair>
			{
				new NameValuePair { Name = _actionConversions[history.EntityType], Value = assetLink.AssetName},
			};

			return trackedItems;
		}

		/// <summary>
		/// Builds the action.
		/// </summary>
		/// <param name="entityType">Type of the entity.</param>
		/// <param name="action">The action.</param>
		/// <returns>System.String.</returns>
		public override string BuildAction(string entityType, string action)
		{
			return action.Replace("AssetLink", _actionConversions[entityType]);
		}

		/// <summary>
		/// Gets the previous entity history.
		/// </summary>
		/// <param name="entityHistoryPrevious">The entity history previous.</param>
		/// <param name="entityHistoryCurrent">The entity history current.</param>
		/// <returns>EntityHistory.</returns>
		protected override EntityHistory GetPreviousEntityHistory(EntityHistory entityHistoryPrevious,
			EntityHistory entityHistoryCurrent)
		{
			return entityHistoryPrevious;
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
			taskDelta.MetaDeltaList.Add(
				new MetaDelta
				{
					AriaFieldName = "AriaAssetName",
					Id = Guid.NewGuid(),
					OrignalValue = "",
					ModifiedValue = ((EntityAssetLinkHistory) entityHistoryCurrent).AssetLink.AssetName
				});
		}
	}
}