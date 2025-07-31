using BeautyMap.Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;

namespace BeautyMap.Application.Tools.ValidationHelper
{
    public static class UserValidation
    {
        public static async Task PasswordValidation(this UserManager<UserEntity> userManager, UserEntity newUser, string password)
        {
            var passwordValidator = new PasswordValidator<UserEntity>();
            var result = await passwordValidator.ValidateAsync(userManager, newUser, password);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errorMessage);
            }
        }
    }
}
