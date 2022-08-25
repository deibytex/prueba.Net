using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Auth.Models
{
    public class MenuUsuarioDTO : OpcionesUsuario
    {      
        public List<string> LstOperacion { get; set; }
      
    }
    public class MenuDesagregadoDTO : OpcionesUsuario
    {
        public string Operacion { get; set; }

    }

    public class OpcionesUsuario
    {
        public string UserName { get; set; }
        public string Nombre { get; set; }
        public string Sigla { get; set; }
        public string NombreOpcion { get; set; }
        public string Accion { get; set; }
        public string Controlador { get; set; }
        public string Logo { get; set; }
        public bool EsVisible { get; set; }
        public int OrganizacionId { get; set; }
       
        public int? OpcionPadreId { get; set; }
        public int OpcionId { get; set; }

        public int? Orden { get; set; }
        public string NombreOperacion { get; set; }

        public string ParametrosAdicionales { get; set; }

    }
}
