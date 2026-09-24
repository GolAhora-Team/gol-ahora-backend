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

    public class DescuentoConfiguration : IEntityTypeConfiguration<Descuento>
    {
        public void Configure(EntityTypeBuilder<Descuento> entity)
        {
            entity.HasKey(d => d.Id);

            entity.Property(d => d.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(d => d.Descripcion)
                .HasMaxLength(250);

            entity.Property(d => d.Porcentaje)
                .HasColumnType("decimal(5,2)")
                .IsRequired();

            entity.Property(d => d.FechaInicio)
                .IsRequired();

            entity.Property(d => d.FechaFin)
                .IsRequired();

            entity.HasMany(d => d.Precios)
                .WithOne(p => p.Descuento)
                .HasForeignKey(p => p.DescuentoId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
