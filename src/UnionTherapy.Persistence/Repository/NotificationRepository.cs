using Microsoft.EntityFrameworkCore;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Persistence.Context;

namespace UnionTherapy.Persistence.Repository;

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    public NotificationRepository(BaseDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: n => n.UserId == userId,
            include: q => q.Include(n => n.User),
            orderBy: q => q.OrderByDescending(n => n.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(
            predicate: n => n.UserId == userId && !n.IsRead,
            include: q => q.Include(n => n.User),
            orderBy: q => q.OrderByDescending(n => n.CreatedAt),
            cancellationToken: cancellationToken);
    }

    public async Task<int> GetUnreadNotificationCountByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
    }

    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await GetAsync(n => n.Id == notificationId, cancellationToken: cancellationToken);

        if (notification != null)
        {
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
            await UpdateAsync(notification, cancellationToken);
        }
    }

    public async Task MarkAllAsReadByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await GetListAsync(
            predicate: n => n.UserId == userId && !n.IsRead,
            cancellationToken: cancellationToken);

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
        }

        await UpdateRangeAsync(notifications, cancellationToken);
    }
} 