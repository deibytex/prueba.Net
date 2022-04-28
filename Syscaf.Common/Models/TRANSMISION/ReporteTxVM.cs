using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.TRANSMISION
{
    public class ReporteTxVM
    {
        public int? assetCodigo { get; set; }
        public string registrationNumber { get; set; }
        public string assetsDescription { get; set; }
        public int diffAVL { get; set; }
        public DateTime AVL { get; set; }
        public string clientenNombre { get; set; }
        public string Cliente { get; set; }
        public string Sitio { get; set; }
        public string SitioSoporte { get; set; }
        public string nombre { get; set; }
        public string estadoSyscaf { get; set; }
        public string assetId { get; set; }
    }
}
