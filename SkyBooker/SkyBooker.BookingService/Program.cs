using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SkyBooker.BookingService.Data;
using SkyBooker.BookingService.Repositories;
using SkyBooker.BookingService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Set application URL to use port 5003
builder.WebHost.UseUrls("http://localhost:5003");

// Add services to the container
builder.Services.AddControllers();

// Configure DbContext
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Register Services
builder.Services.AddScoped<IBookingService, BookingService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? 
                    throw new InvalidOperationException("JWT SecretKey not configured")))
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
        Title = "SkyBooker Booking Service API",
        Version = "v1",
        Description = "Booking management API for SkyBooker Airline Ticket Booking System",
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkyBooker Booking Service API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Auto-migrate database on startup
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
await dbContext.Database.MigrateAsync();

app.Run();
