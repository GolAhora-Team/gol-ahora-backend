using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IClases
{
    public interface IClaseQuery
    {
            Task<Clase?> GetClaseById(int id);
    
            Task<bool> ClaseExists(int id);
    
            Task<List<Clase>> GetAllClases();
    }
}
