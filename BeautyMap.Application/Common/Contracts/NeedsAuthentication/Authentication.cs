namespace BeautyMap.Application.Common.Contracts.NeedsAuthentication
{
    public class Authentication : IAuthentication
    {
        public string UserId { get; set; }
    }

    public record AuthenticationRecord : IAuthentication
    {
        public string UserId { get; set; }
    }
}
