using Hospital.Domain.AuthEntity;
using Hospital.Domain.DoctorEntity;
using Hospital.Domain.LoggerEntity;
using Hospital.Domain.QueueEntity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Hospital.Infrastructure.Contexts
{
    public class QueueDbContext : DbContext
    {
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        #region Auth
        public DbSet<User> Users { get; set; }
        public DbSet<ApiUser> ApiUsers { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RoleRoute> RoleRouteAccess { get; set; }
        #endregion

        #region Log
        public DbSet<Log> Logs { get; set; }
        #endregion
        public QueueDbContext(DbContextOptions<QueueDbContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            base.OnModelCreating(modelBuilder);

            // Additional configurations for queue system could be added here.
            // For example, if you need to implement a queue system for the appointments,
            // you may need to create a Queue entity and define the relationships with
            // appointments and other entities in the system.
        }

    }
}
