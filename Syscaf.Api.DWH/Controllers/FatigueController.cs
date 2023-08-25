using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.Models.FATIGUE;
using Syscaf.Service.Fatigue;
using Syscaf.Service.Helpers;

namespace Syscaf.Api.DWH.Controllers
 
{
    /// <summary>
    /// Controlador de transmisión
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class FatigueController : ControllerBase
    {
        private readonly IFatigueService _Fatigue;
        public FatigueController(IFatigueService _Fatigue)
        {
            this._Fatigue = _Fatigue;
        }
        /// <summary>
        /// Setea las respuestas del usuario desde el movil
        /// </summary>
        /// <param name="RespuestasPreoperacional"></param>
        /// <returns></returns>
        /// /// <summary>
        /// Servicio para guardar la paremetrizacion de alertas para el sistema de fatigue
        /// </summary>
        /// <param name="Clave"></param>
        /// <param name="Nombre"></param>
        /// <param name="Tiempo"></param>
        /// <param name="Condicion"></param>
        /// <param name="ClienteId"></param>
        /// <param name="EsActivo"></param>
        /// <param name="ConfiguracionAlertaId"></param>
        /// <returns></returns>
        [HttpPost("SetConfiguracionAlerta")]
        public async Task<ResultObject> SetConfiguracionAlerta([FromBody] SetFatigueVM Data)
        {
            //Se llama al servicio
            return await _Fatigue.SetConfiguracionAlerta(Data.Clave, Data.Nombre, Data.Tiempo, Data.Condicion, Data.Columna, Data.ClienteId, Data.EsActivo, Data.ConfiguracionAlertaId);
        }
        /// <summary>
        /// Se obtienen las conficuraciones
        /// </summary>
        /// <param name="Nombre"></param>
        /// <param name="ClienteId"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        [HttpPost("GetConfiguracionAlerta")]
        public async Task<ResultObject> GetConfiguracionAlerta([FromBody] GetFatigueVM Data)
        {
            //Se llama al servicio
            return await _Fatigue.GetConfiguracionAlerta(Data.Nombre, Data.ClienteId, Data.EsActivo);
        }

    }
}
