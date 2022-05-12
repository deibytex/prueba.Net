using Syscaf.Data;
using Syscaf.Data.Helpers.Portal;

using Syscaf.Data.Models;
using Syscaf.Data.Models.Portal;
using System;
using System.Data;
using System.Threading.Tasks;
using Helper = Syscaf.Common.Helpers.Helpers;
namespace Syscaf.Common.Integrate.LogNotificaciones
{
    public class LogService : ILogService
    {
        private readonly ISyscafConn _conn;
        public LogService(ISyscafConn conn)
        {
            _conn = conn;
        }
     

        public async void SetLog(LogDTO log)
        {          
             await _conn.Insert(PortalQueryHelper._InsertLog, log, commandType: CommandType.Text);           
        }

        public async void SetLogError(int OptionId, string Method, string Description)
        {
            await _conn.Insert(PortalQueryHelper._InsertLog, getClassLogDTO("Error", OptionId, Method, Description), commandType: CommandType.Text);
        }

        private LogDTO getClassLogDTO(string Level, int OptionId, string Method, string Description) {
            return new LogDTO() { Level = Level, OptionId = OptionId, Method = Method, Description = Description,
            Date = DateTime.Now};
        }
    }
}
