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
using TaskMangemnet.Shared.Extensions;

namespace TaskMangement.Application.Features.Tasks.Queries.GetTaskById
{
    public sealed class GetTaskByIdQueryHandler: IRequestHandler<GetTaskByIdQuery, Result<TaskResponse>>
    {
        private readonly ITaskRepository _repository;
        private readonly ICurrentUser _currentUser;
        private readonly ICacheService _cacheService;
        public GetTaskByIdQueryHandler(ITaskRepository repository, ICurrentUser currentUser, ICacheService cacheService)
        {
            _repository = repository;
            _currentUser = currentUser;
            _cacheService = cacheService;
        }

        public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery request,CancellationToken cancellationToken)
        {
            var cacheKey = CacheKeys.GenerateCacheKeyForTask(request.Id);

            var cached = await _cacheService.GetAsync<TaskResponse>(cacheKey);

            if (cached is not null)
            {
                return Result<TaskResponse>.Success(cached);
            }

            var task = await _repository.GetByIdAsync(request.Id,x => !x.IsDeleted,cancellationToken);

            if (task is null)
                return Result<TaskResponse>.Failure("Task not found");

            var response = task.Adapt<TaskResponse>();

            await _cacheService.SetAsync(
                cacheKey,
                response,
                TimeSpan.FromMinutes(10));

            return Result<TaskResponse>.Success(response);
        }
    }
}
