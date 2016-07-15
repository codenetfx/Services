using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Configuration;
using UL.Aria.Service.Contracts.Dto;


namespace UL.Aria.Service.Manager.Validation
{
    /// <summary>
    /// Validates the TaskOwner is a valid UL email address
    /// </summary>
    public class TaskOwnerIsUlEmailValidator : ITaskValidator
    {
        private readonly IProfileManager _profileManager;
        private readonly IServiceConfiguration _serviceConfiguration;
        
        private static readonly ConcurrentDictionary<string, Fetcheduser> _fetchedUsers = new ConcurrentDictionary<string, Fetcheduser>();
        private readonly int _slidingExpiration = 60;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskOwnerIsUlEmailValidator" /> class.
        /// </summary>
        /// <param name="profileManager">The profile manager.</param>
        /// <param name="serviceConfiguration">The service configuration.</param>
        public TaskOwnerIsUlEmailValidator(IProfileManager profileManager, IServiceConfiguration serviceConfiguration)
        {
            _profileManager = profileManager;
            _serviceConfiguration = serviceConfiguration;
            
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="taskValidationContext">The task validation context.</param>
        /// <param name="errors">The errors.</param>
        public void Validate(TaskValidationContext taskValidationContext, List<Contracts.Dto.TaskValidationEnumDto> errors)
        {
            var taskOwner = taskValidationContext.Entity.TaskOwner;

            if (!IsUlUser(taskOwner))
            {
                errors.Add(TaskValidationEnumDto.TaskOwnerMustBeUlUser);
            }
        }

        private bool IsUlUser(string taskOwner)
        {
            var doFetch = true;
            Fetcheduser result;
            var validUserTokens = new string[]
            {
                "unassigned",
                _serviceConfiguration.TaskReviewGroupEmail.ToLowerInvariant()
            };

            var taskOwnerLower = string.IsNullOrWhiteSpace(taskOwner) ? null : taskOwner.ToLowerInvariant().Trim();

            if (taskOwnerLower == null || validUserTokens.Contains(taskOwnerLower))
            {
                return true;
            }
            if (_fetchedUsers.TryGetValue(taskOwner, out result))
            {
                doFetch = result.TimeStamp.AddSeconds(_slidingExpiration) < DateTime.UtcNow;
            }

            if (doFetch)
            {
                var bo = _profileManager.FetchByUserName(taskOwner);
                var compId = _serviceConfiguration.UlCompanyId;
                result = new Fetcheduser
                {
                    IsUlUser = bo != null && bo.CompanyId == compId, 
                    TimeStamp = DateTime.UtcNow
                };
                
                _fetchedUsers.TryAdd(taskOwner, result);
              
            }

            return result.IsUlUser;

        }

        private class Fetcheduser
        {
            public bool IsUlUser { get; set; }
            public DateTime TimeStamp { get; set; }
        }

    }
}
