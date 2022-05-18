
using AutoMapper;
using Dapper;
using MiX.Integrate.Shared.Entities.Events;
using MiX.Integrate.Shared.Entities.Trips;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.DataTableSql;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal.Models;
using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Syscaf.Common.Helpers.Enums;

namespace Syscaf.Service.Portal
{
    public class PortalMService : IPortalMService
    {
        private readonly IAssetsService _asset;
       
        private readonly IClientService _clientService;
        private readonly IMixIntegrateService _Mix;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        private readonly ICommonService _commonService;
        private readonly ISyscafConn _connDWH;


        public PortalMService(ISyscafConn _connDWH, IAssetsService _asset, 
            IClientService _clientService, IMixIntegrateService _Mix,
            IMapper _mapper, ICommonService _commonService
            )
        {
            this._connDWH = _connDWH;
            this._asset = _asset;
            
            this._clientService = _clientService;
            this._Mix = _Mix;
            this._mapper = _mapper;
            this._commonService = _commonService;
        }
        public async Task<ResultObject> Get_ViajesMetricasPorClientes(int? Clienteids, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            ResultObject result = new();
            // traemos el listado de clientes
            var ListadoClientes = await _clientService.GetAsync(1, (Clienteids.HasValue) ? Clienteids.Value : -1);

            foreach (var item in ListadoClientes)
            {
                //// guardamos los viajes por periodo, se debe determinar a traves de los viajes cual es el perido al que pertenece la informacion y guardarlo al que corresponda
                IList<Trip> trips =
                    (FechaFinal == null && FechaFinal == null) ?
                    await _Mix.GetUltimosViajesCreadosByOrganization(item.clienteId, item.clienteIdS, "GetViajesPortal")
                    :
                    await _Mix.getViajes(new List<long>() { item.clienteId }, FechaInicial.Value, FechaFinal.Value, item.clienteIdS);
                ;

                if (trips != null && trips.Count > 0)
                {
                    trips.GroupBy(g => new { Constants.GetFechaServidor(g.TripStart).Month, Constants.GetFechaServidor(g.TripStart).Year })
                                              .Select(s => new { Period = s.Key.Month.ToString() + s.Key.Year.ToString(), Viajes = s }).ToList().ForEach(async f =>
                                              {
                                                  //// verifica que existan y se manda la fecha desde el cual elsistema empieza a validar si existen
                                                  var ResultTrips = await GetIdsNoIngresadosByClienteAsync(f.Viajes.Select(s => s.TripId).ToList(), f.Period, (int)Enums.PortalTipoValidacion.viajes, item.clienteIdS);
                                                  //// cruzamos el resultado con el listado general de eventos
                                                  var tripsFilters = f.Viajes.Where(w => ResultTrips.Any(a => a == w.TripId)).ToList();

                                                  if (tripsFilters.Count > 0)
                                                  {

                                                      var tripsInserts = _mapper.Map<List<TripsNew>>(tripsFilters);
                                                       result = await SetDatosPortalByClienteAsync(HelperDatatable.ToDataTable(tripsInserts), f.Period, "Trips", item.clienteIdS);
                                                      if (!result.Exitoso)
                                                          _logService.SetLogError(0, "Inserta Trips ", $"Cliente: {item.clienteNombre} , {result.Mensaje}");
                                                  }
                                              });

                    if (item.Metrics)
                    {
                        DateTime minDateMetricas = trips.Min(min => min.TripStart);
                        minDateMetricas = minDateMetricas.Date;
                        DateTime maxDateMetricas = trips.Max(max => max.TripEnd);
                        maxDateMetricas = maxDateMetricas.Date;

                        while (minDateMetricas <= maxDateMetricas)
                        {

                            DateTime fechaconsulta = minDateMetricas;
                            minDateMetricas = minDateMetricas.AddDays(1);
                            DateTime fechaconsultaf = minDateMetricas;
                            var totalTrips = trips.Where(w => w.TripStart >= fechaconsulta && w.TripStart <= fechaconsultaf);

                            if (totalTrips.Count() > 0)
                            {
                                //  treemos las metricas por rangos no superiores a  7 días dependiendo del los rangos de fechas
                                List<TripRibasMetrics> metricasMix = await _Mix.GetMetricasPorDriver(trips.Select(s => s.DriverId).Distinct().ToList(), fechaconsulta.AddHours(-5), fechaconsultaf.AddHours(-5), item.clienteIdS);


                                metricasMix
                                    .GroupBy(g => new { Constants.GetFechaServidor(g.TripStart).Month, Constants.GetFechaServidor(g.TripStart).Year })
                                    .Select(s => new { Period = s.Key.Month.ToString() + s.Key.Year.ToString(), Metricas = s }).ToList().ForEach(async f =>
                                    {

                                        // verifica que existan 
                                        var ResultMetricas = await GetIdsNoIngresadosByClienteAsync(f.Metricas.Select(s => s.TripId).ToList(), f.Period, (int)Enums.PortalTipoValidacion.metricas, item.clienteIdS);

                                        // cruzamos el resultado con el listado general de eventos
                                        var metricasFilter = f.Metricas.Where(w => ResultMetricas.Any(a => a == w.TripId)).ToList();

                                        if (metricasFilter.Count > 0)
                                        {
                                            var tripsMetris = _mapper.Map<List<MetricsNew>>(metricasFilter);


                                            result=   await SetDatosPortalByClienteAsync(HelperDatatable.ToDataTable(tripsMetris), f.Period, "TripsMetrics", item.clienteIdS);
                                           
                                            if (!result.Exitoso)
                                                _logService.SetLogError(0, "Inserta Trips Metrics ", $"Cliente: {item.clienteNombre} , {result.Mensaje}");
                                        }

                                    });

                            }

                        }


                    }
                }
            }

            return result;
        }


