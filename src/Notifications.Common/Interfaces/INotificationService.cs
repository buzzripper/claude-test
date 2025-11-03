using Dyvenix.Notifications.Common.DTOs;

namespace Dyvenix.Notifications.Common.Interfaces;

public interface INotificationService
{
    Task<ApiResponse<NotificationResponse>> SendEmailAsync(SendEmailRequest request, string requestedBy);
    Task<ApiResponse<NotificationResponse>> SendSmsAsync(SendSmsRequest request, string requestedBy);
}
