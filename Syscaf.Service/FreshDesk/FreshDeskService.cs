using Microsoft.Extensions.Options;
using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Common.Services;
using Syscaf.Common.Utils;
using Syscaf.Data;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.FreshDesk
{
    public class FreshDeskService: IFreshDeskService
    {
        private readonly FreshdeskVariablesConn _freshConn;
        private readonly ISyscafConn _conprod;
        readonly ILogService _logService;
        public FreshDeskService(IOptions<FreshdeskVariablesConn> _freshConn, ISyscafConn _conprod, ILogService logService)
        {
            this._conprod = _conprod;
            this._freshConn = _freshConn.Value;
            _logService = logService;
        }

        public async Task<ResultObject> GetTickets()
        {
            try
            {
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn);
                return await s.GetTickets();
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de tickets", "", ex.ToString());
            }

            return null;
        }
        public async Task<ResultObject> GetTicketsCampos()
        {
            try
            {
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn);
                return await s.GetTicketsFields();
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de campos de los tickets", "", ex.ToString());
            }

            return null;
        }
        
    }
    public interface IFreshDeskService
    {
        Task<ResultObject> GetTickets();
        Task<ResultObject> GetTicketsCampos();
    }
}
