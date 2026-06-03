using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Auth.Login
{
    public class LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
