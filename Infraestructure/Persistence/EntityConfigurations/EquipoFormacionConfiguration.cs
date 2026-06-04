using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.EntityConfigurations
{
    public class EquipoFormacionConfiguration : IEntityTypeConfiguration<EquipoFormacion>
    {
        public void Configure(EntityTypeBuilder<EquipoFormacion> entity)
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(ef => ef.Equipo)
                .WithMany(e => e.Formaciones)
                .HasForeignKey(ef => ef.EquipoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
