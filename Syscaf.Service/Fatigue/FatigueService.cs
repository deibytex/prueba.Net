﻿using Syscaf.Common.Integrate.LogNotificaciones;
using Syscaf.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Service.Fatigue
{
    public class FatigueService : IFatigueService
    {
        private readonly SyscafCoreConn _connCore;
        private readonly ILogService _log;
        private readonly ISyscafConn _conProd;

        public FatigueService(SyscafCoreConn conn, ILogService _log, ISyscafConn _conProd)
        {
            _connCore = conn;
            this._log = _log;
            this._conProd = _conProd;
        }


        //public async  Task<List<dynamic>> GetFatigue(string Periodo, int ClienteIds) { 



        //}

        public async Task<int> RellenoEventosDistancia(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal)
        {
            return await _conProd.ExecuteAsync("FATG.RellenoFatiga", new { PeriodoFecha = FechaInicial, clienteIdS, FechaInicial, FechaFinal }, 240);
        }
    }

    public interface IFatigueService
    {
        Task<int> RellenoEventosDistancia(int clienteIdS, DateTime FechaInicial, DateTime FechaFinal);
    }
}