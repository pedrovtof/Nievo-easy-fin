
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Add controller foi API aspnet core (dotnet)
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Build swagger API endpoint for explore API during development
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Build context Database for PGSQL Node MAIN and Node READ_REPLICA
/// </summary>
builder.Services.AddDbContext<auth.Data.Context.AuthOrigin>();
builder.Services.AddDbContext<auth.Data.Context.AuthReplica>();

var app = builder.Build();


/// <summary>
/// Swagger can only be in ENV-> DEV
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();