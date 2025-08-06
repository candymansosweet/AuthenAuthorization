using Application.Accounts.Commands;
using Application.Accounts.Queries;
using Application.Authenticates.Commands;
using Application.Authenticates.Dto;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prometheus;
using System.Diagnostics.Metrics;
using Web.App.API.Dtos.Authenticates;
using Web.App.API.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web.App.API.Controllers
{
    public class AuthenticatesController : ApiControllerBase
    {
        private static readonly Histogram _loginDuration = Metrics.CreateHistogram(
    "app_login_duration_seconds",
    "Thời gian xử lý login (giây)",
    new HistogramConfiguration
    {
        Buckets = Histogram.ExponentialBuckets(start: 0.01, factor: 2, count: 10)
        // 0.01s, 0.02s, 0.04s, ..., ~10s
    });

        private static readonly Histogram _logoutDuration = Metrics.CreateHistogram(
            "app_logout_duration_seconds",
            "Thời gian xử lý logout (giây)",
            new HistogramConfiguration
            {
                Buckets = Histogram.ExponentialBuckets(0.01, 2, 10)
            });

        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

        // Đếm số lần login thành công và thất bại
        private static readonly Counter _loginSuccessCounter = Metrics
            .CreateCounter("app_login_success_total", "Số lần đăng nhập thành công");

        private static readonly Counter _loginFailCounter = Metrics
            .CreateCounter("app_login_fail_total", "Số lần đăng nhập thất bại");

        private static readonly Gauge _userOnlineGauge = Metrics.CreateGauge(
    "app_users_online", "Số người dùng đang online");
        //private readonly IAuthenticateRespository _authenticateRespository;
        public AuthenticatesController(
            //IAuthenticateRespository authenticateRespository,
            IOptions<AppSettings> appSettings,
            IUserService userService
            )
        {
            //_authenticateRespository = authenticateRespository;
            _appSettings = appSettings.Value;
            _userService = userService;

        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginDto>> Login(LoginRequest request)
        {
            using (_loginDuration.NewTimer()) // 👈 bắt đầu đo
            {
                var command = new LoginCommand
                {
                    SecretString = _appSettings.Secret,
                    Username = request.Username,
                    Password = request.Password
                };
                try
                {
                    var result = await Mediator.Send(command);
                    _loginSuccessCounter.Inc();
                    _userOnlineGauge.Inc();
                    return result;
                }
                catch (AppException ex) when (ex.Code == ExceptionCode.Invalidate)
                {
                    _loginFailCounter.Inc();
                    throw new AppException(ExceptionCode.Invalidate, "Username or password is incorrect");
                }
            } // 👈 kết thúc đo khi thoát `using`
        }
        [HttpPost("logout")]
        public async Task<ActionResult<LogoutDto>> Logout()
        {
            using (_logoutDuration.NewTimer())
            {
                var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return BadRequest("Token không hợp lệ.");
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    LogoutCommand command = new LogoutCommand
                    {
                        Token = token,
                    };
                    var result = await Mediator.Send(command);

                    _userOnlineGauge.Dec(); // giảm số người online
                    return result;
                }
                catch (Exception ex)
                {
                    throw new AppException(ExceptionCode.Invalidate, "Không thể đăng xuất");
                }
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var a = _userService.GetUserInfor();
            // Example usage of IUserService, replace with actual logic as needed
            return Ok(a);
        }



    }
}
