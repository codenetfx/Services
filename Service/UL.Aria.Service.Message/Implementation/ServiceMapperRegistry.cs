using System;

using UL.Enterprise.Foundation.Mapper;
using UL.Aria.Service.Contracts.Dto;
using UL.Aria.Service.Message.Domain;
namespace UL.Aria.Service.Message.Implementation
{
	/// <summary>
	/// Host project mapper registry class.
	/// </summary>
	public class ServiceMapperRegistry : MapperEngineMapperRegistryBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceMapperRegistry"/> class.
		/// </summary>
		public ServiceMapperRegistry()
		{
            Configuration.AddGlobalIgnore("RecordAction");
			Configuration.CreateMap<OrderMessage, OrderMessageDto>();
			Configuration.CreateMap<OrderMessageDto, OrderMessage>()
				  .ConstructUsing((OrderMessageDto x) => new OrderMessage(Guid.NewGuid()))
				  .ForMember(x => x.Id, y => y.Ignore());
		}
	}
}