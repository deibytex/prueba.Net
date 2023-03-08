using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Report.ViewModels.Sotramac
{
   public class ParametrosReportes
    {
        public DateTime FechaReporte { get; set; }
        public bool EsKg { get; set; }
        public decimal   ParametroKg { get; set; }
        public string NameMes { get; set; }
    }
}
