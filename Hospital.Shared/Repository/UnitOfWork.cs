using Microsoft.EntityFrameworkCore;

namespace Hospital.Shared.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly DbContext dbContext;

        public UnitOfWork(DbContext context)
        {
            dbContext = context;
        }
        public virtual void Save()
        {
            dbContext.SaveChanges();
        }

        public virtual async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public bool IsDisposed { get; protected set; }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
            {
                return;
            }

            if (disposing)
            {

                if (dbContext != null)
                {
                    dbContext.Dispose();
                }
            }

            IsDisposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }


    }
}
