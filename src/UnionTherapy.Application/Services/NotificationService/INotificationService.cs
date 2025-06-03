using UnionTherapy.Application.Models.Notification.Request;
using UnionTherapy.Application.Models.Notification.Response;

namespace UnionTherapy.Application.Services.NotificationService;

public interface INotificationService
{
    Task<NotificationGetByIdResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationGetByIdResponse>> GetNotificationsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationGetByIdResponse>> GetUnreadNotificationsByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificationGetByIdResponse>> GetNotificationsByTypeAsync(string type, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    
    Task<CreateNotificationResponse> CreateAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default);
    Task<NotificationGetByIdResponse?> UpdateAsync(Guid id, CreateNotificationRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken = default);
    Task<bool> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> SendNotificationAsync(CreateNotificationRequest request, CancellationToken cancellationToken = default);
    Task<bool> SendBulkNotificationAsync(IEnumerable<CreateNotificationRequest> requests, CancellationToken cancellationToken = default);
    Task<bool> DeleteNotificationAsync(Guid notificationId, CancellationToken cancellationToken = default);
} 