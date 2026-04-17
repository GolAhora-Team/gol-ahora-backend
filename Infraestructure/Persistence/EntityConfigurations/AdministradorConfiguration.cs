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

    public class AdministradorConfiguration : IEntityTypeConfiguration<Administrador>
    {
        public void Configure(EntityTypeBuilder<Administrador> entity)
        {
            entity.Property(a => a.Identificador)
                .IsRequired();

            entity.Property(a => a.FechaAlta)
                .IsRequired();

            entity.Property(a => a.PuedeFacturar)
                .IsRequired();
        }
    }
}
