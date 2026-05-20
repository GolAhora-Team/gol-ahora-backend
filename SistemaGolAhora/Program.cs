using Aplication.Interfaces.IAdmin;
using Aplication.Interfaces.IAsistencia;
using Aplication.Interfaces.ICancha;
using Aplication.Interfaces.IClases;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IDescuento;
using Aplication.Interfaces.IJugador;
using Aplication.Interfaces.IPrecio;
using Aplication.Interfaces.IProfesor;
using Aplication.UseCase;
using Aplication.Interfaces.IReserva;
using Aplication.Interfaces.IUsuario;
using Aplication.Mappers;
using Aplication.UseCase.Clientes;
using Aplication.UseCase.Descuentos;
using Infraestructure.Command;
using Infraestructure.Querys;
using Microsoft.EntityFrameworkCore;
using Aplication.Interfaces.IEquipo;
using Aplication.Interfaces.ICompeticion;

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

//Clientes
builder.Services.AddScoped<IClienteServices, ClientesService>();
builder.Services.AddScoped<IClientesQuery, ClientesQuery>();
builder.Services.AddScoped<IClientesCommand, ClientesCommand>();
builder.Services.AddScoped<IClienteMapper, ClienteMapper>();

//Cancha
builder.Services.AddScoped<ICanchaCommand, CanchaCommand>();
builder.Services.AddScoped<ICanchaQuery, CanchasQuery>();

//Precio
builder.Services.AddScoped<IPrecioCommand, PrecioCommand>();
builder.Services.AddScoped<IPrecioQuery, PreciosQuery>();

//Jugador
builder.Services.AddScoped<IJugadorQuery, JugadoresQuery>();
builder.Services.AddScoped<IJugadorCommand, JugadoresCommand>();

//Equipo
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IEquipoCommand, EquipoCommand>();
builder.Services.AddScoped<IEquipoQuery, EquipoQuery>();
builder.Services.AddScoped<IEquipoMapper, EquipoMapper>();

//Competicion
builder.Services.AddScoped<ICompeticionService, CompeticionService>();
builder.Services.AddScoped<ICompeticionCommand, CompeticionCommand>();
builder.Services.AddScoped<ICompeticionQuery, CompeticionQuery>();
builder.Services.AddScoped<ICompeticionMapper, CompeticionMapper>();

//Administrador
builder.Services.AddScoped<IAdminCommand, AdministradorCommand>();
builder.Services.AddScoped<IAdminQuery, AdminQuery>();
builder.Services.AddScoped<IAdminService, AdministradorService>();
builder.Services.AddScoped<IAdminMapper, AdminMapper>();

//Profesor
builder.Services.AddScoped<IProfesorService, ProfesorService>();
builder.Services.AddScoped<IProfesorQuery, ProfesorQuery>();
builder.Services.AddScoped<IProfesorCommand, ProfesorCommand>();
builder.Services.AddScoped<IProfesorService, ProfesorService>();
builder.Services.AddScoped<IProfesorMapper, ProfesorMapper>();

//Descuento
builder.Services.AddScoped<IDescuentoService, DescuentoService>();
builder.Services.AddScoped<IDescuentoCommand, DescuentoCommand>();
builder.Services.AddScoped<IDescuentoQuery, DescuentoQuery>();


//Asistencia
builder.Services.AddScoped<IAsistenciaCommand, AsistneciaCommand>();
builder.Services.AddScoped<IAsistenciaQuery, AsistenciaQuery>();

// Clases
builder.Services.AddScoped<IClaseCommand, ClaseCommand>();
builder.Services.AddScoped<IClaseQuery, ClaseQuery>();

// Reservas
builder.Services.AddScoped<IReservaCommand, ReservaCommand>();
builder.Services.AddScoped<IReservaQuery, ReservaQuery>();

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
