using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Auth.Admin.Queries
{
    public record GetUsersQuery(int page, int pageSize): IRequest<Result<List<UserDto>>>;
}
