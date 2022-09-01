using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.ADM
{
    public class UsuarioOrganizacionVM
    {
        public string UserId { get; set; }
        public int UsuarioIds { get; set; }
        public string NombreUsuario { get; set; }

        public bool EsSeleccionado { get; set; }
    }
}
