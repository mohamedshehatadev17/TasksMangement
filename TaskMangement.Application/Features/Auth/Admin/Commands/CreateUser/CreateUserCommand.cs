using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Auth.Admin.Commands.CreateUser
{
    public record CreateUserCommand(
       string Name,
       string Email,
       string Password,
       string Role
   ) : IRequest<Result<Guid>>;
}
