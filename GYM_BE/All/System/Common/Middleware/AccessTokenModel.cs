namespace GYM_BE.All.System.Common.Middleware
{
    public class AccessTokenModel
    {
        public string AccessToken { get; set; }
        public string Sid { get; set; }
        public string Typ { get; set; }
        public string Iat { get; set; }
        public string IsAdmin { get; set; }
        public DateTime Expires { get; set; }
    }
}
