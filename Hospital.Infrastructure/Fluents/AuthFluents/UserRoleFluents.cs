using Hospital.Domain.AuthEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Fluents.AuthFluents
{
    public class UserRoleFluents : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasOne(c => c.User).WithMany(c => c.UserRoles);
            builder.HasOne(c => c.Role).WithMany(c => c.UserRoles);
        }
    }
}
