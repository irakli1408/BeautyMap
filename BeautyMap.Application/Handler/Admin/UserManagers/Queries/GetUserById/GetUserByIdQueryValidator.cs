using FluentValidation;

namespace BeautyMap.Application.Handlers.Admin.UserManagers.Queries.GetUserById
{
    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
