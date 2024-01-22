using MediatR;

namespace MagicCardTracker.Pwa.Notifications;

internal class Notification : INotification
{
    public Notification(string message, NotificationType notificationType = NotificationType.Info)
    {
        Message = message;
        NotificationType = notificationType;
    }

    public string Message { get; }

    public NotificationType NotificationType { get; }
}
