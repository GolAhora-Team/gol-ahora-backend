using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructure.Persistence.EntityConfigurations
{
    public class PersonaConfiguration : IEntityTypeConfiguration<Persona>
    {
        public void Configure(EntityTypeBuilder<Persona> entity)
        {
            entity.HasKey(p => p.Id);

            entity.HasDiscriminator<string>("TipoPersona")
                .HasValue<Cliente>("Cliente")
                .HasValue<Profesor>("Profesor")
                .HasValue<Administrador>("Administrador");

            entity.Property(p => p.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Apellido)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Dni)
                .IsRequired();

            entity.Property(p => p.Telefono)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(p => p.Direccion)
                .HasMaxLength(200);

            entity.Property(p => p.Localidad)
                .HasMaxLength(100);

            entity.Property(p => p.Provincia)
                .HasMaxLength(100);

            entity.Property(p => p.Pais)
                .HasMaxLength(100);

            entity.Property(p => p.CodigoPostal)
                .HasMaxLength(20);

            entity.Property(p => p.ContactoEmergencia)
                .HasMaxLength(150);

            entity.Property(p => p.FechaNacimiento)
                .IsRequired();

            entity.Property(p => p.FechaRegistro)
                .HasDefaultValueSql("GETDATE()");

            entity.HasIndex(p => p.Dni)
                .IsUnique();
        }
    }
}
