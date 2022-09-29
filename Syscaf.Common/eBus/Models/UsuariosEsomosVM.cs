using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.eBus.Models
{
    public class UsuariosEsomosVM
    {
        public int UsuarioIds { get; set; }
        public string NombreUsuario { get; set; }
    }

    public class UsuariosClientesEsomosVM
    {
        public int? clienteIdS { get; set; }
        public string clienteNombre { get; set; }
        public bool EsSeleccionado { get; set; }
    }
}
