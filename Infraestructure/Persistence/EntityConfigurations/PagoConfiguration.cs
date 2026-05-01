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
    public class PagoConfiguration : IEntityTypeConfiguration<Pago>
    {
        public void Configure(EntityTypeBuilder<Pago> entity)
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Monto)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            entity.Property(p => p.Metodo)
                .HasConversion<int>();

            entity.Property(p => p.Estado)
                .HasConversion<int>();

            entity.HasOne(p => p.Factura)
                .WithMany(f => f.Pagos)
                .HasForeignKey(p => p.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
