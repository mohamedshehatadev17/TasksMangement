using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.Shared;
using TaskMangement.Domain.Enums;

namespace TaskMangement.Application.Features.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<Result<Guid>>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
