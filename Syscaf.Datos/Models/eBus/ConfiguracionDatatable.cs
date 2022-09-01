using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.EBUS
{
   public  class ConfiguracionDatatable
    {
        public int ConfiguracionDatatableId { get; set; }
        public int UsuarioIds { get; set; }
        public int Columna { get; set; }
        public string IdTabla { get; set; }
        public int OpcionId { get; set; }
        public DateTime FechaSistema { get; set; }
        public string NombreReporte { get; set; }
        public string ReporteIdPBI { get; set; }
    }
}
