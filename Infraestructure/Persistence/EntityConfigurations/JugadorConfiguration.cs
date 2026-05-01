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

            // 🔗 1 a 1 con Cliente
            entity.HasOne(j => j.Cliente)
                .WithOne(c => c.Jugador)
                .HasForeignKey<Jugador>(j => j.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
