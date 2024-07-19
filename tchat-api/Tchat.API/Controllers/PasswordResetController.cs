using Microsoft.AspNetCore.Mvc;
using Tchat.Api.Services.User;

namespace Tchat.API.Controllers
{
    [Route("api/user/password")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IResetUserPassword _resetUserPassword;

        public PasswordResetController(IResetUserPassword resetUserPassword) 
        {
            _resetUserPassword = resetUserPassword;
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ConfirmResetDto dto)
        {
            await _resetUserPassword.ResetPassword(dto.email, dto.code, dto.newPassword);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> SendResetPasswordLink([FromQuery] string email)
        {
            await _resetUserPassword.SendResetPasswordLinkToUser(email);
            return Ok();
        }

        public record ConfirmResetDto(string code, string email, string newPassword);
    }
}
