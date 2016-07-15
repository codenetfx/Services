using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Domain.Entity;
using UL.Aria.Service.Domain.Search;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	///     Repository for <see cref="DepartmentCode" />
	/// </summary>
	public class DepartmentCodeRepository :
		LookupCodeRepositoryBase<DepartmentCode, DepartmentCodeSearchResult, DepartmentCodeSearchResultSet>,
		IDepartmentCodeRepository
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DepartmentCodeRepository"/> class.
		/// </summary>
		public DepartmentCodeRepository() : base(EntityTypeEnumDto.DepartmentCode)
		{
		}

		/// <summary>
		/// Gets the name of the identifier field.
		/// </summary>
		/// <value>
		/// The name of the identifier field.
		/// </value>
		protected override string IdFieldName
		{
			get { return "DepartmentCodeID"; }
		}

		/// <summary>
		/// Gets the name of the table.
		/// </summary>
		/// <value>
		/// The name of the table.
		/// </value>
		protected override string TableName
		{
			get { return "DepartmentCode"; }
		}
	}
}