using TryEntityFrameworkCore.Domains;

namespace TryEntityFrameworkCore.Services
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
