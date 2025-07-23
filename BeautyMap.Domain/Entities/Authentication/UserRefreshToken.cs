using BeautyMap.Domain.Common.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace BeautyMap.Domain.Entities.Authentication
{
    public class UserRefreshToken : TrackedEntity
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
