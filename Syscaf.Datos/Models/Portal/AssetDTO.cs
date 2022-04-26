
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    
    public class AssetDTO
    {
        public long AssetId { get; set; }
        public int AssetTypeId { get; set; }
        public string Description { get; set; }
        public string RegistrationNumber { get; set; }
        public long SiteId { get; set; }
      
        public long ClienteId { get; set; }
        public float? Odometer { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? FmVehicleId { get; set; }
        public string CreatedBy { get; set; }
        public string UserState { get; set; }
        public string AssetImageUrl { get; set; }

        #region deviceConfiguration  - GetMobileUnitDeviceConfigurationsByGroupId
        public string UnitIMEI { get; set; }
        public string UnitSCID { get; set; }
        public string DeviceType { get; set; }
        public string ConfigurationGroup { get; set; }

        #endregion
        #region deviceConfiguration  - GetConfigurationState
        public string DriverOBC { get; set; }
        public string DriverOBCLoadDate { get; set; }

        #endregion
        #region MobileUnitCommunicationSettings  - GetCommunicationSettings
        public string GPRSContext { get; set; }

        #endregion
        #region diagnostic  - GetDiagnosticAssets
        public string DriverCAN { get; set; }
        public string LastConfiguration { get; set; }

        #endregion

        public string LastTrip { get; set; }

    }

    public class AssetTypeDTO
    {
        public int AssetTypeId { get; set; }     
        public string Name { get; set; }
        

    }
}
