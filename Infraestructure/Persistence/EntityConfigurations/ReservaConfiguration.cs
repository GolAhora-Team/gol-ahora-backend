using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.EntityConfigurations
{
    public class ReservaConfiguration : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> entity)
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Estado)
                .HasConversion<int>();

            entity.HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Cancha)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.CanchaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
