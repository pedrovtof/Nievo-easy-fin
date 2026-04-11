using System.Reflection;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Services.Base.Users;
using NievoEasyfin.Application.Services.Auth;
using NievoEasyfin.Application.Models;
using NievoEasyfin.Application.Services.Base.Authenticator;
using NievoEasyfin.Application.Infrastructure.Auth;
using NievoEasyfin.Application.Services.Security;
using FluentValidation;

namespace NievoEasyfin.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            Console.WriteLine($"Configurated Startup App");
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("Define the Cors rules as 0.0.0.0");
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            Console.WriteLine("Add controllers and endpoints");
            services.AddControllers();
            services.AddEndpointsApiExplorer();


            services.AddSwaggerGen(c =>
            {
                var xmlFileAuth = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPathAuth = Path.Combine(AppContext.BaseDirectory, xmlFileAuth);
                c.IncludeXmlComments(xmlPathAuth);
            });
            Console.WriteLine("Configured Swagger with xml paths and endpoints");

            Console.WriteLine("Creating Database services");
            services.AddDbContext<AuthOrigin>();
            services.AddDbContext<AuthReplica>();

            Console.WriteLine("Creating context services");
            services.AddScoped<SSoProviderAuth>();
            services.AddScoped<UserModel>();
            services.AddScoped<UserProviderSsoModel>();
            services.AddScoped<UserProviderSsoModel>();
            services.AddScoped<CryptoPasswordService>();
            services.AddScoped<AuthenticatorService>();
            services.AddScoped<AuthService>();
            services.AddScoped<UsersService>();
        }

        // Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            Console.WriteLine("Configuring the app to use Cors, MapControllers and make the app Run");
            app.UseCors();
            app.MapControllers();
            app.Run();
        }
    }
}