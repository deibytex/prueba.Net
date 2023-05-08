using MiX.Integrate.Shared.Entities.Assets;
using Syscaf.Common.Models;
using Syscaf.Data.Models.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Automaper.MapperDTO
{
    public class AssetBaseData
    {
        public List<Asset> ListaAssets { get; set; }
        public List<ReporteConfiguracion> ListaConfiguracion{ get; set; }
        public List<AdditionalDetails> assetsDetails { get; set; }


    }
    public class AssetResult
    {
        public List<AssetDTO> Resultado { get; set; }


    }

    public class SiteResult
    {
        public List<SiteDTO> Resultado { get; set; }


    }
}
