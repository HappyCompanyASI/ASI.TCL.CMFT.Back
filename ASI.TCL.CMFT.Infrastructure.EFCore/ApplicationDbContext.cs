using ASI.TCL.CMFT.Application;
using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Infrastructure.EFCore.EntityTypeConfiguration;
using ASI.TCL.CMFT.Infrastructure.EFCore.Extensions;
using ASI.TCL.CMFT.Infrastructure.EFCore.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASI.TCL.CMFT.Infrastructure.EFCore
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;

        //public DbSet<MessageGroup> MessageGroups { get; set; }
        //public DbSet<Message> Messages { get; set; }

        //public DbSet<VoiceGroup> VoiceGroups { get; set; }
        //public DbSet<Voice> Voices { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        public async Task AddAsync<T>(T entity) where T : class => await Set<T>().AddAsync(entity);
        public async Task<bool> ExistsAsync<T, TId>(TId id) where T : AggregateRoot<TId> => await Set<T>().AnyAsync(x => EF.Property<TId>(x, "Id")!.Equals(id));
        public async Task<T> LoadAsync<T, TId>(TId id) where T : AggregateRoot<TId> => await Set<T>().FirstOrDefaultAsync(x => EF.Property<TId>(x, "Id").Equals(id));
        public new void Remove<T>(T entity) where T : class => Set<T>().Remove(entity);

        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ApplyAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 先讓 Identity 建好它的東西
            base.OnModelCreating(modelBuilder);

            // 再套你的設定，蓋掉預設的 AspNetUsers / AspNetRoles
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims", "dbo");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins", "dbo");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens", "dbo");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims", "dbo");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles", "dbo");

            //modelBuilder.ApplyConfiguration(new MessageGroupConfiguration());
            //modelBuilder.ApplyConfiguration(new MessageConfiguration());
            //modelBuilder.ApplyConfiguration(new VoiceGroupConfiguration());
            //modelBuilder.ApplyConfiguration(new VoiceConfiguration());

            // 針對所有實作 IAuditable 的 Entity，統一設定 CreatedAt / UpdatedAt 的 Default

            var isNpgsql = Database.ProviderName != null && Database.ProviderName.Contains("Npgsql");
            var nowSql = isNpgsql
                ? "CURRENT_TIMESTAMP AT TIME ZONE 'UTC'"
                : "SYSUTCDATETIME()";

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(IAuditable).IsAssignableFrom(clrType))
                {
                    var entity = modelBuilder.Entity(clrType);

                    entity.Property("CreatedAt")
                        .HasDefaultValueSql(nowSql);

                    entity.Property("UpdatedAt")
                        .HasDefaultValueSql(nowSql);
                }
            }
            modelBuilder.UseSnakeCaseNames();
        }

        private static readonly Guid SystemUserId = new("00000000-0000-0000-0000-000000000001");

        private void ApplyAudit()
        {
            var uid = _currentUserService.GetCurrentUserId();
            if (uid == Guid.Empty)
                uid = SystemUserId;

            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditable auditable)
                {
                    if (entry.State == EntityState.Added)
                    {
                        auditable.CreatedAt = now;
                        auditable.CreatedBy = uid;

                        auditable.UpdatedAt = now;
                        auditable.UpdatedBy = uid;
                    }
                    else if (entry.State == EntityState.Modified)
                    {
                        auditable.UpdatedAt = now;
                        auditable.UpdatedBy = uid;
                    }
                }
            }
        }

    }
}