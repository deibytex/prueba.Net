using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.EBUS
{
    public class Parametrizacion
    {
        public int ParametrizacionId { get; set; }
        public int TipoParametroId { get; set; }
        public int ClienteIds { get; set; }
        public string Valor { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? UltimaActualizacion { get; set; }
        public DateTime FechaSistema { get; set; }
        public bool EsActivo { get; set; }

    }
}
