using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;

namespace UL.Aria.Service.Manager
{
    /// <summary>
    /// Defines operations for managing templates for <see cref="ProductFamily"/> objects.
    /// </summary>
    public interface IProductFamilyTemplateManager
    {
        /// <summary>
        /// Fetches the template whose <see cref="ProductFamily" /> is identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the <see cref="ProductFamily"/>.</param>
        /// <param name="templateType"></param>
        /// <returns>A <see cref="Stream"/> with the contents of the template.</returns>
        Stream FetchProductFamilyTemplate(Guid id, ProductFamilyTemplate templateType);
    }
}
