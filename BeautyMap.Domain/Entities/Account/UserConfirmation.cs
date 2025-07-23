using BeautyMap.Domain.Common.BaseEntities;
using BeautyMap.Domain.Common.Contract;
using BeautyMap.Domain.Entities.Notifications;

namespace BeautyMap.Domain.Entities.Account
{
    public class UserConfirmation : BaseEntity<int>, IDeletable
    {
        public UserConfirmation() { }
        public UserConfirmation(string userId, string confirmationCode)
        {
            UserId = userId;
            Code = confirmationCode;
            ExpirationDate = DateTime.UtcNow.AddMinutes(10);
            CreateDate = DateTime.UtcNow;
        }
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreateDate { get; set; }

        #region Relationship 

        #region User
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        #endregion

        #region NotificationTypes
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
        #endregion

        #endregion

        #region Delete
        public DateTime? DeleteDate { get; set; }
        public void Delete()
        {
            DeleteDate = DateTime.UtcNow;
        }
        #endregion
    }
}
