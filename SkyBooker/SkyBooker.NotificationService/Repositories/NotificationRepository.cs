using Microsoft.EntityFrameworkCore;
using SkyBooker.NotificationService.Data;
using SkyBooker.NotificationService.Entities;

namespace SkyBooker.NotificationService.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _context;

    public NotificationRepository(NotificationDbContext context)
    {
        _context = context;
    }

    public async Task<Notification?> FindByNotificationId(string notificationId)
    {
        return await _context.Notifications
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
    }

    public async Task<List<Notification>> FindByRecipientId(string recipientId)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<List<Notification>> FindByRecipientIdAndIsRead(string recipientId, bool isRead)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId && n.IsRead == isRead)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<int> CountByRecipientIdAndIsRead(string recipientId, bool isRead)
    {
        return await _context.Notifications
            .CountAsync(n => n.RecipientId == recipientId && n.IsRead == isRead);
    }

    public async Task<List<Notification>> FindByType(string type)
    {
        return await _context.Notifications
            .Where(n => n.Type == type)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<List<Notification>> FindByRelatedBookingId(string relatedBookingId)
    {
        return await _context.Notifications
            .Where(n => n.RelatedBookingId == relatedBookingId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();
    }

    public async Task<Notification> Add(Notification notification)
    {
        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<Notification> Update(Notification notification)
    {
        notification.UpdatedAt = DateTime.UtcNow;
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
        return notification;
    }

    public async Task<bool> DeleteByNotificationId(string notificationId)
    {
        var notification = await FindByNotificationId(notificationId);
        if (notification == null)
            return false;

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Notification>> FindByRecipientIdWithPagination(string recipientId, int page, int pageSize)
    {
        return await _context.Notifications
            .Where(n => n.RecipientId == recipientId)
            .OrderByDescending(n => n.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<bool> MarkAsRead(string notificationId)
    {
        var notification = await FindByNotificationId(notificationId);
        if (notification == null || notification.IsRead)
            return false;

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        notification.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> MarkAllAsRead(string recipientId)
    {
        var unreadNotifications = await _context.Notifications
            .Where(n => n.RecipientId == recipientId && !n.IsRead)
            .ToListAsync();

        if (!unreadNotifications.Any())
            return false;

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
