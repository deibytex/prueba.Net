using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Service.Helpers;
using System.ComponentModel.DataAnnotations;
using Syscaf.Service.Portal;
using Syscaf.Common.Models.MOVIL;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovilController : ControllerBase
    {

        private readonly IMovilService _Movil;

        public MovilController(IMovilService _Movil)
        {
            this._Movil = _Movil;
        }
        /// <summary>
        /// Setea las respuestas del usuario desde el movil
        /// </summary>
        /// <param name="Respuestas"></param>
        /// <returns></returns>
        [HttpPost("SetRespuestasPreoperacional")]
        public async Task<ResultObject> SetRespuestasPreoperacional([FromBody] RespuestasVM Respuestas)
        {
            return await _Movil.SetRespuestasPreoperacional(Respuestas);
        }
        /// <summary>
        /// Se consultan las respuestas ya sea por cliente, por pregunta, o por usuarios pero opcional
        /// </summary>
        /// <param name="Fecha"></param>
        /// <param name="UsuarioId"></param>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetRespuestasPreoperacional")]
        public async Task<ResultObject> GetRespuestasPreoperacional(string? Fecha, string? UsuarioId, Int64? ClienteId)
        {
            return await _Movil.GetRespuestasPreoperacional(Fecha, UsuarioId, ClienteId);
        }
    }
}
