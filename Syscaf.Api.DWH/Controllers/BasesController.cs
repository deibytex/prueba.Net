using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController : ControllerBase
    {
        private readonly IAssetsService _asset;
        private readonly ITransmisionService _Transmision;

        public BasesController(IAssetsService _asset, ITransmisionService _Transmision)
        {
            this._asset = _asset;
            this._Transmision = _Transmision;
        }

        [HttpGet("actualizarVehiculos")]      
        public async Task<ActionResult<ResultObject>> GetAssetsMixByGroup()
        { 
            return await _asset.Add(); 
        }
        [HttpPost("ObtenerInformeTransmision")]
        public async Task<ResultObject> GetReporteTransmision(int Usuario, long? ClienteId)
        {
            return await _Transmision.GetReporteTransmision(Usuario,ClienteId);
        }
    }
}
