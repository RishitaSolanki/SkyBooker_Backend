using SkyBooker.NotificationService.DTOs;
using SkyBooker.NotificationService.Entities;
using SkyBooker.NotificationService.Repositories;

namespace SkyBooker.NotificationService.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(INotificationRepository repository, ILogger<NotificationService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<NotificationDto?> SendNotification(CreateNotificationDto notificationDto)
    {
        try
        {
            var notification = new Notification
            {
                RecipientId = notificationDto.RecipientId,
                Type = notificationDto.Type,
                Title = notificationDto.Title,
                Message = notificationDto.Message,
                Channel = notificationDto.Channel,
                RelatedBookingId = notificationDto.RelatedBookingId,
                SentAt = DateTime.UtcNow
            };

            var result = await _repository.Add(notification);
            await SendEmail(notification);
            await SendSMS(notification);

            return MapToDto(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to recipient: {RecipientId}", notificationDto.RecipientId);
            return null;
        }
    }

    public async Task<NotificationDto?> SendBookingConfirmation(string recipientId, string bookingId, string pnrCode)
    {
        try
        {
            var notification = new Notification
            {
                RecipientId = recipientId,
                Type = "BOOKING_CONFIRMED",
                Title = "Booking Confirmed",
                Message = $"Your booking {pnrCode} has been confirmed. E-ticket and boarding pass will be sent to your email.",
                Channel = "EMAIL",
                RelatedBookingId = bookingId,
                SentAt = DateTime.UtcNow
            };

            var result = await _repository.Add(notification);
            await SendBookingConfirmationEmail(result);

            return MapToDto(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending booking confirmation for booking: {BookingId}", bookingId);
            return null;
        }
    }

    public async Task<List<NotificationDto>> SendBulkNotifications(List<CreateNotificationDto> notificationDtos)
    {
        var results = new List<NotificationDto>();

        foreach (var dto in notificationDtos)
        {
            var result = await SendNotification(dto);
            if (result != null)
            {
                results.Add(result);
            }
        }

        return results;
    }

    public async Task<bool> MarkAsRead(string notificationId)
    {
        try
        {
            return await _repository.MarkAsRead(notificationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking notification as read: {NotificationId}", notificationId);
            return false;
        }
    }

    public async Task<bool> MarkAllAsRead(string recipientId)
    {
        try
        {
            return await _repository.MarkAllAsRead(recipientId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking all notifications as read for recipient: {RecipientId}", recipientId);
            return false;
        }
    }

    public async Task<List<NotificationDto>> GetNotificationsByRecipient(string recipientId)
    {
        try
        {
            var notifications = await _repository.FindByRecipientId(recipientId);
            return notifications.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications for recipient: {RecipientId}", recipientId);
            return new List<NotificationDto>();
        }
    }

    public async Task<List<NotificationDto>> GetUnreadNotifications(string recipientId)
    {
        try
        {
            var notifications = await _repository.FindByRecipientIdAndIsRead(recipientId, false);
            return notifications.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unread notifications for recipient: {RecipientId}", recipientId);
            return new List<NotificationDto>();
        }
    }

    public async Task<int> GetUnreadCount(string recipientId)
    {
        try
        {
            return await _repository.CountByRecipientIdAndIsRead(recipientId, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unread count for recipient: {RecipientId}", recipientId);
            return 0;
        }
    }

    public async Task<NotificationDto?> GetNotificationById(string notificationId)
    {
        try
        {
            var notification = await _repository.FindByNotificationId(notificationId);
            return notification != null ? MapToDto(notification) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notification: {NotificationId}", notificationId);
            return null;
        }
    }

    public async Task<bool> DeleteNotification(string notificationId)
    {
        try
        {
            return await _repository.DeleteByNotificationId(notificationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting notification: {NotificationId}", notificationId);
            return false;
        }
    }

    public async Task<List<NotificationDto>> GetNotificationsByType(string type)
    {
        try
        {
            var notifications = await _repository.FindByType(type);
            return notifications.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications by type: {Type}", type);
            return new List<NotificationDto>();
        }
    }

    public async Task<List<NotificationDto>> GetNotificationsByBooking(string bookingId)
    {
        try
        {
            var notifications = await _repository.FindByRelatedBookingId(bookingId);
            return notifications.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving notifications by booking: {BookingId}", bookingId);
            return new List<NotificationDto>();
        }
    }

    public async Task<List<NotificationDto>> GetNotificationsWithPagination(string recipientId, int page, int pageSize)
    {
        try
        {
            var notifications = await _repository.FindByRecipientIdWithPagination(recipientId, page, pageSize);
            return notifications.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paginated notifications for recipient: {RecipientId}", recipientId);
            return new List<NotificationDto>();
        }
    }

    private NotificationDto MapToDto(Notification notification)
    {
        return new NotificationDto
        {
            NotificationId = notification.NotificationId,
            RecipientId = notification.RecipientId,
            Type = notification.Type,
            Title = notification.Title,
            Message = notification.Message,
            Channel = notification.Channel,
            RelatedBookingId = notification.RelatedBookingId,
            IsRead = notification.IsRead,
            SentAt = notification.SentAt,
            ReadAt = notification.ReadAt,
            CreatedAt = notification.CreatedAt,
            UpdatedAt = notification.UpdatedAt
        };
    }

    private async Task SendEmail(Notification notification)
    {
        try
        {
            if (notification.Channel == "EMAIL" || notification.Channel == "APP")
            {
                _logger.LogInformation("Sending email notification to {RecipientId}: {Title}", notification.RecipientId, notification.Title);
                // TODO: Implement MailKit/SendGrid email sending
                await Task.Delay(100); // Simulate email sending
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email notification: {NotificationId}", notification.NotificationId);
        }
    }

    private async Task SendSMS(Notification notification)
    {
        try
        {
            if (notification.Channel == "SMS")
            {
                _logger.LogInformation("Sending SMS notification to {RecipientId}: {Title}", notification.RecipientId, notification.Title);
                // TODO: Implement Twilio SMS sending
                await Task.Delay(100); // Simulate SMS sending
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS notification: {NotificationId}", notification.NotificationId);
        }
    }

    private async Task SendBookingConfirmationEmail(Notification notification)
    {
        try
        {
            _logger.LogInformation("Sending booking confirmation email to {RecipientId} for booking {BookingId}", 
                notification.RecipientId, notification.RelatedBookingId);
            // TODO: Implement email with PDF attachment via MimeKit
            await Task.Delay(100); // Simulate email sending with PDF attachment
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending booking confirmation email: {NotificationId}", notification.NotificationId);
        }
    }
}
