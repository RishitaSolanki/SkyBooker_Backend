using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.PaymentService.DTOs;
using SkyBooker.PaymentService.Services;

namespace SkyBooker.PaymentService.Controllers;

[ApiController]
[Route("api/Payment")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentById(string id)
    {
        var result = await _paymentService.GetPaymentById(id);
        if (result == null)
            return NotFound(new { message = "Payment not found" });
        return Ok(result);
    }

    [HttpGet("booking/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentByBooking(string bookingId)
    {
        var result = await _paymentService.GetPaymentByBooking(bookingId);
        if (result == null)
            return NotFound(new { message = "Payment not found" });
        return Ok(result);
    }

    [HttpGet("transaction/{transactionId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentByTransaction(string transactionId)
    {
        var result = await _paymentService.GetPaymentByTransaction(transactionId);
        if (result == null)
            return NotFound(new { message = "Payment not found" });
        return Ok(result);
    }

    [HttpGet("user/{userId}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentsByUser(string userId)
    {
        var result = await _paymentService.GetPaymentsByUser(userId);
        return Ok(result);
    }

    [HttpGet("status/{status}")]
    [Authorize]
    public async Task<IActionResult> GetPaymentsByStatus(string status)
    {
        var result = await _paymentService.GetPaymentsByStatus(status);
        return Ok(result);
    }

    [HttpGet("user/{userId}/revenue")]
    [Authorize]
    public async Task<IActionResult> GetTotalRevenueByUser(string userId)
    {
        var result = await _paymentService.GetTotalRevenueByUser(userId);
        return Ok(new { totalRevenue = result });
    }

    [HttpGet("date-range")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetPaymentsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _paymentService.GetPaymentsByDateRange(startDate, endDate);
        return Ok(result);
    }

    [HttpGet("revenue")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> GetRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _paymentService.GetRevenue(startDate, endDate);
        return Ok(new { revenue = result });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> InitiatePayment([FromBody] CreatePaymentDto request)
    {
        var result = await _paymentService.InitiatePayment(request);
        if (result == null)
            return BadRequest(new { message = "Failed to initiate payment" });
        return CreatedAtAction(nameof(GetPaymentById), new { id = result.PaymentId }, result);
    }

    [HttpPost("{id}/process")]
    [Authorize]
    public async Task<IActionResult> ProcessPayment(string id, [FromBody] ProcessPaymentDto request)
    {
        var result = await _paymentService.ProcessPayment(id, request.TransactionId, request.GatewayResponse);
        if (result == null)
            return NotFound(new { message = "Payment not found" });
        return Ok(result);
    }

    [HttpPost("{id}/refund")]
    [Authorize(Roles = "ADMIN, AIRLINE_STAFF")]
    public async Task<IActionResult> RefundPayment(string id)
    {
        var result = await _paymentService.RefundPayment(id);
        if (result == null)
            return NotFound(new { message = "Payment not found or cannot be refunded" });
        return Ok(result);
    }

    [HttpGet("{id}/status")]
    [Authorize]
    public async Task<IActionResult> GetPaymentStatus(string id)
    {
        var result = await _paymentService.GetPaymentStatus(id);
        if (result == null)
            return NotFound(new { message = "Payment not found" });
        return Ok(new { status = result.Status });
    }

    [HttpGet("user/{userId}/status")]
    [Authorize]
    public async Task<IActionResult> GetPaymentStatusByUserId(string userId)
    {
        var result = await _paymentService.GetPaymentStatusByUserId(userId);
        if (result == null || !result.Any())
            return NotFound(new { message = "No payments found for this user" });
        return Ok(result);
    }

    [HttpPost("create-order")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateOrder([FromBody] RazorpayOrderRequest request)
    {
        try
        {
            var orderId = await _paymentService.CreateRazorpayOrder(request.Amount, request.Currency);
            var keyId = await _paymentService.GetRazorpayKeyId();
            return Ok(new { orderId, keyId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyPayment([FromBody] RazorpayVerifyRequest request)
    {
        var isValid = await _paymentService.VerifyRazorpayPayment(request.OrderId, request.PaymentId, request.Signature);
        if (isValid)
        {
            // Here you would typically also update the payment status in the DB
            return Ok(new { status = "success" });
        }
        return BadRequest(new { status = "failure" });
    }
}

public class RazorpayOrderRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
}

public class RazorpayVerifyRequest
{
    public string OrderId { get; set; } = string.Empty;
    public string PaymentId { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

public class ProcessPaymentDto
{
    public string TransactionId { get; set; } = string.Empty;
    public string GatewayResponse { get; set; } = string.Empty;
}
