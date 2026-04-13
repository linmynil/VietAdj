using Abp.Notifications;
using triluatsoft.tls.Dto;

namespace triluatsoft.tls.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}