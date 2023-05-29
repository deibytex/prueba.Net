using AutoMapper;
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
    public class FreshDeskService : IFreshDeskService
    {
        private readonly FreshdeskVariablesConn _freshConn;
        private readonly IMapper _mapper;
        private readonly ISyscafConn _conprod;
        readonly ILogService _logService;
        public FreshDeskService(IOptions<FreshdeskVariablesConn> _freshConn, ISyscafConn _conprod, ILogService logService, IMapper _mapper)
        {
            this._conprod = _conprod;
            this._freshConn = _freshConn.Value;
            _logService = logService;
            this._mapper = _mapper;
        }

        public async Task<ResultObject> GetTickets()
        {
            try
            {
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
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
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                return await s.GetTicketsFields();
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de campos de los tickets", "", ex.ToString());
            }

            return null;
        }
        public async Task<ResultObject> GetListAgentes()
        {
            try
            {
                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                return await s.GetAgents();
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de listado de agentes", "", ex.ToString());
            }

            return null;
        }
        public async Task<ResultObject> GetListaTicketsSemana(DateTime FechaInicial, DateTime FechaFinal)
        {
            try
            {

                FreshDeskServiceConn s = new FreshDeskServiceConn(_freshConn, _mapper);
                return await s.GetTicketsSemana(FechaInicial, FechaFinal);
            }
            catch (Exception ex)
            {
                _logService.SetLog("Obtención de tickets", "", ex.ToString());
            }

            return null;
        }
    }
    public interface IFreshDeskService
    {
        Task<ResultObject> GetTickets();
        Task<ResultObject> GetTicketsCampos();
        Task<ResultObject> GetListAgentes();
        Task<ResultObject> GetListaTicketsSemana(DateTime FechaInicial, DateTime FechaFinal);
    }
}
