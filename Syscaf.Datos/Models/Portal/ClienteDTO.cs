using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class ClienteDTO : ClienteSaveDTO
    {
        public int clienteIdS { get; set; }
        
        public int estadoClienteIdS { get; set; }
        
        public bool notificacion { get; set; }       
        public bool? GeneraIMG { get; set; }

        public bool Trips { get; set; }
        public bool Metrics { get; set; }
        public bool Event { get; set; }
        public bool Position { get; set; }
        public bool? ActiveEvent { get; set; }

    }

    public class ClienteSaveDTO {
        public long clienteId { get; set; }
        public string clienteNombre { get; set; }
        public DateTime fechaIngreso { get; set; }
    }
}
