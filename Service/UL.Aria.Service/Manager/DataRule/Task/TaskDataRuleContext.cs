using System;
using System.Collections.Generic;
using System.Linq;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Enterprise.Foundation.Mapper;
using ULF = UL.Enterprise.Foundation.Framework;

namespace UL.Aria.Service.Manager.DataRule.Task
{
    /// <summary>
    /// Establishes a context
    /// </summary>
    public class TaskDataRuleContext : IDataRuleContext<Domain.Entity.Project, Domain.Entity.Task>
    {

        /// <summary>
        /// The _ user altered task ids
        /// </summary>
        private HashSet<Guid> _UserAlteredTaskIds;
        /// <summary>
        /// The _ dirty tasks
        /// </summary>
        protected Dictionary<Guid?, Domain.Entity.Task> _DirtyTasks;

        private Dictionary<Guid?, Domain.Entity.Task> _OriginalTasks;
        private Dictionary<Guid?, Domain.Entity.Task> _WorkingTasks;
        private Dictionary<Guid?, Domain.Entity.Task> _DeletedTasks;

        private readonly IMapperRegistry _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDataRuleContext"/> class.
        /// </summary>
        protected TaskDataRuleContext()
        {
            _DirtyTasks  = new Dictionary<Guid?, Domain.Entity.Task>();
            _UserAlteredTaskIds = new HashSet<Guid>();
            _OriginalTasks = new Dictionary<Guid?, Domain.Entity.Task>();
            _WorkingTasks = new Dictionary<Guid?, Domain.Entity.Task>();
             SyncErrors = new Dictionary<Guid, IList<TaskValidationEnumDto>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDataRuleContext"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="originalProject">The original project.</param>
        /// <param name="userAlteredTasks">The user altered task.</param>
        /// <param name="isDeleteMode">if set to <c>true</c> [is delete mode].</param>
        public TaskDataRuleContext(IMapperRegistry mapper, Domain.Entity.Project originalProject, List<Domain.Entity.Task> userAlteredTasks, bool isDeleteMode = false):this()
        {
            ULF.Guard.IsNotNull(mapper, "mapper");
            ULF.Guard.IsNotNull(originalProject, "originalProject");
            ULF.Guard.IsNotNull(userAlteredTasks, "userAlteredTasks");
            _mapper = mapper;
            Init(originalProject, userAlteredTasks);
            InitDelete(originalProject, userAlteredTasks, isDeleteMode);
        }

        private void Init(Domain.Entity.Project originalProject,List<Domain.Entity.Task> userAlteredTasks)
        {
            userAlteredTasks.Where(x => !x.Id.HasValue || x.Id == Guid.Empty).ToList().ForEach(x => x.Id = Guid.NewGuid());
            var workingTasks = Common.Framework.Guard.Clone(originalProject.Tasks) ?? new List<Domain.Entity.Task>();
            var workingDict = workingTasks.ToDictionary(x => x.Id);
            var originalDict =  originalProject.Tasks != null
                ? originalProject.Tasks.ToDictionary(originalTask => originalTask.Id) 
                : new  Dictionary<Guid?, Domain.Entity.Task>();
            var userAlteredTaskIds = new HashSet<Guid>(userAlteredTasks.Select(task => task.Id.GetValueOrDefault()));
            var activeProject = workingTasks.Count() > 0? workingTasks.First().Project: Common.Framework.Guard.Clone(originalProject);
           
            // Sync updated tasks with copy from the database.
            SyncTasks(userAlteredTasks, workingDict, activeProject, originalDict);
            workingTasks = workingDict.Values.ToList();
            activeProject.Tasks = new List<Domain.Entity.Task>(workingTasks);

            var toProcess = workingTasks.Where(x => userAlteredTaskIds.Contains(x.Id.GetValueOrDefault()));
            
            _DirtyTasks = toProcess.ToDictionary(x => x.Id);
            _UserAlteredTaskIds = userAlteredTaskIds;
            _OriginalTasks = originalDict;
            _WorkingTasks = workingDict;
            ActiveProject = activeProject;
            OriginalProject = originalProject;            
            OriginalProject.Tasks = new List<Domain.Entity.Task>();
            ProcessingQueue = new Queue<Domain.Entity.Task>(toProcess);

            WorkflowState = new Dictionary<string, object>();
        }

        private void InitDelete(Domain.Entity.Project originalProject,List<Domain.Entity.Task> userDeletedTasks,bool isDeleteMode)
        {
            if (isDeleteMode)
            {                
                var deletedDict = userDeletedTasks.ToDictionary(x => x.Id);
                _DeletedTasks = WorkingTasks.Values.Where(x => deletedDict.ContainsKey(x.Id)).ToDictionary(x => x.Id);
                _DeletedTasks.Values.ToList().ForEach(x => x.IsDeleted = true);
            }
            else
            {
                _DeletedTasks = new Dictionary<Guid?, Domain.Entity.Task>();
            }
        }

        /// <summary>
        /// Gets the synchronize errors.
        /// </summary>
        /// <value>
        /// The synchronize errors.
        /// </value>
        public IDictionary<Guid, IList<TaskValidationEnumDto>> SyncErrors { get; private set; }
        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public Domain.Entity.Project OriginalProject { get; private set; }

        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>
        /// The project.
        /// </value>
        public Domain.Entity.Project ActiveProject { get; private set; }

        /// <summary>
        /// Gets or sets the process stack.
        /// </summary>
        /// <value>
        /// The process stack.
        /// </value>
        public Queue<Domain.Entity.Task> ProcessingQueue { get; protected set; }

        /// <summary>
        /// Gets the original tasks2.
        /// </summary>
        /// <value>
        /// The original tasks2.
        /// </value>
        public Dictionary<Guid?, Domain.Entity.Task> OriginalTasks 
        { 
            get 
            { 
                return _OriginalTasks; 
            } 
        }

        /// <summary>
        /// Gets the user altered task ids.
        /// </summary>
        /// <value>
        /// The user altered task ids.
        /// </value>
        public IEnumerable<Guid> UserAlteredTaskIds 
        { 
            get 
            { 
                return _UserAlteredTaskIds.ToList() ; 
            } 
        }

        /// <summary>
        /// Gets the working tasks.
        /// </summary>
        /// <value>
        /// The working tasks.
        /// </value>
        public IDictionary<Guid?, Domain.Entity.Task> WorkingTasks 
        { 
            get 
            { 
                return _WorkingTasks; 
            } 
        }


        /// <summary>
        /// Gets or sets the state of the workflow.
        /// </summary>
        /// <value>
        /// The state of the workflow.
        /// </value>
        public IDictionary<string, object> WorkflowState{ get; set; }

        /// <summary>
        /// Records the task update.
        /// </summary>
        /// <param name="updatedTask">The updated task.</param>
        public void RecordEntityUpdate(Domain.Entity.Task updatedTask)
        {
            if (_WorkingTasks.ContainsKey(updatedTask.Id) && _WorkingTasks[updatedTask.Id] != updatedTask)
            {
                throw new ArgumentOutOfRangeException("The updatedTask must be contained in the working working task collection."
                                + "This exception is normally caused by making operations on copies of tasks that are not the operational copy.");
            }

            _DirtyTasks[updatedTask.Id] = updatedTask;
            this.ProcessingQueue.Enqueue(updatedTask);
      
        }
        
        /// <summary>
        /// Determines whether [has dirty entities].
        /// </summary>
        /// <returns></returns>
        public bool HasDirtyEntities()
        {
            return _DirtyTasks.Count > 0;
        }

        /// <summary>
        /// Removes the task network references.
        /// </summary>
        /// <param name="tasks">The tasks.</param>
        /// <param name="removeSubTasks">if set to <c>true</c> [remove sub tasks].</param>
        /// <returns></returns>
        internal protected List<Domain.Entity.Task> RemoveTaskNetworkReferences(List<Domain.Entity.Task> tasks, bool removeSubTasks=true)
        {
            foreach (var task in tasks)
            {
                task.Parent = null;
                task.Project = null;
                task.PredecessorRefs = null;
                task.SuccessorRefs = null;
                
                if (removeSubTasks)
                    task.SubTasks = null;
            }
            return tasks;
        }

        /// <summary>
        /// Synchronizes the tasks.
        /// </summary>
        /// <param name="srcTasks">The source tasks.</param>
        /// <param name="destTasks">The dest tasks.</param>
        /// <param name="activeProject">The active project.</param>
        /// <param name="originalTasks">The original tasks.</param>
        private void SyncTasks(List<Domain.Entity.Task> srcTasks, Dictionary<Guid?, Domain.Entity.Task> destTasks
            , Domain.Entity.Project activeProject, Dictionary<Guid?, Domain.Entity.Task> originalTasks)
        {
            var destTaskNumberDict = destTasks.Values.ToDictionary(x => x.TaskNumber);
            srcTasks.Where(x => !x.Id.HasValue || x.Id == Guid.Empty).ToList().ForEach(x => x.Id = Guid.NewGuid());
            var srcTaskNumberDict = srcTasks.ToDictionary(x => x.TaskNumber);
            var srcTaskIdDict = srcTasks.ToDictionary(x => x.Id);

            var existingIds = srcTasks.Select(x => x.Id).Intersect(destTasks.Values.Select(x => x.Id));
            var newIds = srcTasks.Select(x => x.Id).Except(existingIds).ToList();

            newIds.ForEach(Id =>
            {
                var srcTask = srcTaskIdDict[Id];
                var destTask = Common.Framework.Guard.Clone(srcTask);
                destTask.Project = activeProject;
                destTask.Parent = null;
                if (destTask.ParentTaskNumber.GetValueOrDefault() > 0)
                {
                    Guid? tempParentGuid = null;

                    if (srcTaskNumberDict.ContainsKey(destTask.ParentTaskNumber.GetValueOrDefault()))
                    {
                        tempParentGuid = srcTaskNumberDict[destTask.ParentTaskNumber.GetValueOrDefault()].Id.GetValueOrDefault();
                    }
                    else if (destTaskNumberDict.ContainsKey(destTask.ParentTaskNumber.GetValueOrDefault()))
                    {
                        tempParentGuid = destTaskNumberDict[destTask.ParentTaskNumber.GetValueOrDefault()].Id.GetValueOrDefault();
                    }
                    destTask.ParentId = tempParentGuid.GetValueOrDefault();
                }
                else if (srcTask.SubTasks.Count > 0) {
                    srcTask.SubTasks.ToList().ForEach(x =>
                    {                        
                        x.ParentTaskNumber = srcTask.TaskNumber;
                    });                  
                }
                destTasks[destTask.Id] = destTask;
                destTaskNumberDict[destTask.TaskNumber] = destTask;
            });           
           

            srcTasks.ForEach(srcTask =>
            {
                // enrich srcTask predecessor.  Must happen before map because TaskId is missing.
                EnrichPredecessors(srcTask, destTaskNumberDict, srcTaskNumberDict);

                var destTask = destTasks[srcTask.Id.GetValueOrDefault()];
                var destPreds = destTask.Predecessors.ToList();
                
                // Map the updated task data to the original task.            
                _mapper.Map(srcTask, destTask);
                destTask.Predecessors = destPreds;

                // enrich destTask predecessor.  Must happen before map because TaskId is missing.
                EnrichPredecessors(destTask, destTaskNumberDict, srcTaskNumberDict);

                // Enforce properties that are Muted after inital set
                if (originalTasks.ContainsKey(destTask.Id))
                {
                    var originalTask = originalTasks[destTask.Id];
                    destTask.ShouldTriggerBilling = originalTask.ShouldTriggerBilling;
                    destTask.TaskTemplateId = originalTask.TaskTemplateId;
                }

                //Enforce project inherit values
                destTask.OrderNumber = activeProject.OrderNumber;
                destTask.CompanyId = activeProject.CompanyId;
                destTask.CompanyName = activeProject.CompanyName;
                destTask.ContainerId = activeProject.ContainerId;

                // Map the updated task's subtasks to the original task.
                if ((srcTask.Parent == null && srcTask.ParentId == Guid.Empty
                    && srcTask.ParentTaskNumber.GetValueOrDefault() > 0)
                    || srcTask.ParentId != destTask.ParentId)
                {
                    srcTask.ParentId =
                        destTaskNumberDict[srcTask.ParentTaskNumber.GetValueOrDefault()].Id.GetValueOrDefault();
                }

                if ((srcTask.Parent == null && srcTask.ParentId != Guid.Empty)
                    || (srcTask.Parent != null && srcTask.ParentId != destTask.ParentId))
                {
                    // Add new subtask.  
                    // May need to add sync error in else block. See predecessor below.
                    if (destTasks[srcTask.ParentId] != destTask)
                    {
                        destTask.Parent = destTasks[srcTask.ParentId];
                        destTask.ParentId = srcTask.ParentId;
                        destTask.ParentTaskNumber = srcTask.ParentTaskNumber;

                        if (!destTask.Parent.SubTasks.Select(t => t.Id).Contains(srcTask.Id))
                        {
                            destTask.Parent.SubTasks.Add(destTasks[srcTask.Id.GetValueOrDefault()]);
                        }
                        else if (!destTask.Parent.SubTasks.Contains(destTasks[srcTask.Id.GetValueOrDefault()]))
                        {
                            var subTasks = (destTask.Parent.SubTasks as List<Domain.Entity.Task>);
                            if (subTasks != null)
                            {
                                subTasks.RemoveAll(x => x.Id == srcTask.Id);
                            }

                            destTask.Parent.SubTasks.Add(destTasks[srcTask.Id.GetValueOrDefault()]);
                        }
                    }
                  
                }

                else if (destTask.Parent != null && srcTask.ParentId == Guid.Empty)
                {
                    // Remove old subtask.
                    destTask.Parent.SubTasks.Remove(destTask);
                    destTask.Parent = null;
                }

                // Map the updated task's predecessors to the original task.
                foreach (var task in srcTask.Predecessors)
                {
                    //Add new predecessor/sucessor to working list.
                    var predecessorToAdd = destTasks[task.TaskId];
                    if (destTask != predecessorToAdd)
                    {
                        if (!destTask.PredecessorRefs.Contains(predecessorToAdd))
                            destTask.PredecessorRefs.Add(predecessorToAdd);

                        if (!predecessorToAdd.SuccessorRefs.Contains(destTask))
                            predecessorToAdd.SuccessorRefs.Add(destTask);
                    }
                    else
                    {
                        if (!SyncErrors.ContainsKey(task.TaskId))
                        {
                            SyncErrors[task.TaskId] = new List<TaskValidationEnumDto>()
                            {
                                TaskValidationEnumDto.SelfReferencingPredecessor
                            };
                        }
                        // Required when a second validation error type is added to sync.
                        //else
                        //{
                        //    SyncErrors[task.TaskId].Add(TaskValidationEnumDto.SelfReferencingPredecessor);
                        //}
                      
                    }
                }

                destTasks[srcTask.Id.GetValueOrDefault()].Predecessors.Select(x => x.TaskId).Except(srcTask.Predecessors.Select(x => x.TaskId))
                .ToList()
                .ForEach(predecessorId =>
                {
                    // Remove old predecessor/sucessor to working list.
                    var predecessorToRemove = destTasks[predecessorId];
                    destTask.PredecessorRefs.Remove(predecessorToRemove);
                    predecessorToRemove.SuccessorRefs.Remove(destTask);
                });

                destTask.Predecessors = destTask.PredecessorRefs.Select(predecessor =>
                    new Domain.Entity.TaskPredecessor
                    {
                        TaskId = predecessor.Id.GetValueOrDefault(),
                        Title = predecessor.Title,
                        TaskNumber = predecessor.TaskNumber,
                        SuccessorId = destTask.Id.GetValueOrDefault()
                    }).ToList();
            });
        }


        private void EnrichPredecessors(Domain.Entity.Task task, Dictionary<int, Domain.Entity.Task> destTaskNumberDict, 
            Dictionary<int, Domain.Entity.Task> srcTaskNumberDict)
        {
            task.Predecessors.ForEach(x =>
            {
                if (x.TaskId == Guid.Empty)
                {
                    if (destTaskNumberDict.ContainsKey(x.TaskNumber))
                    {
                        x.TaskId = destTaskNumberDict[x.TaskNumber].Id.GetValueOrDefault();
                    }
                    else if (srcTaskNumberDict.ContainsKey(x.TaskNumber))
                    {
                        // Handles case for new predecessors
                        x.TaskId = srcTaskNumberDict[x.TaskNumber].Id.GetValueOrDefault();
                    }
                    //TODO: Else should potentially throw exception
                }
                x.SuccessorId = task.Id.GetValueOrDefault();
            });
        }

        
        /// <summary>
        /// Records the task update.
        /// </summary>
        /// <param name="deletedTask">The deleted task.</param>
        public void RecordEntityDelete(Domain.Entity.Task deletedTask)
        {
            if (WorkingTasks.ContainsKey(deletedTask.Id) && WorkingTasks[deletedTask.Id] != deletedTask)
            {
                throw new ArgumentOutOfRangeException("The deletedTask must be contained in the working working task collection."
                                + "This exception is normally caused by making operations on copies of tasks that are not the operational copy.");
            }

            deletedTask.IsDeleted = true;
            _DeletedTasks[deletedTask.Id] = deletedTask;
            this.ProcessingQueue.Enqueue(deletedTask);
        }

        /// <summary>
        /// Gets the updated entities.
        /// </summary>
        /// <returns></returns>
        public List<Domain.Entity.Task> GetUpdatedEntities(bool flattened = true)
        {
            var clones = Common.Framework.Guard.Clone(_DirtyTasks.Values.ToList())
                .Where(x => !_DeletedTasks.ContainsKey(x.Id))
                .ToList();
            clones = RemoveTaskNetworkReferences(clones, flattened);
           
            if (!flattened) 
                clones.RemoveAll(x => x.ParentTaskNumber.GetValueOrDefault() > 0  && clones.Exists(y=> y.TaskNumber == x.ParentTaskNumber));

            return clones;

        }

        /// <summary>
        /// Gets the deleted entities.
        /// </summary>
        /// <returns></returns>
        public List<Domain.Entity.Task> GetDeletedEntities()
        {
            var clones = Common.Framework.Guard.Clone(_DeletedTasks.Values.ToList());
            clones = RemoveTaskNetworkReferences(clones);

            return clones;
        }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is active project dirty.
		/// </summary>
		/// <value><c>true</c> if this instance is active project dirty; otherwise, <c>false</c>.</value>
		public bool IsActiveProjectDirty { get; set; }

		/// <summary>
		/// Gets the updated parent.
		/// </summary>
		/// <returns>TParent.</returns>
	    public Project GetUpdatedParent()
	    {
		    var tasks = ActiveProject.Tasks;
		    ActiveProject.Tasks = null;
		    var clone = Common.Framework.Guard.Clone(ActiveProject);
			ActiveProject.Tasks = tasks;

		    return clone;
	    }
    }
}