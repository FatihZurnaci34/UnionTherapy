using AutoMapper;
using UnionTherapy.Application.Models.Notification.Request;
using UnionTherapy.Application.Models.Notification.Response;
using UnionTherapy.Application.Repository;
using UnionTherapy.Domain.Entities;
using UnionTherapy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace UnionTherapy.Application.Services.NotificationService;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<NotificationGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var notification = await _notificationRepository.GetAsync(
            predicate: n => n.Id == id,
            include: q => q.Include(n => n.User),
            cancellationToken: cancellationToken);

        return notification != null ? _mapper.Map<NotificationGetByIdResponse>(notification) : null;
    }

    public async Task<IEnumerable<NotificationGetByIdResponse>> GetNotificationsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.GetListAsync(
            predicate: n => n.UserId == userId,
            include: q => q.Include(n => n.User),
            orderBy: q => q.OrderByDescending(n => n.CreatedAt),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<NotificationGetByIdResponse>>(notifications);
    }

    public async Task<IEnumerable<NotificationGetByIdResponse>> GetUnreadNotificationsByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.GetListAsync(
            predicate: n => n.UserId == userId && !n.IsRead,
            include: q => q.Include(n => n.User),
            orderBy: q => q.OrderByDescending(n => n.CreatedAt),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<NotificationGetByIdResponse>>(notifications);
    }

    public async Task<IEnumerable<NotificationGetByIdResponse>> GetNotificationsByTypeAsync(string type, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<NotificationType>(type, true, out var notificationType))
            return new List<NotificationGetByIdResponse>();

        var notifications = await _notificationRepository.GetListAsync(
            predicate: n => n.Type == notificationType,
            include: q => q.Include(n => n.User),
            orderBy: q => q.OrderByDescending(n => n.CreatedAt),
            cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<NotificationGetByIdResponse>>(notifications);
    }

    public async Task<int> GetUnreadCountByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _notificationRepository.CountAsync(n => n.UserId == userId && !n.IsRead, cancellationToken);
    }

    public async Task<CreateNotificationResponse> CreateAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var notification = _mapper.Map<Notification>(request);
        notification.CreatedAt = DateTime.UtcNow;
        notification.IsRead = false;

        var createdNotification = await _notificationRepository.AddAsync(notification, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);

        var notificationResponse = _mapper.Map<NotificationGetByIdResponse>(createdNotification);
        return new CreateNotificationResponse { Notification = notificationResponse };
    }

    public async Task<NotificationGetByIdResponse?> UpdateAsync(Guid id, CreateNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var existingNotification = await _notificationRepository.GetByIdAsync(id, cancellationToken);
        if (existingNotification == null)
            return null;

        _mapper.Map(request, existingNotification);
        existingNotification.UpdatedAt = DateTime.UtcNow;

        var updatedNotification = await _notificationRepository.UpdateAsync(existingNotification, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<NotificationGetByIdResponse>(updatedNotification);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _notificationRepository.DeleteAsync(id, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId, cancellationToken);
        if (notification == null)
            return false;

        notification.IsRead = true;
        notification.ReadDate = DateTime.UtcNow;
        notification.UpdatedAt = DateTime.UtcNow;

        await _notificationRepository.UpdateAsync(notification, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var notifications = await _notificationRepository.GetListAsync(
            predicate: n => n.UserId == userId && !n.IsRead,
            cancellationToken: cancellationToken);

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            notification.UpdatedAt = DateTime.UtcNow;
        }

        if (notifications.Any())
        {
            await _notificationRepository.UpdateRangeAsync(notifications, cancellationToken);
            await _notificationRepository.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    public async Task<bool> SendNotificationAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var notification = _mapper.Map<Notification>(request);
        notification.CreatedAt = DateTime.UtcNow;
        notification.IsRead = false;

        await _notificationRepository.AddAsync(notification, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> SendBulkNotificationAsync(IEnumerable<CreateNotificationRequest> requests, CancellationToken cancellationToken = default)
    {
        var notifications = requests.Select(request =>
        {
            var notification = _mapper.Map<Notification>(request);
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;
            return notification;
        });

        await _notificationRepository.AddRangeAsync(notifications, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteNotificationAsync(Guid notificationId, CancellationToken cancellationToken = default)
    {
        await _notificationRepository.DeleteAsync(notificationId, cancellationToken);
        await _notificationRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
} 