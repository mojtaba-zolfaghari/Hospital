using Hospital.Domain.DoctorEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Infrastructure.Fluents.DoctorFluents
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .ValueGeneratedOnAdd()
                .HasConversion<Guid>();

            builder.Property(s => s.CreationDate)
                .ValueGeneratedOnAdd()
                .HasConversion<DateTime>();

            builder.Property(s => s.ModifiedDate)
                .ValueGeneratedOnUpdate()
                .HasConversion<DateTime>();

            builder.Property(d => d.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(d => d.Appointments)
                .WithOne(a => a.Doctor)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
