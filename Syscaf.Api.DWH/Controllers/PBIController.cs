using Microsoft.AspNetCore.Mvc;
using Syscaf.PBIConn.Services;
using Syscaf.Service.Helpers;
using Syscaf.Service.RAG;

namespace Syscaf.Api.DWH.Controllers
{
    public class PBIController : ControllerBase
    {
        
        private readonly IRagService _MixService;
        public PBIController(IRagService _MixService)
        {
            this._MixService = _MixService;
        }
        /// <summary>
        /// Se consulta el servicio en mix y se guarda en la tabla creada.
        /// </summary>
        /// <param name="DatasetId"></param>
        /// /// <param name="Fecha"></param>
        /// <returns></returns>
        [HttpGet("CargarSafetyDataset")]
        public async Task<ResultObject> UploadSafety(string? DatasetId, DateTime? Fecha)
        {

            DatasetId = DatasetId ?? "a9d03ee2-3a8e-497a-8cb8-e7000a8d560f";

            using (var pbiClient = await EmbedService.GetPowerBiClient())
            {              
                var informeEficiencia = (await _MixService.getInformacionSafetyByClient(914,Fecha)).ToList<object>();
                return  await EmbedService.SetDataDataSet(pbiClient, ConfigValidatorService.WorkspaceId, DatasetId, informeEficiencia, "Safety");
            }
           
        }


        //a9d03ee2-3a8e-497a-8cb8-e7000a8d560f

    }
}
