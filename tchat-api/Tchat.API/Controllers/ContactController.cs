using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tchat.Api.Models;
using Tchat.Api.Services;
using Tchat.API.Args;
using Tchat.API.Policies;

namespace Tchat.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost("message")]
        [AllowAnonymous]
        public async Task<IActionResult> AskQuestion(ContactQuestionDto dto)
        {
            await _contactService.AskQuestion(dto);
            return Ok();
        }

        [HttpGet("all")]
        [Authorize(Policy = nameof(PoliciesName.CONTACT_MESSAGE_MANAGEMENT))]
        public async Task<IActionResult> GetAllQuestions([FromQuery] ContactQuestionSearchArgs args)
        {
            var questions = await _contactService.GetAllQuestions(args);
            return Ok(questions);
        }

        [HttpDelete("message/{id}")]
        [Authorize(Policy = nameof(PoliciesName.CONTACT_MESSAGE_MANAGEMENT))]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var result = await _contactService.DeleteQuestion(id);
            return Ok(result);
        }

        [HttpPut("message")]
        [Authorize(Policy = nameof(PoliciesName.CONTACT_MESSAGE_MANAGEMENT))]
        public async Task<IActionResult> AnswerQuestion(ContactQuestionDto dto)
        {
            var result = await _contactService.AnswerQuestion(dto);
            return Ok(result);
        }
    }
}
