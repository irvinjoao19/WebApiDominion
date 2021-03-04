using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class JefeCuadrilla
    {
        public int cuadrillaId { get; set; }
        public int empresaId { get; set; }
        public string empresa { get; set; }
        public int jefeCuadrillaId { get; set; }
        public string nombreJefe { get; set; }
        public decimal asignado { get; set; }
        public decimal terminado { get; set; }
        public decimal pendiente { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string icono { get; set; }
    }
}
