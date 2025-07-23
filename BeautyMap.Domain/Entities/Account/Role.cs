using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BeautyMap.Domain.Entities.Account
{
    public class Role : IdentityRole<string>
    {
        [MaxLength(30)]
        public override string Name { get => base.Name; set => base.Name = value; }

        #region Relationship
        public ICollection<RoleLocale> RoleLocales { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        #endregion
    }
}
