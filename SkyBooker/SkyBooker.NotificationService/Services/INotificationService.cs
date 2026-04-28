using SkyBooker.NotificationService.DTOs;

namespace SkyBooker.NotificationService.Services;

public interface INotificationService
{
    Task<NotificationDto?> SendNotification(CreateNotificationDto notificationDto);
    Task<NotificationDto?> SendBookingConfirmation(string recipientId, string bookingId, string pnrCode);
    Task<List<NotificationDto>> SendBulkNotifications(List<CreateNotificationDto> notificationDtos);
    Task<bool> MarkAsRead(string notificationId);
    Task<bool> MarkAllAsRead(string recipientId);
    Task<List<NotificationDto>> GetNotificationsByRecipient(string recipientId);
    Task<List<NotificationDto>> GetUnreadNotifications(string recipientId);
    Task<int> GetUnreadCount(string recipientId);
    Task<NotificationDto?> GetNotificationById(string notificationId);
    Task<bool> DeleteNotification(string notificationId);
    Task<List<NotificationDto>> GetNotificationsByType(string type);
    Task<List<NotificationDto>> GetNotificationsByBooking(string bookingId);
    Task<List<NotificationDto>> GetNotificationsWithPagination(string recipientId, int page, int pageSize);
}
