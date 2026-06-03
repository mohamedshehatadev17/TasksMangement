using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}
