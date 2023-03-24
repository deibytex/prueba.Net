using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Service.Helpers;
using System.ComponentModel.DataAnnotations;
using Syscaf.Service.Portal;
using Syscaf.Common.Models.MOVIL;
using Syscaf.Data.Helpers.Movil;
using Syscaf.PBIConn.Services;
using Syscaf.Common.Helpers;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovilController : ControllerBase
    {

        private readonly IMovilService _Movil;
        private readonly IPortalMService _portalService;
        private readonly IAdmService _admService;

        public MovilController(IMovilService _Movil, IPortalMService _portalService, IAdmService _admService)
        {
            this._Movil = _Movil;
            this._portalService = _portalService;
            this._admService = _admService;
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

        //Metodo para obetener los datos de reportes por tipo para Preoperacional
        [HttpGet("GetReportePorTipo")]
        public async Task<List<dynamic>> GetReportePorTipo(string? FechaInicial, string? FechaFinal, string? Tipo)
        {
            //Recibimos los parametros y los preparamos para pasar en las consultas
            Dapper.DynamicParameters param = new Dapper.DynamicParameters();
            param.Add("Clienteids", null);
            param.Add("FechaI", FechaInicial);
            param.Add("FechaF", FechaFinal);
            param.Add("Tipo", Tipo);            

            //Consultas al DWH y al Core de los reportes a usar
            var datosPreoperacional = await _admService.getDynamicValueCore(MovilQueryHelper._MOVQueryHelper, MovilQueryHelper._getConsolidadoReportesPorTipo, param);

            var datosReporte = await _portalService.getDynamicValueProcedureDWH(MovilQueryHelper._PREOPQueryHelper, MovilQueryHelper._getInformeViajesVsPreoperacional, param);

            //Validacion del tipo de reporte y modelado de la información
            if (Tipo == "2")
            {
                var resultadoReporte = (from reporte in datosReporte
                                        join preop in datosPreoperacional
                                             on reporte.mes equals preop.Mes into result
                                        from d in result.DefaultIfEmpty()
                                        select new
                                        {
                                            Mes = reporte.mes,
                                            TotalViajes = reporte.Cantidad,
                                            TotalPreop = d != null ? d.Total : 0,
                                            // Porc = (reporte.Cantidad / preop.Total),
                                            MesName = Constants.CultureDate.GetMonthName((int)reporte.mes).ToUpper()
                                        }
                                        ).Select(s =>
                                        {

                                            int Mes = s.Mes;
                                            int TotalViajes = s.TotalViajes;
                                            int TotalPreop = s.TotalPreop;

                                            return new { Mes, TotalViajes, TotalPreop, s.MesName };
                                        }).ToList<dynamic>();

                return resultadoReporte;
            }
            else if (Tipo == "3")
            {
                var resultadoReporte = (from reporte in datosReporte
                                        join preop in datosPreoperacional
                                             on reporte.DriverId equals preop.driverid
                                             into result
                                        from d in result.DefaultIfEmpty()
                                        select new
                                        {
                                            reporte.DriverId,
                                            TotalViajes = reporte.Cantidad,
                                            TotalPreop = d != null ? d.Total : 0,
                                            reporte.conductor
                                        }
                                          ).Select(s =>
                                          {

                                              long DriverId = s.DriverId;
                                              int TotalViajes = s.TotalViajes;
                                              int TotalPreop = s.TotalPreop;
                                              string Conductor = s.conductor;
                                              return new { DriverId, TotalViajes, TotalPreop, Conductor };
                                          }).ToList<dynamic>();

                return resultadoReporte;
            }

            //Si falla de reportna una lista vacia
            return new List<dynamic>();
        }

    }
}
