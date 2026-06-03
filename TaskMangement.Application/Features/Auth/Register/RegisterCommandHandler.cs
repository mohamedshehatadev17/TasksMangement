using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Features.Auth.Register;
using TaskMangement.Application.Helpers;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models;

namespace TaskMangement.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
    {
        private readonly UserManager<Domain.Models.User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public RegisterCommandHandler(UserManager<Domain.Models.User> userManager,IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<RegisterResponse>> Handle(RegisterCommand request,CancellationToken cancellationToken)
        {
            var validatorResult = await new RegisterCommandValidator().ValidateAsync(request,cancellationToken);
            if (!validatorResult.IsValid)
            {
                return Result<RegisterResponse>.Failure(
                    string.Join(",",
                    validatorResult.Errors.Select(x => x.ErrorMessage)));
            }
            var user = new Domain.Models.User(name: request.Name,email: request.Email);


            var result = await _userManager.CreateAsync(user,request.Password);

            if (!result.Succeeded)
            {
                return Result<RegisterResponse>.Failure(
                    string.Join(",",
                    result.Errors.Select(x => x.Description)));
            }
            await _userManager.AddToRoleAsync(user, Roles.User);
            var token = await _jwtTokenGenerator.GenerateTokenAsync(user);
            var response = new RegisterResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Token = token,
                ExpireAt = DateTime.UtcNow.AddMinutes(60) 
            };
            return Result<RegisterResponse>.Success(response);
        }
    }
}