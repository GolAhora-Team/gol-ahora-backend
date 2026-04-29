using Aplication.Interfaces.ICancha;
using Aplication.Interfaces.ICliente;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class CanchasQuery : ICanchaQuery
    {
        private readonly AppDbContext _context;

        public CanchasQuery(AppDbContext context)
        {
            _context = context;
        }

        public Task<Cancha> GetCanchaById(int idCancha)
        {
            throw new NotImplementedException();
        }

        public Task<Cancha> GetCanchasActivas()
        {
            throw new NotImplementedException();
        }

        public Task<Cancha> GetCanchasDisponibles(DateTime fecha, TimeSpan hora)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Cliente>> GetListCancha()
        {
            throw new NotImplementedException();
        }
    }
}
