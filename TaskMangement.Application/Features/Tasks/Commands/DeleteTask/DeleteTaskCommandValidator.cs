using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TaskMangement.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandValidator:AbstractValidator<DeleteTaskCommand>
    {
        public DeleteTaskCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Task Id is required.");
        }
    }
}
