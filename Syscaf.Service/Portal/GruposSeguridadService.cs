using AutoMapper;
using Newtonsoft.Json;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Dapper;

namespace Syscaf.Service.Portal
{
    public class GruposSeguridadService : IGruposSeguridadService
    {
        private readonly SyscafCoreConn _connCore;
        private readonly ISyscafConn _conDWH;
        private readonly IMapper _mapper;
        private readonly ILogService _log;

        public GruposSeguridadService(SyscafCoreConn conn, ILogService _log, IMapper _mapper, ISyscafConn __conn)
        {
            _connCore = conn;
            this._log = _log;
            this._mapper = _mapper;
            this._conDWH = __conn;
        }
        public async Task<DataTableVM> GetListaOrganizaciones(int? OrganzacionId)
        {
            var r = new DataTableVM();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("OrganzacionId", OrganzacionId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<OrganizacionVM>(GruposSeguridadQueryHelper.getOrganizaciones, parametros, commandType: CommandType.StoredProcedure));
                    r.data = result.ToList();
                    r.recordsTotal = result.Count;
                    r.recordsFiltered = result.Count;
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    throw;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
            return r;
        }
        public async Task<ResultObject> GetListadoUsuarioOrganizacion(int OrganzacionId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("OrganzacionId", OrganzacionId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<UsuarioOrganizacionVM>(GruposSeguridadQueryHelper.getUsuariosOrganizacion, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa.";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Organizacion " + OrganzacionId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> GuardarOrganizacion(OrganizacionPostVM Organzacion)
        {
            var r = new ResultObject();
            try
            {

                var parametros = new Dapper.DynamicParameters();
                parametros.Add("OrganzacionId", Organzacion.OrganizacionId);
                parametros.Add("Nombre", Organzacion.Nombre);
                parametros.Add("Descripcion", Organzacion.Descripcion);
                parametros.Add("FechaSistema", Organzacion.FechaSistema);
                parametros.Add("EsActivo", Organzacion.EsActivo);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.Insert<string>(GruposSeguridadQueryHelper.postOrganizacion, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = null;
                    r.Exitoso = true;
                    r.Mensaje = result.ToString();
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Organizacion " + Organzacion.OrganizacionId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> setEstadosOrganizacion(int OrganizacionId, bool EsActivo)
        {
            var r = new ResultObject();
            try
            {

                var parametros = new Dapper.DynamicParameters();
                parametros.Add("OrganzacionId", OrganizacionId);
                parametros.Add("EsActivo", EsActivo);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.Insert<string>(GruposSeguridadQueryHelper.SetEstadosOrganizacion, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = null;
                    r.Exitoso = true;
                    r.Mensaje = result.ToString();
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Organizacion " + OrganizacionId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> SetListadoUsuariosOrganizaciones(int OrganizacionId, List<ListadoUsuariosVM> Usuarios)
        {
            var r = new ResultObject();
            try
            {
                var ListaUsuarios = JsonConvert.SerializeObject(Usuarios);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("OrganzacionId", OrganizacionId);
                parametros.Add("Usuarios", ListaUsuarios);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.Insert<string>(GruposSeguridadQueryHelper.SetUsuariosOrganizacion, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = null;
                    r.Exitoso = true;
                    r.Mensaje = result.ToString();
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Organizacion " + OrganizacionId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> ListarGruposSeguridadUsuarios(int? UsuarioId)
        {
            var r = new ResultObject();
            try
            {

                var parametros = new Dapper.DynamicParameters();
                parametros.Add("UsuarioId", UsuarioId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<GrupoSeguridadListaVM>(GruposSeguridadQueryHelper.GetUsuariosGruposSeguridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Usuario " + UsuarioId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> ObtenerDataSiteCliente(List<string> ClienteId)
        {
            var r = new ResultObject();
            try
            {

                var Clientes = JsonConvert.SerializeObject(String.Join(",", ClienteId.ToArray()));
                string str = Regex.Replace(Clientes, "[@\\.\"'\\\\]", string.Empty);
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteId", str);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<SitiosVM>(GruposSeguridadQueryHelper.GetDataSiteCliente, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Cliente(s) " + ClienteId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> GetGrupoSeguridadSitios(string ClienteId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteId", ClienteId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<GruposSitiosVM>(GruposSeguridadQueryHelper.GetGrupoSeguridadSitios, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Cliente(s) " + ClienteId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> GetSitiosGrupoSeguridad(string ClienteId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteId", ClienteId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<GruposSitiosSelectVM>(GruposSeguridadQueryHelper.GetSitiosGrupoSeguridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Cliente(s) " + ClienteId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> AsignarSitioAgrupoSeguridad(SitiosGrupoSeguridadAsignacionVM Modelo)
        {
            var r = new ResultObject();
            try
            {
                //Se crean variables locales.
                var GruposeguridadId = Modelo.GrupoSeguridadId;
                var ClienteId = Modelo.ClienteId;
                var TipoSeguridadId = Modelo.TipoSeguridadId;
                var EsActivo = Modelo.EsActivo;
                var Sites = JsonConvert.SerializeObject(String.Join(",", Modelo.SiteIds.ToArray()));
                string? str = Regex.Replace(Sites, "[@\\.\"'\\\\]", string.Empty);
                //se crean los parametros

                var parametros = new Dapper.DynamicParameters();
                parametros.Add("GruposeguridadId", GruposeguridadId);
                parametros.Add("ClienteId", ClienteId);
                parametros.Add("TipoSeguridadId", TipoSeguridadId);
                parametros.Add("Sites", str);
                parametros.Add("EsActivo", EsActivo);

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.Insert<string>(GruposSeguridadQueryHelper.InsertSitioAgrupoSeguridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = null;
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Cliente(s) " + ClienteId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> EliminarGrupoSeguridad(int GrupoSeguridadId)
        {
            var r = new ResultObject();
            try
            {
                //se crean los parametros
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("GruposeguridadId", GrupoSeguridadId);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.Insert<int>(GruposSeguridadQueryHelper.EliminarGrupoSeguridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Grupo de seguridad " + GrupoSeguridadId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> ConsultarGrupoSeguridad(int? GrupoSeguridadId, string UsuarioIds)
        {
            var r = new ResultObject();
            try
            {
                //se crean los parametros
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("GruposeguridadId", GrupoSeguridadId);
                parametros.Add("UsuarioIds", UsuarioIds);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<GrupoSeguirdadUsuarioSeleccionadosVM>(GruposSeguridadQueryHelper.ConsultarGrupoSeguridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Grupo de seguridad " + GrupoSeguridadId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> AsignarUsuarioAgrupoSeguridad(UsuarioGrupoSeguridadVM Modelo)
        {
            var r = new ResultObject();
            try
            {
                var usuarioId = Modelo.UsuarioId;
                var usuarioids = Modelo.usuarioIdS;
                var EsActivo = Modelo.EsActivo;
                var GruposSeguridadIds = JsonConvert.SerializeObject(String.Join(",", Modelo.GruposSeguridadIds.ToArray()));
                string str = Regex.Replace(GruposSeguridadIds, "[@\\.\"'\\\\]", string.Empty);

                //se crean los parametros
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("usuarioId", usuarioId);
                parametros.Add("usuarioids", usuarioids);
                parametros.Add("GruposSeguridadIds", str);
                parametros.Add("EsActivo", EsActivo);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.Insert<string>(GruposSeguridadQueryHelper.AsignarUsuarioAgrupoSeguridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = null;
                    r.Exitoso = true;
                    r.Mensaje = result?.ToString();
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Usuario " + usuarioId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> ConsultarGrupoSeguridadClientes(int? clienteIdS)
        {
            var r = new ResultObject();
            try
            {
                //se crean los parametros
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("clienteIdS", clienteIdS);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<GruposSeguridadClientesVM>(GruposSeguridadQueryHelper.ConsultarGrupoSeguridadClientes, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Cliente " + clienteIdS);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> ConsultarSitiosGrupoSeguridadCliente(int? clienteIds, int GrupoSeguridadID)
        {
            var r = new ResultObject();
            try
            {
                //se crean los parametros
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("clienteIds", clienteIds);
                parametros.Add("GrupoSeguridadID", GrupoSeguridadID);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.GetAll<SitioClienteVM>(GruposSeguridadQueryHelper.ConsultarSitiosGrupoSeguridadCliente, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Grupo de seguridad " + GrupoSeguridadID);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }
        public async Task<ResultObject> GurdarEditarGrupoSeguridad(GrupoSeguridadPostVM Modelo)
        {
            var r = new ResultObject();
            try
            {
                int Clave = Modelo.Clave;
                var clienteIdS = Modelo.clienteIdS;
                var Nombre = Modelo.Nombre;
                bool? EsActivo = Modelo.EsActivo;
                var TipoSeguridadId = Modelo.TipoSeguridadId;
                bool? EsAdministrador = Modelo.EsAdministrador;
                var GrupoSeguridadId = Modelo.GrupoSeguridadId;
                string Descripcion = Modelo.Descripcion;
                var Sitios = JsonConvert.SerializeObject(String.Join(",", Modelo.Sitios.ToArray()));
                string str = Regex.Replace(Sitios, "[@\\.\"'\\\\]", string.Empty);

                //se crean los parametros
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("Clave", Clave);
                parametros.Add("clienteIdS", clienteIdS);
                parametros.Add("Nombre", Nombre);
                parametros.Add("Descripcion", Descripcion);
                parametros.Add("TipoSeguridadId", TipoSeguridadId);
                parametros.Add("EsActivo", EsActivo);
                parametros.Add("EsAdministrador", EsAdministrador);
                parametros.Add("GrupoSeguridadId", GrupoSeguridadId);
                parametros.Add("Sitios", str);
                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_connCore.Insert<string>(GruposSeguridadQueryHelper.GuardarEditarGrupoSeguridad, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result;
                    r.Exitoso = true;
                    r.Mensaje = "Operación Éxitosa";
                }
                catch (Exception ex)
                {
                    r.error(ex.Message + " Grupo de seguridad " + Modelo.GrupoSeguridadId);
                }
            }
            catch (Exception ex)
            {
                r.error(ex.Message);
                throw;
            }
            return r;
        }


        public async Task<List<dynamic>> getDynamicValueDWH(string Clase, string NombreConsulta, DynamicParameters lstparams)
        {
            try
            {
                string consulta = await _connCore.Get<string>(PortalQueryHelper.getConsultasByClaseyNombre, new { Clase, NombreConsulta }, commandType: CommandType.Text);

                if (consulta != null && consulta.Length > 0)
                    //Se ejecuta el procedimiento almacenado.
                    return await Task.FromResult(_connCore.GetAll<dynamic>(consulta, lstparams, commandType: CommandType.Text));

                else
                    throw new Exception("La consulta no se ha encontrado");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

    }
}
public interface IGruposSeguridadService
{
    Task<DataTableVM> GetListaOrganizaciones(int? OrganzacionId);
    Task<ResultObject> GetListadoUsuarioOrganizacion(int OrganzacionId);
    Task<ResultObject> GuardarOrganizacion(OrganizacionPostVM Organzacion);
    Task<ResultObject> setEstadosOrganizacion(int OrganizacionId, bool EsActivo);
    Task<ResultObject> SetListadoUsuariosOrganizaciones(int OrganizacionId, List<ListadoUsuariosVM> Usuarios);
    Task<ResultObject> ListarGruposSeguridadUsuarios(int? UsuarioId);
    Task<ResultObject> ObtenerDataSiteCliente(List<string> ClienteId);
    Task<ResultObject> GetGrupoSeguridadSitios(string ClienteId);
    Task<ResultObject> GetSitiosGrupoSeguridad(string ClienteId);
    Task<ResultObject> AsignarSitioAgrupoSeguridad(SitiosGrupoSeguridadAsignacionVM Modelo);
    Task<ResultObject> EliminarGrupoSeguridad(int GrupoSeguridadId);
    Task<ResultObject> ConsultarGrupoSeguridad(int? GrupoSeguridadId, string UsuarioIds);
    Task<ResultObject> AsignarUsuarioAgrupoSeguridad(UsuarioGrupoSeguridadVM Modelo);
    Task<ResultObject> ConsultarGrupoSeguridadClientes(int? clienteIdS);
    Task<ResultObject> ConsultarSitiosGrupoSeguridadCliente(int? clienteIds, int GrupoSeguridadID);
    Task<ResultObject> GurdarEditarGrupoSeguridad(GrupoSeguridadPostVM Modelo);
    Task<List<dynamic>> getDynamicValueDWH(string Clase, string NombreConsulta, DynamicParameters lstparams);
}