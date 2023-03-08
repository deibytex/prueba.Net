using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AplicacionSyscaf.Models.Sotramac
{
    public class GenerarReporteSotramacVM
    {
        public string RangoFecha { get; set; }
        public int clienteIdS { get; set; }
        public string DriversIdS { get; set; }
        public string Assetsids { get; set; }
        public int AssetTypeId { get; set; }
        public Int64? SiteId { get; set; }
    }
}