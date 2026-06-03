using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMangement.Application.Helpers;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models;

namespace TaskMangement.Application.Features.Auth.Admin.Commands.CreateUser
{
    public class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly UserManager<Domain.Models.User> _userManager;

        public CreateUserCommandHandler(
            UserManager<Domain.Models.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand request,CancellationToken cancellationToken)
        {
            var validator = new CreateUserCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result<Guid>.Failure(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));

            var user = new Domain.Models.User(request.Name, request.Email);

            var result = await _userManager.CreateAsync(user,request.Password);

            if (!result.Succeeded)
                return Result<Guid>.Failure(string.Join(",", result.Errors.Select(x => x.Description)));

            await _userManager.AddToRoleAsync(user, Roles.User);

            return Result<Guid>.Success(user.Id);
        }
    }
}
