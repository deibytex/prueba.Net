using System;

namespace Syscaf.Common.Models
{
    public class ReporteConfiguracion
    {
        
        public long AssetId { get; set; }       
        public string DriverOBC { get; set; }
        public string DriverCAN { get; set; }
        public string DriverOBCLoadDate { get; set; }
        public string LastConfiguration { get; set; }      
        public string DeviceType { get; set; }
        public string ConfigurationGroup { get; set; }
        public string GPRSContext { get; set; }
        public string UnitIMEI { get; set; }
        public string UnitSCID { get; set; }
        public string LastTrip { get; set; }

    }
}