using Microsoft.OpenApi.Models;
using NetLocator.IPLookupService.API.Middlewares;
using NetLocator.IPLookupService.Business.DI;
using NetLocator.IPLookupService.Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddBusinessDependencies();

builder.Services.Configure<IpStackConfiguration>(configuration.GetSection("IpStack"));

builder.Services.AddControllers();

builder.Services.AddAutoMapper(_ => {}, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NetLocator IP Lookup Service API",
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetLocator IP Lookup Service API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();