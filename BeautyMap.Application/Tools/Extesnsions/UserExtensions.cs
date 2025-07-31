using BeautyMap.Application.Common.Models;
using BeautyMap.Application.Common;
using BeautyMap.Application.Persistence;
using BeautyMap.Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BeautyMap.Application.Tools.ValidationHelper;

namespace BeautyMap.Application.Tools.Extesnsions
{
    public static class UserExtensions
    {
        // გამოძახება ხდება await userManager.IsRegistered;
        public static async Task IsRegistered(this UserManager<UserEntity> userManager, string email)
        {
            var existingUserByIdentityNumberAndEmail = await userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email && x.DeleteDate == null);
            if (existingUserByIdentityNumberAndEmail is not null)
                throw new Exception("Email Already Registered!");
        }
        // გამოძახება ხდება db.CheckUser(userId);
        public static async Task<UserEntity> CheckUser(this IBlogLikeDbContext db, string request)
        {
            return await db.Users
                .AsNoTracking()
                .Include(x => x.UserConfirmationCodes)
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                        .ThenInclude(x => x.RoleLocales)
                .FirstOrDefaultAsync(x => (x.Id == request || x.Email == request) && x.DeleteDate == null);
        }
        public static async Task IsEmailRegistered(this IBlogLikeDbContext db, string newEmail)
        {
            if (await db.Users.AsNoTracking().AnyAsync(x => x.Email == newEmail))
                throw new Exception("Email Already Exists!");
        }
        public static async Task<UserEntity> RegisterUser(this UserManager<UserEntity> userManager, UserCommonModel request, string password, string roleId)
        {
            await userManager.IsRegistered(request.Email);

            Guid guid = Guid.NewGuid();

            var newUser = new UserEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email.Split('@')[0] + guid,
                NormalizedUserName = (request.Email.Split('@')[0] + guid).ToUpper(),
                OriginalUserName = request.Email.Split('@')[0],
                Email = request.Email,
                CreateDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                IsActive = false,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
            };

            await userManager.PasswordValidation(newUser, password);

            var createResult = await userManager.CreateAsync(newUser, password);
            ThrowIfIdentityResultFailed(createResult);

            var roleEnum = UserRoleHelperExtensions.GetRoleCodeEnum(roleId).ToString();
            var addToRoleResult = await userManager.AddToRoleAsync(newUser, roleEnum);
            ThrowIfIdentityResultFailed(addToRoleResult);

            return newUser;
        }

        public static async Task<List<NamedData<string>>> GetUserRoles(this UserManager<UserEntity> userManager, RoleManager<Role> roleManager, int languageId, UserEntity user, string roleId = null)
        {
            var rolesQuery = UserRoleHelperExtensions.GetRoles().AsQueryable();

            if (!string.IsNullOrWhiteSpace(roleId))
            {
                rolesQuery = rolesQuery.Where(x => x.Id == roleId);
            }

            var certainRole = rolesQuery.Select(x => x.Name).ToList();

            var userRoleNames = await userManager.GetRolesAsync(user);

            var userAdminRoles = userRoleNames.Intersect(certainRole).ToList();

            var roles = await roleManager.Roles
                                  .Include(x => x.RoleLocales)
                                  .Where(r => userAdminRoles.Contains(r.Name))
                                  .Select(r => new NamedData<string>
                                  {
                                      Id = r.Id,
                                      Name = r.RoleLocales.FirstOrDefault(x => x.LanguageId == languageId).Name,
                                  })
                                  .ToListAsync();

            return roles;
        }

        #region Private
        public static void ThrowIfIdentityResultFailed(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception(errorMessage);
            }
        }
        #endregion
    }
}
