using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
	/// <summary>
	/// Class TaskTypeSearchModelDto.
	/// </summary>

    [DataContract]
    public class TaskTypeSearchModelDto : SearchBaseDto<TaskTypeDto>
	{
	}
}
