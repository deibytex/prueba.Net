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
    public class UnidadesActivasVM
    {
        public DateTime FechaCreacion { get; set; }
        public string Matricula { get; set; }
        public string Base { get; set; }
        public string Vertical { get; set; }
        public string Descripcion { get; set; }
        public string Site { get; set; }
        public string Equipo { get; set; }
        public string IMEI { get; set; }
        public string SerialSim { get; set; }
        public string Observacion { get; set; }
        public string Administrador { get; set; }
    }
}
