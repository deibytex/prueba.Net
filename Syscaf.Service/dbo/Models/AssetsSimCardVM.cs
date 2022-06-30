using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.dbo.Models
{
    public class AssetsSimCardVM
    {
        public long AssetId { get; set; }
        public long clienteid { get; set; }      
        public string Description { get; set; }
        public int ProcesoGeneracionDatosId { get; set; }
    }

    public class PruebaSimCard
    {//TB_PruebasMovistar
        public int Id { get; set; }

        public string Placa { get; set; }

        public DateTime UltimoAvl { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        public float? Velocidad { get; set; }

        public int ProcesoGeneracionDatosId { get; set; }

        public DateTime FechaSistema { get; set; }
    }
}
