using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models
{
    public class ParametroVM
    {
        public int ParametrizacionId { get; set; }
        public int TipoParametroId  { get; set; }
        public int ClienteIds { get; set; }
        public string Valor { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? UltimaActualizacion { get; set; }
        public DateTime FechaSistema { get; set; }
        public bool EsActivo { get; set; }
        public int Clave { get; set; }
    }
}
