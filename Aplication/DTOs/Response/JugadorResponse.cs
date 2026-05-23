using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.DTOs.Response
{
    public class JugadorResponse
    {
        public int Id { get; set; }

        public int Numero { get; set; }

        public bool EsCapitan { get; set; }

        public bool EsTitular { get; set; }

        public int Posicion { get; set; }

        public EstadoJugador Estado { get; set; }
        
        public int ClienteId { get; set; }        

        public ICollection<SancionResponse> Sanciones { get; set; }
           
        public int EquipoId { get; set; }
        public string EquipoNombre { get; set; }
    }
}
