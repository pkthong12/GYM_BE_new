namespace GYM_BE.Main
{
    public class HttpContextAccessorHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext => _httpContextAccessor.HttpContext;
    }
}
