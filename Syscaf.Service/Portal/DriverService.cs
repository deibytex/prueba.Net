using AutoMapper;
using Dapper;
using MiX.Integrate.Shared.Entities.Assets;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;

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

    public class DriverService : IDriverService
    {
        private readonly SyscafCoreConn _conn;
        private readonly ILogService _log;
        private readonly IClientService _clientService;
        private readonly IMixIntegrateService _Mix;
        private readonly IMapper _mapper;
        private readonly ISyscafConn _conProd;
        public DriverService(SyscafCoreConn conn, ILogService _log, IClientService _clientService, IMixIntegrateService _Mix, IMapper _mapper, ISyscafConn _conProd)
        {
            _conn = conn;
            this._log = _log;
            this._clientService = _clientService;
            this._Mix = _Mix;
            this._mapper = _mapper;
            this._conProd = _conProd;
        }
        // adiciona los mensajes a la tabla con el periodo seleccionado

        public async Task<ResultObject> Add(List<ClienteDTO> clientes)
        {

            var r = new ResultObject();
            try
            {
                _log.SetLogError(0, "DriverService - Add", "Inicio Actualizar Site");

                
               if(clientes == null)
                 clientes = await _clientService.GetAsync(1);

                foreach (var cliente in clientes)
                {
                    var ListaSites = await _Mix.getDrivers(cliente.clienteId, cliente.clienteIdS);                 

                    // mapeamos ambas listas para que nos de la final
                    var resultadolista = _mapper.Map<List<DriverDTO>>(ListaSites);
                    // asignamos el cliente para diferenciarlos en la base de datos
                    resultadolista = resultadolista.Select(s => { s.ClienteId = cliente.clienteId; s.FechaSistema = Constants.GetFechaServidor(); return s; }).ToList();

                    if (resultadolista.Count > 0)
                    {
                        // lo guardamos en la base de datos
                        var parametros = new DynamicParameters();
                        parametros.Add("drivers", HelperDatatable.ToDataTable(resultadolista).AsTableValuedParameter("PORTAL.UDT_Driver"));

                        try
                        {                           //// debe validr que la tabla a la que va a isnertar el mensaje exista            
                             await Task.FromResult(_conn.GetAll<int>(DriverQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
                            // insertamos en la replica
                            await Task.FromResult(_conProd.GetAll<int>(DriverQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
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

    public interface IDriverService
    {

        Task<ResultObject> Add(List<ClienteDTO> clientes);

    }
}
