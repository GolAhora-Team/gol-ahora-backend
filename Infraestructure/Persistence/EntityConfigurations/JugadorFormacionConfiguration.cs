using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.EntityConfigurations
{
    public class JugadorFormacionConfiguration : IEntityTypeConfiguration<JugadorFormacion>
    {
        public void Configure(EntityTypeBuilder<JugadorFormacion> entity)
        {
            entity.HasKey(jf => new { jf.FormacionId, jf.JugadorId });

            entity.HasOne(jf => jf.Formacion)
                .WithMany(f => f.JugadoresPosiciones)
                .HasForeignKey(jf => jf.FormacionId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(jf => jf.Jugador)
                .WithMany(j => j.FormacionesPosiciones)
                .HasForeignKey(jf => jf.JugadorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
