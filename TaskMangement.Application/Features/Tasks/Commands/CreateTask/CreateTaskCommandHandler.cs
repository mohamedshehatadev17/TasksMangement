using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mapster;
using MapsterMapper;
using MediatR;
using TaskMangement.Application.Abstractions.Contracts;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Application.DTOs;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<Guid>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IBackgroundTaskQueue _queue;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper, ICurrentUser currentUser, IBackgroundTaskQueue queue)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            _currentUser = currentUser;
            _queue = queue;
        }
        public async Task<Result<Guid>> Handle(CreateTaskCommand request,CancellationToken cancellationToken)
        {
            var validationResult = new CreateTaskCommandValidator().Validate(request);
            if (!validationResult.IsValid)
                return Result<Guid>.Failure(validationResult.Errors.First().ErrorMessage);

            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);
            var userId = _currentUser.UserId;
            var existingTask = await _taskRepository.GetByIdAsync(userId,
                x => x.Title == request.Title
                     && x.CreatedAt >= today
                     && x.CreatedAt < tomorrow,
                cancellationToken);

            if (existingTask is not null)
                return Result<Guid>.Failure("A task with the same title already exists for today.");
            Domain.Models.Task task = request.Adapt<Domain.Models.Task>();
            task.UserId = userId;
            await _taskRepository.AddAsync(task);
            await _queue.QueueTaskAsync(task.Id);
            await _taskRepository.SaveChangesAsync();

            return Result<Guid>.Success(task.Id);
        }
    }
}