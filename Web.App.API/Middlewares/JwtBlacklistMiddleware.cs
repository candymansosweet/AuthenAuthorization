using Common.Constants;
using Common.Services.TokenBlacklist;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Web.App.API.Services;

namespace Web.App.API.Middlewares
{
    public class JwtBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITokenBlacklist tokenBlacklist)
        {
            var jti = context.Items[ContextItems.Jti] as string;
            
            if (!string.IsNullOrEmpty(jti) && tokenBlacklist.IsRevoked(jti))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var result = new { message = "Unauthorized" };
                var json = JsonSerializer.Serialize(result);

                await context.Response.WriteAsync(json);
                return;
            }

            await _next(context);
        }
    }


}
