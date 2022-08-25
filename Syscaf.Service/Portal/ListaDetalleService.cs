using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;

using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Syscaf.Service.Portal
{

    public class ListaDetalleService : IListaDetalleService
    {
        private readonly SyscafCoreConn _conn;
        private readonly ISyscafConn _conProd;

        public ListaDetalleService(SyscafCoreConn conn, ISyscafConn _conProd)
        {
            _conn = conn;
            this._conProd = _conProd;
        }

        public Task<List<ClienteDTO>> GetAsync(int Estado)
        {
            throw new NotImplementedException();
        }

        // adiciona los mensajes a la tabla con el periodo seleccionado

        public async Task<List<ClienteDTO>> GetListaAsync(int Estado)
        {    
            var parametros = new Dapper.DynamicParameters();           
            parametros.Add("Estado", Estado, DbType.Int32);      

            return await Task.FromResult(_conProd.GetAll<ClienteDTO>(ClientQueryHelper._Get, parametros, commandType: CommandType.Text).ToList());
           
        }

        public async Task<ResultObject> getDetalleListas(string sigla)
        {
            var r = new ResultObject();
            try
            {
                var parametros = new Dapper.DynamicParameters();
                parametros.Add("siglas", sigla, DbType.String);
                parametros.Add("ListaId", null, DbType.String);
                parametros.Add("EsActivo", 1, DbType.Int32);

                try
                {
                    var Listas = await Task.FromResult(_conn.GetAll<ListaDTO>(ListasQueryHelper._GetLista, parametros, commandType: CommandType.Text).ToList());

                    int ListaId = Listas.Select(s => s.ListaId).First();

                    var parametoslistas = new Dapper.DynamicParameters();
                    parametoslistas.Add("siglas", null, DbType.String);
                    parametoslistas.Add("ListaId", ListaId, DbType.Int32);
                    parametoslistas.Add("EsActivo", 1, DbType.Int32);

                    var result = await Task.FromResult(_conn.GetAll<DetalleListaDTO>(ListasQueryHelper._GetDetalleLista, parametoslistas, commandType: CommandType.Text).ToList());


                    r.Data = result;
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
    }

    public interface IListaDetalleService
    {

        Task<List<ClienteDTO>> GetAsync(int Estado);
        Task<ResultObject> getDetalleListas(string sigla);

    }
}
