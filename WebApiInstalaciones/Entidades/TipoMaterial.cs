using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class TipoMaterial
    {
        public int detalleId { get; set; }
        public int grupoId { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int estado { get; set; }

    }
}
