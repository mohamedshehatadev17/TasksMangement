using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models;

namespace TaskMangement.Infrastructure.Persistance.Authentication;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtProvider;
    public RefreshTokenGenerator(UserManager<User> userManager, IJwtTokenGenerator jwtProvider)
    {
        _userManager = userManager;
        _jwtProvider = jwtProvider;
    }
    public async Task<Result<RefreshTokenResponse>> GenerateRefreshTokenAsync(User? user, RefreshToken? storedToken)
    {
        if (user is null)
            return Result<RefreshTokenResponse>.Failure("Invalid refresh token");


        if (storedToken.ExpiresAt <= DateTime.UtcNow)
            return Result<RefreshTokenResponse>.Failure("Refresh token expired");

        if (storedToken.IsRevoked)
            return Result<RefreshTokenResponse>.Failure("Refresh token revoked");

        // Revoke old token
        storedToken.Revoke();

        // Create new refresh token

        var newRefreshToken = RefreshToken.Generate(user.Id);

        user.RefreshTokens.Add(newRefreshToken);

        await _userManager.UpdateAsync(user);

        var accessToken = await _jwtProvider.GenerateTokenAsync(user);

        return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });
    }
}
