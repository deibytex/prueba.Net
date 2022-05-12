using AutoMapper;
using Dapper;
using Syscaf.Common.Helpers;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;

using Syscaf.Data.Models.Portal;
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

    public class ClientService : IClientService
    {
        private readonly SyscafCoreConn _conn;
        private readonly IMixIntegrateService _Mix;
        private readonly INotificacionService _notificacionService;
        private readonly IMapper _mapper;
        public ClientService(SyscafCoreConn conn, IMixIntegrateService _Mix, INotificacionService _notificacionService, IMapper _mapper)
        {
            _conn = conn;
            this._Mix = _Mix;
            this._notificacionService = _notificacionService;
            this._mapper = _mapper;
        }

        public async Task<ResultObject> Add()
        {
            ResultObject result = new();
            try
            {
                var clientesMix = await _Mix.getClientes();
                var resultadolista = _mapper.Map<List<ClienteSaveDTO>>(clientesMix);
                var parametros = new Dapper.DynamicParameters();
                resultadolista = resultadolista.Select(s => { s.fechaIngreso = Constants.GetFechaServidor(); return s; }).ToList();
                parametros.Add("clientes", HelperDatatable.ToDataTable(resultadolista).AsTableValuedParameter("PORTAL.UDT_Cliente"));


                try
                {
                    //// debe validr que la tabla a la que va a isnertar el mensaje exista            

                    await Task.FromResult(_conn.GetAll<int>(ClientQueryHelper._Insert, parametros, commandType: CommandType.StoredProcedure));
                    result.success(_mapper.Map<List<ClienteDTO>>(resultadolista));
                }
                catch (Exception ex)
                {

                    result.error(ex.Message);
                }

            }
            catch (Exception ex)
            {
              await _notificacionService.CrearLogNotificacion(Enums.TipoNotificacion.Sistem,  ex.Message, Enums.ListaDistribucion.LSSISTEMA);              

            }

            return result;
        }

    // adiciona los mensajes a la tabla con el periodo seleccionado

    public async Task<List<ClienteDTO>> GetAsync(int Estado)
        {    
            var parametros = new Dapper.DynamicParameters();           
            parametros.Add("Estado", Estado, DbType.Int32);      

            return await Task.FromResult(_conn.GetAll<ClienteDTO>(ClientQueryHelper._Get, parametros, commandType: CommandType.Text).ToList());
           
        }
    }

    public interface IClientService
    {

        Task<List<ClienteDTO>> GetAsync(int Estado);

        Task<ResultObject> Add();

    }
}
