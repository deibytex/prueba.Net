using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models
{
    public class BaseEntity
    {
        public DateTime FechaSistema { get; set; }
        public bool EsActivo { get; set; }
    }
}
