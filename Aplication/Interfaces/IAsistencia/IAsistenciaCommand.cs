using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces.IAsistencia
{
    public interface IAsistenciaCommand
    {
            Task InsertAsistencia(Asistencia asistencia);
            Task UpdateAsistencia(Asistencia asistencia);
            Task RemoveAsistencia(Asistencia Asistencia);
            Task InsertAsistenciaEntrenamiento(AsistenciaEntrenamiento asistencia);
            Task RemoveAsistenciaEntrenamiento(AsistenciaEntrenamiento asistencia);
            Task InsertRegistroAsistenciaClase(RegistroAsistenciaClase registro);
            Task RemoveRegistroAsistenciaClase(RegistroAsistenciaClase registro);
    }
}
