using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UL.Aria.Service.Contracts.Dto
{
    /// <summary>
    /// Search Results object for LinkDto.
    /// </summary>
    [DataContract]
    public class LinkSearchDto : SearchBaseDto<LinkDto> { }
}
