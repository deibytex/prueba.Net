using AutoMapper;
using Dapper;
using MiX.Integrate.Shared.Entities.Assets;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Data.Interface;
using Syscaf.Data.Models.Portal;
using Syscaf.Helpers;
using Syscaf.Service.Automaper.MapperDTO;
using Syscaf.Service.DataTableSql;
using Syscaf.Service.Helpers;
using SyscafWebApi.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal
{

    public class AssetsService : IAssetsService
    {
        private readonly ISyscafConn _conn;
        private readonly ILogService _log;
        private readonly IClientService _clientService;
        private readonly IMixIntegrateService _Mix;
        private readonly IMapper _mapper;
        public AssetsService(ISyscafConn conn, ILogService _log, IClientService _clientService, IMixIntegrateService _Mix, IMapper _mapper)
        {
            _conn = conn;
            this._log = _log;
            this._clientService = _clientService;
            this._Mix = _Mix;
            this._mapper = _mapper;
        }
        // adiciona los mensajes a la tabla con el periodo seleccionado

        public async Task<ResultObject> Add()
        {

            var r = new ResultObject();
            try
            {
                _log.SetLogError(0, "AssetsService - Add", "Inicio Actualizar Cliente");

                var ClientesActivos = await _clientService.GetAsync(1);
                // obtenemos el listado de propiedades para hacer la insersi[on 
                // o actualizacion de datos
                // 
                var propertiesAssets =  PropertyHelper.GetProperties(typeof(AssetDTO));
                foreach (var cliente in ClientesActivos)
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

                            var result = await Task.FromResult(_conn.Insert<int>(AssetsQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
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
    }

    public interface IAssetsService
    {

        Task<ResultObject> Add();

    }
}
