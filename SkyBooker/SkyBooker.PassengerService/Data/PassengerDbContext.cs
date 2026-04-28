using Microsoft.EntityFrameworkCore;
using SkyBooker.PassengerService.Entities;

namespace SkyBooker.PassengerService.Data;

public class PassengerDbContext : DbContext
{
    public PassengerDbContext(DbContextOptions<PassengerDbContext> options) : base(options)
    {
    }

    public DbSet<PassengerInfo> PassengerInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PassengerInfo>(entity =>
        {
            entity.HasKey(e => e.PassengerId);

            entity.Property(e => e.PassengerId)
                .HasMaxLength(36);

            entity.Property(e => e.BookingId)
                .IsRequired()
                .HasMaxLength(36);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Gender)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.PassportNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.Nationality)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.TicketNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.PassengerType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("ADULT");

            entity.HasIndex(e => e.BookingId);
            entity.HasIndex(e => e.PassportNumber);
            entity.HasIndex(e => e.TicketNumber);
            entity.HasIndex(e => e.SeatId);
        });
    }
}
