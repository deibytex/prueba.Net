using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Models
{
    public class ParqueoInteligenteVM
    {
        public string LocationId { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public DateTime AVL { get; set; }
        public string AVLString { get; set; }
        public string Localizacion { get; set; }
        public string Placa { get; set; }
        public string AssetId { get; set; }
    }
}
