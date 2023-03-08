using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Report.ViewModels.Sotramac
{
    public class FactoresVM
    {
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Sigla { get; set; }
        public DateTime FechaSistema { get; set; }
        public bool EsActivo { get; set; }
        public int DetalleListaId { get; set; }
    }
}
