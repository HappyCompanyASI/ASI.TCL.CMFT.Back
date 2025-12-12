using System.Data.Common;

using ASI.TCL.CMFT.Application;
using ASI.TCL.CMFT.Domain;
using ASI.TCL.CMFT.Domain.SYS;
using ASI.TCL.CMFT.Infrastructure.EFCore;
using ASI.TCL.CMFT.Infrastructure.EFCore.Identity;
using ASI.TCL.CMFT.Infrastructure.JWTAuthentication;
using ASI.TCL.CMFT.WebAPI.ConfigrueOptions;
using ASI.TCL.CMFT.WebAPI.RequestPipeline;
using ASI.TCL.CMFT.WebAPI.Startup;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Npgsql;

using Serilog;

namespace ASI.TCL.CMFT.WebAPI.Extensions
{
    internal static class ServiceCollectionExtensions 
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            services.AddSingleton(sp => new LoggerFactory().AddSerilog(Log.Logger, dispose: true));

            // 註冊「基礎層」(Infrastructure Layer)
            // EFCore DbContext & Repository & UnitOfWork
            var databaseProvider = configuration.GetValue<string>("DatabaseProvider");
            var connectionString = configuration.GetConnectionString(databaseProvider);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));
            });
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            // AspNetCore Identity
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureOptions<ConfigureIdentityOptions>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentitySeeder, IdentitySeeder>();


            //services.AddScoped<IAggregateStore, AggregateStore>();
            //services.AddScoped<IOperatorAccessor, OperatorAccessor>();
            //services.AddScoped<IOperatorRecorder, OperatorRecorder>();
            //services.AddScoped<IHashingService, HashingService>();
            //services.AddScoped<ILoginService, LoginService>();

            // JWT/Token
            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            services.Configure<JWTSettings>(jwtSettingsSection);
            services.AddScoped<ITokenService, JWTAuthenticationService>();



            // 註冊「領域層」(Domain Layer)
            //services.AddScoped<DbConnection>(_ => new SqlConnection(connectionString));
            //services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();


            // 註冊「應用程式服務層」(Application Layer)
 
            services.AddTransient<Func<DbConnection>>(_ => () => new NpgsqlConnection(connectionString));
            services.AddTransient<IQueryService, QueryService>();

            services.AddScoped<IApplicationEventBus, ApplicationEventBus>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            //services.AddScoped<UserApplicationService>();
            //services.AddScoped<RoleApplicationService>();
            services.AddScoped<IAppInitializer, AppInitializer>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowVite5174", policy =>
                {
                    policy.WithOrigins("http://localhost:5174")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // ======================註冊「MVC / Web API」(這是分水嶺) 上面放 應用層級 相關註冊====================

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            // ======================註冊「MVC / Web API」(這是分水嶺) 下面放 中介軟體 相關註冊====================

            // 註冊錯誤處理（應該放在中間，確保它能攔截大部分錯誤）
            services.AddProblemDetails();
            // 這應該放在 ExceptionHandler 之前
            services.AddExceptionHandler<ExceptionHandler>();

            // 註冊「認證 & 授權」
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer();
            services.ConfigureOptions<ConfigureJwtBearerOptions>();
            services.AddAuthorization(options =>
            {
                foreach (var permission in Authority.AuthorityList.Select(x=>x.Code))
                {
                    options.AddPolicy(permission, policy =>
                        policy.RequireClaim("Permission", permission));
                }
            });

            services.AddTsaLamsSwagger();
        }
    }
}
