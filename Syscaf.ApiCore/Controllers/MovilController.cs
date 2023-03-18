using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Service.Helpers;
using System.ComponentModel.DataAnnotations;
using Syscaf.Service.Portal;
using Syscaf.Common.Models.MOVIL;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Identity;
using Syscaf.Service.Drive;
using Syscaf.Data.Helpers.Auth;
using Syscaf.Data.Models.Auth;
using Syscaf.Data.Helpers.Movil;
using System.Data;
using Syscaf.Common.Helpers;
using Dapper;

namespace Syscaf.ApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovilController : ControllerBase
    {

        private readonly IMovilService _Movil;
        private readonly IAdmService _admin;



        public MovilController(IMovilService _Movil, IAdmService _admin)
        {
            this._Movil = _Movil;
            this._admin = _admin;
        }
        /// <summary>
        /// Setea las respuestas del usuario desde el movil
        /// </summary>
        /// <param name="RespuestasPreoperacional"></param>
        /// <returns></returns>
        [HttpPost("SetRespuestasPreoperacional")]
        public async Task<ResultObject> SetRespuestasPreoperacional([FromBody] String RespuestasPreoperacional)
        {
            return await _Movil.SetRespuestasPreoperacional(RespuestasPreoperacional);
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
        /// <summary>
        /// Se consultan las preguntas de preoperacional.
        /// </summary>
        /// <param name="UsuarioId"></param>
        /// <param name="NombrePlantilla"></param>
        /// <param name="TipoPregunta"></param>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetPreguntasPreoperacional")]
        public async Task<ResultObject> GetPreguntasPreoperacional(string? UsuarioId, string? NombrePlantilla, string? TipoPregunta, long? ClienteId)
        {
            return await _Movil.GetPreguntasPreoperacional(UsuarioId, NombrePlantilla, TipoPregunta, ClienteId);
        }


        [HttpGet("GetActualizaciones")]
        public async Task<List<dynamic>> GetActualizaciones(string UsuarioId, string Device)
        {
            DynamicParameters d = new DynamicParameters();
            d.Add("UsuarioId", UsuarioId);
            d.Add("Device", Device);
            d.Add("Fecha", Constants.GetFechaServidor());
            return await _admin.getDynamicValueCore("MOVQueryHelper", "GetActualizaciones", d);
        }


        [HttpGet("SetActualizaciones")]
        public async Task<int> SetActualizaciones(string UsuarioId, string Device)
        {
            DynamicParameters d = new DynamicParameters();
            d.Add("UsuarioId", UsuarioId);
            d.Add("Device", Device);
            d.Add("Fecha",  Constants.GetFechaServidor());
            return await _admin.setDynamicValueCore("MOVQueryHelper", "SetActualizacion", d);
        }

        [HttpGet("getAssetsPorUsuarios")]
        public async Task<List<dynamic>> getAssetsPorUsuarios(string UsuarioId)
        {
            DynamicParameters d = new DynamicParameters();
            d.Add("UsuarioId", UsuarioId);
            return await _admin.getDynamicValueCore("MOVQueryHelper", "getAssetsByUsuario", d);
        }




    }


}

