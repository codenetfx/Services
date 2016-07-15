using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for working with <see cref="IncomingOrderContact"/> objects.
    /// </summary>
    public interface IContactManager
    {
        /// <summary>
        /// Updates all contacts for any <see cref="Project"/> or <see cref="IncomingOrder"/> with the supplied <paramref name="orderNumber"/>.
        /// This will only be done for a <see cref="Project"/> if the project isn't closed (<see cref="T:ProjectStatusEnumDto.Completed"/> or <see cref="T:ProjectStatusEnumDto.Canceled"/>).
        /// This method is asynchronous.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        void UpdateContactsByOrderNumberAsync(string orderNumber);
    }
}
