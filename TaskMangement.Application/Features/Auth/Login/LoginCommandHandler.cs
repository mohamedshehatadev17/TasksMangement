using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Features.Auth.Login;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models; // Add this using for User and RefreshToken

namespace TaskMangement.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler: IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<Domain.Models.User> _userManager; // Use User from Domain.Models
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        public LoginCommandHandler(UserManager<Domain.Models.User> userManager, IJwtTokenGenerator jwtTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<Result<LoginResponse>> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result<LoginResponse>.Failure("Invalid credentials");

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(
                user,
                request.Password);

            if (!isPasswordCorrect)
                return Result<LoginResponse>.Failure("Invalid credentials");

            var accessToken = await _jwtTokenGenerator.GenerateTokenAsync(user);
            var refreshToken = await CreateRefreshTokenAsync(user);

            return Result<LoginResponse>.Success(new LoginResponse
            {
                UserId = user.Id,
                Name= user.Name,
                Email = user.Email!,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(60), 
                Roles = await _userManager.GetRolesAsync(user),

            });

        }

        private async Task<Domain.Models.RefreshToken> CreateRefreshTokenAsync(Domain.Models.User user) 
        {
            var refreshToken = Domain.Models.RefreshToken.Generate(user.Id);

            user.RefreshTokens.Add(refreshToken);

            await _userManager.UpdateAsync(user);

            return refreshToken;
        }
    }
}