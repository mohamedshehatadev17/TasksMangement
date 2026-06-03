using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TaskMangement.Application.Features.Tasks.Commands.UpateTask
{
    internal class UpdateTaskStatusCommandValidator : AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Task Id is required.");
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid task status.");
        }
    }
}
