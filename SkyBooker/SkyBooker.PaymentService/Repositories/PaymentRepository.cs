using Microsoft.EntityFrameworkCore;
using SkyBooker.PaymentService.Data;
using SkyBooker.PaymentService.Entities;

namespace SkyBooker.PaymentService.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> FindByPaymentId(string paymentId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    public async Task<Payment?> FindByBookingId(string bookingId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.BookingId == bookingId);
    }

    public async Task<Payment?> FindByTransactionId(string transactionId)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);
    }

    public async Task<List<Payment>> FindByUserId(string userId)
    {
        return await _context.Payments.Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task<List<Payment>> FindByStatus(string status)
    {
        return await _context.Payments.Where(p => p.Status == status).ToListAsync();
    }

    public async Task<decimal> SumAmountByUserId(string userId)
    {
        return await _context.Payments.Where(p => p.UserId == userId && p.Status == "PAID").SumAsync(p => p.Amount);
    }

    public async Task<List<Payment>> FindByPaidAtBetween(DateTime startDate, DateTime endDate)
    {
        return await _context.Payments
            .Where(p => p.PaidAt.HasValue && p.PaidAt.Value >= startDate && p.PaidAt.Value <= endDate)
            .ToListAsync();
    }

    public async Task<Payment> Add(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> Update(Payment payment)
    {
        payment.UpdatedAt = DateTime.UtcNow;
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<bool> Delete(string paymentId)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        if (payment != null)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
