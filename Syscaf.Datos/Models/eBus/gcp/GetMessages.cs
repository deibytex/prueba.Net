using System;
using System.Collections.Generic;
using System.Text;

namespace Syscaf.Data.Models.eBus.gcp
{
    public class GetMessages
    {
        public DateTime FechaHora { get; set; }
        public string Mensaje { get; set; }
        public string ProfileData { get; set; }
    }
}
