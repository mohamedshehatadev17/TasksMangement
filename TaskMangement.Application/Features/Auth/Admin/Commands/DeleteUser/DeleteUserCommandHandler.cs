using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models;

namespace TaskMangement.Application.Features.Auth.Admin.Commands.DeleteUser
{
    public class DeleteUserCommandHandler: IRequestHandler<DeleteUserCommand,Result<bool>>
    {
        private readonly UserManager<Domain.Models.User> _userManager;

        public DeleteUserCommandHandler(UserManager<Domain.Models.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand request,CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user is null)
                return Result<bool>.Failure("User not found");

            user.MarkAsDeleted();

            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
                return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));

            return Result<bool>.Success(true);
        }
    }
}
