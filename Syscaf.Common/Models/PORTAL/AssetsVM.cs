using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.PORTAL
{
    public class AssetsVM
    {
        public string? AssetId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? Description { get; set; }
        public string? UserState { get; set; }
        public int EstadoTxId { get; set; }
        public string? SiteName { get; set; }
        public string? estado { get; set; }
    }
}
