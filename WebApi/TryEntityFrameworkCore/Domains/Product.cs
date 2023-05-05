using System.ComponentModel.DataAnnotations.Schema;
using TryEntityFrameworkCore.Repositories;

namespace TryEntityFrameworkCore.Domains
{
    public class Product : AuditableEntity<long>, IAggregateRoot, IHasDomainEvent
    {
        public long? OriginalId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();
    }
}
