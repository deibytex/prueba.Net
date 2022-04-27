using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class ListaDTO : BaseEntity
    {
        public int ListaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Sigla { get; set; }       

        public List<DetalleListaDTO> Detalle { get; set; }

    }
}
