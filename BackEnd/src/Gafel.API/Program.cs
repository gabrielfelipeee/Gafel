using Gafel.API.Converters;
using Gafel.API.Extensions;
using Gafel.API.Handlers;
using Gafel.API.Models;
using Gafel.Application;
using Gafel.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Web/Controllers e JSON
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


// Configurações de Infraestrutura e Aplicação
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);


// Segurança (Auth)
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();


// Handler Global
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();


// Documentação
builder.Services.AddSwaggerGen(options =>
{
    options.MapType<GafelId>(() => new OpenApiSchema
    {
        Type = JsonSchemaType.String,
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ativa o Handler Global
app.UseExceptionHandler();


app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

await app.UseDatabaseInitialization();

app.UseCors("AllowSpecificOrigin");

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}
