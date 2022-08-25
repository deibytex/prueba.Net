using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.PORTAL
{
    public class AssetsVM
    {
        public string? ClienteId { get; set; }
        public string? AssetId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Description { get; set; }
        public string? UserState { get; set; }
        public int EstadoTxId { get; set; }
        public string? SiteName { get; set; }
        public string? estado { get; set; }
        public int VerticalId { get; set; }
        public int ClasificacionId { get; set; }
        public int ingresoSalidaId { get; set; }
        public string? UnitIMEI { get; set; }
        public string? UnitSCID { get; set; }
        public string? vertical { get; set; }
        public string? clasificacion { get; set; }
        public string? ingresoSalida { get; set; }
        public bool? EsManual { get; set; }
        public string? UsuarioPrueba { get; set; }
    }
}
