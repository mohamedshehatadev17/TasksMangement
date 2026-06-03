using System;
using MediatR;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Tasks.Commands.UpateTask
{
    public class UpdateTaskStatusCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
        public TaskStatus Status { get; set; }
    }
}
