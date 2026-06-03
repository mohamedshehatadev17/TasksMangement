using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskMangement.Application.Abstractions.Contracts.Persistance;
using TaskMangement.Infrastructure.Persistance.Contexts;

namespace TaskMangement.Infrastructure.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id,Expression<Func<T, bool>>? predicate = null,CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            int page,
            int pageSize,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(predicate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public bool Update(T entity)
        {
            _dbSet.Update(entity);

            return true;
        }

        public bool IsExist(Guid id)
        {
            var entity = _dbSet.Find(id); // no CancellationToken here
            return entity != null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
