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
    public class EquipoJugadorConfiguration : IEntityTypeConfiguration<EquipoJugador>
    {
        public void Configure(EntityTypeBuilder<EquipoJugador> entity)
        {
            entity.HasKey(ej => new { ej.EquipoId, ej.JugadorId });

            entity.HasOne(ej => ej.Equipo)
                .WithMany(e => e.Jugadores)
                .HasForeignKey(ej => ej.EquipoId);

            entity.HasOne(ej => ej.Jugador)
                .WithMany()
                .HasForeignKey(ej => ej.JugadorId);
        }
    }
}
