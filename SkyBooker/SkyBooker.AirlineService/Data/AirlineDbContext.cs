using Microsoft.EntityFrameworkCore;
using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Data;

public class AirlineDbContext : DbContext
{
    public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options)
    {
    }

    public DbSet<Airline> Airlines { get; set; } = null!;
    public DbSet<Airport> Airports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Airline>(entity =>
        {
            entity.HasKey(e => e.AirlineId);
            
            entity.Property(e => e.AirlineId)
                .HasMaxLength(36)
                .ValueGeneratedNever();
                
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.IataCode)
                .IsRequired()
                .HasMaxLength(3);
                
            entity.Property(e => e.IcaoCode)
                .HasMaxLength(3);
                
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(500);
                
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.ContactEmail)
                .HasMaxLength(100);
                
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(20);
                
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");
                
            entity.HasIndex(e => e.IataCode)
                .IsUnique();
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.AirportId);
            
            entity.Property(e => e.AirportId)
                .HasMaxLength(36)
                .ValueGeneratedNever();
                
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.IataCode)
                .IsRequired()
                .HasMaxLength(3);
                
            entity.Property(e => e.IcaoCode)
                .HasMaxLength(3);
                
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Latitude)
                .HasColumnType("double");
                
            entity.Property(e => e.Longitude)
                .HasColumnType("double");
                
            entity.Property(e => e.Timezone)
                .HasMaxLength(50);
                
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");
                
            entity.HasIndex(e => e.IataCode)
                .IsUnique();
        });
    }
}
