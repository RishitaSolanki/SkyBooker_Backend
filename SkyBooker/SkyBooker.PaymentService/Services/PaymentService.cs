using SkyBooker.PaymentService.DTOs;
using SkyBooker.PaymentService.Entities;
using SkyBooker.PaymentService.Repositories;

namespace SkyBooker.PaymentService.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentService(IPaymentRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaymentDto?> GetPaymentById(string paymentId)
    {
        var payment = await _repository.FindByPaymentId(paymentId);
        return payment != null ? MapToDto(payment) : null;
    }

    public async Task<PaymentDto?> GetPaymentByBooking(string bookingId)
    {
        var payment = await _repository.FindByBookingId(bookingId);
        return payment != null ? MapToDto(payment) : null;
    }

    public async Task<PaymentDto?> GetPaymentByTransaction(string transactionId)
    {
        var payment = await _repository.FindByTransactionId(transactionId);
        return payment != null ? MapToDto(payment) : null;
    }

    public async Task<List<PaymentDto>> GetPaymentsByUser(string userId)
    {
        var payments = await _repository.FindByUserId(userId);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<List<PaymentDto>> GetPaymentsByStatus(string status)
    {
        var payments = await _repository.FindByStatus(status);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<decimal> GetTotalRevenueByUser(string userId)
    {
        return await _repository.SumAmountByUserId(userId);
    }

    public async Task<List<PaymentDto>> GetPaymentsByDateRange(DateTime startDate, DateTime endDate)
    {
        var payments = await _repository.FindByPaidAtBetween(startDate, endDate);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<PaymentDto?> InitiatePayment(CreatePaymentDto request)
    {
        var payment = new Payment
        {
            PaymentId = Guid.NewGuid().ToString(),
            BookingId = request.BookingId,
            UserId = request.UserId,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = "PENDING",
            PaymentMode = request.PaymentMode,
            TransactionId = request.TransactionId,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repository.Add(payment);
        return MapToDto(result);
    }

    public async Task<PaymentDto?> ProcessPayment(string paymentId, string transactionId, string gatewayResponse)
    {
        var payment = await _repository.FindByPaymentId(paymentId);
        if (payment == null)
            return null;

        payment.Status = "PAID";
        payment.TransactionId = transactionId;
        payment.GatewayResponse = gatewayResponse;
        payment.PaidAt = DateTime.UtcNow;

        var result = await _repository.Update(payment);
        return MapToDto(result);
    }

    public async Task<PaymentDto?> RefundPayment(string paymentId)
    {
        var payment = await _repository.FindByPaymentId(paymentId);
        if (payment == null)
            return null;

        if (payment.Status != "PAID")
            return null;

        payment.Status = "REFUNDED";
        payment.RefundAmount = payment.Amount;
        payment.RefundedAt = DateTime.UtcNow;

        var result = await _repository.Update(payment);
        return MapToDto(result);
    }

    public async Task<PaymentDto?> GetPaymentStatus(string paymentId)
    {
        var payment = await _repository.FindByPaymentId(paymentId);
        return payment != null ? MapToDto(payment) : null;
    }

    public async Task<List<PaymentDto>> GetPaymentStatusByUserId(string userId)
    {
        var payments = await _repository.FindByUserId(userId);
        return payments.Select(MapToDto).ToList();
    }

    public async Task<decimal> GetRevenue(DateTime startDate, DateTime endDate)
    {
        var payments = await _repository.FindByPaidAtBetween(startDate, endDate);
        return payments.Where(p => p.Status == "PAID").Sum(p => p.Amount);
    }

    private PaymentDto MapToDto(Payment payment)
    {
        return new PaymentDto
        {
            PaymentId = payment.PaymentId,
            BookingId = payment.BookingId,
            UserId = payment.UserId,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Status = payment.Status,
            PaymentMode = payment.PaymentMode,
            TransactionId = payment.TransactionId,
            GatewayResponse = payment.GatewayResponse,
            PaidAt = payment.PaidAt,
            RefundedAt = payment.RefundedAt,
            RefundAmount = payment.RefundAmount,
            CreatedAt = payment.CreatedAt,
            UpdatedAt = payment.UpdatedAt
        };
    }
}
