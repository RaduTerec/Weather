using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Weather.Api.Core;
using Weather.Api.Core.Models;
using Weather.Api.Endpoints;
using Weather.Api.Extensions;
using Weather.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connectionString = builder.Configuration.GetConnectionString("WeatherConnectionString") ?? string.Empty;
builder.Services.AddDbContextPool<WeatherDbContext>(options => options
                   .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)
               ));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.AddApiHealthCheck();
builder.AddApiRateLimits();

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
app.UseRateLimiter();

app.MapGroup("/user")
   .ToUserEndpoints()
   .WithTags("Public");

app.Run();

public sealed partial class Program;
