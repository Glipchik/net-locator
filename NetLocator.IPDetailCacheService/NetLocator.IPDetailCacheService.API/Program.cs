using Microsoft.OpenApi.Models;
using NetLocator.IPDetailCacheService.API.Middlewares;
using NetLocator.IPDetailCacheService.Business.DI;
using NetLocator.IPDetailCacheService.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.Configure<IpLookupConfiguration>(configuration.GetSection("IpLookup"));

builder.Services.AddBusinessDependencies();

builder.Services.AddAutoMapper(_ => {}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NetLocator IP Detail Cache Service API",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetLocator IP Detail Cache Service API v1");
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();