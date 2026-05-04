using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IUsuario
{
    public interface IUsuarioCommand
    {
        Task Create(Usuario usuario);
        Task Update(Usuario usuario);
        Task Delete(Usuario usuario);
    }
}
