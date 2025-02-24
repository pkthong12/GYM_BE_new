namespace GYM_BE.Main
{
    public class AddSid
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddSid(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void AddSidToHttpContext(string sid)
        {

           
        }
    }
}
