using Hospital.Application.Interfaces.Logger;
using Hospital.Domain.LoggerEntity;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Implementation.Logger
{
    public class LoggerRepository : Repository<Log>, ILoggerRepository
    {
        public LoggerRepository(DbContext context) : base(context)
        {
        }
    }
}
