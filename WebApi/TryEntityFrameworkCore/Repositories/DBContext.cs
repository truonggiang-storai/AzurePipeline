using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using TryEntityFrameworkCore.Domains;
using TryEntityFrameworkCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TryEntityFrameworkCore.Repositories
{
    public partial class DBContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDomainEventService _domainEventService;

        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            IDomainEventService domainEventService)
            : base(options)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _domainEventService = domainEventService;
        }

        public virtual DbSet<Product> Products { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var entities = ChangeTracker.Entries().Where(t => t.State == EntityState.Modified || t.State == EntityState.Added).ToArray();

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.CurrentValues[nameof(AuditableEntity<object>.CreatedBy)] = "Admin";
                        entity.CurrentValues[nameof(AuditableEntity<object>.CreatedDate)] = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entity.CurrentValues[nameof(AuditableEntity<object>.ModifiedBy)] = "Admin";
                        entity.CurrentValues[nameof(AuditableEntity<object>.ModifiedDate)] = DateTime.UtcNow;
                        break;
                }
            }

            int result = await base.SaveChangesAsync(cancellationToken);

            await DispatchEvents();

            return result;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.Name).HasMaxLength(500);

                entity.Property(e => e.Price).HasPrecision(8, 2);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });
        }

        private async Task DispatchEvents()
        {
            var domainEventEntities = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .ToArray();

            foreach (var domainEvent in domainEventEntities)
            {
                await _domainEventService.Publish(domainEvent);
            }
        }
    }
}
