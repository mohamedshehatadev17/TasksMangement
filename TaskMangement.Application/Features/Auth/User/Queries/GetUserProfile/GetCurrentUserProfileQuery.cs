
using MediatR;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Auth.User.Queries.GetUserProfile
{
    public record GetCurrentUserProfileQuery(): IRequest<Result<UserProfileResponse>>;
}
