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

    public class CambioConfiguration : IEntityTypeConfiguration<Cambio>
    {
        public void Configure(EntityTypeBuilder<Cambio> entity)
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Hora)
                .IsRequired();

            entity.HasOne(c => c.JugadorSale)
                .WithMany()
                .HasForeignKey(c => c.JugadorSaleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.JugadorEntra)
                .WithMany()
                .HasForeignKey(c => c.JugadorEntraId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Partido)
                .WithMany(p => p.Cambios)
                .HasForeignKey(c => c.PartidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
