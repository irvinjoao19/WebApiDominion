using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class OtPhoto
    {
        public int otPhotoId { get; set; }
        public int otDetalleId { get; set; }
        public string nombrePhoto { get; set; }
        public string urlPhoto { get; set; }
        public int estado { get; set; }
    }
}
