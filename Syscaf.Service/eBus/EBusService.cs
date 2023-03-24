using Dapper;
using Syscaf.Common.DataTables;
using Syscaf.Common.eBus;
using Syscaf.Common.eBus.Models;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Integrate.PORTAL;
using Syscaf.Common.Models;
using Syscaf.Common.Models.ADM;
using Syscaf.Data;
using Syscaf.Data.Helpers.eBus;
using Syscaf.Data.Helpers.eBus.Gcp;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Data.Models.Auth;
using Syscaf.Service.eBus.Models;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.eBus
{
    public class EBusService : IEBusService
    {
      
        private readonly ISyscafConn _conprod;
        private readonly SyscafCoreConn _connCore;
        private readonly IClientService _clienteService;
        private readonly ILogService _logService;
        private readonly IeBusClass ieBusClass;
        private readonly IAdmService _admService;
        public EBusService(ISyscafConn _conprod, SyscafCoreConn _connCore, IClientService _clienteService, IeBusClass ieBusClass, ILogService logService, IAdmService _admService)
        {
            this._conprod = _conprod;
            this._connCore = _connCore;
            this._clienteService = _clienteService;
            this.ieBusClass = ieBusClass;
            this._logService = logService;
            this._admService = _admService;
        }
       
        public async Task<List<ParametrizacionVM>> ConsultarTiempoActualizacion(int ClienteId)
        {
            try
            {
                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "GetParametrizacionPorTipo" }, commandType: CommandType.Text);
                return await _conprod.GetAllAsync<ParametrizacionVM>(consulta, new { TipoParametroId = (int)ebusEnum.TipoParametro.Tiempos_Actualizacion, ClienteIds  = ClienteId }, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logService.SetLog("Ebusc - Class ConsultarTiempoActualizacion", "", ex.ToString());
            }

            return null;
        }

        public async Task<List<ParametrizacionVM>> ConsultarTiempoActualizacion(int ClienteId, int tipo)
        {
            try
            {
                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "GetParametrizacionPorTipo" }, commandType: CommandType.Text);

                return await _conprod.GetAllAsync<ParametrizacionVM>(consulta, new { TipoParametroId = tipo }, commandType: CommandType.Text);

            }
            catch (Exception ex)
            {
                _logService.SetLog("Ebusc - Class ConsultarTiempoActualizacion", "", ex.ToString());
            }

            return null;
        }
        public async Task<List<ParqueoInteligenteVM>> GetUltimaPosicionVehiculos(int ClienteIds, string Periodo)
        {
            List<ParqueoInteligenteVM> result = new List<ParqueoInteligenteVM>() { };
            try
            {
                return await _conprod.GetAllAsync<ParqueoInteligenteVM>("EBUS.GetUltimaPosicionVehiculos", new { ClienteIds, Periodo });

            }
            catch (Exception ex)
            {
                _logService.SetLog("Ebusc - GetUltimaPosicionVehiculos", "", ex.ToString());
            }
            return result;
        }
        public async Task<ResultObject> SetEventosActivos(string Period, int Clienteids)
        {
            ResultObject resultado = new ResultObject();

            try
            {

                DataTable dt = EbusDT.GetDTEventActiveViaje();
                DataTable dtRec = EbusDT.GetDTEventActiveRecarga();
                var cliente = (await _clienteService.GetAsync(1, clienteIds: Clienteids)).First();

                var eventosactivos = await ieBusClass.GetEventosActivosByClienteId(Clienteids, cliente.clienteId);



                if (eventosactivos.EventActiveRecarga != null)
                    eventosactivos.EventActiveRecarga.ForEach(f =>
                    {
                        dtRec.Rows.Add(f.EventId, f.Fecha, f.EventTypeId, f.Consecutivo, f.Carga, f.AssetId, f.DriverId, f.Soc,
                            f.Corriente, f.Voltaje, f.Potencia, f.Energia, f.ETA, f.Odometer, f.Latitud, f.Longitud, Constants.GetFechaServidor());
                    });

                if (eventosactivos.EventActiveViaje != null)
                    eventosactivos.EventActiveViaje.ForEach(f =>
                    {
                        dt.Rows.Add(f.EventId, f.Fecha, f.EventTypeId, f.AssetId, f.DriverId, f.Altitud, f.EnergiaRegenerada, f.EnergiaDescargada, f.Soc, f.Energia, f.PorRegeneracion
                            , f.Distancia, f.Localizacion, f.Latitud, f.Longitud, f.Autonomia, f.VelocidadPromedio, Constants.GetFechaServidor());
                    });


                var parametros = new Dapper.DynamicParameters();
                parametros.Add("Day", Period, DbType.String);
                parametros.Add("Clienteids", Clienteids, DbType.Int32);
                parametros.Add($"EventosActivos", dt.AsTableValuedParameter($"EBUS.UDT_ActiveEventsViaje"));

                // guardamos los eventos de viajes 
                int result = await _conprod.ExecuteAsync("EBUS.AddEventActiveByDayAndClient", parametros);
                // guardamos los eventos de rec argas 
                parametros = new Dapper.DynamicParameters();
                parametros.Add("Day", Period, DbType.String);
                parametros.Add("Clienteids", Clienteids, DbType.Int32);
                parametros.Add($"EventosActivos", dtRec.AsTableValuedParameter($"EBUS.UDT_ActiveEventsRecarga"));

                result = await _conprod.ExecuteAsync("EBUS.AddEventActiveRecargaByDayAndClient @Day ,@Clienteids,  @EventosActivos ", parametros);
                resultado.success(null);

            }
            catch (Exception ex)
            {
                resultado.error(ex.ToString());
                _logService.SetLog("Ebusc - SetEventosActivos", "", ex.ToString());
            }

            return resultado;
        }

        public List<T> getEventosActivosViaje<T>(int clienteids, string period, string command)
        {
            return _conprod.GetAll<T>($"EBUS.GetEventActive{command}ByDayAndClient", new { Day = period, clienteids });
        }

        public async Task<ResultObject> GuardarParametrizacion(ParametroVM Modelo)
        {
            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                Modelo.FechaSistema = Constants.GetFechaServidor();


                if (Modelo.Clave == 1)
                {
                    // el nombre debe ser único,, verificamos antes de guardar la información

                    var _esunico = await ConsultarTiempoActualizacion(Modelo.ClienteIds, Modelo.TipoParametroId);

                    if (_esunico.Count() > 0)
                    {
                        result.Mensaje = "El cliente ya tiene una parametrizacion existente en la base de datos, favor verifique.";
                        return result;
                    }


                    // creamos el objeto de  
                    var Parametro = new
                    {
                        ClienteIds = Modelo.ClienteIds,
                        EsActivo = Modelo.EsActivo,
                        TipoParametroId = Modelo.TipoParametroId,
                        UltimaActualizacion = Modelo.UltimaActualizacion,
                        UsuarioId = Modelo.UsuarioId,
                        Userid = Modelo.Userid,
                        Valor = Modelo.Valor,
                        FechaSistema = Modelo.FechaSistema,
                    };

                    int rowsaffected = await _conprod.ExecuteAsync(eBusQuery.newParametrizacion, Parametro, CommandType.Text);

                    result.success("Operación éxitosa.");

                }
                else if (Modelo.Clave == 2)
                {
                    int rowsaffected = await _conprod.ExecuteAsync(eBusQuery.updateParametrizacion, new
                    {
                        Modelo.ParametrizacionId,
                        Modelo.EsActivo,
                        UltimaActualizacion = Modelo.FechaSistema,
                        Valor = Modelo.Valor,

                    }, CommandType.Text);

                    result.success("Parametrización actualizada exitosamente.");

                }
                else if (Modelo.Clave == 3)
                {
                    int rowsaffected = await _conprod.ExecuteAsync(eBusQuery.updateEstadoParametrizacion, new
                    {
                        Modelo.ParametrizacionId,
                        Modelo.EsActivo

                    }, CommandType.Text);

                    result.success("Se ha cambiado el estado exitosamente.");
                }
                else
                {
                    result.Mensaje = "Clave invalida, no se puede continuar con la operación";
                }

            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                _logService.SetLog("Ebusc - GuardarParametrizacion", "", ex.ToString());
            }
            return result;
        }

        public async Task<DataTableVM> GetListadoParametros(DataTableVM Modelo, List<ApplicationUser> usuarios)
        {
            try
            {
                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "GetlListadoParametros" }, commandType: CommandType.Text);
                var resultado = (await _conprod.GetAllAsync<dynamic>(consulta, null, commandType: CommandType.Text))
                                         .Select(s => new
                                         {
                                             s.UsuarioId,
                                             s.Userid,
                                             s.Valor,
                                             s.Tipo,
                                             s.FechaSistema,
                                             s.ParametrizacionId,
                                             s.ClienteIds,
                                             s.cliente,
                                             s.TipoParametroId,
                                             s.EsActivo,
                                             Usuario = usuarios.Where(w => w.usuarioIdS == s.UsuarioId || w.Id == s.Userid).FirstOrDefault().Nombres
                                         }).ToList();

                if (Modelo.Buscar != null)
                {
                    resultado = (from c in resultado
                                 where (Modelo.Buscar == null)
                                               || c.cliente.ToLower().Contains(Modelo.Buscar.ToLower())
                                               || c.Valor.ToLower().Contains(Modelo.Buscar.ToLower())
                                               || c.Tipo.ToLower().Contains(Modelo.Buscar.ToLower())
                                               || c.Valor.ToLower().Contains(Modelo.Buscar.ToLower())
                                               || c.Usuario.ToLower().Contains(Modelo.Buscar.ToLower())
                                               || Convert.ToString(c.ParametrizacionId).ToLower() == Modelo.Buscar.ToLower()
                                 select c).ToList();
                }

                int fila = (resultado.Count - Modelo.start);

                var FilasMostradas = resultado.GetRange(Modelo.start, fila < Modelo.length ? fila : Modelo.length);

                var resul = (from s in FilasMostradas
                             select new
                             {
                                 s.Usuario,
                                 s.cliente,
                                 s.Valor,
                                 s.Tipo,
                                 s.FechaSistema,
                                 s.EsActivo,
                                 s.ParametrizacionId,
                                 s.ClienteIds,
                                 s.TipoParametroId
                             });

                Modelo.data = resul.ToList();
                Modelo.recordsTotal = resultado.Count;
                Modelo.recordsFiltered = resultado.Count;

                return Modelo;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                _logService.SetLog("Ebusc - GetListadoParametros", "", ex.ToString());
                throw;
            }
        }


        public async Task<DataTableVM> GetListaUsuariosClientes(DataTableVM Modelo, List<ApplicationUser> usuarios)
        {
            try
            {

                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "GetListaUsuariosClientes" }, commandType: CommandType.Text);
                var resultado = (await _conprod.GetAllAsync<dynamic>(consulta, null, commandType: CommandType.Text))
                                         .Select(s => new
                                         {
                                             s.ClienteUsuarioId,
                                             s.UsuarioIdS,
                                             s.clienteIdS,
                                             s.EsActivo,
                                             s.FechaSistema,
                                             s.Userid,
                                             Usuario = usuarios.Where(w => w.usuarioIdS == s.UsuarioIdS || w.Id == s.Userid).FirstOrDefault().Nombres
                                         }).ToList();


                if (Modelo.Buscar != null)
                {
                    resultado = (from c in resultado
                                 where (Modelo.Buscar == null)
                                               || c.Usuario.ToLower().Contains(Modelo.Buscar.ToLower())
                                               || Convert.ToString(c.ClienteUsuarioId).ToLower() == Modelo.Buscar.ToLower()
                                 select c).ToList();
                }

                int fila = (resultado.Count - Modelo.start);

                var FilasMostradas = resultado.GetRange(Modelo.start, fila < Modelo.length ? fila : Modelo.length);

                var resul = (from s in FilasMostradas
                             select new
                             {
                                 s.Usuario,
                                 s.UsuarioIdS,
                                 s.ClienteUsuarioId,
                                 s.clienteIdS,
                                 s.EsActivo,
                                 s.FechaSistema
                             });
                Modelo.data = resul.ToList();
                Modelo.recordsTotal = resultado.Count;
                Modelo.recordsFiltered = resultado.Count;

                return Modelo;
            }
            catch (Exception ex)
            {

                _logService.SetLog("Ebusc - GetListadoParametros", "", ex.ToString());
                throw;
            }
        }

        public async Task<DataTableVM> GetListaClientes(DataTableVM Modelo, string Clientes)
        {

            try
            {

                string[] listadoClientes = Clientes.Split(',');

                var clientes = await _clienteService.GetAsync(1);

                var clientefiltrado = clientes.Where(w => listadoClientes.Any(a => a == w.clienteId.ToString())).ToList();



                if (Modelo.Buscar != null)
                {
                    clientefiltrado = (from c in clientefiltrado
                                       where (Modelo.Buscar == null)
                                                   || c.clienteNombre.ToLower().Contains(Modelo.Buscar.ToLower())
                                       select c).ToList();
                }

                int fila = (clientefiltrado.Count - Modelo.start);

                var FilasMostradas = clientefiltrado.GetRange(Modelo.start, fila < Modelo.length ? fila : Modelo.length);

                Modelo.data = clientefiltrado.ToList();
                Modelo.recordsTotal = clientefiltrado.Count;
                Modelo.recordsFiltered = clientefiltrado.Count;

                return Modelo;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        public List<UsuariosClientesEsomosVM> GetListadoClienteUsuario(int? UsuarioIdS, int? ClienteUsuarioId)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultObject> GetListadoClientesUsuario(string CLientes)
        {
            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteIds", CLientes);
                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "GetClientesUsuariosEsomos" }, commandType: CommandType.Text);
                result.Data = (await _conprod.GetAllAsync<dynamic>(consulta, parametros, commandType: CommandType.Text)).ToList();
                result.Exitoso = true;
            }
            catch (Exception ex)
            {

                result.Mensaje = ex.Message.ToString(); ;
            }
            return result;
        }

        public ResultObject SetAsignacionUsuarioClientes(ClientesUsuarioVM Modelo)
        {
            throw new NotImplementedException();
        }
        //SE CONVIRTIO EN DINAMICA
        public async Task<ResultObject> SetActiveEventCliente(ClienteActiveEventVM Modelo)
        {
            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ActiveEvent", Modelo.ActiveEvent);
                parametros.Add("ClienteId", Modelo.ClienteId);

                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EBUSQueryHelper", NombreConsulta = "SetActiveEventClientes" }, commandType: CommandType.Text);
                result.Data =  await Task.FromResult(_conprod.GetAll<dynamic>(consulta, parametros, commandType:  CommandType.Text));
                result.Mensaje = "Operación Éxitosa";
                result.Exitoso = true;
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message.ToString();
            }
            return result;
        }

        public async Task<ResultObject> GetListaClientesActiveEvent(string? ClienteId)
        {
            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                string consulta = await _connCore.GetAsync<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EBUSQueryHelper", NombreConsulta = "GetClientesEbusActiveEvent" }, commandType: CommandType.Text);
                var resultado = (await _conprod.GetAllAsync<dynamic>(consulta, null, commandType: CommandType.Text))
                                .Select(s => new
                                {
                                    s.clienteNombre,
                                    s.fechaIngreso,
                                    s.ClienteIdS,
                                    clienteId = Convert.ToString(s.clienteId)
                                }).ToList();

                result.Data = resultado;
                result.Exitoso = true;
            }
            catch (Exception ex)
            {
                result.Mensaje =  ex.Message.ToString();
            }
            return result;
        }

        public ResultObject GetClientesUsuariosSelect(int UsuarioIdS, int? ClienteIds)
        {
            throw new NotImplementedException();
        }

        public Task<ResultObject> RellenoPowerBI(int clienteids, DateTime period, DateTime? FechaInicial, DateTime? FechaFinal)
        {
            throw new NotImplementedException();
        }

        public List<EficienciaVM> GetReporteEficiencia(int ClienteIds, string Reporte)
        {
            throw new NotImplementedException();
        }

        public List<ZPVM> GetReporteZP(int ClienteIds, string Reporte)
        {
            throw new NotImplementedException();
        }

        public List<AlarmasVM> GetReporteAlarmas(int ClienteIds, string Reporte)
        {
            throw new NotImplementedException();
        }

        public List<Viajes1MinVM> GetReporteViajesMin(int ClienteIds, string Reporte)
        {
            throw new NotImplementedException();
        }

        public void SetReporte(int ClienteIds, string Reporte, string ReporteIds)
        {
            throw new NotImplementedException();
        }

        public Task<ResultObject> GetDataEventosSomos(int ClienteIds, DateTime? fecha, DateTime? fecha2)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultObject> SetColumnasDatatable(ConfiguracionDatatableVM Modelo)
        {
            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("Clave", Modelo.Clave);
                parametros.Add("Columna", Modelo.Columna);
                parametros.Add("ConfiguracionDatatableId", Modelo.ConfiguracionDatatableId);
                parametros.Add("UsuarioIds", Modelo.UsuarioIds);
                parametros.Add("IdTabla", Modelo.IdTabla);
                parametros.Add("OpcionId", Modelo.OpcionId);
                parametros.Add("FechaSistema", Modelo.FechaSistema);
                dynamic consulta = await _connCore.GetAsync<dynamic>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EBUSQueryHelper", NombreConsulta = "SetColumnasDatatable" }, commandType: CommandType.Text);
                result.Exitoso = true;
                return await Task.FromResult(_conprod.GetAll<dynamic>(consulta.Consulta, parametros, commandType: (consulta.Tipo == 2) ? CommandType.Text : CommandType.StoredProcedure));
            }
            catch (Exception ex)
            {
                result.Mensaje =  ex.ToString();
            }
            return result;
        }

        public async  Task<List<Object>> GetColumnasDatatable(int OpcionId, int UsuarioIds, string IdTabla)
        {
            List<Object> result = new List<Object>();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("OpcionId", OpcionId);
                parametros.Add("UsuarioIds", UsuarioIds);
                parametros.Add("IdTabla", IdTabla);
                dynamic consulta = await _connCore.GetAsync<dynamic>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EBUSQueryHelper", NombreConsulta = "GetColumnasDatatable" }, commandType: CommandType.Text);
                return await Task.FromResult(_conprod.GetAll<dynamic>(consulta.Consulta, parametros, commandType: (consulta.Tipo == 2) ? CommandType.Text : CommandType.StoredProcedure));
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
            return result;
        }
        public async Task<ResultObject> GetClientesUsuarios(string? UsuarioID, string UserId)
        {
            ResultObject resul = new ResultObject() { Exitoso = false };
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("usuarioids", UsuarioID.ToString());
                parametros.Add("userid", UserId);
                dynamic consulta = await _connCore.GetAsync<dynamic>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "getListClienteAsignados" }, commandType: CommandType.Text);
                resul.Data = await Task.FromResult(_conprod.GetAll<dynamic>(consulta.Consulta, parametros, commandType: (consulta.Tipo == 2) ? CommandType.Text : CommandType.StoredProcedure));
                resul.Exitoso = true;
            }
            catch (Exception ex)
            {
                resul.Mensaje = ex.ToString();
            }
            return resul;
        }
        public ResultObject GetOpcionesOganizacion(int OrganizacionId)
        {
            throw new NotImplementedException();
        }

        public List<ItemClass> GetListReportePowerBI(int OpcionId, int UsuarioIds)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultObject> GetLocations(string ClienteId, bool? IsParqueo)
        {
            ResultObject resul = new ResultObject() { Exitoso = false };
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteId", ClienteId);
                parametros.Add("IsParqueo", IsParqueo);
                dynamic consulta = await _connCore.GetAsync<dynamic>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "GetLocacionesClientes" }, commandType: CommandType.Text);
                resul.Data = await Task.FromResult(_conprod.GetAll<dynamic>(consulta.Consulta, parametros, commandType:  CommandType.Text));
                resul.Exitoso = true;
            }
            catch (Exception ex)
            {
                resul.Mensaje = ex.Message.ToString();
            }
            return resul;
        }

        public ResultObject SetLocations(int ClienteIds, bool IsParqueo, List<long> Locations)
        {
            throw new NotImplementedException();
        }

        public ResultObject setEstadoUsuariosClientes(int ClienteUsuarioId, bool Estado, int ClienteIds)
        {
            throw new NotImplementedException();
        }

        public ResultObject SetTiempoActualizacion(int ClienteId, string Tiempo, int UsuarioIds)
        {

            throw new NotImplementedException();
        }

        public ResultObject setAsignarUsuarios(ClientesUsuarioVM Modelo)
        {
            throw new NotImplementedException();
        }

        public Task<ResultObject> SetEventosHistoricalActivos(string Period, int Clienteids, DateTime fi, DateTime ff)
        {
            throw new NotImplementedException();
        }

        public List<RecargasHistoricalVM> GetReporteRecargasHistorical(int ClienteIds, string Reporte)
        {
            throw new NotImplementedException();
        }

        public List<ClientesEsomosVM> GetClientesEsomos(int? UsuarioIdS)
        {
            throw new NotImplementedException();
        }

        ResultObject IEBusService.GuardarParametrizacion(ParametroVM Modelo)
        {
            throw new NotImplementedException();
        }

        public DataTableVM GetListadoParametros(DataTableVM Modelo)
        {
            throw new NotImplementedException();
        }

        public ResultObject GetTiposParametros(string Sigla)
        {
            throw new NotImplementedException();
        }

        public DataTableVM GetListaUsuariosClientes(DataTableVM Modelo)
        {
            throw new NotImplementedException();
        }

        DataTableVM IEBusService.GetListaClientes(DataTableVM Modelo, string Clientes)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultObject> GetUsuariosEsomos(int? UsuarioIdS, int? OrganzacionId, int? ClienteId)
        {
            ResultObject result = new ResultObject { Exitoso = false};
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("UsuarioIdS", UsuarioIdS);
                parametros.Add("OrganzacionId", OrganzacionId);
                parametros.Add("ClienteId", ClienteId);
                dynamic consulta = await _connCore.GetAsync<dynamic>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase = "EbusQueryHelper", NombreConsulta = "GetUsuariosEsomos" }, commandType: CommandType.Text);
                result.Data = await Task.FromResult(_conprod.GetAll<dynamic>(consulta.Consulta, parametros, commandType: CommandType.Text));
            }
            catch (Exception ex)
            {
                result.Mensaje =  ex.Message.ToString();
            }
            return result;
        }
    }

    public interface IEBusService
    {
       Task<ResultObject> SetEventosActivos(string Period, int Clienteids);
        List<T> getEventosActivosViaje<T>(int clienteids, string period, string command);
        Task<List<ParametrizacionVM>> ConsultarTiempoActualizacion(int ClienteId);
        Task<List<ParqueoInteligenteVM>> GetUltimaPosicionVehiculos(int ClienteIds, string Periodo);
        List<ClientesEsomosVM> GetClientesEsomos(int? UsuarioIdS);
        ResultObject GuardarParametrizacion(ParametroVM Modelo);
        DataTableVM GetListadoParametros(DataTableVM Modelo);
        ResultObject GetTiposParametros(string Sigla);
        DataTableVM GetListaUsuariosClientes(DataTableVM Modelo);
        DataTableVM GetListaClientes(DataTableVM Modelo, string Clientes);
        List<UsuariosClientesEsomosVM> GetListadoClienteUsuario(int? UsuarioIdS, int? ClienteUsuarioId);
        Task<ResultObject> GetListadoClientesUsuario(string CLientes);
        ResultObject SetAsignacionUsuarioClientes(ClientesUsuarioVM Modelo);
        //SE CONVIRTIO EN DINAMICA
        Task<ResultObject> SetActiveEventCliente(ClienteActiveEventVM Modelo);
        Task<ResultObject> GetListaClientesActiveEvent(string? ClienteId);
        ResultObject GetClientesUsuariosSelect(int UsuarioIdS, int? ClienteIds);
        Task<ResultObject> RellenoPowerBI(int clienteids, DateTime period, DateTime? FechaInicial, DateTime? FechaFinal);

        #region tablasPowerBi
        List<EficienciaVM> GetReporteEficiencia(int ClienteIds, string Reporte);
        List<ZPVM> GetReporteZP(int ClienteIds, string Reporte);

        List<AlarmasVM> GetReporteAlarmas(int ClienteIds, string Reporte);
        List<Viajes1MinVM> GetReporteViajesMin(int ClienteIds, string Reporte);
        void SetReporte(int ClienteIds, string Reporte, string ReporteIds);
        Task<ResultObject> GetDataEventosSomos(int ClienteIds, DateTime? fecha, DateTime? fecha2);
        #endregion
        Task<ResultObject> SetColumnasDatatable(ConfiguracionDatatableVM Modelo);
        Task<List<Object>> GetColumnasDatatable(int OpcionId, int UsuarioIds, string IdTabla);
        ResultObject GetOpcionesOganizacion(int OrganizacionId);
        List<ItemClass> GetListReportePowerBI(int OpcionId, int UsuarioIds);
        Task<ResultObject> GetLocations(string ClienteId, bool? IsParqueo);
        ResultObject SetLocations(int ClienteIds, bool IsParqueo, List<long> Locations);
        ResultObject setEstadoUsuariosClientes(int ClienteUsuarioId, bool Estado, int ClienteIds);
        ResultObject SetTiempoActualizacion(int ClienteId, string Tiempo, int UsuarioIds);
        ResultObject setAsignarUsuarios(ClientesUsuarioVM Modelo);
        Task<ResultObject> SetEventosHistoricalActivos(string Period, int Clienteids, DateTime fi, DateTime ff);
        List<RecargasHistoricalVM> GetReporteRecargasHistorical(int ClienteIds, string Reporte);
        Task<ResultObject> GetClientesUsuarios(string? UsuarioID, string UserId);
        Task<ResultObject> GetUsuariosEsomos(int? UsuarioIdS, int? OrganzacionId, int? ClienteId);
    }
}
