using Microsoft.EntityFrameworkCore;

using Weather.Api.Extensions;
using Weather.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("WeatherConnectionString") ?? string.Empty;
builder.Services.AddDbContextPool<WeatherDbContext>(options => options
                   .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)
               ));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddApiHealthCheck();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction())
{
   app.UseHsts();
}
else
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapApiHealthCheck();

app.Run();

public sealed partial class Program;
