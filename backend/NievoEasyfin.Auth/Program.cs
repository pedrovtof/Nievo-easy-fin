using NievoEasyfin.Auth;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.TraversePath().Load();
var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

startup.Configure(app, builder.Environment);