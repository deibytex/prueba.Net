using System;
using System.Collections.Generic;

namespace AplicacionSyscaf.Models.Sotramac
{
    public class DescargaReporteSotramacVM
    {
        public int TipoInforme { get; set; }
        public string CategoriaInforme { get; set; }

        public List<long> Conductores { get; set; }

        public List<long> Vehiculos { get; set; }

        public string RangoFecha { get; set; }
        public int AssetTypeId { get; set; }
        public Int64? SiteId { get; set; }
    }
}