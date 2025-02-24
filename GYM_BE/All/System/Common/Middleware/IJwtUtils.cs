using GYM_BE.ENTITIES;

namespace GYM_BE.All.System.Common.Middleware
{
    public interface IJwtUtils
    {
        public AccessTokenModel GenerateAccessToken(SYS_USER user);
        public SYS_USER? GetUserFromAccessToken(string token);
    }
}
