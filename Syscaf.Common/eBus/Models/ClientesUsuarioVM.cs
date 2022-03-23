using Syscaf.Data.Models.EBUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.eBus.Models
{
    public class ClientesUsuarioVM : ClienteEbusVM
    {
        public int Clave { get; set; }
        public List<int> Usuarios { get; set; }
    }
    public class ClienteActiveEventVM
    {
        public int clienteIdS { get; set; }
        public bool ActiveEvent { get; set; }
    }
}
