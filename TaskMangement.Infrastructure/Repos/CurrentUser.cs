using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TaskMangement.Application.Abstractions.Contracts.Persistance;

namespace TaskMangement.Infrastructure.Repos
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId
        {
            get
            {
                var userIdString = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirstValue(JwtRegisteredClaimNames.Sub);

                if (!Guid.TryParse(userIdString, out var userId))
                    throw new UnauthorizedAccessException("Invalid user identifier.");

                return userId;
            }
        }
    }
}
