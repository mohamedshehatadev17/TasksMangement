using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Tasks.Queries.GetAllTasks
{
    public sealed record GetTasksQuery(
        int page = 1,
        int pageSize = 10)
        : IRequest<Result<List<TaskResponse>>>;
}
