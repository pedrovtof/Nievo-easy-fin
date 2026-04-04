using System.Reflection;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Services.Base.Users;
using NievoEasyfin.Application.Services.Auth;
using NievoEasyfin.Application.Models;
using FluentValidation;

namespace NievoEasyfin.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                var xmlFileAuth = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPathAuth = Path.Combine(AppContext.BaseDirectory, xmlFileAuth);
                c.IncludeXmlComments(xmlPathAuth);
            });

            services.AddDbContext<AuthOrigin>();
            services.AddDbContext<AuthReplica>();
            services.AddScoped<SsoProviderModel>();
            services.AddScoped<UserModel>();
            services.AddScoped<UserProviderSsoModel>();
            services.AddScoped<UserProviderSsoModel>();
            services.AddScoped<CryptoPasswordModel>();
            services.AddScoped<AuthService>();
            services.AddScoped<UsersService>();
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseCors();
            app.MapControllers();
            app.Run();
        }
    }
}