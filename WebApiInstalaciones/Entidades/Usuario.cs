using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario
    {
        public int usuarioId { get; set; }
        public string nroDoc { get; set; }
        public string apellidos { get; set; }
        public string nombres { get; set; }
        public string email { get; set; }
        public int tipoUsuarioId { get; set; }
        public int perfilId { get; set; }
        public string pass { get; set; }
        public int estado { get; set; }
        public int personalId { get; set; }
        public int empresaId { get; set; }
        public string nombreEmpresa { get; set; }
        public string mensaje { get; set; }
        public List<Accesos> accesos { get; set; }
    }
}
