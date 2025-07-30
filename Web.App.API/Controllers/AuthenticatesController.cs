using Application.Accounts.Commands;
using Application.Accounts.Queries;
using Application.Authenticates.Commands;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Prometheus;
using System.Diagnostics.Metrics;
using Web.App.API.Dtos.Authenticates;
using Web.App.API.Services;

namespace Web.App.API.Controllers
{
    public class AuthenticatesController : ApiControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

        // Đếm số lần login thành công và thất bại
        private static readonly Counter _loginSuccessCounter = Metrics
            .CreateCounter("app_login_success_total", "Số lần đăng nhập thành công");

        private static readonly Counter _loginFailCounter = Metrics
            .CreateCounter("app_login_fail_total", "Số lần đăng nhập thất bại");


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
        public async Task<ActionResult<string>> Login(LoginRequest request)
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
                return result;
            }
            catch (AppException ex) when (ex.Code == ExceptionCode.Invalidate)
            {
                _loginFailCounter.Inc();
                throw new AppException(ExceptionCode.Invalidate, "Username or password is incorrect");
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
