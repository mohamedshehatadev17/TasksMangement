using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;
namespace TaskMangement.Application.Features.Auth.User.Queries.GetUserProfile
{
    public sealed class GetCurrentUserProfileHandler: IRequestHandler<GetCurrentUserProfileQuery, Result<UserProfileResponse>>
    {
        private readonly UserManager<Domain.Models.User> _userManager;
        private readonly ICurrentUser _currentUser;
        public GetCurrentUserProfileHandler(UserManager<Domain.Models.User> userManager,ICurrentUser currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
        }

        public async Task<Result<UserProfileResponse>> Handle(GetCurrentUserProfileQuery request,CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == _currentUser.UserId, cancellationToken);
            if (user is null)
                return Result<UserProfileResponse>.Failure("User not found");
            var userProfile = user.Adapt<UserProfileResponse>();
            userProfile.Roles = await _userManager.GetRolesAsync(user);
            return Result<UserProfileResponse>.Success(userProfile);
        }
    }
}
