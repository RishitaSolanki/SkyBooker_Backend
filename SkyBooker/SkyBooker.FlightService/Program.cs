using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SkyBooker.FlightService.Configurations;
using SkyBooker.FlightService.Data;
using SkyBooker.FlightService.Repositories;
using SkyBooker.FlightService.Repositories.Interfaces;
using SkyBooker.FlightService.Services;
using SkyBooker.FlightService.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
var maskedConn = System.Text.RegularExpressions.Regex.Replace(connectionString, "Password=[^;]+", "Password=***");
Console.WriteLine($"[DATABASE_DEBUG] Connecting with: {maskedConn}");

builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseNpgsql(ConvertPostgresUri(connectionString), x => x.MigrationsHistoryTable("__EFMigrationsHistory_FlightService")));

// Register Repositories
builder.Services.AddScoped<IFlightRepository, FlightRepository>();

// Register Services
builder.Services.AddScoped<IFlightService, FlightService>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Clear default claim type mapping so JWT claims keep their original names (e.g. "role" stays "role")
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.IncludeErrorDetails = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured"))),
            // Map "role" claim to the role principal so [Authorize(Roles = "ADMIN")] works
            RoleClaimType = "role"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("ADMIN"));
    options.AddPolicy("AirlineStaffOnly", policy => policy.RequireRole("AIRLINE_STAFF"));
    options.AddPolicy("PassengerOnly", policy => policy.RequireRole("PASSENGER"));
    options.AddPolicy("AdminOrAirlineStaff", policy => policy.RequireRole("ADMIN", "AIRLINE_STAFF"));
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SkyBooker Flight Service API",
        Version = "v1",
        Description = "Flight management and search API for SkyBooker Airline Ticket Booking System",
        Contact = new OpenApiContact
        {
            Name = "SkyBooker Support",
            Email = "support@skybooker.com"
        }
    });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyBooker Flight Service API v1");
    c.RoutePrefix = "swagger";
});

// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FlightDbContext>();
    context.Database.EnsureCreated();
}

app.Run(); 
static string ConvertPostgresUri(string uri)
{
    if (string.IsNullOrEmpty(uri)) return uri;
    if (!uri.StartsWith("postgres://") && !uri.StartsWith("postgresql://")) return uri;
    var databaseUri = new Uri(uri);
    var userInfo = databaseUri.UserInfo.Split(':');
    var port = databaseUri.Port == -1 ? 5432 : databaseUri.Port;
    return $"Host={databaseUri.Host};Port={port};Database={databaseUri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true;";
}