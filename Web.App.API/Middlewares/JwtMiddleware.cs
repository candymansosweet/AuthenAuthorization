using Common.Constants;
using Common.Models;
using Common.Services.JwtTokenService;
using Microsoft.Extensions.Options;
using Web.App.API.Services;

namespace Web.App.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;   

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }
        public async Task Invoke(HttpContext context, IJwtTokenService jwtTokenService, IUserService userService)
        {
            string? token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
            var userInfor = jwtTokenService.ValidateToken(token, _appSettings.Secret);
            if (userInfor != null)
            {
                //var userInfor = userService.GetUserInfor(int.Parse(userId));
                context.Items[ContextItems.UserId] = userInfor.AccountId;
                context.Items[ContextItems.Username] = userInfor.AccountName;
                context.Items[ContextItems.Permissions] = userInfor.Permissions;
            }
            await _next(context);
        }
    }
}
