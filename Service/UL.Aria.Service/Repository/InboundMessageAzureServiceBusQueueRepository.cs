using System.Diagnostics.CodeAnalysis;
using UL.Aria.Service.Contracts.Dto;

namespace UL.Aria.Service.Repository
{
	/// <summary>
	/// Class InboundMessageAzureServiceBusQueueRepository. This class cannot be inherited.
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class InboundMessageAzureServiceBusQueueRepository :
		AzureServiceBusQueueRepositoryBase<InboundMessageDto>
	{
	}
}