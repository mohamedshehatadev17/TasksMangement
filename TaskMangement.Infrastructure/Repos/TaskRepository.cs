using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskMangement.Application.Abstractions.Contracts;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Infrastructure.Persistance.Contexts;

namespace TaskMangement.Infrastructure.Repos
{
    public class TaskRepository : GenericRepository<Domain.Models.Task>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async override Task<IEnumerable<Domain.Models.Task>> GetAllAsync(
            int page,
            int pageSize,
            Expression<Func<Domain.Models.Task, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(predicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .OrderByDescending(x=>x.Priority)
                    .ThenByDescending(x=>x.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
