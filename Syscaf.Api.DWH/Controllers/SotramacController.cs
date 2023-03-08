using AplicacionSyscaf.Models.Sotramac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Data.Helpers.Movil;
using Syscaf.Data.Models.Portal;
using Syscaf.Report.Helpers;
using Syscaf.Report.ViewModels.Sotramac;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using Syscaf.Service.Sotramac;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Syscaf.Api.DWH.Controllers
{
    /// <summary>
    /// Controlador de transmisión
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    public class SotramacController : ControllerBase
    {

        private readonly ISotramacService _SotramacService;
        private readonly IPortalMService _portalService;
   
        public SotramacController(ISotramacService _SotramacService, IPortalMService _portalService)
        {
            this._SotramacService = _SotramacService;
            this._portalService = _portalService;
        }
        /// <summary>
        /// Obtiene el  listado de Assets segun filtros
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("DescargarReporte")]
        public async Task<ResultObject> DescargarReporte([FromBody] DescargaReporteSotramacVM model)
        {
            var r = new ResultObject();

            try
            {
                string  reporteNombre = "", Datos = "";

                var vehiculos = new List<ReporteSotramacVM>();
                //Para las Fechas inicial y final
                string[] fechas = model.RangoFecha.Split('-');

                string f1 = Convert.ToString(fechas[0]).Trim();
                string f2 = Convert.ToString(fechas[1]).Trim();
                DateTime fechaInicial = Convert.ToDateTime(f1);
                DateTime fechaFinal = Convert.ToDateTime(f2).AddSeconds(86399);
                //El cliente Sotramac 898
                int clienteIdS = 898;
                //Conductores
                string Conductores = model.Conductores?.Select(s => s.ToString()).Aggregate((i, j) => i + "," + j);

                //Vehiculos
                string Vehiculos = "";

                Vehiculos = model.Vehiculos?.Select(s => s.ToString()).Aggregate((i, j) => i + "," + j);

                string extensionFile = "excel";


                if (model.CategoriaInforme == "EOAPC")
                {
                    var ReporteConductores = await _SotramacService.GetReporteSotramacConductores(fechaInicial, fechaFinal, clienteIdS, Conductores, model.AssetTypeId, model.SiteId);
                    vehiculos = ReporteConductores;
                    Datos = "DatosConductores";
                    reporteNombre = "InformeConductor";
                }
                //else if (model.CategoriaInforme == "EOAPV")
                //{
                //    var ReporteVehiculos = await _SotramacService.GetReporteSotramacVehiculos(fechaInicial, fechaFinal, clienteIdS, Vehiculos, model.AssetTypeId, model.SiteId);
                //    vehiculos = ReporteVehiculos;
                //    Datos = "DatosVehiculos";
                //    reporteNombre = "InformeVehiculos";
                //}
                ////else 
                ////{
                ////    var ReporteConductoresVehiculos = await _SotramacService.GetReporteSotramacCOxVH(fechaInicial, fechaFinal, clienteIdS, Conductores, Vehiculos, model.AssetTypeId, model.SiteId);
                ////    vehiculos = ReporteConductoresVehiculos;
                ////    Datos = "DatosConductoresVehiculos";
                ////    reporteNombre = "InformeConductorVehiculos";
                //}

                ReportBase rb = new ReportBase
                {
                    NombreReporte = reporteNombre,
                    PathEnviroment = "bin/Reportes/Sotramac/"
                };

                var builder = WebApplication.CreateBuilder();

                var webRootPath = builder.Environment.ContentRootPath;

                Dapper.DynamicParameters param = new Dapper.DynamicParameters();
                param.Add("Sigla", "Fact_M3");

                DetalleListaVM factorM3 = (await _portalService.getDynamicValueProcedureDWH("PortalQueryHelper", "GetDetallesListaBySisglas", param));
                decimal _ValorfactorM3 = 0;
                if (factorM3 != null)
                    Decimal.TryParse(((DetalleListaVM)factorM3).Valor, out _ValorfactorM3);


                // modificamos el parametro EsKg para identificar si son busetones o articulados
                // 11 para articulados // 12 para busetones segun clasificación de los vehículos en 
                //Mix

                rb.CargarDataset("Parametros", new List<ParametrosReportes>() { new ParametrosReportes() { EsKg = (model.AssetTypeId == 11), ParametroKg = _ValorfactorM3, FechaReporte = fechaFinal , NameMes = fechaFinal.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"))
            } });

                rb.CargarDatos(Datos, vehiculos);
                var bytesFile = rb.ObtenerReporte(extensionFile);
                Response.Clear();
                MemoryStream ms = new MemoryStream(bytesFile);
                r.success();
                return r;
            }
            catch (Exception e)
            {
               

            }       


            r.success();
            return r;
        }

        //[HttpGet("GetAssets/ClienteIds")]
        //public async Task<List<AssetShortDTO>> GetByClienteIds(int? ClienteIds, string UsertState)
        //{
        //    return await _assetService.GetByClienteIdsAsync(ClienteIds, UsertState);
        //}
        ////Obtiene assets
        //[HttpGet("getAssets")]
        //public async Task<ActionResult<ResultObject>> getAssets([Required] long ClienteId)
        //{
        //    return await _assetService.getAssets(ClienteId);
        //}

        ////Obtiene estados TX o los uqe pidan
        //[HttpGet("getEstadosAssets")]
        //public async Task<ActionResult<ResultObject>> getEstadosAssets([Required] int tipoIdS)
        //{
        //    return await _assetService.getEstadosTx(tipoIdS);
        //}

        ////Actualizar assets
        //[HttpPost("updateAssets")]
        //public async Task<ActionResult<ResultObject>> updateAssets([FromBody] AssetsVM assets)
        //{
        //    return await _assetService.updateAssets(assets);
        //}

    }
}
