
using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Infrastructure.Workers;

namespace TaskMangement.Infrastructure.Queue;
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Guid> _queue;
    private readonly ILogger<BackgroundTaskQueue> _logger;

    public BackgroundTaskQueue(ILogger<BackgroundTaskQueue> logger)
    {
        _queue = Channel.CreateUnbounded<Guid>();
        _logger = logger;
    }

    public async ValueTask QueueTaskAsync(Guid taskId)
    {
        await _queue.Writer.WriteAsync(taskId);
        _logger.LogInformation("Task {TaskId} has been queued for processing.", taskId);
    }
    public async ValueTask<Guid> DequeueTaskAsync(CancellationToken cancellationToken)
    {
        var taskId = await _queue.Reader.ReadAsync(cancellationToken);
        _logger.LogInformation("Task {TaskId} has been dequeued for processing.", taskId);
        return taskId;
    }
}

