using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UL.Enterprise.Foundation.Data;

using UL.Aria.Service.Domain.Search;
using UL.Aria.Service.Domain.Entity;
using System.Data;
using UL.Enterprise.Foundation.Framework;
using System.Diagnostics.CodeAnalysis;

namespace UL.Aria.Service.Domain.Repository
{
    /// <summary>
    /// Association Repository abstract class.
    /// </summary>
    /// <typeparam name="E"></typeparam>
    [ExcludeFromCodeCoverage] //temorary excluded, abstract and exact code is currently covered in other base repo
    public abstract class AssocationRepository<E> : RepositoryAssociationBase<E, SearchResultSetBase<E>, Sort, RefinementItem, SearchSummary>
        where E : AuditableEntity, ISearchResult, new()
    {
        /// <summary>
        /// Maps the primary entity to data row.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="dest">The dest.</param>
        protected override void MapPrimaryEntityToDataRow(E src, DataRow dest)
        {
            dest["CreatedBy"] = src.CreatedById;
            dest["UpdatedBy"] = src.UpdatedById;
            dest["CreatedDT"] = src.CreatedDateTime;
            dest["UpdatedDT"] = src.UpdatedDateTime;
        }


        /// <summary>
        /// Defines the primary entity i data reader mapping.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        protected override void DefinePrimaryEntityIDataReaderMapping(Enterprise.Foundation.Mapper.IMapperRegistry mapper)
        {
            var chain = mapper.Configuration.CreateMap<IDataReader, E>()
                .ForMember(x => ((IPrimaryEntity)x).Id, x => x.Ignore())
                .ForMember(x => x.CreatedById, x => x.MapFrom(y => y.GetValue<Guid>("CreatedBy")))
                .ForMember(x => x.CreatedDateTime, x => x.MapFrom(y => y.GetValue<DateTime>("CreatedDT")))
                .ForMember(x => x.UpdatedById, x => x.MapFrom(y => y.GetValue<Guid>("UpdatedBy")))
                .ForMember(x => x.UpdatedDateTime, x => x.MapFrom(y => y.GetValue<DateTime>("UpdatedDT")));

            MapPrimaryEntityToDataReader(chain);


        }
    }
}