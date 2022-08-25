using Dapper;
using System.Data;

namespace Syscaf.Data
{
    public interface ISyscafConn
    {
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        Task<int> Execute(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);

        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);

        Task<T> Get<T>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAll<T>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);

        Task<List<dynamic>> GetAll(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<dynamic>> GetAll(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        Task<int> Insert(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        Task<int> Update(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> Execute(string sp, object parms, int Timeout, CommandType commandType = CommandType.StoredProcedure);
    }
}