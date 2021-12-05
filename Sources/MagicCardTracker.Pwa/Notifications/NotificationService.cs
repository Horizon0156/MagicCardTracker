using System;

namespace MagicCardTracker.Pwa.Notifications
{
    internal sealed class NotificationService : INotificationService
    {
        public event EventHandler<Notification>? NotificationReceived;

        public void SendNotification(Notification notification)
        {
            OnNotificationReceived(notification);
        }

        private void OnNotificationReceived(Notification notification)
        {
            NotificationReceived?.Invoke(this, notification);
        }
    }
}
