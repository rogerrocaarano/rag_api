using Infrastructure.Provider.DI;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var configuration = builder.Configuration;
var services = builder.Services;

configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
services.AddOpenApi();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "VIALMENTOR API Bridge", Version = "v1" });
});

services.InjectDependencies(configuration);

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "docs";
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vialmentor API Bridge");
});

app.Run();