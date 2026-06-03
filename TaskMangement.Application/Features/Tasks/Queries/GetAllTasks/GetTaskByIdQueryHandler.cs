using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using TaskMangement.Application.Abstractions.Contracts;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Tasks.Queries.GetAllTasks
{
    public sealed class GetTasksQueryHandler: IRequestHandler<GetTasksQuery, Result<List<TaskResponse>>>
    {
        private readonly ITaskRepository _repository;
        private readonly ICurrentUser _currentUser;
        public GetTasksQueryHandler(ITaskRepository repository, ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<Result<List<TaskResponse>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(
                request.page,
                request.pageSize,
                x => !x.IsDeleted && x.UserId == _currentUser.UserId,
                cancellationToken);

            if (result is null || !result.Any())
                return Result<List<TaskResponse>>.Failure("No tasks found.");
            return Result<List<TaskResponse>>.Success(result.Adapt<List<TaskResponse>>());
        }
    }
}
