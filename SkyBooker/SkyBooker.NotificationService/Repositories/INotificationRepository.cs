using SkyBooker.NotificationService.Entities;

namespace SkyBooker.NotificationService.Repositories;

public interface INotificationRepository
{
    Task<Notification?> FindByNotificationId(string notificationId);
    Task<List<Notification>> FindByRecipientId(string recipientId);
    Task<List<Notification>> FindByRecipientIdAndIsRead(string recipientId, bool isRead);
    Task<int> CountByRecipientIdAndIsRead(string recipientId, bool isRead);
    Task<List<Notification>> FindByType(string type);
    Task<List<Notification>> FindByRelatedBookingId(string relatedBookingId);
    Task<Notification> Add(Notification notification);
    Task<Notification> Update(Notification notification);
    Task<bool> DeleteByNotificationId(string notificationId);
    Task<List<Notification>> FindByRecipientIdWithPagination(string recipientId, int page, int pageSize);
    Task<bool> MarkAsRead(string notificationId);
    Task<bool> MarkAllAsRead(string recipientId);
}
