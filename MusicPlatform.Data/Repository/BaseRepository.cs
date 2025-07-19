namespace MusicPlatform.Data.Repository
{
    using Microsoft.EntityFrameworkCore;

    using MusicPlatform.Data.Repository.Interfaces;

    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading.Tasks;

    using static MusicPlatform.GCommon.ApplicationConstants;
    using static MusicPlatform.GCommon.ExceptionMessages;

    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class
    {
        protected readonly MusicPlatformDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        protected BaseRepository(MusicPlatformDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await this.DbSet.AddAsync(entity);
            await this.DbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await this.DbSet.AddRangeAsync(entities);
            await this.DbContext.SaveChangesAsync();
        }

        public Task<int> CountAsync()
        {
            return this.DbSet.CountAsync();
        }

        public Task<bool> DeleteAsync(TEntity entity)
        {
            this.PerformSoftDelete(entity);
            return this.UpdateAsync(entity);
        }

        public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.DbSet.ToArrayAsync();
        }

        public IQueryable<TEntity> GetAllAsQueryable()
        {
            return this.DbSet.AsQueryable();
        }

        public ValueTask<TEntity?> GetByIdAsync(TKey id)
        {
            return this.DbSet.FindAsync(id);
        }

        public async Task<bool> HardDeleteAsync(TEntity entity)
        {
            this.DbSet.Remove(entity);
            int updatedCount = await this.DbContext.SaveChangesAsync();
            return updatedCount > 0;
        }

        public async Task SaveChangesAsync()
        {
            await this.DbContext.SaveChangesAsync();
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.SingleOrDefaultAsync(predicate);
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                this.DbSet.Attach(entity);
                this.DbContext.Entry(entity).State = EntityState.Modified;
                await this.DbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void PerformSoftDelete(TEntity entity)
        {
            // TODO: We might want to introduce a GetIsDeletedProperty private method.
            PropertyInfo? isDeletedProperty = typeof(TEntity).GetProperty(IsDeletedPropertyName);
            if (isDeletedProperty == null || isDeletedProperty.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException(string.Format(SoftDeleteOnNonSoftDeletableEntity, typeof(TEntity).Name));
            }
            isDeletedProperty.SetValue(entity, true);

            PropertyInfo? deletedOnProperty = typeof(TEntity).GetProperty("DeletedOn");
            if (deletedOnProperty != null && deletedOnProperty.PropertyType == typeof(DateTime?))
            {
                deletedOnProperty.SetValue(entity, DateTime.UtcNow);
            }
        }
    }
}
