using API;
using Azure.Core;
using GYM_BE.All.System.Common.Middleware;
using GYM_BE.All.System.SysUser;
using GYM_BE.Core.Dto;
using GYM_BE.Entities;
using GYM_BE.ENTITIES;
using GYM_BE.Main;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GYM_BE.All.System.Authentication
{
    [ApiExplorerSettings(GroupName = "001-SYSTEM-AUTHEN")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SysUserRepository _sysUserRepository;
        private readonly FullDbContext _dbContext;

        public AuthenticationController(
             IOptions<AppSettings> options,
             FullDbContext dbContext)
        {
            _appSettings = options.Value;
            _dbContext = dbContext;
            _sysUserRepository = new(dbContext);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ClientsLogin([FromBody] ClientLoginRequest Credentials)
        {
            try
            {
                if (Credentials == null) return Unauthorized();
                var r = await _sysUserRepository.ClientsLogin(Credentials.Username, Credentials.Password);
                if (r.MessageCode != "")
                {
                    if (r.MessageCode == "ERROR_USERNAME_INCORRECT")
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = "ERROR_USERNAME_INCORRECT" });
                    }
                    else
                    {
                        return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = r.MessageCode });
                    }
                }

                AuthResponse? user = r.InnerBody as AuthResponse;

                if (user?.IsLock == true) // dù đang là Admin mà bị Lock thì vẫn Lock như thường (ví dụ cần khóa khẩn)
                {
                    return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = "USER_LOCKED" });
                }

                if (user != null)
                {
                    var iat = user.EmployeeId != null ? user.EmployeeId.ToString() : "0";
                    var isAdmin = (user.IsAdmin == true).ToString();
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                        new Claim(JwtRegisteredClaimNames.Typ, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Iat, iat!),
                        new Claim("IsAdmin", isAdmin!),
                        new Claim("Password", Credentials.Password!)
                    };

                    string tokenString = BuildToken(claims, 1);
                    AuthResponse data = new()
                    {
                        Id = user.Id,
                        EmployeeId = user.EmployeeId,
                        UserName = user.UserName,
                        FullName = user.FullName,
                        Avatar = user.Avatar,
                        IsAdmin = user.IsAdmin,
                        IsRoot = user.IsRoot,
                        Token = tokenString,
                        Decentralization = user.Decentralization,
                    };

                    return Ok(new FormatedResponse()
                    {
                        MessageCode = "LOG_IN_SUCCESS",
                        InnerBody = data
                    });
                }
                else
                {
                    return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 401 };
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(AuthResponse authResponse)
        {
            var response = Request.Refresh(_appSettings, authResponse);
            if (response == null)
            {
                return Ok(new FormatedResponse()
                {
                    InnerBody = new
                    {
                        IsExpire = false
                    }
                });
            }
            var x = EpochTime.DateTime(response.TimeExpire!.Value);
            if(DateTime.Now > x)
            {
                return Ok(new FormatedResponse()
                {
                    InnerBody = new
                    {
                        IsExpire = false
                    }
                });
            }
            var r = await _sysUserRepository.ClientsLogin(response.UserName, response.Password);
            if (r.MessageCode != "")
            {
                return Ok(new FormatedResponse() { 
                    InnerBody = new  
                    { 
                        IsExpire = false 
                    } 
                });
            }

            AuthResponse? user = r.InnerBody as AuthResponse;

            if (user?.IsLock == true) // dù đang là Admin mà bị Lock thì vẫn Lock như thường (ví dụ cần khóa khẩn)
            {
                return Ok(new FormatedResponse() { StatusCode = EnumStatusCode.StatusCode400, ErrorType = EnumErrorType.CATCHABLE, MessageCode = "USER_LOCKED" });
            }

            if (user != null)
            {
                var iat = user.EmployeeId != null ? user.EmployeeId.ToString() : "0";
                var isAdmin = (user.IsAdmin == true).ToString();
                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                        new Claim(JwtRegisteredClaimNames.Typ, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Iat, iat!),
                        new Claim("IsAdmin", isAdmin!),
                        new Claim("Password", response.Password!)
                    };

                string tokenString = BuildToken(claims, 1);
                AuthResponse data = new()
                {
                    Id = user.Id,
                    EmployeeId = user.EmployeeId,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    IsAdmin = user.IsAdmin,
                    IsRoot = user.IsRoot,
                    Token = tokenString,
                    DateExpire = x,
                    Decentralization = user.Decentralization,
                };

                return Ok(new FormatedResponse()
                {
                    InnerBody = data
                });
            }
            else
            {
                return new JsonResult("WAR_UNABLE_TO_SIGN_IN") { StatusCode = 401 };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await Task.Run(() => DeleteTokenCookie());
            return Ok(new FormatedResponse() { InnerBody = "Successfully logged out" });
        }

        private CookieOptions cookieOptions(int expiresIn)
        {
            return new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(expiresIn)
            };
        }

        private void DeleteTokenCookie()
        {
            Response.Cookies.Delete("HiStaffRefreshToken", cookieOptions(-1));
        }

        private string BuildToken(Claim[] claims, int type)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtToken.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token;
            switch (type)
            {
                case 1:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.WebDaysOfLife),
                        signingCredentials: creds
                    );
                    break;
                case 2:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.WebMinutesOfLife),
                        signingCredentials: creds
                    );
                    break;
                case 3:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.MobileDaysOfLife),
                        signingCredentials: creds
                    );
                    break;
                default:
                    token = new JwtSecurityToken(
                        _appSettings.JwtToken.Issuer,
                        _appSettings.JwtToken.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddDays(_appSettings.JwtToken.WebMinutesOfLife),
                        signingCredentials: creds
                    );
                    break;
            }
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string IpAddress()
        {
            if (Request.Headers.TryGetValue("X-Forwarded-For", out Microsoft.Extensions.Primitives.StringValues value))
                return value!;
            else
                return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }
    }


    public class AuthResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsRoot { get; set; }
        public bool? IsLock { get; set; }
        public long? EmployeeId { get; set; }
        public long? TimeExpire { get; set; }
        public DateTime? DateExpire { get; set; }
        public List<string> Decentralization { get; set; } = new List<string>();
    }
}
