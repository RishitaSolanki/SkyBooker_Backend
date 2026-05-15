using Microsoft.EntityFrameworkCore;
using SkyBooker.FlightService.Configurations;
using SkyBooker.FlightService.Models;

namespace SkyBooker.FlightService.Data;

public class FlightDbContext : DbContext
{
    public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options)
    {
    }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<Airline> Airlines { get; set; }
    public DbSet<Airport> Airports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply entity configurations from separate configuration classes
        modelBuilder.ApplyConfiguration(new FlightConfiguration());
        modelBuilder.ApplyConfiguration(new AirlineConfiguration());
        modelBuilder.ApplyConfiguration(new AirportConfiguration());

        // Seed initial data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Airlines
        modelBuilder.Entity<Airline>().HasData(
            new Airline { AirlineId = 1, AirlineName = "Air India", IATACode = "AI", ICAOCode = "AIC", Country = "India", IsActive = true },
            new Airline { AirlineId = 2, AirlineName = "IndiGo", IATACode = "6E", ICAOCode = "IGO", Country = "India", IsActive = true },
            new Airline { AirlineId = 3, AirlineName = "SpiceJet", IATACode = "SG", ICAOCode = "SEJ", Country = "India", IsActive = true },
            new Airline { AirlineId = 4, AirlineName = "Vistara", IATACode = "UK", ICAOCode = "VTI", Country = "India", IsActive = true },
            new Airline { AirlineId = 5, AirlineName = "AirAsia India", IATACode = "I5", ICAOCode = "IAD", Country = "India", IsActive = true }
        );

        // Seed Airports
        modelBuilder.Entity<Airport>().HasData(
            new Airport { AirportId = 1, IATACode = "DEL", ICAOCode = "VIDP", AirportName = "Indira Gandhi International Airport", City = "New Delhi", Country = "India", Timezone = "Asia/Kolkata", Latitude = 28.5665M, Longitude = 77.1031M },
            new Airport { AirportId = 2, IATACode = "BOM", ICAOCode = "VABB", AirportName = "Chhatrapati Shivaji Maharaj International Airport", City = "Mumbai", Country = "India", Timezone = "Asia/Kolkata", Latitude = 19.0896M, Longitude = 72.8656M },
            new Airport { AirportId = 3, IATACode = "BLR", ICAOCode = "VOBL", AirportName = "Kempegowda International Airport", City = "Bangalore", Country = "India", Timezone = "Asia/Kolkata", Latitude = 13.1986M, Longitude = 77.7066M },
            new Airport { AirportId = 4, IATACode = "MAA", ICAOCode = "VOMM", AirportName = "Chennai International Airport", City = "Chennai", Country = "India", Timezone = "Asia/Kolkata", Latitude = 12.9941M, Longitude = 80.1709M },
            new Airport { AirportId = 5, IATACode = "CCU", ICAOCode = "VECC", AirportName = "Netaji Subhas Chandra Bose International Airport", City = "Kolkata", Country = "India", Timezone = "Asia/Kolkata", Latitude = 22.6547M, Longitude = 88.4467M },
            new Airport { AirportId = 6, IATACode = "HYD", ICAOCode = "VOHS", AirportName = "Rajiv Gandhi International Airport", City = "Hyderabad", Country = "India", Timezone = "Asia/Kolkata", Latitude = 17.2403M, Longitude = 78.4294M },
            new Airport { AirportId = 7, IATACode = "PNQ", ICAOCode = "VAPO", AirportName = "Pune Airport", City = "Pune", Country = "India", Timezone = "Asia/Kolkata", Latitude = 18.5822M, Longitude = 73.9197M },
            new Airport { AirportId = 8, IATACode = "AMD", ICAOCode = "VAAH", AirportName = "Sardar Vallabhbhai Patel International Airport", City = "Ahmedabad", Country = "India", Timezone = "Asia/Kolkata", Latitude = 23.0772M, Longitude = 72.6347M }
        );

        // Seed Sample Flights
        modelBuilder.Entity<Flight>().HasData(
            new Flight { FlightId = 1, FlightNumber = "AI101", AirlineId = 1, OriginAirportCode = "DEL", DestinationAirportCode = "BOM", DepartureTime = new DateTime(2026, 5, 14, 6, 0, 0), ArrivalTime = new DateTime(2026, 5, 14, 8, 30, 0), DurationMinutes = 150, AircraftType = "Boeing 787", TotalSeats = 250, AvailableSeats = 180, BusinessPrice = 8500.00M, EconomyPrice = 5500.00M, Status = Common.Enums.FlightStatus.Scheduled, CreatedAt = new DateTime(2026, 4, 27, 0, 0, 0) },
            new Flight { FlightId = 2, FlightNumber = "6E202", AirlineId = 2, OriginAirportCode = "BOM", DestinationAirportCode = "BLR", DepartureTime = new DateTime(2026, 5, 14, 10, 0, 0), ArrivalTime = new DateTime(2026, 5, 14, 11, 45, 0), DurationMinutes = 105, AircraftType = "Airbus A320", TotalSeats = 180, AvailableSeats = 120, BusinessPrice = 6200.00M, EconomyPrice = 4200.00M, Status = Common.Enums.FlightStatus.Scheduled, CreatedAt = new DateTime(2026, 4, 27, 0, 0, 0) },
            new Flight { FlightId = 3, FlightNumber = "UK303", AirlineId = 4, OriginAirportCode = "DEL", DestinationAirportCode = "BOM", DepartureTime = new DateTime(2026, 5, 14, 18, 0, 0), ArrivalTime = new DateTime(2026, 5, 14, 20, 30, 0), DurationMinutes = 150, AircraftType = "Airbus A321", TotalSeats = 200, AvailableSeats = 45, BusinessPrice = 9200.00M, EconomyPrice = 6200.00M, Status = Common.Enums.FlightStatus.Scheduled, CreatedAt = new DateTime(2026, 4, 27, 0, 0, 0) },
            new Flight { FlightId = 4, FlightNumber = "SG404", AirlineId = 3, OriginAirportCode = "DEL", DestinationAirportCode = "BOM", DepartureTime = new DateTime(2026, 5, 15, 7, 30, 0), ArrivalTime = new DateTime(2026, 5, 15, 10, 0, 0), DurationMinutes = 150, AircraftType = "Boeing 737", TotalSeats = 180, AvailableSeats = 90, BusinessPrice = 7200.00M, EconomyPrice = 4800.00M, Status = Common.Enums.FlightStatus.Scheduled, CreatedAt = new DateTime(2026, 4, 27, 0, 0, 0) },
            new Flight { FlightId = 5, FlightNumber = "AI102", AirlineId = 1, OriginAirportCode = "BOM", DestinationAirportCode = "DEL", DepartureTime = new DateTime(2026, 5, 14, 21, 0, 0), ArrivalTime = new DateTime(2026, 5, 14, 23, 30, 0), DurationMinutes = 150, AircraftType = "Boeing 787", TotalSeats = 250, AvailableSeats = 200, BusinessPrice = 8200.00M, EconomyPrice = 5200.00M, Status = Common.Enums.FlightStatus.Scheduled, CreatedAt = new DateTime(2026, 4, 27, 0, 0, 0) }
        );
    }
}