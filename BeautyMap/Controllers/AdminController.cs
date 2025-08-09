using BeautyMap.API.Settings;
using BeautyMap.Application.Handlers.Account.Commands.ChangePassword;
using BeautyMap.Application.Handlers.Account.Commands.ConfirmEmail;
using BeautyMap.Application.Handlers.Account.Commands.EditProfile;
using BeautyMap.Application.Handlers.Account.Commands.RefreshToken;
using BeautyMap.Application.Handlers.Account.Commands.ResetPassword;
using BeautyMap.Application.Handlers.Account.Commands.ResetPasswordConfirmation;
using BeautyMap.Application.Handlers.Account.Commands.UserLogin;
using BeautyMap.Application.Handlers.Account.Commands.UserLogout;
using BeautyMap.Application.Handlers.Account.Queries.CheckEmail;
using BeautyMap.Application.Handlers.Account.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BeautyMap.API.Controllers
{
    public class AdminController : ApiControllerBase
    {
        public AdminController(IMediator mediator)
        : base(mediator) { }

        #region Authentication/Authorization
        [HttpPost("Login")]
        [SwaggerOperation("Login")]
        [AllowAnonymous]
        public Task<TokenApiResponseModel> Login([FromBody] UserLoginCommand request)
            => mediator.Send(request);

        [HttpPatch("Logout")]
        [SwaggerOperation("Logout")]
        [Authorize]
        public Task Logout()
        {
            return mediator.Send(new UserLogoutCommand { UserId = User?.Identity?.Name });
        }

        [HttpPatch("Refresh")]
        [SwaggerOperation("Refresh token")]
        [Authorize]
        public Task<TokenApiResponseModel> Refresh([FromBody] RefreshCommand request)
            => mediator.Send(request);

        #endregion

        #region Password
        [HttpPatch("change-password")]
        [SwaggerOperation("change password")]
        [Authorize]
        public Task EditProfile([FromBody] ChangePasswordCommandModel request)
            => mediator.Send(new ChangePasswordCommand { UserId = User.Identity.Name, Model = request });

        [HttpPatch("Password/Confirmation")]
        [SwaggerOperation("confirm password reset")]
        [AllowAnonymous]
        public Task PasswordReset([FromBody] ResetPasswordConfirmationCommand request)
            => mediator.Send(request);

        [HttpPatch("Password/Reset")]
        [SwaggerOperation("Reset Password")]
        [AllowAnonymous]
        public Task PasswordResetConfirmation(string email)
            => mediator.Send(new ResetPasswordCommand { Email = email });

        [HttpGet("confirm-email")]
        [SwaggerOperation("confirm email")]
        [AllowAnonymous]
        public Task ConfirmEmail(string confirmationCode)
            => mediator.Send(new ConfirmEmailCommand
            {
                ConfirmationCode = confirmationCode
            });
        #endregion

        #region Check Email 

        [HttpPost("email-exists")]
        [SwaggerOperation("email exists")]
        [AllowAnonymous]
        public Task<bool> CheckEmail([FromBody] string request) => mediator.Send(new EmailExistsQuery
        {
            Email = request
        });
        #endregion

        #region Account
        [HttpPost("edit-profile")]
        [SwaggerOperation("edit profile")]
        [Authorize]
        public Task EditProfile([FromBody] EditProfileCommandModel request)
            => mediator.Send(new EditProfileCommand { UserId = User.Identity.Name, Model = request });

        [HttpGet("Profile")]
        [SwaggerOperation("Get user Profile")]
        [Authorize]
        public Task<ProfileResponseModel> GetProfile()
            => mediator.Send(new GetProfileQuery { UserId = User.Identity.Name });
        #endregion

        #region UserManagement
        [HttpGet("get-Roles")]
        [SwaggerOperation("Get Roles")]
        [AuthorizeByAnyRole("Administrator")]
        public Task<List<GetRolesResponse>> GetRoles()
            => mediator.Send(new GetRolesQuery());

        [HttpGet("get-admin-roles")]
        [SwaggerOperation("Get Admin Roles")]
        [AuthorizeByAnyRole("Administrator")]
        public Task<List<GetAdminRolesResponse>> GetAdminRoles()
            => mediator.Send(new GetAdminRolesQuery());

        [HttpPost("add-user")]
        [SwaggerOperation("Add user")]
        [AuthorizeByAnyRole("Administrator")]
        public Task AddAdmin([FromBody] AddUserCommand request)
            => mediator.Send(request);

        [HttpGet("get-users")]
        [SwaggerOperation("get users")]
        [AuthorizeByAnyRole("Administrator")]
        public Task<PagedData<GetUsersQueryResponse>> GetUsers(int page, int offset, string searchKey = null, string roleId = null, bool? isBlocked = null)
            => mediator.Send(new GetUsersQuery
            {
                SearchKey = searchKey,
                RoleId = roleId,
                IsBlocked = isBlocked,
                Page = page,
                Offset = offset
            });

        [HttpGet("get-user/{id}")]
        [SwaggerOperation("get user by id")]
        [AuthorizeByAnyRole("Administrator")]
        public Task<GetUserByIdQueryResponse> GetUserById(string id)
            => mediator.Send(new GetUserByIdQuery { Id = id });

        [HttpPatch("edit-user")]
        [SwaggerOperation("edit user")]
        [AuthorizeByAnyRole("Administrator")]
        public Task EditUser([FromBody] EditUserCommand request)
        => mediator.Send(request);

        [HttpDelete("delete-user/{id}")]
        [SwaggerOperation("delete user")]
        [AuthorizeByAnyRole("Administrator")]
        public Task DeleteUser(string id)
        => mediator.Send(new DeleteUserCommand
        {
            Id = id
        });

        #endregion
    }
}
