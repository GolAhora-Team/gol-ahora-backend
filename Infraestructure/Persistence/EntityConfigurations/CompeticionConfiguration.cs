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

    public class CompeticionConfiguration : IEntityTypeConfiguration<Competicion>
    {
        public void Configure(EntityTypeBuilder<Competicion> entity)
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Descripcion)
                .HasMaxLength(250);

            entity.Property(c => c.Tipo)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(c => c.CantidadEquipos)
                .IsRequired();

            entity.HasMany(c => c.Equipos)
                .WithOne(e => e.Competicion)
                .HasForeignKey(e => e.CompeticionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.Partidos)
                .WithOne(p => p.Competicion)
                .HasForeignKey(p => p.CompeticionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
