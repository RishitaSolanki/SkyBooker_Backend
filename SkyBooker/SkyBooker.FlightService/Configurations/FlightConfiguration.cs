using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SkyBooker.FlightService.Models;

namespace SkyBooker.FlightService.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        // Table name
        builder.ToTable("Flights");

        // Primary Key
        builder.HasKey(f => f.FlightId);

        // Properties
        builder.Property(f => f.FlightNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(f => f.OriginAirportCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(f => f.DestinationAirportCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(f => f.AircraftType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(f => f.BusinessPrice)
            .HasColumnType("decimal(10,2)");

        builder.Property(f => f.EconomyPrice)
            .HasColumnType("decimal(10,2)");

        builder.Property(f => f.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(f => f.Status)
            .HasDefaultValue(Common.Enums.FlightStatus.Scheduled);

        // Indexes
        builder.HasIndex(f => f.FlightNumber)
            .IsUnique()
            .HasDatabaseName("IX_Flight_FlightNumber");

        builder.HasIndex(f => new { f.OriginAirportCode, f.DestinationAirportCode, f.DepartureTime })
            .HasDatabaseName("IX_Flight_Route_DateTime");

        builder.HasIndex(f => f.Status)
            .HasDatabaseName("IX_Flight_Status");

        builder.HasIndex(f => f.AirlineId)
            .HasDatabaseName("IX_Flight_AirlineId");

        // Relationships
        builder.HasOne(f => f.Airline)
            .WithMany(a => a.Flights)
            .HasForeignKey(f => f.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AirlineConfiguration : IEntityTypeConfiguration<Airline>
{
    public void Configure(EntityTypeBuilder<Airline> builder)
    {
        builder.ToTable("Airlines");

        builder.HasKey(a => a.AirlineId);

        builder.Property(a => a.AirlineName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.IATACode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(a => a.ICAOCode)
            .HasMaxLength(4);

        builder.Property(a => a.Country)
            .HasMaxLength(100);

        builder.Property(a => a.LogoUrl)
            .HasMaxLength(500);

        builder.Property(a => a.IsActive)
            .HasDefaultValue(true);

        builder.HasIndex(a => a.IATACode)
            .IsUnique()
            .HasDatabaseName("IX_Airline_IATACode");
    }
}

public class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.ToTable("Airports");

        builder.HasKey(a => a.AirportId);

        builder.Property(a => a.IATACode)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(a => a.ICAOCode)
            .HasMaxLength(4);

        builder.Property(a => a.AirportName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Timezone)
            .HasMaxLength(50);

        builder.Property(a => a.Latitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(a => a.Longitude)
            .HasColumnType("decimal(9,6)");

        builder.HasIndex(a => a.IATACode)
            .IsUnique()
            .HasDatabaseName("IX_Airport_IATACode");
    }
}