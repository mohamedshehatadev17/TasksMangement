using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models;

namespace TaskMangement.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler
        : IRequestHandler<CreateRefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly UserManager<Domain.Models.User> _userManager;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        public RefreshTokenCommandHandler(UserManager<Domain.Models.User> userManager, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _userManager = userManager;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(CreateRefreshTokenCommand request,CancellationToken cancellationToken)
        {
            var validationResult = await new CreateRefreshTokenValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result<RefreshTokenResponse>.Failure(validationResult.Errors.First().ErrorMessage);

            var user = await _userManager.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x =>x.RefreshTokens.Any(r =>r.Token == request.refreshToken));

            var storedToken = user?.RefreshTokens
                .First(x => x.Token == request.refreshToken);
           var result = await _refreshTokenGenerator.GenerateRefreshTokenAsync(user, storedToken);
           return result.IsSuccess 
                ? Result<RefreshTokenResponse>.Success(result.Value) 
                : Result<RefreshTokenResponse>.Failure("Invalid refresh token");
        }
    }
}
