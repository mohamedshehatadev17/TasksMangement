using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskMangement.Application.Features.Auth.Admin.Commands.CreateUser;
using TaskMangement.Application.Features.Auth.Admin.Commands.DeleteUser;
using TaskMangement.Application.Features.Auth.Admin.Queries;
using TaskMangement.Application.Helpers;

namespace TaskMangement.API.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUsers),new { id },id);
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(new GetUsersQuery(page, pageSize));
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }
    }
}
