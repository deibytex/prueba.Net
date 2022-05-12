﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syscaf.Data.Models.Portal;
using Syscaf.Service.Helpers;
using Syscaf.Service.Portal;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BasesController : ControllerBase

    {
        private readonly IAssetsService _asset;
        private readonly ISiteService _siteService;
        private readonly IDriverService _driverService;
        private readonly IEventTypeService _eventTypeService;
        private readonly IClientService _clientService;
        public BasesController(IAssetsService _asset, ISiteService _siteService, IEventTypeService _eventTypeService, IDriverService _driverService, IClientService _clientService)
        {
            this._asset = _asset;
            this._siteService = _siteService;
            this._eventTypeService = _eventTypeService;
            this._driverService = _driverService;
            this._clientService = _clientService;
        }

        [HttpGet("actualizarClientes")]
        public async Task<ActionResult<ResultObject>> GetClientesMixByGroup()
        {
            ResultObject response =  await _clientService.Add();

            if (response.Exitoso) {
                var listaclientes = (List<ClienteDTO>)response.Data;
                response  =  await _asset.Add(listaclientes);
                if (response.Exitoso)
                    response = await _siteService.Add(listaclientes);
                if (response.Exitoso)
                    response = await _eventTypeService.Add(listaclientes);
                if (response.Exitoso)
                    response = await _driverService.Add(listaclientes);
            }

            return response;
        }
        [HttpGet("actualizarVehiculos")]      
        public async Task<ActionResult<ResultObject>> GetAssetsMixByGroup()
        { 
            return await _asset.Add(null); 
        }

        [HttpGet("actualizarSites")]
        public async Task<ActionResult<ResultObject>> GetSitesMixByGroup()
        {
            return await _siteService.Add(null);
        }

        [HttpGet("actualizarEventType")]
        public async Task<ActionResult<ResultObject>> GetEvenTypeMixByGroup()
        {
            return await _eventTypeService.Add(null);
        }

        [HttpGet("actualizarDrivers")]
        public async Task<ActionResult<ResultObject>> GetDriversMixByGroup()
        {
            return await _driverService.Add(null);
        }

    }
}
