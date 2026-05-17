using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using SkyBooker.SeatService.Configurations;
using SkyBooker.SeatService.Data;
using SkyBooker.SeatService.Repositories;
using SkyBooker.SeatService.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";
var maskedConn = System.Text.RegularExpressions.Regex.Replace(connectionString, "Password=[^;]+", "Password=***");
Console.WriteLine($"[DATABASE_DEBUG] Connecting with: {maskedConn}");

builder.Services.AddDbContext<SeatDbContext>(options =>
    options.UseNpgsql(ConvertPostgresUri(connectionString), x => x.MigrationsHistoryTable("__EFMigrationsHistory_SeatService")));

// Register Repositories
builder.Services.AddScoped<ISeatRepository, SeatRepository>();

// Register Services
builder.Services.AddScoped<ISeatService, SeatService>();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Clear default claim type mapping so JWT claims keep their original names
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured"))),
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
        Title = "SkyBooker Seat Service API",
        Version = "v1",
        Description = "Seat management API for SkyBooker Airline Ticket Booking System",
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
builder.Logging.SetMinimumLevel(LogLevel.Trace);

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyBooker Seat Service API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Create database and run migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<SeatDbContext>();
    context.Database.Migrate();

    // Auto-generate seats for mock flights (Flight IDs 1 to 5) if the table is empty
    if (!context.Seats.Any())
    {
        var seats = new List<SkyBooker.SeatService.Entities.Seat>();
        for (int flightId = 1; flightId <= 5; flightId++)
        {
            for (int i = 1; i <= 20; i++)
                seats.Add(new SkyBooker.SeatService.Entities.Seat { FlightId = flightId, SeatNumber = "B" + i, SeatClass = "Business", Status = "AVAILABLE", CreatedAt = DateTime.UtcNow });
            
            for (int i = 1; i <= 180; i++)
                seats.Add(new SkyBooker.SeatService.Entities.Seat { FlightId = flightId, SeatNumber = "E" + i, SeatClass = "Economy", Status = "AVAILABLE", CreatedAt = DateTime.UtcNow });
        }
        context.Seats.AddRange(seats);
        context.SaveChanges();
    }
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