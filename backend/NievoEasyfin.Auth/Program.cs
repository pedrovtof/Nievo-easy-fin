
using System.Reflection;
using NievoEasyfin.Application.Data.Context.Database;
using NievoEasyfin.Application.Services.Base.Users;
using NievoEasyfin.Application.Services.Base.Authenticator;
using NievoEasyfin.Application.Services.Auth;


using NievoEasyfin.Application.Interfaces.Request;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFileAuth = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPathAuth = Path.Combine(AppContext.BaseDirectory, xmlFileAuth);
    c.IncludeXmlComments(xmlPathAuth);
});

builder.Services.AddDbContext<AuthOrigin>();
builder.Services.AddDbContext<AuthReplica>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthenticatorService>();
builder.Services.AddScoped<UsersService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();