using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.FRESH;
using Syscaf.Common.Models.TRANSMISION;
using Syscaf.Common.Services;
using Syscaf.Common.Utils;
using Syscaf.Data;
using Syscaf.Data.Helpers.TX;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;


namespace Syscaf.Service.FreshDesk
{
    public class FreshDeskService : IFreshDeskService
    {
        private readonly FreshdeskVariablesConn _freshConn;
        private readonly IMapper _mapper;
        private readonly ISyscafConn _conprod;
        readonly ILogService _logService;
        private readonly SyscafCoreConn _conn;
        public FreshDeskService(IOptions<FreshdeskVariablesConn> _freshConn, ISyscafConn _conprod, ILogService logService, IMapper _mapper, SyscafCoreConn conn)
        {
            this._conprod = _conprod;
            this._freshConn = _freshConn.Value;
            _logService = logService;
            this._mapper = _mapper;
            _conn = conn;
        }

        public async Task<ResultObject> GetTickets()
        {
            try
            {
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                return await s.GetTickets();
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de tickets", "", ex.ToString());
            }

            return null;
        }
        public async Task<ResultObject> GetTicketsCampos()
        {
            try
            {
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                return await s.GetTicketsFields();
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de campos de los tickets", "", ex.ToString());
            }

            return null;
        }
        public async Task<ResultObject> GetListAgentes()
        {
            try
            {
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                return await s.GetAgents();
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de listado de agentes", "", ex.ToString());
            }

            return null;
        }
        public async Task<ResultObject> GetListaTicketsSemana(DateTime FechaInicial, DateTime FechaFinal)
        {
            try
            {

                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                return await s.GetTicketsSemana(FechaInicial, FechaFinal);
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de tickets", "", ex.ToString());
            }

            return null;
        }
        public async Task<ResultObject> SetSnapShotTickets(List<TicketsVM> json)
        {
            var r = new ResultObject();
            try
            {
                DateTime Fecha = json[0].Fecha;
                var jsonconvert = JsonConvert.SerializeObject(json);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("JSON_STR", jsonconvert);
                parametros.Add("Fecha", Fecha);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<string>(TransmisionQueryHelper._PostSnapShotTickets, parametros, commandType: CommandType.StoredProcedure));
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }

        public async Task<ResultObject> SetAgentes(List<AgentsVM> json)
        {
            var r = new ResultObject();
            try
            {
                var a = json.Select(x => new AgentesVM
                {
                    active = x.contact.active,
                    email = x.contact.email,
                    created_at = x.contact.created_at,
                    id = x.id,
                    language = x.contact.language,
                    mobile = x.contact.mobile,
                    name = x.contact.name,
                    phone = x.contact.phone,
                    updated_at = x.contact.updated_at
                });
                var jsonconvert = JsonConvert.SerializeObject(a);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("JSON_STR", jsonconvert);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<string>(TransmisionQueryHelper._SetAgents, parametros, commandType: CommandType.StoredProcedure));
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }

        public async Task<ResultObject> SetEstados(string json)
        {
            var r = new ResultObject();
            try
            {
                var jsonconvert = JsonConvert.DeserializeObject(json);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("JSON_STR", json);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<string>(TransmisionQueryHelper._SetEstados, parametros, commandType: CommandType.StoredProcedure));
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }

        public async Task<ResultObject> SetTiketBaseDatos(string json)
        {
            var r = new ResultObject();
            try
            {
              //  var jsonconvert = JsonConvert.DeserializeObject(json);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("JSON_STR", json);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<string>(TransmisionQueryHelper._SetTicket, parametros, commandType: CommandType.StoredProcedure));
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> SetPrioridad(string json)
        {
            var r = new ResultObject();
            try
            {
                //  var jsonconvert = JsonConvert.DeserializeObject(json);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("JSON_STR", json);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Insert<string>(TransmisionQueryHelper._SetPrioridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }

        

        public async Task<ResultObject> SetListaTickets(DateTime FechaInicial, DateTime FechaFinal)
        {
            try
            {
                //Se instancian los servicios
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                //Se consulta la lista de agentes
                List<AgentsVM> Agentes = (List<AgentsVM>)s.GetAgents().Result.Data;
                //Se setean en la base de datos
                var opAgentes = await SetAgentes(Agentes);
                //Se consultan los estados
                var datosEstados =  s.GetStatusTickets().Result.Data;
                //se setean los estados
                var resulstados = await SetEstados(datosEstados.ToString());

                var Prioridad = s.GetPrioridad().Result.Data;
                var resulprioridad = await SetPrioridad(Prioridad.ToString());
                //se trae el listado de tickets
                var tickets = await s.GetTicketsSemana(FechaInicial, FechaFinal);
                var a = tickets.Data;
                object j = JsonConvert.SerializeObject(a);
                string result = Convert.ToString(j);
                var resulTikets = await SetTiketBaseDatos(result);



                return tickets; 
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de tickets", "", ex.ToString());
            }

            return null;
        }

        



    }
    public interface IFreshDeskService
    {
        Task<ResultObject> GetTickets();
        Task<ResultObject> GetTicketsCampos();
        Task<ResultObject> GetListAgentes();
        Task<ResultObject> GetListaTicketsSemana(DateTime FechaInicial, DateTime FechaFinal);
        Task<ResultObject> SetListaTickets(DateTime FechaInicial, DateTime FechaFinal);
    }
}
