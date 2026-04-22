using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Persistence.EntityConfigurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProfesorConfiguration : IEntityTypeConfiguration<Profesor>
    {
        public void Configure(EntityTypeBuilder<Profesor> entity)
        {
            entity.Property(p => p.Certificacion)
                .HasMaxLength(150);

            entity.Property(p => p.Especialidad)
                .HasMaxLength(100);

            // Relaciones

            //entity.HasMany(p => p.Clases)
            //    .WithOne(c => c.Profesor)
            //    .HasForeignKey(c => c.ProfesorId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //entity.HasMany(p => p.Entrenamientos)
            //    .WithOne(e => e.Profesor)
            //    .HasForeignKey(e => e.ProfesorId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
