using Microsoft.EntityFrameworkCore;
using SkyBooker.BookingService.Entities;

namespace SkyBooker.BookingService.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {
    }

    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId);
            entity.HasIndex(e => e.PnrCode).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.FlightId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.UserId, e.Status });
            entity.HasIndex(e => new { e.FlightId, e.Status });
        });
    }
}
