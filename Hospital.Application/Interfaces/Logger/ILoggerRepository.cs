using Hospital.Domain.LoggerEntity;
using Hospital.Shared.Repository;

namespace Hospital.Application.Interfaces.Logger
{
    public interface ILoggerRepository : IRepository<Log>
    {
    }
}
