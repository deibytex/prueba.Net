using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Service.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Syscaf.Api.DWH.Controllers
{
    /// <summary>
    /// Controlador de transmisión
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdmController : ControllerBase
    {
        private readonly IGruposSeguridadService _GruposSeguridad;
        public AdmController(IGruposSeguridadService _GruposSeguridad)
        {
            this._GruposSeguridad = _GruposSeguridad;
        }
        /// <summary>
        /// Obtiene la lista de organizaciones.
        /// </summary>
        /// <param name="OrganzacionId"></param>
        /// <returns></returns>
        [HttpGet("GetOrganizaciones")]
        public async Task<DataTableVM> GetListaOrganizaciones(int? OrganzacionId)
        {
            return await _GruposSeguridad.GetListaOrganizaciones(OrganzacionId);
        }
        /// <summary>
        /// Se obtiene el listado de usuarios si es por organizacion, true o false si esta seleccionado.
        /// </summary>
        /// <param name="OrganzacionId"></param>
        /// <returns></returns>
        [HttpGet("GetUsuarioOrganizacion")]
        public async Task<ResultObject> GetUsuarioOrganizacion([Required] int OrganzacionId)
        {
            return await _GruposSeguridad.GetListadoUsuarioOrganizacion(OrganzacionId);
        }
        /// <summary>
        /// Guarda o modifica una organizacion
        /// </summary>
        /// <param name="OrganizacionId"></param>
        /// <param name="Nombre"></param>
        /// <param name="Descripcion"></param>
        /// <param name="EsActivo"></param>
        /// <param name="FechaSistema"></param>
        /// <returns></returns>
        [HttpPost("GuardarOrganizacion")]
        public async Task<ResultObject> GuardarOrganizacion(int OrganizacionId, [Required] string Nombre, [Required] string Descripcion, [Required]  bool EsActivo, [Required] DateTime FechaSistema)
        {
            OrganizacionPostVM Organzacion = new OrganizacionPostVM
            {
                Nombre = Nombre,
                Descripcion = Descripcion,
                EsActivo = EsActivo,
                FechaSistema = FechaSistema,
                OrganizacionId = OrganizacionId
            };
            return await _GruposSeguridad.GuardarOrganizacion(Organzacion);
        }
        /// <summary>
        /// Cambia el estado de las organizaciones
        /// </summary>
        /// <param name="OrganizacionId"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        [HttpGet("setEstadosOrganizacion")]
        public async Task<ResultObject> setEstadosOrganizacion([Required] int OrganizacionId,  [Required] bool EsActivo)
        {
            return await _GruposSeguridad.setEstadosOrganizacion(OrganizacionId, EsActivo);
        }
        /// <summary>
        /// Se asigna los usuarios a la organizacion
        /// </summary>
        /// <param name="OrganizacionId"></param>
        /// <param name="Usuarios"></param>
        /// <returns></returns>
        [HttpGet("setUsuariosOrganizaciones")]
        public async Task<ResultObject> SetListadoUsuariosOrganizaciones([Required] int OrganizacionId,[FromBody] [Required] List<ListadoUsuariosVM> Usuarios)
        {
            return await _GruposSeguridad.SetListadoUsuariosOrganizaciones(OrganizacionId, Usuarios);
        }
        /// <summary>
        /// Se lista el grupo de seguridad por usuario
        /// </summary>
        /// <param name="UsuarioId"></param>
        /// <returns></returns>
        [HttpGet("ListarGruposSeguridadUsuarios")]
        public async Task<ResultObject> ListarGruposSeguridadUsuarios(int? UsuarioId)
        {
            return await _GruposSeguridad.ListarGruposSeguridadUsuarios(UsuarioId);
        }
        /// <summary>
        /// Obtener sitios del cliente o de los clientes.
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpPost("ObtenerDataSiteCliente")]
        public async Task<ResultObject> ObtenerDataSiteCliente([FromBody] List<string> ClienteId)
        {
            return await _GruposSeguridad.ObtenerDataSiteCliente(ClienteId);
        }
        /// <summary>
        /// Se obtienen los grupos de seguridad segun el cliente
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetGrupoSeguridadSitios")]
        public async Task<ResultObject> GetGrupoSeguridadSitios(string ClienteId)
        {
            return await _GruposSeguridad.GetGrupoSeguridadSitios(ClienteId);
        }
        /// <summary>
        /// Trae los sitios seleccionados del cliente
        /// </summary>
        /// <param name="ClienteId"></param>
        /// <returns></returns>
        [HttpGet("GetSitiosGrupoSeguridad")]
        public async Task<ResultObject> GetSitiosGrupoSeguridad(string ClienteId)
        {
            return await _GruposSeguridad.GetSitiosGrupoSeguridad(ClienteId);
        }
        /// <summary>
        /// Se asignan los sitios o se eliminan
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpPost("AsignarSitioAgrupoSeguridad")]
        public async Task<ResultObject> AsignarSitioAgrupoSeguridad([FromBody] SitiosGrupoSeguridadAsignacionVM Modelo)
        {
            return await _GruposSeguridad.AsignarSitioAgrupoSeguridad(Modelo);
        }
        /// <summary>
        /// Se eliminan los grupos de seguridad
        /// </summary>
        /// <param name="GruposeguridadId"></param>
        /// <returns></returns>
        [HttpGet("EliminarGrupoSeguridad")]
        public async Task<ResultObject> EliminarGrupoSeguridad([Required] int GruposeguridadId)
        {
            return await _GruposSeguridad.EliminarGrupoSeguridad(GruposeguridadId);
        }
        /// <summary>
        /// Se consultan los grupos de seguridad.
        /// </summary>
        /// <param name="GruposeguridadId"></param>
        /// /// <param name="UsuarioIds"></param>
        /// <returns></returns>
        [HttpGet("ConsultarGrupoSeguridad")]
        public async Task<ResultObject> ConsultarGrupoSeguridad(int? GruposeguridadId, string UsuarioIds)
        {
            return await _GruposSeguridad.ConsultarGrupoSeguridad(GruposeguridadId, UsuarioIds);
        }
        /// <summary>
        /// Se asigna el usuario a uno o varios grupos de seguridad
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpPost("AsignarUsuarioAgrupoSeguridad")]
        public async Task<ResultObject> AsignarUsuarioAgrupoSeguridad([FromBody] UsuarioGrupoSeguridadVM Modelo)
        {
            return await _GruposSeguridad.AsignarUsuarioAgrupoSeguridad(Modelo);
        }
        /// <summary>
        /// Se consulta el grupo de seguridad por cliente
        /// </summary>
        /// <param name="clienteIdS"></param>
        /// <returns></returns>
        [HttpGet("ConsultarGrupoSeguridadClientes")]
        public async Task<ResultObject> ConsultarGrupoSeguridadClientes(int? clienteIdS)
        {
            return await _GruposSeguridad.ConsultarGrupoSeguridadClientes(clienteIdS);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clienteIds"></param>
        /// <param name="GrupoSeguridadID"></param>
        /// <returns></returns>
        [HttpGet("ConsultarSitiosGrupoSeguridadCliente")]
        public async Task<ResultObject> ConsultarSitiosGrupoSeguridadCliente(int? clienteIds,int GrupoSeguridadID)
        {
            return await _GruposSeguridad.ConsultarSitiosGrupoSeguridadCliente(clienteIds, GrupoSeguridadID);
        }
        /// <summary>
        /// Guarda o Edita un grupo de seguridad
        /// </summary>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        [HttpPost("GuardarEditarGrupoSeguridad")]
        public async Task<ResultObject> GuardarEditarGrupoSeguridad([FromBody] GrupoSeguridadPostVM Modelo)
        {
            return await _GruposSeguridad.GurdarEditarGrupoSeguridad(Modelo);
        }
    }
}