        // ingreso de informacion del reporte sotramac
        #region REPORTE SOTRAMAC
        #endregion

     
        #region GUARDA DATOS TABLAS PORTAL

        public async Task<ResultObject> SetDatosPortalByClienteAsync(DataTable data, string Periodo, string tabla, int Clienteids)
        {
            ResultObject resultado = new ResultObject();
            try
            {
                using (SyscafBD ctx = new SyscafBD())
                {
                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Clienteids", Clienteids, DbType.Int32);
                    parametros.Add("Period", Periodo, DbType.String);
                    parametros.Add($"Data{tabla}", data.AsTableValuedParameter($"PORTAL.UDT_{tabla}"));

                    //  traemos la información de los identificadores que no existen en la base de datos                   
                    await Task.FromResult(_connDWH.Execute(PortalQueryHelper._guardaTablasPortal(tabla), parametros, commandType: CommandType.StoredProcedure));

                    resultado.success(null);
                }
            }
            catch (Exception ex)
            {
                resultado.error(ex.ToString());
            }
            return resultado;
        }

        #endregion
        #region VERIFICA IDS
        // trae de la bd los identificadoes no ingresados en el sietma
        public async Task<List<long>> GetIdsNoIngresadosByClienteAsync(List<long> Ids, string Periodo, int tipo, int Clienteids)
        {
            try
            {
                using (SyscafBD ctx = new SyscafBD())
                {

                    var parametros = new Dapper.DynamicParameters();
                    var IdsList = Ids.Select(s => new { Id = s }).ToList();
                    parametros.Add("Clienteids", Clienteids, DbType.Int32);
                    parametros.Add("Period", Periodo, DbType.String);
                    parametros.Add("Table", tipo, DbType.Int32);
                    parametros.Add("Data", HelperDatatable.ToDataTable(IdsList).AsTableValuedParameter("dbo.UDT_TableIdentity"));

                    //  traemos la información de los identificadores que no existen en la base de datos                   
                    return await Task.FromResult(_connDWH.GetAll<long>(PortalQueryHelper._verificaIdExistentes, parametros, commandType: CommandType.StoredProcedure));
                    // trae la información de eventos 
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
    public interface IPortalMService
    {
        Task<ResultObject> Get_ViajesMetricasPorClientes(int? Clienteids, DateTime? FechaInicial, DateTime? FechaFinal);

    }
}
