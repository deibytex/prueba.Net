using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class DetalleListaVM
    {
        public int DetalleListaId { get; set; }
        public int ListaId { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Sigla { get; set; }
        public DateTime FechaSistema { get; set; }
        public bool EsActivo { get; set; }
    }
}
