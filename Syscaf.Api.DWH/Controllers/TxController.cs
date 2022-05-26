﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.Models.TRANSMISION;
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
    
    public class TxController : ControllerBase
    {
        
        private readonly ITransmisionService _Transmision;
       /// <summary>
       /// Controlador de transmisión
       /// </summary>
       /// <param name="_Transmision">aaa</param>
        public TxController(ITransmisionService _Transmision)
        {
            this._Transmision = _Transmision;
        }
        /// <summary>
        /// Obtiene el informe de transmisión.
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("ObtenerInformeTransmision")]
        public async Task<ResultObject> GetReporteTransmision([Required] int Usuario, long? ClienteId)
        {
           
            return await _Transmision.GetReporteTransmision(Usuario, ClienteId);
        }
               
        /// <summary>
        /// Se obtiene la fotografia de transmisión.
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Fecha"></param>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetSnapShotTransmision")]
        public async Task<ResultObject> GetSnapShotTransmision([Required] int Usuario,  DateTime? Fecha, long? ClienteId)
        {
            return await _Transmision.GetSnapShotTransmision(Usuario, Fecha, ClienteId);
        }
        /// <summary>
        /// Se obtiene la fotografia de unidades activas
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Fecha"></param>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetReporteUnidadesActivas")]
        public async Task<ResultObject> GetSnapshotUnidadesActivas([Required] string Usuario,  DateTime? Fecha, long? ClienteId)
        {
            return await _Transmision.GetSnapshotUnidadesActivas(Usuario, Fecha, ClienteId);
        }
       /// <summary>
       /// Se rellena la tabla SnapShot de transmisión.
       /// </summary>
       /// <returns></returns>
        [HttpGet("SetSnapShotTransmision")]
        public async Task<ResultObject> SetSnapShotTransmision()
        {
            return await _Transmision.SetSnapShotTransmision();
        }
        /// <summary>
        /// Se rellena la tabla SnapShot de unidades activas.
        /// </summary>
        /// <returns></returns>
        [HttpGet("SetSnapShotUnidadesActivas")]
        public async Task<ResultObject> SetSnapShotUnidadesActivas()
        {
            return await _Transmision.SetSnapShotUnidadesActivas();
        }
        /// <summary>
        /// Se obtiene el listado de los admininistradores de flota.
        /// </summary>
        /// <param name="UsuarioId"></param>
        /// <param name="Nombres"></param>
        /// <returns></returns>
        [HttpGet("GetAdministradores")]
        public async Task<ResultObject> GetAdministradores(string? UsuarioId, string? Nombres)
        {
            return await _Transmision.GetAdministradores(UsuarioId, Nombres);
        }
        /// <summary>
        /// Consulta el listado de semana del año que se le pase y el tipo sea Unidades activas tipo 1 o transmisión tipo 2.
        /// </summary>
        /// <param name="Anio"></param>
        /// <param name="Tipo"></param>
        /// <returns></returns>
        [HttpGet("GetListaSemanaReportes")]
        public async Task<ResultObject> GetSemanasAnual(int Anio, int Tipo)
        {
            return await _Transmision.GetSemanasAnual(Anio, Tipo);
        }
        /// <summary>
        /// Inserta el json de las los tickets a la base de datos.
        /// </summary>
        /// <param  name="json"></param>
        /// <format>textarea</format>
        /// <returns></returns>
        [HttpPost("SetSnapShotTickets")]
        public async Task<ResultObject> SetSnapShotTickets([FromBody] List<TicketsVM> json)
        {

            return await _Transmision.SetSnapShotTickets(json);
        }
    }
}
