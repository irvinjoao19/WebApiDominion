using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Ot
    {
        public int otId { get; set; }
        public int tipoOrdenId { get; set; }
        public string nombreTipoOrden { get; set; }
        public int servicioId { get; set; }
        public string nombreArea { get; set; }
        public string nroObra { get; set; }
        public string direccion { get; set; }
        public int distritoId { get; set; }
        public string nombreDistritoId { get; set; }
        public string referenciaOt { get; set; }
        public string descripcionOt { get; set; }
        public string fechaRegistro { get; set; }
        public string fechaAsignacion { get; set; }
        public string horaAsignacion { get; set; }
        public int empresaId { get; set; }
        public string nombreEmpresa { get; set; }
        public string tipoEmpresa { get; set; }
        public int personalJCId { get; set; }
        public string nombreJO { get; set; }
        public int otOrigenId { get; set; }
        public int estadoId { get; set; }
        public string nombreEstado { get; set; }
        public string vencimiento { get; set; }


        public string observacion { get; set; }
        public int motivoPrioridadId { get; set; }
        public string nombrePrioridad { get; set; }
        public string observaciones { get; set; }
        public int ordenamientoOt { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public int usuarioId { get; set; }
        public int identity { get; set; }
        public int estado { get; set; }
              
        public int distritoIdGps { get; set; }
        public string suministroTD { get; set; }
        public string nroSed { get; set; }
        public string fotoAnterior { get; set; }
        public string fotoCabecera { get; set; }
        public int viajeIndebido { get; set; }


        public string fechaInicioTrabajo { get; set; }
        public string fechaFinTrabajo { get; set; } 
        public string urlPdf { get; set; } 

        public List<OtDetalle> detalles { get; set; }
    }
}
