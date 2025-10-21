using Microsoft.OpenApi.Models;
using NetLocator.BatchProcessingService.API.Middlewares;
using NetLocator.BatchProcessingService.Business.DI;
using NetLocator.BatchProcessingService.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<BatchProcessingConfiguration>(configuration.GetSection("BatchProcessing"));

builder.Services.AddBusinessDependencies();

builder.Services.AddAutoMapper(_ => {}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient("IpLookupService", client =>
{
    var config = configuration.GetSection("BatchProcessing").Get<BatchProcessingConfiguration>();
    client.BaseAddress = new Uri(config?.IpLookupServiceUrl ?? "http://localhost:5001");
    client.Timeout = TimeSpan.FromMinutes(5);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NetLocator Batch Processing Service API",
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetLocator Batch Processing Service API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();