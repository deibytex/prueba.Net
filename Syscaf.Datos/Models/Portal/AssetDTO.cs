
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Data.Models.Portal
{
    public class AssetShortDTO
    {
        public long AssetId { get; set; }
        public int AssetTypeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public long SiteId { get; set; }
        public long ClienteId { get; set; }
        public string UserState { get; set; } = string.Empty;
    }
    public class AssetDTO { 
        public long AssetId { get; set; }
        public int AssetTypeId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public long SiteId { get; set; }
        public long ClienteId { get; set; }     
        public float? Odometer { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? FmVehicleId { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public string UserState { get; set; } = string.Empty;
        public string AssetImageUrl { get; set; } = string.Empty;

        #region deviceConfiguration  - GetMobileUnitDeviceConfigurationsByGroupId
        public string UnitIMEI { get; set; } = string.Empty;
        public string UnitSCID { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string ConfigurationGroup { get; set; } = string.Empty;

        #endregion
        #region deviceConfiguration  - GetConfigurationState
        public string DriverOBC { get; set; } = string.Empty;
        public string DriverOBCLoadDate { get; set; } = string.Empty;

        #endregion
        #region MobileUnitCommunicationSettings  - GetCommunicationSettings
        public string GPRSContext { get; set; } = string.Empty;

        #endregion
        #region diagnostic  - GetDiagnosticAssets
        public string DriverCAN { get; set; } = string.Empty;
        public string LastConfiguration { get; set; } = string.Empty;

        #endregion

        public string LastTrip { get; set; } = string.Empty;

    }

    public class AssetTypeDTO
    {
        public int AssetTypeId { get; set; }
        public string Name { get; set; } = string.Empty;


    }
}
