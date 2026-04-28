using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.NotificationService.DTOs;
using SkyBooker.NotificationService.Services;

namespace SkyBooker.NotificationService.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendNotification([FromBody] CreateNotificationDto request)
    {
        var result = await _notificationService.SendNotification(request);
        if (result == null)
            return BadRequest(new { message = "Failed to send notification" });
        return CreatedAtAction(nameof(GetNotificationById), new { id = result.NotificationId }, result);
    }

    [HttpPost("booking-confirmation")]
    [Authorize]
    public async Task<IActionResult> SendBookingConfirmation([FromBody] BookingConfirmationRequest request)
    {
        var result = await _notificationService.SendBookingConfirmation(request.RecipientId, request.BookingId, request.PnrCode);
        if (result == null)
            return BadRequest(new { message = "Failed to send booking confirmation" });
        return Ok(result);
    }

    [HttpPost("bulk")]
    [Authorize(Roles = "ADMIN, AIRLINE_STAFF")]
    public async Task<IActionResult> SendBulkNotifications([FromBody] List<CreateNotificationDto> requests)
    {
        var results = await _notificationService.SendBulkNotifications(requests);
        return Ok(results);
    }

    [HttpGet("recipient/{recipientId}")]
    [Authorize]
    public async Task<IActionResult> GetNotificationsByRecipient(string recipientId)
    {
        var result = await _notificationService.GetNotificationsByRecipient(recipientId);
        return Ok(result);
    }

    [HttpGet("recipient/{recipientId}/unread")]
    [Authorize]
    public async Task<IActionResult> GetUnreadNotifications(string recipientId)
    {
        var result = await _notificationService.GetUnreadNotifications(recipientId);
        return Ok(result);
    }

    [HttpGet("recipient/{recipientId}/unread-count")]
    [Authorize]
    public async Task<IActionResult> GetUnreadCount(string recipientId)
    {
        var result = await _notificationService.GetUnreadCount(recipientId);
        return Ok(new { unreadCount = result });
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetNotificationById(string id)
    {
        var result = await _notificationService.GetNotificationById(id);
        if (result == null)
            return NotFound(new { message = "Notification not found" });
        return Ok(result);
    }

    [HttpGet("type/{type}")]
    [Authorize(Roles = "ADMIN, AIRLINE_STAFF")]
    public async Task<IActionResult> GetNotificationsByType(string type)
    {
        var result = await _notificationService.GetNotificationsByType(type);
        return Ok(result);
    }

    [HttpGet("booking/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> GetNotificationsByBooking(string bookingId)
    {
        var result = await _notificationService.GetNotificationsByBooking(bookingId);
        return Ok(result);
    }

    [HttpGet("recipient/{recipientId}/paginated")]
    [Authorize]
    public async Task<IActionResult> GetNotificationsWithPagination(string recipientId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _notificationService.GetNotificationsWithPagination(recipientId, page, pageSize);
        return Ok(result);
    }

    [HttpPut("{id}/mark-read")]
    [Authorize]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var result = await _notificationService.MarkAsRead(id);
        if (!result)
            return NotFound(new { message = "Notification not found or already read" });
        return Ok(new { message = "Notification marked as read" });
    }

    [HttpPut("recipient/{recipientId}/mark-all-read")]
    [Authorize]
    public async Task<IActionResult> MarkAllAsRead(string recipientId)
    {
        var result = await _notificationService.MarkAllAsRead(recipientId);
        if (!result)
            return NotFound(new { message = "No unread notifications found" });
        return Ok(new { message = "All notifications marked as read" });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteNotification(string id)
    {
        var result = await _notificationService.DeleteNotification(id);
        if (!result)
            return NotFound(new { message = "Notification not found" });
        return Ok(new { message = "Notification deleted successfully" });
    }
}

public class BookingConfirmationRequest
{
    public string RecipientId { get; set; } = string.Empty;
    public string BookingId { get; set; } = string.Empty;
    public string PnrCode { get; set; } = string.Empty;
}
