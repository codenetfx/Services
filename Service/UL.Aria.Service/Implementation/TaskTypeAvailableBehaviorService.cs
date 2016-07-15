using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Contracts.Service;
using UL.Aria.Service.Manager;
using UL.Enterprise.Foundation;
using UL.Enterprise.Foundation.Framework;
using UL.Enterprise.Foundation.Mapper;
using UL.Enterprise.Foundation.Service.Configuration;

namespace UL.Aria.Service.Implementation
{
    /// <summary>
    /// Implements service operations for Task Type Available Behaviors
    /// </summary>
    /// 
    // currently not administered through UI, so only provides method to fetch fields.
    [AutoRegisterRestService]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = false,
        InstanceContextMode = InstanceContextMode.PerCall)]
    public class TaskTypeAvailableBehaviorService : ITaskTypeAvailableBehaviorService
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITaskTypeAvailableBehaviorFieldManager _taskTypeAvailableBehaviorFieldManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTypeAvailableBehaviorService"/> class.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorFieldManager">The task type available behavior field manager.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        public TaskTypeAvailableBehaviorService(ITaskTypeAvailableBehaviorFieldManager taskTypeAvailableBehaviorFieldManager, IMapperRegistry mapperRegistry)
        {
            _taskTypeAvailableBehaviorFieldManager = taskTypeAvailableBehaviorFieldManager;
            _mapperRegistry = mapperRegistry;
        }

        /// <summary>
        /// Fetches the fields for the corresponding available behavior by identifier.
        /// </summary>
        /// <param name="taskTypeAvailableBehaviorId">The task type available behavior identifier.</param>
        /// <returns>
        /// TaskTypeDto.
        /// </returns>
        public IList<TaskTypeAvailableBehaviorFieldDto> FindByTaskTypeAvailableBehaviorId(string taskTypeAvailableBehaviorId)
        {
            Guard.IsNotNullOrEmptyTrimmed(taskTypeAvailableBehaviorId, "taskTypeAvailableBehaviorId");
            Guid guidId = taskTypeAvailableBehaviorId.ParseOrDefault<Guid>(Guid.Empty);
            Guard.IsNotEmptyGuid(guidId, "taskTypeAvailableBehaviorId");

            var tasktypes = _taskTypeAvailableBehaviorFieldManager.FindByTaskTypeAvailableBehaviorId(guidId);
            var result = _mapperRegistry.Map<IList<TaskTypeAvailableBehaviorFieldDto>>(tasktypes);
            return result;
        }
    }
}
