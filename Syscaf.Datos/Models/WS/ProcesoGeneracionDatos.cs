using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.WS
{
    public class ProcesoGeneracionDatos : BaseEntity
    {
        public int ProcesoGeneracionDatosId { get; set; }
        public int ServicioId { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public int? UsuarioIdS { get; set; }
        public string Nombre { get; set; }
        public int EstadoProcesoGeneracionId { get; set; }

      

    }
}
