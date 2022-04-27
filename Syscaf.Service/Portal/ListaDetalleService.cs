using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Data.Interface;
using Syscaf.Data.Models.Portal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Portal
{

    public class ListaDetalleService : IListaDetalleService
    {
        private readonly ISyscafConn _conn;
      
        public ListaDetalleService(ISyscafConn conn)
        {
            _conn = conn;          
        }
        // adiciona los mensajes a la tabla con el periodo seleccionado

        public async Task<List<ListaDTO>> GetListaAsync(int Estado)
        {    
            var parametros = new Dapper.DynamicParameters();           
            parametros.Add("Estado", Estado, DbType.Int32);      

            return await Task.FromResult(_conn.GetAll<ClienteDTO>(ClientQueryHelper._Get, parametros, commandType: CommandType.Text).ToList());
           
        }
    }

    public interface IListaDetalleService
    {

        Task<List<ClienteDTO>> GetAsync(int Estado);

    }
}
