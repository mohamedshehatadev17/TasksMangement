using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Auth.RefreshToken;
public record CreateRefreshTokenCommand(string refreshToken) : IRequest<Result<RefreshTokenResponse>>;
