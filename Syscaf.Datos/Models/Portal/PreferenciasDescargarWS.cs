using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class PreferenciasDescargarWS : BaseEntity
    {
        public int PreferenciasIdS { get; set; }
        public int clienteIdS { get; set; }
        public int? eventTypeIdS { get; set; }
        public int? usuarioIdS { get; set; }
        public int TipoPreferencia { get; set; }
        public long? EventTypeId { get; set; }
        public string ClientesId { get; set; }
        public bool? isActive { get; set; }
        public string Parametrizacion { get; set; }
    }
}
