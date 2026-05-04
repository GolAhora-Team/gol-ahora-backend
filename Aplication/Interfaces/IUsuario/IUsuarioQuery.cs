using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IUsuario
{
    public interface IUsuarioQuery
    {
        Task<Usuario?> GetById(int id);
        Task<List<Usuario>> GetAll();
    }
}
