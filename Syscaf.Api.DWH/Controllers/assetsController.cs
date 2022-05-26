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
        private readonly IListaDetalleService _listas;
        public assetsController(IAssetsService _asset, IListaDetalleService _listas)
        {
            this._asset = _asset;
            this._listas = _listas;
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

        //Obtiene detalle listas 
        [HttpGet("getDetalleListasAssets")]
        public async Task<ActionResult<ResultObject>> getDetalleListasAssets([Required] string sigla)
        {
            return await _listas.getDetalleListas(sigla);
        }

        [HttpGet("setEstadoAssets")]
        public async Task<ActionResult<ResultObject>> setEstadoAssets([Required] long ClienteId, [Required] long AssetId, [Required] int EstadoTxId, int usuarioIdS)
        {
            return await _asset.setEstadoAssets(ClienteId, AssetId, EstadoTxId, usuarioIdS);
        }
    }
}
