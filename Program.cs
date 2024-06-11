//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using AspNetCoreRateLimit;
//using Microsoft.Extensions.Options;
//using RestSharp;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.Configure<WeatherApi>(builder.Configuration.GetSection("WeatherApi"));
//builder.Services.AddControllers();
//builder.Services.AddScoped<IWeatherService, WeatherService>();

//builder.Services.AddMemoryCache();
//builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimit"));
//builder.Services.AddInMemoryRateLimiting();
//builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseIpRateLimiting();

//app.UseAuthorization();

//app.MapControllers();

////var summaries = new[]
////{
////    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
////};

////app.MapGet("/weatherforecast", () =>
////{
////    var forecast =  Enumerable.Range(1, 5).Select(index =>
////        new WeatherForecast
////        (
////            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
////            Random.Shared.Next(-20, 55),
////            summaries[Random.Shared.Next(summaries.Length)]
////        ))
////        .ToArray();
////    return forecast;
////})
////.WithName("GetWeatherForecast")
////.WithOpenApi();

//app.Run();

////record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
////{
////    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
////}
///

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AspNetCoreRateLimit;
using WeatherApp.Services;
using WeatherApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<WeatherApi>(builder.Configuration.GetSection("WeatherApi"));
builder.Services.AddControllers();
builder.Services.AddScoped<IWeatherService, WeatherService>();

//Logger configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add rate limiting services
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();

app.UseAuthorization();

app.MapControllers();

app.Run();

