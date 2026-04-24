using Aplication.Interfaces.IAsistencia;
using Aplication.Interfaces.IClases;
using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IReserva;
using Aplication.UseCase.Clientes;
using Infraestructure.Command;
using Infraestructure.Querys;
using Microsoft.EntityFrameworkCore;

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

//Asistencia
builder.Services.AddScoped<IAsistenciaCommand, AsistneciaCommand>();
builder.Services.AddScoped<IAsistenciaQuery, AsistenciaQuery>();

// Clases
builder.Services.AddScoped<IClaseCommand, ClaseCommand>();
builder.Services.AddScoped<IClaseQuery, ClaseQuery>();

// Reservas
builder.Services.AddScoped<IReservaCommand, ReservaCommand>();
builder.Services.AddScoped<IReservaQuery, ReservaQuery>();

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
