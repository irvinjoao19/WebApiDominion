using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class OtPlazoDetalle
    {
        public int otId { get; set; }
        public string descripcionEstado { get; set; }
        public string tipoOt { get; set; }
        public string nroObra { get; set; }
        public string direccion { get; set; }
        public string distrito { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string fechaAsignacion { get; set; }
        public string fechaMovil { get; set; }
        public string empresaContratista { get; set; }
        public string jefeCuadrilla { get; set; }
        public string fueraPlazoHoras { get; set; }
        public string fueraPlazoDias { get; set; }
        public int tipoTrabajoId { get; set; }
        public int distritoId { get; set; }
        public string referencia { get; set; }
        public string descripcionOt { get; set; }
        public int estadoId { get; set; }
    }
}
