using Aplication.Interfaces.IPrecio;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Querys
{
    public class PreciosQuery : IPrecioQuery
    {
        private readonly AppDbContext _context;

        public PreciosQuery(AppDbContext context)
        {
            _context = context;
        }

        public Task<Precio> GetPrecioById()
        {
            throw new NotImplementedException();
        }
    }
}
