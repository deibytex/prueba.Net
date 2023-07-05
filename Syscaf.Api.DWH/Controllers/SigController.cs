using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Service.Helpers;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SigController : ControllerBase
    {
        private readonly ITransmisionService _Transmision;
        public SigController(ITransmisionService _Transmision)
        {
            this._Transmision = _Transmision;
        }
        [HttpPost("PostCondiciones")]
        public async Task<ResultObject> PostCondiciones(int Usuario, long? ClienteId)
        {

            return await _Transmision.GetReporteTransmision(Usuario, ClienteId);
        }

    }
}
