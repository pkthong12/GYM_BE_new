namespace GYM_BE.All.System.Authentication
{
    public class ClientLoginRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
