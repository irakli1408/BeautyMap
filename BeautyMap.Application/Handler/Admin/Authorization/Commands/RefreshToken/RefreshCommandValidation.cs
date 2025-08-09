using FluentValidation;

namespace BeautyMap.Application.Handlers.Account.Commands.RefreshToken
{
    public class RefreshCommandValidation : AbstractValidator<RefreshCommand>
    {
        public RefreshCommandValidation()
        {
            RuleFor(x => x.AccessToken).NotNull().NotEmpty();
            RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
        }
    }
}
