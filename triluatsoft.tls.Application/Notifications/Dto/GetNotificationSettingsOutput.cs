using System.Collections.Generic;

namespace triluatsoft.tls.Notifications.Dto
{
    public class GetNotificationSettingsOutput
    {
        public bool ReceiveNotifications { get; set; }

        public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
    }
}