using MediatR;

namespace BeautyMap.Application.Handlers.Account.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<Unit>
    {
        public string ConfirmationCode { get; set; }
    }
}
