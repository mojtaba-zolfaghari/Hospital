using Hospital.Domain.QueueEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Infrastructure.Fluents
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .HasMaxLength(100)
                .IsRequired();    
            
            builder.Property(p => p.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Birthdate)
                .IsRequired();

            builder.Property(p => p.NationalCode)
                .HasMaxLength(150)
                .IsRequired();  
            
            builder.Property(p => p.Gender)
                .IsRequired();

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(11)
                .IsRequired();

            builder.HasMany(p => p.Appointments)
                .WithOne(a => a.Patient)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
