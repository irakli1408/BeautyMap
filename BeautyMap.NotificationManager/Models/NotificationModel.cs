using BeautyMap.NotificationManager.Enums;

namespace BeautyMap.NotificationManager.Models
{
    public class NotificationModel
    {
        public SendNotificationTypes Type { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
