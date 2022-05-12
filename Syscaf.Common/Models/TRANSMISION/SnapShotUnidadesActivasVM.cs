using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.TRANSMISION
{
    public class SnapShotUnidadesActivasVM
    {
        public int SnapshotUnidadesActivasId { get; set; }
        public DateTime Fecha { get; set; }
        public string Matricula { get; set; }
        public string Base { get; set; }
        public string Vertical { get; set; }
        public string Descripcion { get; set; }
        public string Sitio { get; set; }
        public string Equipo { get; set; }
        public string Imei { get; set; }
        public string SerialSim { get; set; }
        public string Administrador { get; set; }
        public string UsuarioIds { get; set; }
        public long SiteId { get; set; }
        public long ClienteId { get; set; }
        public string ClasificacionId { get; set; }

    }
}
