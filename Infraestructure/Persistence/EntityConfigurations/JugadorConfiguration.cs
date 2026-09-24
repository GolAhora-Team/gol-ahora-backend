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
    public class JugadorConfiguration : IEntityTypeConfiguration<Jugador>
    {
        public void Configure(EntityTypeBuilder<Jugador> entity)
        {
            entity.HasKey(j => j.Id);

            entity.Property(j => j.Estado)
                .HasConversion<int>();

            entity.HasOne(j => j.Cliente)
                .WithOne(c => c.Jugador)
                .HasForeignKey<Jugador>(j => j.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(j => j.Equipo)
                .WithMany()
                .HasForeignKey(j => j.EquipoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
