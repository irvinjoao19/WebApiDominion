using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Proveedor
    {
        public int id { get; set; }
        public int empresaId { get; set; }
        public string razonSocial { get; set; }
        public decimal totalMes { get; set; }
        public decimal asignado { get; set; }
        public decimal terminado { get; set; }
        public decimal totaldia { get; set; }
       
    }
}
