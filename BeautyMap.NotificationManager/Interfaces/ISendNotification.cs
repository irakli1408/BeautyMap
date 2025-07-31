using BeautyMap.Domain.Entities.Account;
using BeautyMap.NotificationManager.Models;

namespace BeautyMap.NotificationManager.Interfaces
{
    public interface ISendNotification
    {
        Task SendNotificationAsync(UserEntity user, NotificationModel notification, string text);
    }
}
