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
    public class AsistenciaConfiguration : IEntityTypeConfiguration<Asistencia>
    {
        public void Configure(EntityTypeBuilder<Asistencia> entity)
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Presente)
                .IsRequired();

            entity.Property(a => a.Fecha)
                .IsRequired();

            entity.HasOne(a => a.Cliente)
                .WithMany()
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Clase)
                .WithMany(c => c.Asistencias)
                .HasForeignKey(a => a.ClaseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(a => new { a.ClienteId, a.ClaseId })
                .IsUnique();
        }
    }
}
