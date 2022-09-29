using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.EBUS
{
    public class ClienteEbusVM
    {
        public int ClienteUsuarioId { get; set; }
        public int UsuarioIdS { get; set; }
        public string ClienteIds { get; set; }
        public bool EsActivo { get; set; }
        public DateTime FechaSistema { get; set; }
    }
   
}
