using Hospital.Domain.QueueEntity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Fluents
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.ToTable("Appointments");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasDefaultValueSql("NEWID()");

            builder.Property(q => q.CreationDate).ValueGeneratedOnAdd();
            builder.Property(q => q.ModifiedDate).ValueGeneratedOnUpdate();

            builder.Property(a => a.StartTime).IsRequired();

            builder.Property(a => a.IsConfirmed).IsRequired();
            builder.Property(a => a.Status).IsRequired();

            builder.Property(a => a.PatientId).IsRequired();
            builder.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

        
        }
    }
}
