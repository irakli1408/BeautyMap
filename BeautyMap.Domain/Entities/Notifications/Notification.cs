using BeautyMap.Domain.Common.BaseEntities;
using BeautyMap.Domain.Entities.Account;

namespace BeautyMap.Domain.Entities.Notifications
{
    public class Notification : TrackedEntity<long>
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public UserEntity User { get; set; }
    }
}
