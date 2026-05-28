using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Request.Jugador
{
    public class JugadorRequest
    {
        public int Numero { get; set; }       
        public int Posicion { get; set; }
               
    
        public int EquipoId { get; set; }        
    }
}
