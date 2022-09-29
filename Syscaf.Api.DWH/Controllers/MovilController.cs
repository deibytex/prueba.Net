﻿using Microsoft.AspNetCore.Http;
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
        
    }
}