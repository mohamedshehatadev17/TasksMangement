
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskMangement.Domain.Models.Task;
namespace TaskMangement.Infrastructure.Persistance.Configurations;
public class TaskConfigurations : IEntityTypeConfiguration<Task>
{
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Status)
               .IsRequired();

        builder.Property(t => t.Priority)
               .IsRequired();

        builder.Property(t => t.DueDate)
               .IsRequired();

    }

}
