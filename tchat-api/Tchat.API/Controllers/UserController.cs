using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tchat.Api.Models;
using Tchat.Api.Services;
using Tchat.Api.Services.Args;
using Tchat.API.Policies;

namespace Tchat.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = nameof(PoliciesName.USER_MANAGEMENT))]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDomainService<UserDto, UserSearchArgs> _domainService;

        public UserController(IDomainService<UserDto, UserSearchArgs> domainService)
        {
            _domainService = domainService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            var user = await _domainService.Create(userDto);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _domainService.Delete(id);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UserDto userDto)
        {
            var user = await _domainService.Update(userDto);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _domainService.GetAll();
            return Ok(users);
        }
    }
}
