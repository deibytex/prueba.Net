using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.EBUS
{
   public class Locations
    {
        public long LocationId { get; set; }

        public long OrganisationId { get; set; }
        public int ClienteIds { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ShapeWkt { get; set; }
        public double? Radius { get; set; }
        public string ColourOnMap { get; set; }
        public decimal? OpacityOnMap { get; set; }
        public string LocationType { get; set; }
        public string ShapeType { get; set; }
        public DateTime FechaSistema { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsParqueo { get; set; }
    }
}
