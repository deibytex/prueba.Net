using Syscaf.Data;
using Syscaf.Data.Helpers.eBus.Gcp;

using Syscaf.Data.Models.eBus.gcp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.eBus.Gcp
{
    public class eBusGcpService : IeBusGcpService
    {
        private readonly ISyscafConn _conn;
        public eBusGcpService(ISyscafConn conn)
        {
            _conn = conn;
        }
        // adiciona los mensajes a la tabla con el periodo seleccionado
        public async Task<bool> AddMensaje(GetMessages mensaje, string Periodo)
        {
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("FechaHora", mensaje.FechaHora, DbType.DateTime);
            parametros.Add("Mensaje", mensaje.Mensaje, DbType.String);
            parametros.Add("ProfileData", mensaje.ProfileData, DbType.String);
            string query = string.Format(eBusGcpQuery._tableGetMessages, Periodo);

            // debe validr que la tabla a la que va a isnertar el mensaje exista

            await CreateTableMessageByPeriod(Periodo);

            var result = await Task.FromResult(_conn.Insert<int>(query, parametros, commandType: CommandType.Text));
            return (result == 0);
        }

        // crea o verifica que la tabla correspondiente al mes en que se guardar[a los mensajoes
        // existan
        public async Task<bool> CreateTableMessageByPeriod(string Periodo)
        {
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("Periodo", Periodo, DbType.String);

            var result = await Task.FromResult(_conn.Execute(eBusGcpQuery._Sp_CreateTableMessageByPeriod, parametros));
            return (result == 0);
        }

        public async Task<bool> InsertaMensaje(GetMessages mensaje, string Periodo)
        {
            var parametros = new Dapper.DynamicParameters();
            parametros.Add("FechaRecibido", mensaje.FechaHora, DbType.DateTime);
            parametros.Add("jsondata", mensaje.Mensaje, DbType.String);
            parametros.Add("ProfileData", mensaje.ProfileData, DbType.String);   
            parametros.Add("Periodo", Periodo, DbType.String);


            var result = await Task.FromResult(_conn.Execute(eBusGcpQuery._Sp_InsertaMensaje, parametros));
            return (result == 0);
        }

        public async Task<List<ConfigurationPubSub>> getConfigurationPubSub()
        {
            return (await Task.FromResult(_conn.GetAll<ConfigurationPubSub>(eBusGcpQuery._table_ConfigurationPubSub, null, CommandType.Text)));
        }
    }

    public interface IeBusGcpService
    {
        Task<bool> AddMensaje(GetMessages mensaje, string Periodo);
        Task<bool> CreateTableMessageByPeriod(string Periodo);
        Task<List<ConfigurationPubSub>> getConfigurationPubSub();
        Task<bool> InsertaMensaje(GetMessages mensaje, string Periodo);
    }
}
