using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class OtReporte
    {
        public int otId { get; set; }
        public string nombreTipoOrdenTrabajo { get; set; }
        public string nombreArea { get; set; }
        public string nroObra { get; set; }
        public string direccion { get; set; }
        public string nombreDistrito { get; set; }
        public string fechaAsignacion { get; set; }
        public string nombreEmpresa { get; set; }
        public int personalJefeCuadrillaId { get; set; }
        public string nombreJC { get; set; }
        public string estado { get; set; }
        public string nombreEstado { get; set; }
        public string vencimiento { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }

    }
}
