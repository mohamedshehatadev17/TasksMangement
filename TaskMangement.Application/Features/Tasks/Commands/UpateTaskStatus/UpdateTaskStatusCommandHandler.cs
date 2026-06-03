using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.Abstractions.Contracts;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Application.Shared;
using TaskMangemnet.Shared.Extensions;
using TaskStatus = TaskMangement.Domain.Enums.TaskStatus;

namespace TaskMangement.Application.Features.Tasks.Commands.UpateTask
{
    public class UpdateTaskStatusCommandHandler: IRequestHandler<UpdateTaskStatusCommand, Result<bool>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICacheService _cacheService;
        public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository, ICacheService cacheService)
        {
            _taskRepository = taskRepository;
            _cacheService = cacheService;
        }

        public async Task<Result<bool>> Handle(UpdateTaskStatusCommand request,CancellationToken cancellationToken)
        {
            var validationResult = await new UpdateTaskStatusCommandValidator().ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return Result<bool>.Failure(validationResult.Errors.First().ErrorMessage);
            Domain.Models.Task? task =  await _taskRepository.GetByIdAsync(request.Id);
            if (task is null)
                return Result<bool>.Failure("Task not found.");
            task.Status = (Domain.Enums.TaskStatus)request.Status;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            var cacheKey = CacheKeys.GenerateCacheKeyForTask(request.Id);
            await _cacheService.SetAsync(cacheKey, task, TimeSpan.FromHours(10));
            return Result<bool>.Success(true);
        }
    }
}
