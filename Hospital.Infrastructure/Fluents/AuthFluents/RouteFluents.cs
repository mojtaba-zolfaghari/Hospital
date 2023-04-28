using Hospital.Domain.AuthEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Fluents.AuthFluents
{
    public class RouteFluents : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.Property(c => c.ControllerName).HasMaxLength(100);
            builder.Property(c => c.ActionName).HasMaxLength(100);
        }
    }
}
