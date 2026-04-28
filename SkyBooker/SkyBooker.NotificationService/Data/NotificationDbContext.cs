using Microsoft.EntityFrameworkCore;
using SkyBooker.NotificationService.Entities;

namespace SkyBooker.NotificationService.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);

            entity.Property(e => e.NotificationId)
                .HasMaxLength(50)
                .ValueGeneratedNever();

            entity.Property(e => e.RecipientId)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Message)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(e => e.Channel)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.RelatedBookingId)
                .HasMaxLength(50);

            entity.Property(e => e.SentAt)
                .HasColumnType("datetime");

            entity.Property(e => e.ReadAt)
                .HasColumnType("datetime");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime");

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            entity.HasIndex(e => e.RecipientId);
            entity.HasIndex(e => e.Type);
            entity.HasIndex(e => e.RelatedBookingId);
            entity.HasIndex(e => e.SentAt);
            entity.HasIndex(e => new { e.RecipientId, e.IsRead });
        });
    }
}
