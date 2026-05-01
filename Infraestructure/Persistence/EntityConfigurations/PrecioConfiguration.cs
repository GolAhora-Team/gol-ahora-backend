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
    public class PrecioConfiguration : IEntityTypeConfiguration<Precio>
    {
        public void Configure(EntityTypeBuilder<Precio> entity)
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Monto)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            entity.Property(p => p.FechaVigenciaDesde)
                .IsRequired();

            entity.HasOne(p => p.Cancha)
                .WithMany(c => c.Precios)
                .HasForeignKey(p => p.CanchaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
