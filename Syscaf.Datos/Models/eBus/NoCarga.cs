using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.EBUS
{
    public class NoCarga : BaseEntity
    {
        public int NoCargaId { get; set; }
        public int Consecutivo { get; set; }
        public long assetId { get; set; }
        public long driverId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
