using Aplication.Interfaces.ICliente;
using Aplication.Interfaces.IJugador;
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


//custom

builder.Services.AddScoped<IClienteServices, ClientesService>();

builder.Services.AddScoped<IClienteQuery, ClienteQuery>();
builder.Services.AddScoped<IClientesCommand, ClientesCommand>();
builder.Services.AddScoped<IClientesCommand, ClientesCommand>();
builder.Services.AddScoped<IJugadorQuery, JugadoresQuery>();






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
