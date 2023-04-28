namespace Hospital.Shared.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        //IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        bool IsDisposed { get; }
        void Save();
        Task SaveAsync();
    }
}
