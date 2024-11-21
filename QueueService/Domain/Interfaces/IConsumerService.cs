using Domain.Entities;

namespace Domain.Interfaces;

public interface IConsumerService
{
	Task<List<DomainMessage>> StartAsync();
}
