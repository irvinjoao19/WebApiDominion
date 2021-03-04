using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class OtDetalle
    {
        public int otDetalleId { get; set; }
        public int otId { get; set; }
        public int tipoTrabajoId { get; set; }
        public int tipoMaterialId { get; set; }
        public int tipoDesmonteId { get; set; }
        public decimal largo { get; set; }
        public decimal ancho { get; set; }
        public decimal espesor { get; set; }
        public decimal total { get; set; }
        public string nroPlaca { get; set; }
        public decimal m3Vehiculo { get; set; }
        public int estado { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string nombreTipoMaterial { get; set; }
						
		public float cantPanos { get; set; }
		public decimal medHorizontal { get; set; }
		public decimal medVertical { get; set; }     
		
        public List<OtPhoto> photos { get; set; }
    }
}
