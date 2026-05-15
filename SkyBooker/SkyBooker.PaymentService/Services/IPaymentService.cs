using SkyBooker.PaymentService.DTOs;
using SkyBooker.PaymentService.Entities;

namespace SkyBooker.PaymentService.Services;

public interface IPaymentService
{
    Task<PaymentDto?> GetPaymentById(string paymentId);
    Task<PaymentDto?> GetPaymentByBooking(string bookingId);
    Task<PaymentDto?> GetPaymentByTransaction(string transactionId);
    Task<List<PaymentDto>> GetPaymentsByUser(string userId);
    Task<List<PaymentDto>> GetPaymentsByStatus(string status);
    Task<decimal> GetTotalRevenueByUser(string userId);
    Task<List<PaymentDto>> GetPaymentsByDateRange(DateTime startDate, DateTime endDate);
    Task<PaymentDto?> InitiatePayment(CreatePaymentDto request);
    Task<PaymentDto?> ProcessPayment(string paymentId, string transactionId, string gatewayResponse);
    Task<PaymentDto?> RefundPayment(string paymentId);
    Task<PaymentDto?> GetPaymentStatus(string paymentId);
    Task<List<PaymentDto>> GetPaymentStatusByUserId(string userId);
    Task<decimal> GetRevenue(DateTime startDate, DateTime endDate);
    Task<string> CreateRazorpayOrder(decimal amount, string currency = "INR");
    Task<bool> VerifyRazorpayPayment(string orderId, string paymentId, string signature);
    Task<string> GetRazorpayKeyId();
}
