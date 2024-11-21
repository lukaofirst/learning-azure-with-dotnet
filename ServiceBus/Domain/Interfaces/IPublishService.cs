using Domain.Entities;

namespace Domain.Interfaces;

public interface IPublishService
{
	Task<bool> SendAsync(DomainMessage message);
}
