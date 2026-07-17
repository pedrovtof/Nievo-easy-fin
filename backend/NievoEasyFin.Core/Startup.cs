using System.Reflection;
using NievoEasyFin.Application.Data.Context.Database;
using NievoEasyFin.Application.Services.Cache;
using NievoEasyFin.Application.Services.Base.Users;
using NievoEasyFin.Application.Models;
using NievoEasyFin.Application.Services.Base.Authenticator;
using NievoEasyFin.Application.Interfaces.Services;
using NievoEasyFin.Application.Infrastructure.Auth;
using NievoEasyFin.Application.Services.Security;
using FluentValidation;
using NievoEasyFin.Application.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
using NievoEasyFin.Application.Services.Base;

namespace NievoEasyFin.Core;

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

        ConfigureSwagger(services);

        Console.WriteLine("Configuring JWT authentication");
        var jwtSecret = JsonWebTokenConfiguration.PrivateKey;
        var jwtKey = Encoding.ASCII.GetBytes(jwtSecret);
        var jwtIssuer = JsonWebTokenConfiguration.Issuer;
        var jwtAudience = JsonWebTokenConfiguration.Audience;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                ValidateLifetime = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                ClockSkew = TimeSpan.FromMinutes(5)
            };
        });

        services.AddAuthorization();

        Console.WriteLine("Creating Database services");
        services.AddDbContext<AuthOrigin>();
        services.AddDbContext<AuthReplica>();
        services.AddDbContext<CoreOrigin>();
        services.AddDbContext<CoreReplica>();

        Console.WriteLine("Creating Database Cache services");
        services.AddSingleton<AuthDbCacheService>();

        Console.WriteLine("Creating Transient services");
        services.AddTransient<JsonWebTokenConfiguration>();

        Console.WriteLine("Creating context services");

        // Others
        services.AddScoped<JsonWebTokenService>();
        services.AddScoped<SmtpProvider>();

        // Model
        services.AddScoped<SmtpModel>();
        services.AddScoped<AuthDbCacheService>();
        services.AddScoped<UserModel>();
        services.AddScoped<BankModel>();
        services.AddScoped<BankTypeModel>();


        // Service 
        services.AddScoped<IAccountsService, AccountsService>();
    }

    // Use this method to configure the HTTP request pipeline.
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        Console.WriteLine("Configuring the app to use Cors, Authentication, Authorization and MapControllers");
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NievoEasyFin Core API",
                Version = "v1"
            });

            var xmlFileAuth = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPathAuth = Path.Combine(AppContext.BaseDirectory, xmlFileAuth);
            c.IncludeXmlComments(xmlPathAuth);

            var bearerScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Informe o token JWT no formato: Bearer {token}"
            };

            c.AddSecurityDefinition("Bearer", bearerScheme);
            c.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement
            {
                [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
        Console.WriteLine("Configured Swagger with xml paths and endpoints");
    }
}
