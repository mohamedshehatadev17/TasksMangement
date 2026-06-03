using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskMangement.Application.Abstractions.Contracts;
using TaskMangement.Application.Abstractions.Contracts.Persistance;

namespace TaskMangement.Infrastructure.Workers
{
    public class TaskProcessingService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundTaskQueue _queue;
        private readonly ILogger<TaskProcessingService> _logger;
        public TaskProcessingService(IServiceScopeFactory scopeFactory,IBackgroundTaskQueue queue, ILogger<TaskProcessingService> logger)
        {
            _scopeFactory = scopeFactory;
            _queue = queue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var taskId = await _queue.DequeueTaskAsync(stoppingToken);

                await ProcessTask(taskId, stoppingToken);
            }
        }
        private async Task ProcessTask(Guid taskId,CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var repository =scope.ServiceProvider.GetRequiredService<ITaskRepository>();
            var task = await repository.GetByIdAsync(taskId,cancellationToken: cancellationToken);

            if (task is null)
                return;

            if (task.Status == Domain.Enums.TaskStatus.Cancelled)
                return;

            task.Status = Domain.Enums.TaskStatus.InProgress;
            await repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("InProgress Task {TaskId}",taskId);

            await Task.Delay(TimeSpan.FromSeconds(5),cancellationToken);

            task.Status = Domain.Enums.TaskStatus.Completed;
            await repository.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Completed Task {TaskId}", taskId);
        }
    }
}
