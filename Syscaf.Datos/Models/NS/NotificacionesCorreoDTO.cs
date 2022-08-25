using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.NS
{
    public class NotificacionesCorreoDTO
    {
        public long NotificacionCorreoId { get; set; }
        public int TipoNotificacionId { get; set; }

        public string Descripcion { get; set; }

        public int ListaDistribucionId { get; set; }

        public bool EsNotificado { get; set; }

        public DateTime FechaSistema { get; set; }
        public bool EsActivo { get; set; }

    }
}
