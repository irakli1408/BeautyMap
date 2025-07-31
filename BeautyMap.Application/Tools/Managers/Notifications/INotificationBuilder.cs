using BeautyMap.Domain.Entities.Account;
using BeautyMap.NotificationManager.Enums;

namespace BeautyMap.Application.Tools.Managers.Notifications
{
    public interface INotificationBuilder
    {
        Task BuildNotificationAsync(UserEntity user, SendNotificationTypes type, string text);
    }
}
