
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace TaskMangement.Application.Abstractions.Contracts.Persistance
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        bool Update(T entity);
        bool IsExist(Guid id);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}