namespace MusicPlatform.Data.Repository.Interfaces
{
    using System.Linq.Expressions;

    public interface IBaseRepository<TEntity, TKey> where TEntity : class
    {
        ValueTask<TEntity?> GetByIdAsync(TKey id);

        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync();

        IQueryable<TEntity> GetAllAsQueryable();

        Task<int> CountAsync();

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> DeleteAsync(TEntity entity);

        Task<bool> HardDeleteAsync(TEntity entity);

        Task<bool> UpdateAsync(TEntity entity);

        Task SaveChangesAsync();
    }
}
