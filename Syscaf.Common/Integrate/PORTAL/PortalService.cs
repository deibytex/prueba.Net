using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Data.Interface;
using Syscaf.Data.Models;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.DataTableSql;
using Syscaf.Service.Helpers;
using Syscaf.Service.ViewModels.PORTAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.PORTAL
{
    public class PortalService : IPortalService
    {
        private readonly ILogService _logService;
        private readonly ISyscafConn _conn;
        public PortalService(ISyscafConn conn, ILogService _logService)
        {
            _conn = conn;
            this._logService = _logService;
        }

        public List<CallMethodMixVM> GetCallsMethod(int CredencialId)
        {
            List<CallMethodMixVM> result = null;
            Task task = Task.Run(() =>
                 {
                     try
                     {
                         string sqlCommand = "PORTAL.[GetCallsMethod] @CredencialId";
                         var parametros = new Dapper.DynamicParameters();
                         parametros.Add("CredencialId", CredencialId, DbType.Int32);
                         result = _conn.GetAll<CallMethodMixVM>(sqlCommand, parametros).ToList();
                     }
                     catch (Exception ex)
                     {
                         _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());

                     }
                 });
            task.Wait();
            return result;
        }

        // trae la informacion de todas las creenciaels por clientes
        // las credenciales deben almacenarce en la cache ya que son datos que no cambian
        // de manera seguida 
        public List<CredencialesMixVM> GetCredencialesClientes()
        {
            try
            {
                string sqlCommand = "PORTAL.GetCredentialClients";
                return _conn.GetAll<CredencialesMixVM>(sqlCommand, null).ToList();


            }
            catch (Exception ex)
            {
                _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());
                return null;
            }
        }
        // trae el token que se necesita para traer la informacion en vivo
        // se le debera pasar el cliente y el metodo para verificarlo
        public string GetTokenClientes(int ClienteIds, string Method, string SinceToken = null)
        {
            string valor = null;
            Task task = Task.Run(() =>
            {
                try
                {


                    string sqlCommand = "PORTAL.[GetSinceTokenMethodByCliente]  @Clienteid , @Method,@fechasistema, @SinceToken ";
                    //var param = new SqlParameter()
                    //{
                    //    ParameterName = "SinceToken",
                    //    DbType = DbType.String,
                    //    Value = DBNull.Value
                    //};

                    //if (SinceToken != null)
                    //    param.Value = SinceToken;

                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Clienteid", ClienteIds, DbType.Int32);
                    parametros.Add("Method", Method, DbType.String);
                    parametros.Add("fechasistema", Constants.GetFechaServidor(), DbType.DateTime);
                    parametros.Add("SinceToken", SinceToken, DbType.String);
                    valor = _conn.GetAll<string>(sqlCommand, parametros).FirstOrDefault();



                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());

                }
            });

            task.Wait();

            return valor;
        }

        // trae todos los token de todos los clientes en lista 
        public List<SinceTokenVM> GetTokenClientes()
        {
            throw new NotImplementedException();
        }

        // trae todos los token asociados a los metodos de cliente
        public List<SinceTokenVM> GetTokenClientes(int ClienteIds)
        {
            throw new NotImplementedException();
        }

        public Task SetCallsMethod(int CredencialId, DateTime date, DateTime dateHour, int HourCall, int MinuteCall)
        {
            Task tsk = Task.Run(() =>
            {
                try
                {
                    string sqlCommand = "PORTAL.SetCallsMethod  @CredencialId ,  @date, @dateHour ,@HourCall  ,  @MinuteCall";
                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("CredencialId", CredencialId, DbType.Int32);
                    parametros.Add("date", date, DbType.DateTime);
                    parametros.Add("HourCall", HourCall, DbType.Int32);
                    parametros.Add("MinuteCall", MinuteCall, DbType.Int32);
                    parametros.Add("dateHour", dateHour, DbType.DateTime);
                    _conn.Execute(sqlCommand, parametros);

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());

                }
            });
            tsk.Wait();
            return tsk;
        }

        // guarda el log de uso de las llamadas de los clientes
        // se debe validar que no se exceda por credencial las 20 llamadas por minuto ni 500 por hora
        public void SetLog(int CredencialId, string Method, int StatusResponse, string Response, DateTime FechaSistema, int clienteids)
        {
            try
            {
                string sqlCommand = "PORTAL.SetLog  @CredencialId ,  @Method ,@StatusResponse  ,  @Response ,@FechaSistema, @Clienteids ";
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("CredencialId", CredencialId, DbType.Int32);
                parametros.Add("Method", Method, DbType.String);
                parametros.Add("StatusResponse", StatusResponse, DbType.Int32);
                parametros.Add("Response", Response, DbType.String);
                parametros.Add("FechaSistema", Response, DbType.DateTime);
                parametros.Add("Clienteids", clienteids, DbType.Int32);
                _conn.Execute(sqlCommand, parametros);


            }
            catch (Exception ex)
            {
                _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());

            }
        }


       

        public List<long> GetIdsNoIngresados(List<long> Ids, string Periodo, int tipo)
        {
            List<long> resultado = new List<long>();
            Task task = Task.Run(() =>
            {
                try
                {

                    var dtIds = HelperDatatable.GetDataTableIdentity();
                    // los guarda en el DataTable
                    Ids.ForEach(e =>
                     dtIds.Rows.Add(e));

                    //  traemos la información de los identificadores que no existen en la base de datos

                    //var parmCliente = new SqlParameter("Period", SqlDbType.VarChar)
                    //{
                    //    Value = Periodo
                    //};

                    //var parmTipo = new SqlParameter("Table", SqlDbType.Int)
                    //{
                    //    Value = tipo
                    //};

                    //var parmIds = new SqlParameter("Data", SqlDbType.Structured)
                    //{
                    //    Value = dtIds,
                    //    TypeName = "dbo.UDT_TableIdentity"
                    //};
                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Period", Periodo, DbType.String);
                    parametros.Add("Table", tipo, DbType.Int32);
                    parametros.Add("Data", dtIds);
                    // trae la información de eventos 
                    resultado = _conn.GetAll<long>("[PORTAL].[VerifyDataStageByPeriod] @Period, @Table, @Data ", parametros).ToList();


                }
                catch (Exception exp)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), exp.ToString());

                }
            });

            task.Wait();

            return resultado;

        }

        public ResultObject SetDatosPortal(DataTable data, string Periodo, string tabla)
        {
            ResultObject resultado = new();
            Task task = Task.Run(() =>
            {
                try
                {
                    //  traemos la información de los identificadores que no existen en la base de datos

                    //var parmIds = new SqlParameter("Data", SqlDbType.Structured)
                    //{
                    //    Value = data,
                    //    TypeName = $"[PORTAL].[UDT_{tabla}]"
                    //};
                    // trae la información de eventos 

                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Period", Periodo, DbType.String);

                    parametros.Add("Data", data);
                    // trae la información de eventos 
                    _conn.Execute($"[PORTAL].[Insert{tabla}] @Period,  @Data ", parametros);
                    resultado.success(null);
                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());

                    resultado.error(ex.ToString());
                }
            });

            task.Wait();

            return resultado;

        }


        public List<long> GetIdsNoIngresadosByCliente(List<long> Ids, string Periodo, int tipo, int Clienteids)
        {
            List<long> resultado = new();
            Task task = Task.Run(() =>
            {
                try
                {

                    var dtIds = HelperDatatable.GetDataTableIdentity();
                    // los guarda en el DataTable
                    Ids.ForEach(e =>
                     dtIds.Rows.Add(e));

                    //  traemos la información de los identificadores que no existen en la base de datos


                    //var parmIds = new SqlParameter("Data", SqlDbType.Structured)
                    //{
                    //    Value = dtIds,
                    //    TypeName = "dbo.UDT_TableIdentity"
                    //};


                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Period", Periodo, DbType.String);
                    parametros.Add("Clienteids", Clienteids, DbType.Int32);
                    parametros.Add("Table", tipo, DbType.Int32);
                    parametros.Add("Data", dtIds);
                    // trae la información de eventos 
                    resultado = _conn.GetAll<long>("[PORTAL].[VerifyDataStageByPeriodAndClient] @Period, @Clienteids, @Table, @Data ", parametros).ToList();
                }
                catch (Exception exp)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), exp.ToString());

                    throw;
                }
            });

            task.Wait();

            return resultado;

        }

        public ResultObject SetDatosPortalByCliente(DataTable data, string Periodo, string tabla, int Clienteids)
        {
            ResultObject resultado = new ResultObject();
            Task task = Task.Run(() =>
            {
                try
                {

                    //var parmIds = new SqlParameter("Data", SqlDbType.Structured)
                    //{
                    //    Value = data,
                    //    TypeName = $"[PORTAL].[UDT_{tabla}]"
                    //};
                    // trae la información de eventos 


                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Period", Periodo, DbType.String);
                    parametros.Add("Clienteids", Clienteids, DbType.Int32);
                    parametros.Add("Data", data);
                    // trae la información de eventos 
                    _conn.Execute($"[PORTAL].[Insert{tabla}ByPeriodAndClient] @Period, @Clienteids,  @Data ", parametros);
                    resultado.success(null);

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());

                    resultado.error(ex.ToString());
                }
            });

            task.Wait();

            return resultado;

        }
        public ResultObject SetUpdateLocations(DataTable Locations)
        {
            ResultObject result = new ResultObject();

            Task task = Task.Run(() =>
            {
                try
                {


                    //var parmIds = new SqlParameter("Locations", SqlDbType.Structured)
                    //{
                    //    Value = Locations,
                    //    TypeName = "PORTAL.Locations"
                    //};

                    // trae la información de eventos 

                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("Locations", Locations);
                    // trae la información de eventos 
                    _conn.Execute("[PORTAL].[SetUpdateLocations] @Locations ", parametros);
                    result.success(null);


                }
                catch (Exception exp)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), exp.ToString());

                    result.error(exp.ToString());
                }
            });

            task.Wait();

            return result;

        }

        public ResultObject SetLogUsuario(int UsuarioId, DateTime Fecha, bool isLogin)
        {
            ResultObject result = new();

            Task task = Task.Run(() =>
            {
                try
                {

                    // trae la información de eventos 
                    var parametros = new Dapper.DynamicParameters();
                    parametros.Add("UsuarioId", UsuarioId, DbType.Int32);
                    parametros.Add("Fecha", Fecha, DbType.DateTime);
                    parametros.Add("Login", isLogin, DbType.Byte);
                    // trae la información de eventos 
                    _conn.Execute("[PORTAL].[SetLogUsuario]  @UsuarioId,@Fecha, @Login ", parametros);
                    result.success(null);

                }
                catch (Exception exp)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), exp.ToString());

                    result.error(exp.ToString());
                }
            });

            task.Wait();
            return result;

        }

        public List<ClienteMetricas> ValidateAllMetrics(string Periodo)
        {
            List<ClienteMetricas> resultado = new();
            Task task = Task.Run(async () =>
            {
                try
                {
                    // trae la información de eventos 
                    resultado = await _conn.GetAll<ClienteMetricas>($"[PORTAL].[ValidateAllMetrics] @Period", new { Period = Periodo });

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());

                    throw;
                }
            });
            task.Wait();
            return resultado;
        }

        public ResultObject SetLogOpcionUsuario(int UsuarioID, DateTime Fecha, int Opcion, string nombreOpcion)
        {
            ResultObject result = new ResultObject();

            Task task = Task.Run(() =>
            {
                try
                {

                    var parametros = new
                    {
                        UsuarioId = UsuarioID,
                        Date = Fecha,
                        OpcionId = Opcion,
                        Nombre = nombreOpcion
                    };

                    // trae la información de eventos 
                    _conn.Execute("[PORTAL].[InsertLogOpciones]  @UsuarioId,@OpcionId, @Nombre,@Date", parametros);
                    result.success();


                }
                catch (Exception exp)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), exp.ToString());

                    result.error(exp.ToString());
                }
            });

            task.Wait();
            return result;
        }

        public ResultObject SIG_GeneraSenialesPorDia(DateTime FechaInicial, DateTime FechaFinal, int Lote)
        {
            ResultObject result = new ResultObject();

            Task task = Task.Run(() =>
            {
                try
                {
                    var parametros = new
                    {
                        Fi = FechaInicial,
                        Ff = FechaFinal,
                        Lote
                    };
                    // trae la información de eventos 
                    _conn.Execute("SIG.GeneraSenialesPorDia  @Fi,@Ff, @Lote", parametros);
                    result.success();

                }
                catch (Exception exp)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), exp.ToString());

                    result.error(exp.ToString());
                }
            });

            task.Wait();
            return result;
        }

        public List<PreferenciasDescargarWS> GetPreferenciasDescargarEventos(string clienteIdS)
        {
            List<PreferenciasDescargarWS> preferencias = new List<PreferenciasDescargarWS>();
            Task task = Task.Run(() =>
            {
                try
                {
                    string sqlCommand = $" Where TPDW.TipoPreferencia > 2 AND TPDW.ClientesId LIKE '%{clienteIdS}%'";
                    preferencias = _conn.GetAll<PreferenciasDescargarWS>(PortalQueryHelper._SelectPreferenciasDescargas + sqlCommand, null).ToList();

                }
                catch (Exception ex)
                {
                    _logService.SetLogError(-1, "PortalService." + MethodBase.GetCurrentMethod(), ex.ToString());
                }
            });

            task.Wait();

            return preferencias;
        }
       
    }
}

