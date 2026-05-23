using Aplication.Interfaces.IAdmin;
using Aplication.Interfaces.IAsistencia;
using Aplication.Interfaces.ICancha;
using Aplication.Interfaces.IClases;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IDescuento;
using Aplication.Interfaces.IEquipo;
using Aplication.Interfaces.IFactura;
using Aplication.Interfaces.IJugador;
using Aplication.Interfaces.IPago;
using Aplication.Interfaces.IPrecio;
using Aplication.Interfaces.IProfesor;
using Aplication.Interfaces.IReserva;
using Aplication.Interfaces.IUsuario;
using Aplication.Mappers;
using Aplication.UseCase;
using Aplication.UseCase.Clientes;
using Aplication.UseCase.Descuentos;
using Aplication.UseCase.Facturas;
using Aplication.UseCase.Pagos;
using Infraestructure.Command;
using Infraestructure.Querys;
using Microsoft.EntityFrameworkCore;
using Aplication.Interfaces.IEquipo;
using Aplication.Interfaces.ICompeticion;
using Aplication.Interfaces.IPartido;
using Aplication.Interfaces.IEntrenamiento;
using Aplication.UseCase.Entrenamientos;
using Aplication.Interfaces.ISancion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Inyecciones de dependencias

// Clientes
builder.Services.AddScoped<IClienteServices, ClientesService>();
builder.Services.AddScoped<IClientesQuery, ClientesQuery>();
builder.Services.AddScoped<IClientesCommand, ClientesCommand>();
builder.Services.AddScoped<IClienteMapper, ClienteMapper>();

// Cancha
builder.Services.AddScoped<ICanchaCommand, CanchaCommand>();
builder.Services.AddScoped<ICanchaQuery, CanchasQuery>();

// Precio
builder.Services.AddScoped<IPrecioCommand, PrecioCommand>();
builder.Services.AddScoped<IPrecioQuery, PreciosQuery>();

// Jugador
builder.Services.AddScoped<IJugadorService, JugadoresService>();
builder.Services.AddScoped<IJugadorCommand, JugadoresCommand>();
builder.Services.AddScoped<IJugadorQuery, JugadoresQuery>();
builder.Services.AddScoped<IJugadorMapper, JugadorMapper>();

//Sancion
builder.Services.AddScoped<ISancionService, SancionService>();
builder.Services.AddScoped<ISancionCommand, SancionCommand>();
builder.Services.AddScoped<ISancionQuery, SancionQuery>();
builder.Services.AddScoped<ISancionMapper, SancionMapper>();

// Equipo
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IEquipoCommand, EquipoCommand>();
builder.Services.AddScoped<IEquipoQuery, EquipoQuery>();
builder.Services.AddScoped<IEquipoMapper, EquipoMapper>();

// Competicion
builder.Services.AddScoped<ICompeticionService, CompeticionService>();
builder.Services.AddScoped<ICompeticionCommand, CompeticionCommand>();
builder.Services.AddScoped<ICompeticionQuery, CompeticionQuery>();
builder.Services.AddScoped<ICompeticionMapper, CompeticionMapper>();

// Partido
builder.Services.AddScoped<IPartidoService, PartidoService>();
builder.Services.AddScoped<IPartidoCommand, PartidoCommand>();
builder.Services.AddScoped<IPartidoQuery, PartidoQuery>();
builder.Services.AddScoped<IPartidoMapper, PartidoMapper>();

// Administrador
builder.Services.AddScoped<IAdminCommand, AdministradorCommand>();
builder.Services.AddScoped<IAdminQuery, AdminQuery>();
builder.Services.AddScoped<IAdminService, AdministradorService>();
builder.Services.AddScoped<IAdminMapper, AdminMapper>();

// Profesor
builder.Services.AddScoped<IProfesorService, ProfesorService>();
builder.Services.AddScoped<IProfesorQuery, ProfesorQuery>();
builder.Services.AddScoped<IProfesorCommand, ProfesorCommand>();
builder.Services.AddScoped<IProfesorService, ProfesorService>();
builder.Services.AddScoped<IProfesorMapper, ProfesorMapper>();

// Descuento
builder.Services.AddScoped<IDescuentoService, DescuentoService>();
builder.Services.AddScoped<IDescuentoCommand, DescuentoCommand>();
builder.Services.AddScoped<IDescuentoQuery, DescuentoQuery>();
builder.Services.AddScoped<IDescuentoMapper, DescuentoMapper>();

//Pago
builder.Services.AddScoped<IPagoService, PagoService>();
builder.Services.AddScoped<IPagoCommand, PagoCommand>();
builder.Services.AddScoped<IPagoQuery, PagoQuery>();
builder.Services.AddScoped<IPagoMapper, PagoMapper>();

//Factura
builder.Services.AddScoped<IFacturaService, FacturaService>();
builder.Services.AddScoped<IFacturaCommand, FacturaCommand>();
builder.Services.AddScoped<IFacturaQuery, FacturaQuery>();
builder.Services.AddScoped<IFacturaMapper, FacturaMapper>();

// Asistencia
builder.Services.AddScoped<IAsistenciaCommand, AsistneciaCommand>();
builder.Services.AddScoped<IAsistenciaQuery, AsistenciaQuery>();

// Clases
builder.Services.AddScoped<IClaseCommand, ClaseCommand>();
builder.Services.AddScoped<IClaseQuery, ClaseQuery>();
builder.Services.AddScoped<IClaseService, ClaseService>();
builder.Services.AddScoped<IClaseMapper, ClaseMapper>();

// Entrenamiento
builder.Services.AddScoped<IEntrenamientoCommand, EntrenamientoCommand>();
builder.Services.AddScoped<IEntrenamientoQuery, EntrenamientoQuery>();
builder.Services.AddScoped<IEntrenamientoService, EntrenamientoService>();
builder.Services.AddScoped<IEntrenamientoMapper, EntrenamientoMapper>();

// Reservas
builder.Services.AddScoped<IReservaCommand, ReservaCommand>();
builder.Services.AddScoped<IReservaQuery, ReservaQuery>();

// Usuario
builder.Services.AddScoped<IUsuarioCommand, UsuarioCommand>();
builder.Services.AddScoped<IUsuarioQuery, UsuarioQuery>();
builder.Services.AddScoped<IUsuarioMapper, UsuarioMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
