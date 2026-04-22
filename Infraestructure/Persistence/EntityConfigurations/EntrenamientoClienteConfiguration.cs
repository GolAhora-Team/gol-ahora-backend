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
    public class ClienteEntrenamientoConfiguration : IEntityTypeConfiguration<ClienteEntrenamiento>
    {
        public void Configure(EntityTypeBuilder<ClienteEntrenamiento> entity)
        {
            entity.HasKey(ce => new { ce.ClienteId, ce.EntrenamientoId });

            entity.HasOne(ce => ce.Cliente)
                .WithMany()
                .HasForeignKey(ce => ce.ClienteId);

            entity.HasOne(ce => ce.Entrenamiento)
                .WithMany(e => e.Clientes)
                .HasForeignKey(ce => ce.EntrenamientoId);
        }
    }
}
