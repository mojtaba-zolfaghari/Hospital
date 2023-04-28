using Hospital.Domain.AuthEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hospital.Infrastructure.Fluents.AuthFluents
{
    public class UserFluents : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(x => x.Id);
            builder.HasOne(c => c.UserDetail).WithOne(c => c.User).HasForeignKey<UserDetail>(c => c.UserId);
            //builder.HasOne(c => c.Language);
            //builder.HasMany(c => c.Vendors).WithOne(c => c.User).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
