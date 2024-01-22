namespace MagicCardTracker.Pwa.Notifications;

internal interface INotificationService
{
    event EventHandler<Notification> NotificationReceived;
    
    void SendNotification(Notification notification);
}
