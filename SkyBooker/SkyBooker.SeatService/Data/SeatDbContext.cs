using Microsoft.EntityFrameworkCore;
using SkyBooker.SeatService.Entities;

namespace SkyBooker.SeatService.Data;

public class SeatDbContext : DbContext
{
    public SeatDbContext(DbContextOptions<SeatDbContext> options) : base(options)
    {
    }

    public DbSet<Seat> Seats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId);
            
            entity.HasIndex(e => e.FlightId);
            entity.HasIndex(e => new { e.FlightId, e.SeatClass });
            entity.HasIndex(e => new { e.FlightId, e.SeatNumber }).IsUnique();
            entity.HasIndex(e => e.Status);
        });
    }
}
