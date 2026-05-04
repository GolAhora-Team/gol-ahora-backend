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
    }
}