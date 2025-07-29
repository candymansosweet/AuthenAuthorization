using Application.Accounts.Commands;
using Application.Accounts.Queries;
using Application.Authenticates.Commands;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Web.App.API.Dtos.Authenticates;
using Web.App.API.Services;

namespace Web.App.API.Controllers
{
    public class AuthenticatesController : ApiControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

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
            LoginCommand command = new LoginCommand();
            command.SecretString = _appSettings.Secret;
            command.Username = request.Username;
            command.Password = request.Password;

            return await Mediator.Send(command);
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
