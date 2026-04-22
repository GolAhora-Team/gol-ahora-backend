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
    public class CanchaConfiguration : IEntityTypeConfiguration<Cancha>
    {
        public void Configure(EntityTypeBuilder<Cancha> entity)
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.PrecioPorHora)
                .HasColumnType("decimal(10,2)");

            // Guardar enums como int
            entity.Property(c => c.Tipo)
                .HasConversion<int>();

            entity.Property(c => c.Estado)
                .HasConversion<int>();
        }
    }
}
