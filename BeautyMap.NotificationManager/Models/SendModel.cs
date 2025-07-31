using BeautyMap.Domain.Entities.Account;
using System.ComponentModel.DataAnnotations;

namespace BeautyMap.NotificationManager.Models
{
    public class SendModel : Send
    {
        [EmailAddress]
        [Required]
        public string UserId { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Number { get; private set; } = string.Empty;

        public SendModel(UserEntity user, string subject, string body)
            : base(subject, body)
        {
            UserId = user.Id;
            Email = user.Email;
            Number = user.PhoneNumber;
        }
    }
}
