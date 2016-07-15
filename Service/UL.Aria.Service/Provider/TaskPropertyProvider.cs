using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.OData.Query.SemanticAst;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Repository;
using UL.Enterprise.Foundation.Authorization;
using UL.Enterprise.Foundation.Data;
using UL.Enterprise.Foundation.Mapper;

namespace UL.Aria.Service.Provider
{
    /// <summary>
    ///     Implements operations for working with <see cref="TaskProperty" /> objects.
    /// </summary>
    public class TaskPropertyProvider : SearchProviderBase<TaskProperty>, ITaskPropertyProvider
    {
        private readonly IMapperRegistry _mapperRegistry;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IPrincipalResolver _principalResolver;
        private readonly ITaskPropertyRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskPropertyProvider" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="mapperRegistry">The mapper registry.</param>
        /// <param name="transactionFactory">The transaction factory.</param>
        /// <param name="principalResolver">The principal resolver.</param>
        public TaskPropertyProvider(ITaskPropertyRepository repository, IMapperRegistry mapperRegistry, ITransactionFactory transactionFactory,
            IPrincipalResolver principalResolver)
            : base(repository, principalResolver)
        {
            _repository = repository;
            _mapperRegistry = mapperRegistry;
            _transactionFactory = transactionFactory;
            _principalResolver = principalResolver;
        }

        /// <summary>
        ///     Creates the specified <see cref="TaskProperty" />.
        /// </summary>
        /// <param name="entity">The industry code.</param>
        public override void Create(TaskProperty entity)
        {
            using (var scope = _transactionFactory.Create())
            {
                //currently only saves first tier of children
                var currentDateTime = DateTime.UtcNow;
                var createdById = _principalResolver.UserId;
                SetInfoForCreate(entity, null, createdById, currentDateTime);
                base.Create(entity);
                var list = new List<TaskProperty>();
                SaveChildrenForCreate(entity, createdById, currentDateTime, list);
                scope.Complete();
            }
        }

        private void SaveChildrenForCreate(TaskProperty entity, Guid createdById, DateTime currentDateTime, List<TaskProperty> list)
        {
            foreach (var taskProperty in entity.Children)
            {
                var localTaskProperty = taskProperty;

                SetInfoForCreate(localTaskProperty, entity, createdById, currentDateTime);
                localTaskProperty.ParentTaskPropertyId = entity.Id;
                localTaskProperty.Parent = entity;
                list.Add(localTaskProperty);

                _repository.Save(list);
            }
        }
        private void SaveChildrenForUpdate(TaskProperty entity,  Guid createdById, DateTime currentDateTime, List<TaskProperty> list)
        {
            foreach (var taskProperty in entity.Children)
            {
                var localTaskProperty = taskProperty;

                SetInforForUpdate(localTaskProperty, entity, createdById, currentDateTime);
                localTaskProperty.ParentTaskPropertyId = entity.Id;
                localTaskProperty.Parent = entity;
                list.Add(localTaskProperty);

                _repository.Save(list);
            }
        }

        /// <summary>
        ///     Updates the specified <see cref="TaskProperty" />.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="entity">The unit of measure.</param>
        public override void Update(Guid id, TaskProperty entity)
        {
            using (var scope = _transactionFactory.Create())
            {
                //currently only saves first tier of children
                var currentDateTime = DateTime.UtcNow;
                var createdById = _principalResolver.UserId;
                SetInforForUpdate(entity, null, createdById, currentDateTime);
                base.Create(entity);
                var list = new List<TaskProperty>();
                SaveChildrenForUpdate(entity, createdById, currentDateTime, list);
                scope.Complete();
            }
        }

        /// <summary>
        ///     Finds the by task property type identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <param name="taskPropertyTypeId">The task property type identifier.</param>
        /// <returns></returns>
        public IList<T> FindByTaskPropertyTypeId<T>(Guid taskId, Guid taskPropertyTypeId) where T : TaskProperty, new()
        {
            var taskProperties = _repository.FindByTaskPropertyTypeId(taskId, taskPropertyTypeId);
            // TODO -  think about auto mapping of to possible types of  task property instead of using Generic
            // var concreteTypes =
            //    Assembly.GetExecutingAssembly()
            //        .GetTypes()
            //        .FirstOrDefault(x => null != x.GetCustomAttribute(typeof (TaskPropertyTypeAttribute))
            //                             &&
            //                             ((TaskPropertyTypeAttribute)
            //                                 x.GetCustomAttribute(typeof (TaskPropertyTypeAttribute))).Id ==
            //                             taskPropertyTypeId.ToString());
            //if (null == concreteTypes)
            //    return props;
            var finalTaskProperties = new List<T>();
            foreach (var taskProperty in taskProperties.Where(x => x.TaskPropertyTypeId == taskPropertyTypeId))
            {
                var localTaskPropertyBase = taskProperty;
                    // weird things could happen when using foreach var in closures.
                var localTaskProperty = _mapperRegistry.Map<T>(localTaskPropertyBase);
                PopulateChildrenCollection(localTaskProperty, taskProperties);
                finalTaskProperties.Add(localTaskProperty);
            }

            return finalTaskProperties;
        }

        /// <summary>
        ///     Finds the by task identifier.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public IList<TaskProperty> FindByTaskId(Guid taskId)
        {
            return _repository.FindByTaskId(taskId);
        }

        private static void SetInfoForCreate(TaskProperty entity, TaskProperty parentEntity, Guid userId, DateTime currentDateTime)
        {
            if (!entity.Id.HasValue)
                entity.Id = Guid.NewGuid();
            entity.CreatedById = userId;
            entity.CreatedDateTime = currentDateTime;
            SetInforForUpdate(entity, parentEntity, userId, currentDateTime);
        }

        private static void SetInforForUpdate(TaskProperty entity, TaskProperty parentEntity, Guid userId, DateTime currentDateTime)
        {
            if (null != parentEntity)
                entity.TaskId = parentEntity.TaskId;
            entity.UpdatedById = userId;
            entity.UpdatedDateTime = currentDateTime;
        }

        private static void PopulateChildrenCollection<T>(T parentTaskProperty, IEnumerable<TaskProperty> taskProperties)
            where T : TaskProperty, new()
        {
            parentTaskProperty.Children.Clear();
            taskProperties.Where(x => x.ParentTaskPropertyId.HasValue && x.ParentTaskPropertyId == parentTaskProperty.Id)
                .ForEach(x =>
                {
                    var y = x;
                    PopulateChildrenCollection<TaskProperty>(y, taskProperties);
                    parentTaskProperty.Children.Add(y);
                });
        }
    }
}