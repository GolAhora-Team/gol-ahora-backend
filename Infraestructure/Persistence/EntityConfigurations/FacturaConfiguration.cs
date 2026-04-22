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
    public class FacturaConfiguration : IEntityTypeConfiguration<Factura>
    {
        public void Configure(EntityTypeBuilder<Factura> entity)
        {
            entity.HasKey(f => f.Id);

            entity.Property(f => f.Total)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            entity.Property(f => f.FechaEmision)
                .IsRequired()
                .ValueGeneratedOnAdd();
        }
    }
}
