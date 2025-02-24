using API;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GYM_BE.All.System.Common.Middleware
{
    [ScopedRegistration]
    public class JwtUtils : IJwtUtils
    {
        private readonly FullDbContext _fullDbContext;
        private readonly AppSettings _appSettings;

        public JwtUtils(FullDbContext fullDbContext, IOptions<AppSettings> appSettings)
        {
            _fullDbContext = fullDbContext;
            _appSettings = appSettings.Value;
        }

        public AccessTokenModel GenerateAccessToken(SYS_USER user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
            var expires = DateTime.UtcNow.AddMinutes(_appSettings.JwtToken.WebMinutesOfLife);
            var utcMilliseconds = new DateTimeOffset(expires).ToUnixTimeMilliseconds();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("sid", user.ID),
                    new Claim("typ", user.USERNAME),
                    new Claim("iat", user.EMPLOYEE_ID.ToString()),
                    new Claim("IsAdmin", user.IS_ADMIN.ToString()),
                    new Claim("exp", expires.ToString()),
                    new Claim("iss", _appSettings.JwtToken.Issuer),
                    new Claim("aud", _appSettings.JwtToken.Audience)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return new AccessTokenModel()
            {
                AccessToken = tokenString,
                Sid = user.ID,
                Typ = user.USERNAME,
                Iat = user.EMPLOYEE_ID.ToString(),
                IsAdmin = user.IS_ADMIN.ToString(),
                Expires = expires
            };
        }

        public SYS_USER? GetUserFromAccessToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtToken.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "sid").Value;
                var user = _fullDbContext.SysUsers.Find(userId);

                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
