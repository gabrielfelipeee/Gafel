using Gafel.API.Converters;
using Gafel.API.Handlers;
using Gafel.Application;
using Gafel.Infrastructure;
using Gafel.Infrastructure.Extensions;
using Gafel.Infrastructure.Identity;
using Gafel.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


// Handler de Autorização
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

// Autenticação e Autorização
AddAuthentication();
builder.Services.AddAuthorization();


// DI de Application e Infra
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// Handler Global
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


// Documentação
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ativa o Handler Global
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
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

void AddAuthentication()
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var signinKey = builder.Configuration.GetValue<string>("Settings:Jwt:SigninKey");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signinKey!)),

            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            // Garante que o algoritmo seja exatamente o esperado
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256Signature]
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Armazena a exceção para que o middleware de autorização possa ler depois
                context.HttpContext.Items["JwtException"] = context.Exception;
                return Task.CompletedTask;
            }
        };
    });
}