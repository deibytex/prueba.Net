using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models.PORTAL
{
    public class AssetsVM
    {
        public int assetIdS { get; set; }
        public long AssetId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? assetsDescription { get; set; }
        public string? UserState { get; set; }
        public int estadoSyscafIdS { get; set; }
        public string? Sitio { get; set; }
    }
}
