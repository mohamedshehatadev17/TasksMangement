using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Features.Auth.Login;
using TaskMangement.Application.Features.Auth.RefreshToken;
using TaskMangement.Application.Features.Auth.Register;
using TaskMangement.Application.Features.Auth.User.Queries.GetUserProfile;
using TaskMangement.Application.Shared;

namespace TaskMangement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? Ok(result.Value)
                : Unauthorized(result.Error);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? Ok(result.Value)
                : Unauthorized(result.Error);
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] CreateRefreshTokenCommand request,CancellationToken cancellationToken)
        {
            var command = new CreateRefreshTokenCommand(request.refreshToken);
            var result = await _mediator.Send(command,cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : Unauthorized(result.Error);
        }
        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserProfileResponse>> GetProfile(
    CancellationToken cancellationToken)
        {
            var profile = await _mediator.Send(new GetCurrentUserProfileQuery(),cancellationToken);

            return profile.IsSuccess
                ? Ok(profile.Value)
                : Unauthorized(profile.Error);
        }
    }
}