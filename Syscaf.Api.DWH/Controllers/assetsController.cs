using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using System.ComponentModel.DataAnnotations;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class assetsController : ControllerBase

    {
        private readonly IAssetsService _asset;
        public assetsController(IAssetsService _asset)
        {
            this._asset = _asset;
        }

        //Obtiene assets
        [HttpGet("getAssets")]
        public async Task<ActionResult<ResultObject>> getAssets([Required] long ClienteId)
        {
            return await _asset.getAssets(ClienteId);
        }

        //Obtiene estados TX o los uqe pidan
        [HttpGet("getEstadosAssets")]
        public async Task<ActionResult<ResultObject>> getEstadosAssets([Required] int tipoIdS)
        {
            return await _asset.getEstadosTx(tipoIdS);
        }

        //Actualizar assets
        [HttpPost("updateAssets")]
        public async Task<ActionResult<ResultObject>> updateAssets([FromBody] AssetsVM assets)
        {
            return await _asset.updateAssets(assets.ClienteId, assets.AssetId, assets.UnitIMEI, assets.UnitSCID, assets.ClasificacionId, assets.VerticalId, assets.EstadoTxId);
        }
    }
}
