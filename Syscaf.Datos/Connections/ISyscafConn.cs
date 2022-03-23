using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.Data.Interface
{
    public interface ISyscafConn : IDisposable
    {
        DbConnection GetDbconnection();
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);

        Task<T> Get<T>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAll<T>(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> Execute(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> Insert(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);

        Task<int> Update(string sp, object parms, CommandType commandType = CommandType.StoredProcedure);
    }
}

