using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Repository;
using UL.Enterprise.Foundation.Data;

namespace UL.Aria.Service.Domain.Entity
{



    /// <summary>
    /// User team class.
    /// </summary>
    public class UserTeam : AuditableEntity, ISearchResult
    {
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();
        /// <summary>
        /// Initializes a new instance of the <see cref="UserTeam"/> class.
        /// </summary>
        public UserTeam()
        {
            TeamMembers = new List<UserTeamMember>();
           
        }    

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the team members.
        /// </summary>
        /// <value>
        /// The team members.
        /// </value>
        public IEnumerable<UserTeamMember> TeamMembers { get; set; }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        public EntityTypeEnumDto? EntityType
        {
            get { return EntityTypeEnumDto.UserTeam; }
        }


        /// <summary>
        /// Gets or sets the change date.
        /// </summary>
        /// <value>
        /// The change date.
        /// </value>
        DateTime ISearchResult.ChangeDate
        {
            get { return this.UpdatedDateTime; }
        }

        /// <summary>
        /// Gets or sets the type of the entity.
        /// </summary>
        /// <value>
        /// The type of the entity.
        /// </value>
        string ISearchResult.EntityType
        {
            get { return this.EntityType.ToString(); }
        }


        /// <summary>
        /// Not implemented for this class, returns nulll.
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        IDictionary<string, string> ISearchResult.Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string ISearchResult.Title
        {
            get { return this.Name; }
        }

    
    }
}
