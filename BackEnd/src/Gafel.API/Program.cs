using Gafel.API.Converters;
using Gafel.API.Handlers;
using Gafel.Application;
using Gafel.Infrastructure;
using Gafel.Infrastructure.Extensions;
using Gafel.Infrastructure.Identity;
using Gafel.Infrastructure.Migrations;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();



// Métodos de extensão
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(); // Ativa o handler

app.UseHttpsRedirection();
//app.UseAuthorization();

app.MapControllers();

await InitializeDatabaseAsync();

app.Run();

async Task InitializeDatabaseAsync()
{
    using var scope = app.Services.CreateScope();
    var service = scope.ServiceProvider;

    var connectionString = builder.Configuration.ConnectionString();

    DatabaseMigration.Migrate(service, connectionString);

    await IdentitySeeder.SeedRolesAsync(service);
}
