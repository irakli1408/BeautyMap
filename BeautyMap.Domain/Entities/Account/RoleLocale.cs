using BeautyMap.Domain.Common.BaseEntities;
using BeautyMap.Domain.Common.Contract;
using BeautyMap.Domain.Entities.Admin.Languages;

namespace BeautyMap.Domain.Entities.Account
{
    public class RoleLocale : BaseEntity, ILocales
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        public int LanguageId { get; set; }

        #region Relationship
        public virtual Language LanguageTypeMap { get; set; }
        public virtual Role RoleTypeMap { get; set; }
        #endregion
    }
}
