using Dapper;
using Microsoft.Extensions.Configuration;
using Syscaf.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.Data
{
    public class SyscafCoreConn : SyscafConn
    {
      
        private string  _connectionstring ;

        public SyscafCoreConn(string _connectionstring) : base(_connectionstring)
        {
           
        }
      
       
    }
}
