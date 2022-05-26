using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using System.ComponentModel.DataAnnotations;

namespace Syscaf.Api.DWH.Controllers
{
    /// <summary>
    /// Controlador de transmisión
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class AssetController : ControllerBase
    {

        private readonly IAssetsService _assetService;
        /// <summary>
        /// Controlador de transmisión
        /// </summary>
        /// <param name="_Transmision">aaa</param>
        public AssetController(IAssetsService _Transmision)
        {
            this._assetService = _Transmision;
        }
        /// <summary>
        /// Obtiene el  listado de Assets segun filtros
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="ClienteId"></param>
        /// /// <param name="ClienteIds"></param>
        /// <returns></returns>
        [HttpGet("GetAssets/ClienteId")]
        public async Task<List<AssetShortDTO>> Get( long? ClienteId,string UsertState)
        {
            return await _assetService.GetAsync( ClienteId, UsertState);
        }

        [HttpGet("GetAssets/ClienteIds")]
        public async Task<List<AssetShortDTO>> GetByClienteIds(int? ClienteIds, string UsertState)
        {
            return await _assetService.GetByClienteIdsAsync(ClienteIds, UsertState);
        }


    }
}
