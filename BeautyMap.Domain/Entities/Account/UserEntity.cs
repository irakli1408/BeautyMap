using BeautyMap.Domain.Common.Contract;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BeautyMap.Domain.Entities.Account
{
    public class UserEntity : IdentityUser<string>, IIdentityTrackedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get => base.Id; set => base.Id = value; }
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(25)]
        public string LastName { get; set; }
        public string OriginalUserName { get; set; }
        public bool IsActive { get; set; }

        #region TrackedEntity

        public DateTime? DeleteDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? DeletedBy { get; set; }

        public void Delete()
        {
            DeleteDate = DateTime.UtcNow;
        }

        public void UpdateCreateCredentials(DateTime createDate, string? createdBy)
        {
            CreateDate = createDate;
            CreatedBy = createdBy;
        }

        public void UpdateLastModifiedCredentials(DateTime lastModifiedDate, string? modifiedBy)
        {
            LastModifiedDate = lastModifiedDate;
            LastModifiedBy = modifiedBy;
        }

        public void UpdateDeleteCredentials(DateTime deleteDate, string? deletedBy)
        {
            DeleteDate = deleteDate;
            DeletedBy = deletedBy;
        }

        #endregion

        #region Relationship

        #region UserConfirmationCodes
        public ICollection<UserConfirmation> UserConfirmationCodes { get; set; }

        #endregion  

        #region Notifications
        public virtual ICollection<Notification> Notifications { get; set; }
        #endregion

        #region UserRoles
        public ICollection<UserRole> UserRoles { get; set; }
        #endregion

        #endregion
    }
}
