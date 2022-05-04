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
        

        public BasesController(IAssetsService _asset)
        {
            this._asset = _asset;
           
        }

        [HttpGet("actualizarVehiculos")]      
        public async Task<ActionResult<ResultObject>> GetAssetsMixByGroup()
        { 
            return await _asset.Add(); 
        }
       
    }
}
