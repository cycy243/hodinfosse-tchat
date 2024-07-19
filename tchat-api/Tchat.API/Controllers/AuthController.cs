using Microsoft.AspNetCore.Mvc;
using Tchat.Api.Models;
using Tchat.Api.Service.Auth;
using Tchat.Api.Services;
using Tchat.Api.Services.Auth;

namespace Tchat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GoogleAuthService _googleAuthService;
        private readonly LocaleAuthService _localeAuthService;
        private readonly IEmailSender _emailSender;

        public AuthController(GoogleAuthService googleAuthService, LocaleAuthService localeAuthService, IEmailSender emailSender)
        {
            _googleAuthService = googleAuthService;
            _localeAuthService = localeAuthService;
            _emailSender = emailSender;
        }

        [HttpPost("google")]
        public async Task<ActionResult<UserDto>> GoogleAuth([FromBody] GoogleOAuthToken token)
        {
            var result = await _googleAuthService.AuthUser(new UserDto() { Token = token.Token });
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] UserDto userDto)
        {
            var result = await _localeAuthService.AuthUser(userDto, true);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserDto userDto)
        {
            var result = await _localeAuthService.AuthUser(userDto, false);
            return Ok(result);
        }

        public record GoogleOAuthToken(string Token);
    }
}
