using NievoEasyFin.Auth;

Console.WriteLine("Begin the Auth startup");

var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Builded WebApplication");

DotNetEnv.Env.TraversePath().Load();
Console.WriteLine("Loaded envs");

var startup = new Startup(builder.Configuration);
Console.WriteLine($"Startup declared with builder configuration {builder.Configuration}");

startup.ConfigureServices(builder.Services);
Console.WriteLine($"Startup configured with {builder.Services}");

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine("Inicialized Swagger document");
}

Console.WriteLine($"Starting app {builder.Environment.ApplicationName} in {builder.Environment.EnvironmentName}");
startup.Configure(app, builder.Environment);
