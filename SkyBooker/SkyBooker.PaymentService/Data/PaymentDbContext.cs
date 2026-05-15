using Microsoft.EntityFrameworkCore;
using SkyBooker.PaymentService.Entities;

namespace SkyBooker.PaymentService.Data;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
    }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId);

            entity.Property(e => e.PaymentId)
                .HasMaxLength(36);

            entity.Property(e => e.BookingId)
                .IsRequired()
                .HasMaxLength(36);

            entity.Property(e => e.UserId)
                .IsRequired()
                .HasMaxLength(36);

            entity.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.Currency)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("INR");

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("PENDING");

            entity.Property(e => e.PaymentMode)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.TransactionId)
                .HasMaxLength(100);

            entity.Property(e => e.GatewayResponse)
                .HasColumnType("text");

            entity.HasIndex(e => e.BookingId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.TransactionId);
        });
    }
}
