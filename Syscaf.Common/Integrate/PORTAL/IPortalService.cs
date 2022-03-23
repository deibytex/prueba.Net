using Syscaf.Common.Models.PORTAL;
using Syscaf.Data.Models;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using Syscaf.Service.ViewModels.PORTAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Syscaf.Service.PORTAL
{
    public interface IPortalService
    {

        List<CredencialesMixVM> GetCredencialesClientes();
        string GetTokenClientes(int ClienteIds, string Method, string SinceToken = null);
        List<SinceTokenVM> GetTokenClientes();
        List<SinceTokenVM> GetTokenClientes(int ClienteIds);
        void SetLog(int CredencialId, string Method, int StatusResponse, string Response, DateTime FechaSistema, int clienteids);

        List<CallMethodMixVM> GetCallsMethod(int CredencialId);

        Task SetCallsMethod(int CredencialId, DateTime date, DateTime dateHour, int HourCall, int MinuteCall);

        List<PreferenciasDescargarWS> GetPreferenciasDescargarEventos(string clienteIdS);
        List<long> GetIdsNoIngresados(List<long> Ids, string Periodo, int tipo);
        ResultObject SetDatosPortal(DataTable data, string Periodo, string tabla);

        List<long> GetIdsNoIngresadosByCliente(List<long> Ids, string Periodo, int tipo, int Clienteids);
        ResultObject SetDatosPortalByCliente(DataTable data, string Periodo, string tabla, int Clienteids);
        ResultObject SetUpdateLocations(DataTable Locations);
        List<ClienteMetricas> ValidateAllMetrics(string Periodo);

        ResultObject SetLogUsuario(int UsuarioId, DateTime Fecha, bool isLogin);
        ResultObject SetLogOpcionUsuario(int UsuarioID, DateTime Fecha, int Opcion, string nombreOpcion);
        ResultObject SIG_GeneraSenialesPorDia(DateTime FechaInicial, DateTime FechaFinal, int Lote);
    }
}
