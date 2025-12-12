using System.Data;
using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Domain.DMD;
using ASI.TCL.CMFT.Domain.PA;
using ASI.TCL.CMFT.Domain.SYS;
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
            await SetSessionUserIdAsync(cancellationToken);
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
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (typeof(IAuditable).IsAssignableFrom(clrType))
                {
                    var entity = modelBuilder.Entity(clrType);

                    entity.Property("CreatedAt")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

                    entity.Property("UpdatedAt")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
                }
            }
            modelBuilder.UseSnakeCaseNames();
        }

        private async Task SetSessionUserIdAsync(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetCurrentUserId();
            if (userId == Guid.Empty)
                return;

            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync(cancellationToken);

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "EXEC sp_set_session_context @key = N'UserId', @value = @userId;";

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@userId";
                parameter.Value = userId;
                command.Parameters.Add(parameter);

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}