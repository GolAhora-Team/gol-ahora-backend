using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // 🔹 PERSONAS (herencia)
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Profesor> Profesores { get; set; }
    public DbSet<CertificadoProfesor> CertificadosProfesores { get; set; }
    public DbSet<Administrador> Administradores { get; set; }

    // 🔹 DEPORTIVO
    public DbSet<Jugador> Jugadores { get; set; }
    public DbSet<Sancion> Sanciones { get; set; }

    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<EquipoJugador> EquipoJugadores { get; set; }
    public DbSet<EquipoFormacion> EquipoFormaciones { get; set; }
    public DbSet<JugadorFormacion> JugadorFormaciones { get; set; }

    public DbSet<Competicion> Competiciones { get; set; }
    public DbSet<Partido> Partidos { get; set; }
    public DbSet<Cambio> Cambios { get; set; }

    // 🔹 CLASES / ENTRENAMIENTO
    public DbSet<Clase> Clases { get; set; }
    public DbSet<Asistencia> Asistencias { get; set; }
    public DbSet<AsistenciaEntrenamiento> AsistenciaEntrenamientos { get; set; }
    public DbSet<Entrenamiento> Entrenamientos { get; set; }
    public DbSet<ClienteEntrenamiento> ClienteEntrenamientos { get; set; }

    // 🔹 CANCHAS / RESERVAS
    public DbSet<Cancha> Canchas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }

    public DbSet<Precio> Precios { get; set; }
    public DbSet<Descuento> Descuentos { get; set; }

    // 🔹 FACTURACIÓN
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    public DbSet<Recibo> Recibos { get; set; }

    // 🔹 REPORTES
    public DbSet<Reporte> Reportes { get; set; }

    // 🔹 NOTIFICACIONES
    public DbSet<Notificacion> Notificaciones { get; set; }

    // 🔹 CONFIGURACIONES
    public DbSet<ConfiguracionCancelaciones> ConfiguracionCancelaciones { get; set; }

    // 🔥 CONFIGURACIONES
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 👇 esto es clave (mucho mejor que registrar uno por uno)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // 🔥 HERENCIA (TPH por defecto)
        modelBuilder.Entity<Persona>()
            .HasDiscriminator<string>("TipoPersona")
            .HasValue<Cliente>("Cliente")
            .HasValue<Profesor>("Profesor")
            .HasValue<Administrador>("Administrador");

        // 🔥 SEED DATA (Usuarios por defecto)
        var fechaBase = new DateTime(2024, 1, 1);

        modelBuilder.Entity<Administrador>().HasData(
            new Administrador { Id = 1, Dni = 11111111, Nombre = "Admin", Apellido = "Principal", Email = "admin@golahora.com", Genero = "Masculino", FechaNacimiento = new DateTime(1980, 1, 1), Telefono = "1100000001", Direccion = "Calle Falsa 123", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", ObraSocial = "Ninguna", FechaRegistro = fechaBase, Identificador = 100, FechaAlta = fechaBase, PuedeFacturar = true },
            new Administrador { Id = 2, Dni = 22222222, Nombre = "Personal", Apellido = "Staff", Email = "personal@golahora.com", Genero = "Femenino", FechaNacimiento = new DateTime(1990, 1, 1), Telefono = "1100000002", Direccion = "Avenida Siempreviva 742", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", ObraSocial = "Ninguna", FechaRegistro = fechaBase, Identificador = 101, FechaAlta = fechaBase, PuedeFacturar = false }
        );

        modelBuilder.Entity<Cliente>().HasData(
            new Cliente { Id = 3, Dni = 33333333, Nombre = "Juan", Apellido = "Cliente", Email = "cliente@golahora.com", Genero = "Masculino", FechaNacimiento = new DateTime(1995, 5, 10), Telefono = "1100000003", Direccion = "San Martin 456", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", FechaRegistro = fechaBase, EsSocioActivo = true, ObraSocial = "OSDE", AptoFisico = true, FechaAlta = fechaBase }
        );

        modelBuilder.Entity<Profesor>().HasData(
            new Profesor { Id = 4, Dni = 44444444, Nombre = "Carlos", Apellido = "Profe", Email = "profe@golahora.com", Genero = "Masculino", FechaNacimiento = new DateTime(1985, 8, 20), Telefono = "1100000004", Direccion = "Belgrano 789", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", ObraSocial = "Ninguna", FechaRegistro = fechaBase, Certificacion = "AFA Nivel 2", Especialidad = "Fútbol 11 y Preparación Física" }
        );

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario { Id = 1, PersonaId = 1, Username = "admin", Email = "admin@golahora.com", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Administrador },
            new Usuario { Id = 2, PersonaId = 2, Username = "personal", Email = "personal@golahora.com", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Administrador },
            new Usuario { Id = 3, PersonaId = 3, Username = "cliente", Email = "cliente@golahora.com", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Cliente },
            new Usuario { Id = 4, PersonaId = 4, Username = "profe", Email = "profe@golahora.com", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Profesor }
        );

        // 🔥 SEED: Configuración de cancelaciones por defecto
        modelBuilder.Entity<ConfiguracionCancelaciones>().HasData(
            new ConfiguracionCancelaciones { Id = 1, HorasAntelacionMinima = 24, PorcentajePenalizacion = 50 }
        );

        // 🔗 Relación opcional Reserva -> Factura
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Factura)
            .WithMany()
            .HasForeignKey(r => r.FacturaId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    public override int SaveChanges()
    {
        CapitalizeEntityStrings();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CapitalizeEntityStrings();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void CapitalizeEntityStrings()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            // Capitalizar 'Nombre' si existe y es de tipo string
            var nombreProp = entry.Metadata.FindProperty("Nombre");
            if (nombreProp != null && nombreProp.ClrType == typeof(string))
            {
                var currentVal = entry.Property("Nombre").CurrentValue as string;
                if (!string.IsNullOrWhiteSpace(currentVal))
                {
                    entry.Property("Nombre").CurrentValue = CapitalizeWords(currentVal);
                }
            }

            // Capitalizar 'Apellido' si existe y es de tipo string
            var apellidoProp = entry.Metadata.FindProperty("Apellido");
            if (apellidoProp != null && apellidoProp.ClrType == typeof(string))
            {
                var currentVal = entry.Property("Apellido").CurrentValue as string;
                if (!string.IsNullOrWhiteSpace(currentVal))
                {
                    entry.Property("Apellido").CurrentValue = CapitalizeWords(currentVal);
                }
            }
        }
    }

    private string CapitalizeWords(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        
        var words = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
        {
            var word = words[i];
            if (word.Length > 0)
            {
                words[i] = char.ToUpper(word[0]) + word.Substring(1).ToLower();
            }
        }
        return string.Join(" ", words);
    }
}