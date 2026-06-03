
using System.ComponentModel.DataAnnotations.Schema;
using TaskMangement.Domain.Enums;
using TaskStatus = TaskMangement.Domain.Enums.TaskStatus;

namespace TaskMangement.Domain.Models;
public class Task : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
