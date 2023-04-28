using Hospital.Shared.Shared;
using System.Linq.Expressions;

namespace Hospital.Shared.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll(
                       Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                       int? skip = null,
                       int? take = null,
                       params Expression<Func<T, object>>[] navigationProperties);

        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] navigationProperties);

        T GetOne(
            Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] navigationProperties);

        T GetFirst(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] navigationProperties);

        T GetById(object id);

        Task<IEnumerable<T>> GetAllAsync(
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] navigationProperties);

        long GetCount(Expression<Func<T, bool>> filter = null);
        long GetSum(Expression<Func<T, long>> selector, Expression<Func<T, bool>> filter = null);
        long GetMax(Expression<Func<T, long>> selector, Expression<Func<T, bool>> filter = null);
        long GetMin(Expression<Func<T, long>> selector, Expression<Func<T, bool>> filter = null);
        bool GetExists(Expression<Func<T, bool>> filter = null);

        Task<IEnumerable<T>> GetAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<T, object>>[] navigationProperties);

        Task<T> GetOneAsync(
            Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] navigationProperties);

        Task<T> GetFirstAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] navigationProperties);

        Task<T> GetByIdAsync(object id);

        Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null);
        Task<int> GetSumAsync(Expression<Func<T, int>> selector, Expression<Func<T, bool>> filter = null);

        Task<bool> GetExistsAsync(Expression<Func<T, bool>> filter = null);
        void Create(T entity);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entity);

    }
}
