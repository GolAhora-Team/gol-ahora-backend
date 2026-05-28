using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<Administrador> Administradores { get; set; }

    // 🔹 DEPORTIVO
    public DbSet<Jugador> Jugadores { get; set; }
    public DbSet<Sancion> Sanciones { get; set; }

    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<EquipoJugador> EquipoJugadores { get; set; }

    public DbSet<Competicion> Competiciones { get; set; }
    public DbSet<Partido> Partidos { get; set; }
    public DbSet<Cambio> Cambios { get; set; }

    // 🔹 CLASES / ENTRENAMIENTO
    public DbSet<Clase> Clases { get; set; }
    public DbSet<Asistencia> Asistencias { get; set; }
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
            new Administrador { Id = 1, Dni = 11111111, Nombre = "Admin", Apellido = "Principal", Email = "admin@golahora.com", Genero = "Masculino", FechaNacimiento = new DateTime(1980, 1, 1), Telefono = "1100000001", Direccion = "Calle Falsa 123", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", FechaRegistro = fechaBase, Identificador = 100, FechaAlta = fechaBase, PuedeFacturar = true },
            new Administrador { Id = 2, Dni = 22222222, Nombre = "Personal", Apellido = "Staff", Email = "personal@golahora.com", Genero = "Femenino", FechaNacimiento = new DateTime(1990, 1, 1), Telefono = "1100000002", Direccion = "Avenida Siempreviva 742", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", FechaRegistro = fechaBase, Identificador = 101, FechaAlta = fechaBase, PuedeFacturar = false }
        );

        modelBuilder.Entity<Cliente>().HasData(
            new Cliente { Id = 3, Dni = 33333333, Nombre = "Juan", Apellido = "Cliente", Email = "cliente@golahora.com", Genero = "Masculino", FechaNacimiento = new DateTime(1995, 5, 10), Telefono = "1100000003", Direccion = "San Martin 456", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", FechaRegistro = fechaBase, EsSocioActivo = true, ObraSocial = "OSDE", AptoFisico = true, FechaAlta = fechaBase }
        );

        modelBuilder.Entity<Profesor>().HasData(
            new Profesor { Id = 4, Dni = 44444444, Nombre = "Carlos", Apellido = "Profe", Email = "profe@golahora.com", Genero = "Masculino", FechaNacimiento = new DateTime(1985, 8, 20), Telefono = "1100000004", Direccion = "Belgrano 789", Localidad = "CABA", Provincia = "Buenos Aires", Pais = "Argentina", CodigoPostal = "1000", ContactoEmergencia = "1100000000", FechaRegistro = fechaBase, Certificacion = "AFA Nivel 2", Especialidad = "Fútbol 11 y Preparación Física" }
        );

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario { Id = 1, PersonaId = 1, Email = "admin", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Administrador },
            new Usuario { Id = 2, PersonaId = 2, Email = "personal", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Administrador },
            new Usuario { Id = 3, PersonaId = 3, Email = "cliente", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Cliente },
            new Usuario { Id = 4, PersonaId = 4, Email = "profe", PasswordHash = "1234", TipoUsuario = Domain.Enums.TipoUsuario.Profesor }
        );
    }
}