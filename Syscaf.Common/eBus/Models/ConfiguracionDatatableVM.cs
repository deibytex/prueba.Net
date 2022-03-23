using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.eBus.Models
{
    public class ConfiguracionDatatableVM
    {
        public int ConfiguracionDatatableId { get; set; }
        public int UsuarioIds { get; set; }
        public int Columna { get; set; }
        public string IdTabla { get; set; }
        public int OpcionId { get; set; }
        public DateTime FechaSistema { get; set; }
        public int Clave { get; set; }
    }
}
