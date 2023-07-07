using Hospital.Shared.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Hospital.Shared.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public DbContext Context { get; }

        public DbSet<T> DbSet { get; }

        public Repository(DbContext context) : base()
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null, int? take = null, params Expression<Func<T, object>>[] navigationProperties)
        {
            return GetQueryable(null, orderBy, skip, take, navigationProperties).AsParallel().ToList();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            return GetQueryable(filter, orderBy, skip, take, navigationProperties).ToList();
        }
        public virtual T GetOne(Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            return GetQueryable(filter, null, null, null, navigationProperties).SingleOrDefault();
        }

        public virtual T GetFirst(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            return GetQueryable(filter, orderBy, null, null, navigationProperties).FirstOrDefault();
        }

        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual long GetCount(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        public virtual long GetSum(Expression<Func<T, long>> selector, Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Sum(selector);
        }

        public virtual long GetMax(Expression<Func<T, long>> selector, Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Max(selector);
        }

        public virtual long GetMin(Expression<Func<T, long>> selector, Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Min(selector);
        }

        public virtual bool GetExists(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Any();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            return await GetQueryable(null, orderBy, skip, take, navigationProperties)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            return await GetQueryable(filter, orderBy, skip, take, navigationProperties).ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            return await GetQueryable(filter, null, null, null, navigationProperties).SingleOrDefaultAsync();
        }

        public virtual async Task<T> GetFirstAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            return await GetQueryable(filter, orderBy, null, null, navigationProperties).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetByIdAsync(object id)
        {
            return DbSet.FindAsync(id).AsTask();
        }

        public virtual Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).CountAsync();
        }

        public virtual Task<int> GetSumAsync(Expression<Func<T, int>> selector, Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).SumAsync(selector);
        }

        public virtual Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).AnyAsync();
        }

        public virtual void Create(T entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            entity.CreationDate = DateTime.UtcNow;
            DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            //Context.Set<T>().Attach(entity);
            //Context.Entry(entity).State = EntityState.Modified;
            //entity.CreationDate = DateTime.UtcNow;
            entity.ModifiedDate = DateTime.UtcNow;
            DbSet.Update(entity);

        }

        public virtual void Delete(object id)
        {
            T entity = DbSet.Find(id);
            Delete(entity);
        }

        public virtual void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        private IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> query = Context.Set<T>();


            if (filter != null)
            {
                query = query.Where(filter);
            }

            query = navigationProperties
                .Aggregate(query, (current, navigationProperty) => current.Include(navigationProperty));

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.AsQueryable();
            //return query.AsNoTracking();
        }
    }
}
