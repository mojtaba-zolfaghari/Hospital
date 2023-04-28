using Hospital.Application.Interfaces.Auth;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using Action = Hospital.Domain.AuthEntity.Route;

namespace Hospital.Application.Implementation.Auth
{
    public class RouteRepository : Repository<Action>, IRouteRepository
    {
        public RouteRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
