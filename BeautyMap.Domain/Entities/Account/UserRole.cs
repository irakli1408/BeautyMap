using Microsoft.AspNetCore.Identity;

namespace BeautyMap.Domain.Entities.Account
{
    public class UserRole : IdentityUserRole<string>
    {

        #region Relationship

        public virtual Role Role { get; set; }
        public virtual UserEntity User { get; set; }

        #endregion
    }
}
