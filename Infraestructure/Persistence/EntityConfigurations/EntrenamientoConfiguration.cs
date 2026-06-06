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
    public class EntrenamientoConfiguration : IEntityTypeConfiguration<Entrenamiento>
    {
        public void Configure(EntityTypeBuilder<Entrenamiento> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Profesor)
                .WithMany(p => p.Entrenamientos)
                .HasForeignKey(e => e.ProfesorId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Cancha)
                .WithMany()
                .HasForeignKey(e => e.CanchaId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(e => e.Fecha)
                .IsRequired();

            entity.Property(e => e.PrecioInscripcion)
                .HasColumnType("decimal(18,2)")
                .IsRequired()
                .HasDefaultValue(5000m);
        }
    }
}
