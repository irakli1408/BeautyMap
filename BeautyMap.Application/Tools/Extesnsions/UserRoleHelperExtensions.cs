using BeautyMap.Application.Common.Models;
using BeautyMap.Domain.Enums.Role;

namespace BeautyMap.Application.Tools.Extesnsions
{
    public static class UserRoleHelperExtensions
    {
        public static UserRoleCodeEnum GetRoleCodeEnum(string roleId)
        {
            if (roleId == UserRoleConstant.Administrator)
            {
                return UserRoleCodeEnum.Administrator;
            }
            else
            {
                return UserRoleCodeEnum.ContentManager;
            };
        }

        public static List<NamedData<string>> GetRoles()
        {
            return
            [
                new() {
                    Id = UserRoleConstant.Administrator,
                    Name = nameof(UserRoleCodeEnum.Administrator)
                },
                new()
                {
                    Id = UserRoleConstant.ContentManager,
                    Name = nameof(UserRoleCodeEnum.ContentManager)
                }
            ];
        }
    }
}
