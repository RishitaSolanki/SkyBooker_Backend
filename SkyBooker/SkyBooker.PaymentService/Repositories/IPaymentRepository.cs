using SkyBooker.PaymentService.Entities;

namespace SkyBooker.PaymentService.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> FindByPaymentId(string paymentId);
    Task<Payment?> FindByBookingId(string bookingId);
    Task<Payment?> FindByTransactionId(string transactionId);
    Task<List<Payment>> FindByUserId(string userId);
    Task<List<Payment>> FindByStatus(string status);
    Task<decimal> SumAmountByUserId(string userId);
    Task<List<Payment>> FindByPaidAtBetween(DateTime startDate, DateTime endDate);
    Task<Payment> Add(Payment payment);
    Task<Payment> Update(Payment payment);
    Task<bool> Delete(string paymentId);
}
