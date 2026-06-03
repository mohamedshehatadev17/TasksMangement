using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Application.Shared;

namespace TaskMangement.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler: IRequestHandler<DeleteTaskCommand, Result<bool>>
    {
        private readonly IGenericRepository<Domain.Models.Task> _repository;

        public DeleteTaskCommandHandler(
            IGenericRepository<Domain.Models.Task> repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(
            DeleteTaskCommand request,
            CancellationToken cancellationToken)
        {
            var task = await _repository.GetByIdAsync(request.Id);

            if (task is null)
                return Result<bool>.Failure("Task not found");

            task.MarkAsDeleted();
            _repository.Update(task);

            await _repository.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
    }
}
