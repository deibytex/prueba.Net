using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("getAssets")]
        public async Task<ActionResult<ResultObject>> getAssets([Required] long ClienteId)
        {
            return await _asset.getAssets(ClienteId);
        }

        [HttpGet("setEstadoAssets")]
        public async Task<ActionResult<ResultObject>> setEstadoAssets([Required] long ClienteId, [Required] long AssetId, [Required] int EstadoTxId, int usuarioIdS)
        {
            return await _asset.setEstadoAssets(ClienteId, AssetId, EstadoTxId, usuarioIdS);
        }

    }
}
