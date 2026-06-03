using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models;
using User = TaskMangement.Domain.Models.User; // Add this using directive to import the User type

namespace TaskMangement.Application.Features.Auth.Admin.Queries
{
    public class GetUsersQueryHandler
        : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
    {
        private readonly UserManager<Domain.Models.User> _userManager;

        public GetUsersQueryHandler(
            UserManager<Domain.Models.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users
                .Where(x => !x.IsDeleted)
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync(cancellationToken);

            if (users == null || users.Count == 0)
                return Result<List<UserDto>>.Failure("No users found.");

            var userDtos = users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                IsDeleted = user.IsDeleted,
                Role = _userManager.GetRolesAsync(user).Result.ToList()
            }).ToList();

            return Result<List<UserDto>>.Success(userDtos);
        }
    }
}
