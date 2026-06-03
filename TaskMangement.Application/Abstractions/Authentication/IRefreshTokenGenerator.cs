using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Models;

namespace TaskMangement.Application.Abstractions.Authentication
{
    public interface IRefreshTokenGenerator
    {
        Task<Result<RefreshTokenResponse>> GenerateRefreshTokenAsync(User? user, RefreshToken? refreshToken);
    }
}
