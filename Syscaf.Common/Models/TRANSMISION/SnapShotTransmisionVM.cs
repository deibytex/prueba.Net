using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.TRANSMISION
{
    public class SnapShotTransmisionVM
    {
        public int SnapshotTransmisionId { get; set; }
        public DateTime Fecha { get; set; }
        public int FmVehicleId { get; set; }
        public string registrationNumber { get; set; }
        public string Description { get; set; }
        public long assetId { get; set; }
        public int DiasSinTx { get; set; }
        public DateTime UltimoAvl { get; set; }
        public long ClienteId { get; set; }
        public string clientenNombre { get; set; }
        public long SiteId { get; set; }
        public string Sitio { get; set; }
        public int UsuarioIds { get; set; }
        public string Estado { get; set; }
    }
}
