using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Syscaf.Data.Helpers.Auth;
using Syscaf.Data.Interface;
using Syscaf.Data.Models.Auth;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.Api.DWH.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TxController : ControllerBase
    {
     
        private readonly ITransmisionService _Transmision;
        public TxController( ITransmisionService _Transmision)
        {
        
            this._Transmision = _Transmision;
        }
       
        [HttpPost("ObtenerInformeTransmision")]
        public async Task<ResultObject> GetReporteTransmision(int Usuario, long? ClienteId)
        {
            return await _Transmision.GetReporteTransmision(Usuario, ClienteId);
        }
    }
}
