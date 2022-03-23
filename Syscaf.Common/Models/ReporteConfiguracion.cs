using System;

namespace Syscaf.Common.Models
{
    public class ReporteConfiguracion
    {
        public string BD_Cliente { get; set; }
        public string AssetId { get; set; }
        public string SiteName { get; set; }
        public string VehicleID { get; set; }
        public string VehicleDescription { get; set; }
        public string RegistrationNumber { get; set; }
        public string DriverOBC { get; set; }
        public string DriverCAN { get; set; }
        public string DriverOBCLoadDate { get; set; }
        public string LastConfiguration { get; set; }
        public string CreatedDate { get; set; }
        public string DeviceType { get; set; }
        public string ConfigurationGroup { get; set; }
        public string GPRSContext { get; set; }
        public string UnitIMEI { get; set; }
        public string UnitSCID { get; set; }
        public string LastTrip { get; set; }

    }
}