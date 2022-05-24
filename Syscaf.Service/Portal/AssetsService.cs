using AutoMapper;
using Dapper;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Data.Models.Portal;
using Syscaf.Common.Models.PORTAL;
using Syscaf.Helpers;
using Syscaf.Service.Automaper.MapperDTO;
using Syscaf.Service.DataTableSql;
using Syscaf.Service.Helpers;
using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal
{

    public class AssetsService : IAssetsService
    {
        private readonly SyscafCoreConn _conn;
        private readonly ILogService _log;
        private readonly IClientService _clientService;
        private readonly IMixIntegrateService _Mix;
        private readonly IMapper _mapper;
        public AssetsService(SyscafCoreConn conn, ILogService _log, IClientService _clientService, IMixIntegrateService _Mix, IMapper _mapper)
        {
            _conn = conn;
            this._log = _log;
            this._clientService = _clientService;
            this._Mix = _Mix;
            this._mapper = _mapper;
        }
        // adiciona los mensajes a la tabla con el periodo seleccionado

        public async Task<ResultObject> Add(List<ClienteDTO> clientes)
        {

            var r = new ResultObject();
            try
            {
                _log.SetLogError(0, "AssetsService - Add", "Inicio Actualizar Cliente");

                if (clientes == null)
                    clientes = await _clientService.GetAsync(1);
               
                // obtenemos el listado de propiedades para hacer la insersi[on 
                // o actualizacion de datos
                // 
                var propertiesAssets =  PropertyHelper.GetProperties(typeof(AssetDTO));
                foreach (var cliente in clientes)
                {
                    var ListaAssets = await _Mix.getVehiculosAsync(cliente.clienteId, cliente.clienteIdS);

                    var listConfiguracion = await _Mix.GetConfiguracionAsync(cliente.clienteId);

                    // mapeamos ambas listas para que nos de la final
                    var resultadolista = _mapper.Map<AssetResult>(new AssetBaseData() { ListaAssets = ListaAssets, ListaConfiguracion = listConfiguracion });
                    // asignamos el cliente para diferenciarlos en la base de datos
                    var lstAssestMerge =resultadolista.Resultado.Select(s => { s.ClienteId = cliente.clienteId; return s;  }).ToList();

                    if (lstAssestMerge.Count > 0)
                    {
                        // lo guardamos en la base de datos
                        var parametros = new Dapper.DynamicParameters();
                        parametros.Add("FechaSistema", Constants.GetFechaServidor(), DbType.DateTime);
                        parametros.Add("Assets", HelperDatatable.ToDataTable(lstAssestMerge).AsTableValuedParameter("PORTAL.UDT_Assets"));

                        try
                        {
                            //// debe validr que la tabla a la que va a isnertar el mensaje exista            

                            var result = await Task.FromResult(_conn.GetAll<int>(AssetsQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
                        }
                        catch (Exception ex)
                        {

                            r.error(ex.Message);
                        }
                    }
                }

                r.success();
            }
            catch (Exception ex)
            {

                r.error(ex.Message);
            }
            return r;
        }
        
        // Consulta Assets
        public async Task<ResultObject> getAssets(long ClienteId)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteId", ClienteId);

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.GetAll<AssetsVM>(AssetsQueryHelper._get, parametros, commandType: CommandType.StoredProcedure));
                    r.Data = result.ToList();
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
        
        // Cambia estado Assets
        public async Task<ResultObject> setEstadoAssets(long ClienteId, long AssetId, int EstadoTxId, int usuarioIdS)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("ClienteId", ClienteId);
                parametros.Add("AssetId", AssetId);
                parametros.Add("EstadoTxId", EstadoTxId);
                parametros.Add("usuarioIdS", usuarioIdS);

                try
                {
                    //Se ejecuta el procedimiento almacenado.
                    var result = await Task.FromResult(_conn.Get<int>(AssetsQueryHelper._setEstado, parametros, commandType: CommandType.StoredProcedure));

                    r.Exitoso = (result == 1);
                    r.Mensaje = r.Exitoso ? "Operación Éxitosa." : "Error de Operación";
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
    }

    public interface IAssetsService
    {

        Task<ResultObject> Add(List<ClienteDTO> clientes);
        Task<ResultObject> getAssets(long ClienteId);
        Task<ResultObject> setEstadoAssets(long ClienteId, long AssetId, int EstadoTxId, int usuarioIdS);

    }
}
