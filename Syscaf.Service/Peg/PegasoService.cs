using Microsoft.Extensions.Options;
using MiX.Integrate.Shared.Entities.Positions;
using Newtonsoft.Json;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Services;
using Syscaf.Common.Utils;
using Syscaf.Data;
using Syscaf.Service.Helpers;
using Syscaf.Service.Peg.Models;
using Syscaf.Service.Portal;
using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Syscaf.Service.Peg
{
    public class PegasoService : IPegasoService
    {
        readonly ILogService _logService;
        private readonly ISyscafConn _conprod;
        private readonly PegVariablesConn _pegConn;
        private readonly IDriverService _driverService;
        private readonly IAssetsService _assetsService;
        private readonly IMixIntegrateService _Mix;
        public PegasoService(ILogService _logService, ISyscafConn _conprod, IOptions<PegVariablesConn> _pegConn,
            IDriverService _driverService, IAssetsService _assetsService, IMixIntegrateService _Mix)
        {
            this._logService = _logService;
            this._conprod = _conprod;
            this._pegConn = _pegConn.Value;
            this._driverService = _driverService;
            this._assetsService = _assetsService;
            this._Mix = _Mix;
        }
        #region CONSULTAS BD
        private string _getToken = @"select Token from peg.Token
                                    where ExpirationDate > @ExpirationDate";
        private string _insertLog = @"insert into peg.LogPeticion (Peticion, Respuesta, FechaRespuesta, FechaSistema)
                                    values (@Peticion, @Respuesta, @FechaRespuesta, @FechaSistema)";
        private string _getCodigoEventos = @"select Id, EventTypeId, Description from peg.CodigoEvento
                                                where ClienteIds = @ClienteIds";
        #endregion
        public async Task<string> GetToken()
        {
            try
            {
                return await _conprod.GetAsync<string>(_getToken, new { ExpirationDate = Constants.GetFechaServidor() }, CommandType.Text);
            }
            catch (Exception ex)
            {
                _logService.SetLog("Pegasus GetToken", "", ex.ToString());
            }

            return null;
        }

        public async Task<ResultObject> SetLogPeticion(string Peticion, string Respuesta, DateTime FechaRespuesta, DateTime FechaSistema)
        {
            ResultObject result = new ResultObject();
            try
            {
                await _conprod.Insert(_getToken, new { Peticion, Respuesta, FechaRespuesta, FechaSistema }, CommandType.Text);
                result.success(null);
            }
            catch (Exception ex)
            {
                result.error(ex.ToString());
                _logService.SetLog("Pegasus SetLogPeticion", "", ex.ToString());
            }
            return result;
        }

        public async Task<ResultObject> SetToken(string token)
        {
            ResultObject result = new ResultObject();
            try
            {
                await _conprod.Insert("Peg.setExpirationToken", new { token }, CommandType.StoredProcedure);
                result.success(null);

            }
            catch (Exception ex)
            {
                result.error(ex.ToString());
                _logService.SetLog("Pegasus SetToken", "", ex.ToString());
            }
            return result;
        }

        public async Task<List<CodigoEvento>> GetCodigosEventos(int ClienteIds)
        {
            try
            {
                return await _conprod.GetAllAsync<CodigoEvento>(_getCodigoEventos, new { ClienteIds }, CommandType.Text);

            }
            catch (Exception ex)
            {
                _logService.SetLog("Pegasus GetToken", "", ex.ToString());
            }

            return null;
        }

        #region PEGASO SERVICIO
        public async Task<string> SendData()
        {
            // obtenemos el token guardado, si ha expirado después de 12 horas debera ejecutarse nuevametne
            string token = await GetToken();
            PegasoServiceConn s = new PegasoServiceConn(_pegConn);
            DateTime fechapeticion = DateTime.Now;

            // si el token expir[o se extra y se almacena nuevamente
            if (token == null)
            {
                token = s.getToken().Result;
                await SetToken(token);
            }
            int clienteIds = 862;

            // creamos la peticion, traemos los codigos de eventos
            var codigos = await GetCodigosEventos(clienteIds);
            var drivers = (await _driverService.GetByClienteIds(clienteIds, null)).Select(s =>
            {
                var propiedades = (IDictionary<string, object>)s;
                long DriverId = (long)propiedades["DriverId"];
                string name = (string)propiedades["name"];
                return new { name, DriverId };
            });

            var cliente = (await _assetsService.GetByClienteIdsAsync(clienteIds, "Available")).Select(s =>
            {
                var propiedades = (IDictionary<string, object>)s;
                long Assetid = (long)propiedades["AssetId"];
                string registrationNumber = (string)propiedades["RegistrationNumber"];
                return new { registrationNumber, Assetid };
            });
            // filtramos los vehiculos
            var listvehiculos = new List<string>() {/* "TGL-199", "TGN-280", "WER-354",*/ "WLS-005" };
            var vehiculos = cliente.Where(w => listvehiculos.Any(a => a.Equals(w.registrationNumber, Constants.comparer)));
            var vehiculosids = vehiculos.Select(ss => ss.Assetid).ToList();

           var listadoeventos = await _Mix.GetEventosActivosCreadosPorVehiculos(vehiculosids, clienteIds);

            // filtramos los eventos del vehiculo
            listadoeventos = listadoeventos.Where(w => codigos.Any(a => a.EventTypeId == w.EventTypeId)).ToList();

            if (listadoeventos.Count > 0)
            {
                CultureInfo us = new CultureInfo("en-US");

                var serializado = listadoeventos.Select(ss =>
                {
                    Position p = ss.Position;

                    var dato = new
                    {
                        dblLatitud = p.Latitude.ToString(us),
                        dblLongitud = p.Longitude.ToString(us),
                        dteFechaReporte = Constants.GetFechaServidor(ss.EventDateTime).ToString(Constants.FormatoPegaso),
                        intVelocidadInstantanea = ((int?)p.SpeedKilometresPerHour)?.ToString(us),
                        strPlaca = Regex.Replace(cliente.Where(w => w.Assetid == ss.AssetId).FirstOrDefault().registrationNumber.Replace('-', ' '), @"\s+", ""),
                        intCodigoEvento = codigos.Find(f => f.EventTypeId == ss.EventTypeId).Id.ToString(us),
                        intAltitud = p.AltitudeMetres?.ToString(us),
                        intDistanciaRecorrida = p?.DistanceSinceReadingKilometres?.ToString(us),
                        intHeading = p.Heading?.ToString(us),
                        intSatelitesUsados = p.NumberOfSatellites?.ToString(us),
                        strDocConductor = drivers.Where(f => f.DriverId == ss.DriverId).FirstOrDefault()?.name,
                        strMensajeEvento = "",
                        strDocTransportadora = "",
                        strDireccion = p.FormattedAddress
                    };

                    return dato;
                });

                string request = JsonConvert.SerializeObject(serializado);
                var y = s.SendRequest(token, request);

                await SetLogPeticion(request, y.Result, fechapeticion, Constants.GetFechaServidor());

            
           }

            return "ok";
        }

        #endregion

    }

    public interface IPegasoService
    {
        Task<string> GetToken();
        Task<ResultObject> SetToken(string token);

        Task<ResultObject> SetLogPeticion(string Peticion, string Respuesta, DateTime FechaRespuesta, DateTime FechaSistema);
        Task<List<CodigoEvento>> GetCodigosEventos(int ClienteIds);
        Task<string> SendData();
    }
}
