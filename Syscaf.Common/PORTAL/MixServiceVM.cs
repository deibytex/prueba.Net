using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.PORTAL
{
   public class MixServiceVM
    {
        public int StatusCode { get; set; }
        public bool Exitoso { get; set; }
        public string Response { get; set; }
        public object Data { get; set; }
    }
}
