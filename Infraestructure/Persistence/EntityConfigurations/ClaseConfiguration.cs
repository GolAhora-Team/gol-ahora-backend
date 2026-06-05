using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Persistence.EntityConfigurations
{
    public class ClaseConfiguration : IEntityTypeConfiguration<Clase>
    {
        public void Configure(EntityTypeBuilder<Clase> entity)
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Descripcion)
                .HasMaxLength(250);

            entity.Property(c => c.PrecioInscripcion)
                .HasColumnType("decimal(10,2)");

            entity.HasOne(c => c.Profesor)
                .WithMany(p => p.Clases)
                .HasForeignKey(c => c.ProfesorId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasMany(c => c.Asistencias)
                .WithOne(a => a.Clase)
                .HasForeignKey(a => a.ClaseId);
        }
    }
}
