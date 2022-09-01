
using Syscaf.Service;
using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Linq;
using MiX.Integrate.Shared.Entities.Positions;
using Helper = Syscaf.Common.Helpers.Helpers;
using Syscaf.Common.eBus.Models;
using Syscaf.Data;
using Syscaf.Service.Helpers;
using Syscaf.Common.Models;
using System.Data;
using Syscaf.Common.DataTables;
using Syscaf.Common.Helpers.EBUS;
using Syscaf.Helpers;
using System.Reflection;
using MiX.Integrate.Shared.Entities.Events;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;

namespace Syscaf.Common.eBus
{
    public class eBusClass : IeBusClass
    {

        private readonly IMixIntegrateService _mixIntegrateService;
        private readonly ILogService _logService;
        private readonly ISyscafConn _conprod;
        private readonly SyscafCoreConn _connCore;
        public eBusClass(IMixIntegrateService _mixIntegrateService, ILogService _logService, ISyscafConn _conprod, SyscafCoreConn _connCore)
        {
            this._mixIntegrateService = _mixIntegrateService;
            this._logService = _logService;
            this._conprod = _conprod;
            this._connCore = _connCore; 
        }

        public async Task<EventosActivosVM> GetEventosActivosByClienteId(int ClienteIds, long cliente)
        {

            DateTime fechapeticion = DateTime.Now;
            EventosActivosVM eventos = new EventosActivosVM();
            //  long cliente = _transmisionService.getIdsClientes(Clienteids: ClienteIds)[0];
            // traemos los eventos seGun_ clientes
            string nameProperty = $"ActiveEventoViaje_{ClienteIds}";

            Type type = typeof(ConstanstEventos);         //Get type pointer
            FieldInfo[] fields = type.GetFields();
            var field = fields.Where(w => w.Name.Equals(nameProperty, StringComparison.CurrentCultureIgnoreCase)).First();
            Dictionary<string, long> lsteventos = (Dictionary<string, long>)field.GetValue(nameProperty);

            var listadoeventos = await _mixIntegrateService.GetEventosActivosCreadosPorOrganizacion(cliente,
                lsteventos.Select(s => s.Value).ToList(), ClienteIds);


            if (listadoeventos != null)
            {
                eventos.EventActiveViaje =   listadoeventos.Where(w => lsteventos.Where(ww => ww.Key.Contains("Viaje_")).Any(a => a.Value == w.EventTypeId)
            ).GroupBy(g =>
            {
                DateTime groupDate = new DateTime(g.EventDateTime.Year, g.EventDateTime.Month, g.EventDateTime.Day, g.EventDateTime.Hour, g.EventDateTime.Minute, 0);
                return new
                { Fecha = groupDate, g.AssetId, g.DriverId };
            }).Select(
     s =>
     {
         double distancia = 0;
         var EnergiaDescargadaF = s.Where(w => w.EventTypeId == lsteventos["Viaje_EnergiaDescargada"]).FirstOrDefault();
         var EnergiaRegeneradaF = s.Where(w => w.EventTypeId == lsteventos["Viaje_EnergiaRegenerada"]).FirstOrDefault();
         double? EnergiaDescargada = EnergiaDescargadaF?.Value;
         double? EnergiaRegenerada = EnergiaRegeneradaF?.Value;

         var eventSoc = s.Where(w => w.EventTypeId == lsteventos["Viaje_Soc"]).FirstOrDefault();
         if (eventSoc != null)
             distancia = eventSoc.OdometerKilometres.HasValue ? Convert.ToDouble(eventSoc.OdometerKilometres.Value) : distancia;
         var defaultEventId = s.First().EventId;

         Position Posicion = eventSoc?.Position;


         return new EventosActivosViajeVM
         {
             EventId = eventSoc?.EventId ?? defaultEventId,
             EventTypeId = lsteventos["Viaje_Soc"],
             Fecha = Constants.GetFechaServidor(s.Key.Fecha),
             AssetId = s.Key.AssetId,
             DriverId = s.Key.DriverId,
                                 // Altitud = s.Where(w => w.EventTypeId == lsteventos["Viaje_Altitud"]).FirstOrDefault()?.Value,
                                 EnergiaRegenerada = EnergiaRegenerada,
             EnergiaDescargada = EnergiaDescargada,
             Soc = eventSoc?.Value,
             Distancia = distancia,
             Localizacion = Posicion?.FormattedAddress ?? EnergiaDescargadaF?.Position?.FormattedAddress ?? EnergiaRegeneradaF?.Position?.FormattedAddress,
             Latitud = Posicion?.Latitude,
             Longitud = Posicion?.Longitude,
             Autonomia = 0,
             VelocidadPromedio = 0

         };
     }
     ).ToList();

                var lstsocmax = listadoeventos.Where(w => w.EventTypeId == lsteventos["Soc_Max"]).Select(s =>
                 new EventosActivosViajeVM
                 {
                     EventId = s.EventId,
                     EventTypeId = s.EventTypeId,
                     Fecha = Constants.GetFechaServidor(s.EventDateTime),
                     AssetId = s.AssetId,
                     DriverId = s.DriverId,
                     Soc = s.Value,
                     Distancia = s.OdometerKilometres.HasValue ? Convert.ToDouble(s.OdometerKilometres.Value) : 0,
                     Latitud = s.Position?.Latitude,
                     Longitud = s.Position?.Longitude

                 });
                if (lstsocmax.Count() > 0)
                    eventos.EventActiveViaje = eventos.EventActiveViaje.Union(lstsocmax).ToList();

                eventos.EventActiveRecarga = listadoeventos.Where(w => lsteventos.Where(ww => ww.Key.Contains("Gun_")).Any(a => a.Value == w.EventTypeId)
              ).GroupBy(g => new
              { Fecha = g.EventDateTime.AddTicks(-(g.EventDateTime.Ticks % TimeSpan.TicksPerSecond)), g.AssetId, g.DriverId }).Select(
                   s =>
                   {
                       double? Corriente = s.Where(w => w.EventTypeId == lsteventos["Gun_Curr"]).FirstOrDefault()?.Value;
                       double? Voltaje = s.Where(w => w.EventTypeId == lsteventos["Gun_Voltag"]).FirstOrDefault()?.Value;
                       var eventSoc = s.Where(w => w.EventTypeId == lsteventos["Gun_Soc"]).FirstOrDefault();
                       var defaultEventId = s.First().EventId;

                       return new EventosActivosRecargaVM
                       {
                           EventId = eventSoc?.EventId ?? defaultEventId,
                           EventTypeId = lsteventos["Gun_Soc"],
                           Fecha = Constants.GetFechaServidor(s.Key.Fecha),
                           AssetId = s.Key.AssetId.ToString(),
                           DriverId = s.Key.DriverId.ToString(),
                           Soc = eventSoc?.Value,
                           Corriente = Corriente,
                           Voltaje = Voltaje,
                           Energia = 0,
                           ETA = 0,
                           Odometer = eventSoc?.Value

                       };
                   }
                   ).ToList();

                var listConnectDisconecc = listadoeventos.Where(w => lsteventos.Where(ww => ww.Key.Contains("PG_")).Any(a => a.Value == w.EventTypeId)
                  ).Select(s => new EventosActivosRecargaVM
                  {
                      EventId = s.EventId,
                      EventTypeId = s.EventTypeId,
                      Fecha = Constants.GetFechaServidor(s.EventDateTime),
                      AssetId = s.AssetId.ToString(),
                      DriverId = s.DriverId.ToString(),
                      Consecutivo = (s.EventTypeId == lsteventos["PG_DescConnected"]) ? 2 : 1,
                      Soc = s.Value

                  }).Distinct();
                // filtramos los eventos del vehiculo
                if (listConnectDisconecc.Count() > 0)
                    eventos.EventActiveRecarga = eventos.EventActiveRecarga.Union(listConnectDisconecc).ToList();
            }

            return eventos;

        }

        public async Task<IList<Event>> GetEventosClientePorAssets(List<long> organizacion, List<long> eventosimportante, DateTime FechaInicial, DateTime FechaFinal, int clienteIdS)
        {

            return await _mixIntegrateService.GetEventosClientePorAssets(organizacion, eventosimportante, FechaInicial, FechaFinal, clienteIdS);
        }
     

    }


    public interface IeBusClass
    {
        Task<EventosActivosVM> GetEventosActivosByClienteId(int ClienteIds, long cliente);
      Task<IList<Event>> GetEventosClientePorAssets(List<long> organizacion, List<long> eventosimportante, DateTime FechaInicial, DateTime FechaFinal, int clienteIdS);
    

    }
}