using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tchat.Api.Models;
using Tchat.Api.Services;

namespace Tchat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService): base()
        {
            _messageService = messageService;
        }

        [HttpPut("{messageId:Guid}")]
        public IActionResult UpdateMessage(Guid messageId, [FromBody] dynamic message)
        {
            if (messageId != message.messageId)
            {
                return BadRequest("Wrong message id");
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> AddMessage([FromBody] MessageDto message)
        {
            return Ok(await _messageService.SendMessage(message));
        }

        [HttpGet]
        public async Task<ActionResult> GetMessages([FromQuery] int count)
        {
            return Ok(await _messageService.GetMessages(count));
        }
    }
}
