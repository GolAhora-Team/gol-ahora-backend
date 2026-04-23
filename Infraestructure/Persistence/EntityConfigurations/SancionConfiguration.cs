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
    public class SancionConfiguration : IEntityTypeConfiguration<Sancion>
    {
        public void Configure(EntityTypeBuilder<Sancion> entity)
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Fecha)
                .IsRequired();

            entity.Property(s => s.Suspendido)
                .IsRequired();

            entity.Property(s => s.Tarjeta)
                .HasConversion<int>();

            entity.HasOne(s => s.Jugador)
                .WithMany(j => j.Sanciones)
                .HasForeignKey(s => s.JugadorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
