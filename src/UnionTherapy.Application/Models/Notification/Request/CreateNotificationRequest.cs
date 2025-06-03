using UnionTherapy.Domain.Enums;

namespace UnionTherapy.Application.Models.Notification.Request;

public class CreateNotificationRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
} 