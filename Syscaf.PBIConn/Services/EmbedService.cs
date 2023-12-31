﻿using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using Microsoft.Rest;
using Newtonsoft.Json.Serialization;
using Syscaf.PBIConn.Models;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.PBIConn.Services
{
    public static class EmbedService
    {
        private static readonly string urlPowerBiServiceApiRoot = ConfigValidatorService.urlPowerBiServiceApiRoot;

        public static async Task<PowerBIClient> GetPowerBiClient()
        {
            var tokenCredentials = new TokenCredentials(await AadService.GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }
        public static PowerBIClient GetPowerBiClient(string Token)
        {
            var tokenCredentials = new TokenCredentials(Token, "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }

        public static async Task<string> GetAccessTokenBiClient()
        {        
            return await AadService.GetAccessToken();
        }

        /// <summary>
        /// Get embed params for a report
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL, Report Id, and Report name for single report</returns>
        public static async Task<ReportEmbedConfig> GetEmbedParams(Guid workspaceId, Guid reportId, [Optional] Guid additionalDatasetId)
        {
            try
            {
                using (var pbiClient = await GetPowerBiClient())
                {
                    // Get report info
                    var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

                    /*
                    Check if dataset is present for the corresponding report
                    If no dataset is present then it is a RDL report 
                    */
                    bool isRDLReport = String.IsNullOrEmpty(pbiReport.DatasetId);

                    EmbedToken embedToken;

                    if (isRDLReport)
                    {
                        // Get Embed token for RDL Report
                        embedToken = await GetEmbedTokenForRDLReport(workspaceId, reportId);
                    }
                    else
                    {
                        // Create list of dataset
                        var datasetIds = new List<Guid>();

                        // Add dataset associated to the report
                        datasetIds.Add(Guid.Parse(pbiReport.DatasetId));

                        // Append additional dataset to the list to achieve dynamic binding later
                        if (additionalDatasetId != Guid.Empty)
                        {
                            datasetIds.Add(additionalDatasetId);
                        }

                        // Get Embed token multiple resources
                        embedToken = await GetEmbedToken(reportId, datasetIds, workspaceId);


                    }

                    // Add report data for embedding
                    var embedReports = new List<EmbedReport>() {
                    new EmbedReport
                    {
                        ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                    }
                };

                    // Capture embed params
                    var embedParams = new ReportEmbedConfig
                    {
                        EmbedReports = embedReports,
                        EmbedToken = embedToken
                    };

                    return embedParams;
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        /// <summary>
        /// Get embed params for multiple reports for a single workspace
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL, Report Id, and Report name for multiple reports</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public static async Task<ReportEmbedConfig> GetEmbedParams(Guid workspaceId, IList<Guid> reportIds, [Optional] IList<Guid> additionalDatasetIds)
        {
            // Note: This method is an example and is not consumed in this sample app

            using (var pbiClient = await GetPowerBiClient())
            {
                // Create mapping for reports and Embed URLs
                var embedReports = new List<EmbedReport>();

                // Create list of datasets
                var datasetIds = new List<Guid>();

                // Get datasets and Embed URLs for all the reports
                foreach (var reportId in reportIds)
                {
                    // Get report info
                    var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

                    // Append to existing list of datasets to achieve dynamic binding later
                    datasetIds.Add(Guid.Parse(pbiReport.DatasetId));

                    // Add report data for embedding
                    embedReports.Add(new EmbedReport { ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl });
                }

                // Append to existing list of datasets to achieve dynamic binding later
                if (additionalDatasetIds != null)
                {
                    datasetIds.AddRange(additionalDatasetIds);
                }

                // Get Embed token multiple resources
                var embedToken = await GetEmbedToken(reportIds, datasetIds, workspaceId);

                // Capture embed params
                var embedParams = new ReportEmbedConfig
                {
                    EmbedReports = embedReports,
                    EmbedToken = embedToken
                };



                return embedParams;
            }
        }

        /// <summary>
        /// Get Embed token for single report, multiple datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public static async Task<EmbedToken> GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                // Create a request for getting Embed token 
                // This method works only with new Power BI V2 workspace experience
                var tokenRequest = new GenerateTokenRequestV2(

                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList(),

                targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null
                );

                // Generate Embed token
                var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

                return embedToken;
            }
        }

        /// <summary>
        /// Get Embed token for multiple reports, datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public static async Task<EmbedToken> GetEmbedToken(IList<Guid> reportIds, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            // Note: This method is an example and is not consumed in this sample app

            using (var pbiClient = await GetPowerBiClient())
            {
                // Convert reports to required types
                var reports = reportIds.Select(reportId => new GenerateTokenRequestV2Report(reportId)).ToList();

                // Convert datasets to required types
                var datasets = datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList();

                // Create a request for getting Embed token 
                // This method works only with new Power BI V2 workspace experience
                var tokenRequest = new GenerateTokenRequestV2(

                    datasets: datasets,

                    reports: reports,

                    targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null
                );

                // Generate Embed token
                var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

                return embedToken;
            }
        }

        /// <summary>
        /// Get Embed token for multiple reports, datasets, and optional target workspaces
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public static async Task<EmbedToken> GetEmbedToken(IList<Guid> reportIds, IList<Guid> datasetIds, [Optional] IList<Guid> targetWorkspaceIds)
        {
            // Note: This method is an example and is not consumed in this sample app

            using (var pbiClient = await GetPowerBiClient())
            {
                // Convert report Ids to required types
                var reports = reportIds.Select(reportId => new GenerateTokenRequestV2Report(reportId)).ToList();

                // Convert dataset Ids to required types
                var datasets = datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList();

                // Convert target workspace Ids to required types
                IList<GenerateTokenRequestV2TargetWorkspace> targetWorkspaces = null;
                if (targetWorkspaceIds != null)
                {
                    targetWorkspaces = targetWorkspaceIds.Select(targetWorkspaceId => new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId)).ToList();
                }

                // Create a request for getting Embed token 
                // This method works only with new Power BI V2 workspace experience
                var tokenRequest = new GenerateTokenRequestV2(

                    datasets: datasets,

                    reports: reports,

                    targetWorkspaces: targetWorkspaceIds != null ? targetWorkspaces : null
                );

                // Generate Embed token
                var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

                return embedToken;
            }
        }

        /// <summary>
        /// Get Embed token for RDL Report
        /// </summary>
        /// <returns>Embed token</returns>
        public static async Task<EmbedToken> GetEmbedTokenForRDLReport(Guid targetWorkspaceId, Guid reportId, string accessLevel = "view")
        {
            using (var pbiClient = await GetPowerBiClient())
            {

                // Generate token request for RDL Report
                var generateTokenRequestParameters = new GenerateTokenRequest(
                    accessLevel: accessLevel
                );

                // Generate Embed token
                var embedToken = pbiClient.Reports.GenerateTokenInGroup(targetWorkspaceId, reportId, generateTokenRequestParameters);

                return embedToken;
            }
        }

        /// <summary>
        /// Get embed params for a dashboard
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL for single dashboard</returns>
        public static async Task<DashboardEmbedConfig> EmbedDashboard(Guid workspaceId)
        {
            // Create a Power BI Client object. It will be used to call Power BI APIs.
            using (var client = await GetPowerBiClient())
            {
                // Get a list of dashboards.
                var dashboards = await client.Dashboards.GetDashboardsInGroupAsync(workspaceId);

                // Get the first report in the workspace.
                var dashboard = dashboards.Value.FirstOrDefault();

                if (dashboard == null)
                {
                    throw new NullReferenceException("Workspace has no dashboards");
                }

                // Generate Embed Token.
                var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                var tokenResponse = await client.Dashboards.GenerateTokenInGroupAsync(workspaceId, dashboard.Id, generateTokenRequestParameters);

                if (tokenResponse == null)
                {
                    throw new NullReferenceException("Failed to generate embed token");
                }

                // Generate Embed Configuration.
                var dashboardEmbedConfig = new DashboardEmbedConfig
                {
                    EmbedToken = tokenResponse,
                    EmbedUrl = dashboard.EmbedUrl,
                    DashboardId = dashboard.Id
                };

                return dashboardEmbedConfig;
            }
        }

        /// <summary>
        /// Get embed params for a tile
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL for single tile</returns>
        public static async Task<TileEmbedConfig> EmbedTile(Guid workspaceId)
        {
            // Create a Power BI Client object. It will be used to call Power BI APIs.
            using (var client = await GetPowerBiClient())
            {
                // Get a list of dashboards.
                var dashboards = await client.Dashboards.GetDashboardsInGroupAsync(workspaceId);

                // Get the first report in the workspace.
                var dashboard = dashboards.Value.FirstOrDefault();

                if (dashboard == null)
                {
                    throw new NullReferenceException("Workspace has no dashboards");
                }

                var tiles = await client.Dashboards.GetTilesInGroupAsync(workspaceId, dashboard.Id);

                // Get the first tile in the workspace.
                var tile = tiles.Value.FirstOrDefault();

                // Generate Embed Token for a tile.
                var generateTokenRequestParameters = new GenerateTokenRequest(accessLevel: "view");
                var tokenResponse = await client.Tiles.GenerateTokenInGroupAsync(workspaceId, dashboard.Id, tile.Id, generateTokenRequestParameters);

                if (tokenResponse == null)
                {
                    throw new NullReferenceException("Failed to generate embed token");
                }

                // Generate Embed Configuration.
                var tileEmbedConfig = new TileEmbedConfig()
                {
                    EmbedToken = tokenResponse,
                    EmbedUrl = tile.EmbedUrl,
                    TileId = tile.Id,
                    DashboardId = dashboard.Id
                };

                return tileEmbedConfig;
            }
        }

        #region newpbi

        //Crear DataSet
        public static async Task<ResultObject> SetDataSet(Guid workspaceId)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                ResultObject result = new ResultObject() { Exitoso = false };

                try
                {
                    // Se generan todas las tablas a usar en el Dataset
                    var Eficiencia = new Table()
                    {
                        Name = "Eficiencia",
                        Columns = new List<Column>()
                                {
                                    new Column("Movil", "String"),
                                    new Column("Operador", "String"),
                                    new Column("Fecha", "Datetime", "dd/mm/yy"),
                                    new Column("Inicio", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("Fin", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("Carga", "Double"),
                                    new Column("Descarga", "Double"),
                                    new Column("Distancia", "Double"),
                                    new Column("Duracion", "Datetime","h:mm:ss"),
                                    new Column("DuracionHora", "Double","0.#"),
                                    new Column("TotalConsumo", "Double"),
                                    new Column("MaxSOC", "Int64"),
                                    new Column("MinSOC", "Int64"),
                                    new Column("DSOC", "Int64"),
                                    new Column("RutaCodigo", "String"),
                                    new Column("RutaNombre", "String")
                                }
                    };

                    var ZP = new Table()
                    {
                        Name = "ZP",
                        Columns = new List<Column>()
                                {
                                    new Column("Movil", "String"),
                                    new Column("Operador", "String"),
                                    new Column("Fecha", "Datetime", "dd/mm/yy"),
                                    new Column("Inicio", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("Fin", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("Descripcion", "String"),
                                    new Column("Duracion", "Datetime","h:mm:ss"),
                                    new Column("DuracionHora", "Double","0.#"),
                                    new Column("RutaCodigo", "String"),
                                    new Column("RutaNombre", "String")
                                }
                    };

                    var Recargas30seg = new Table()
                    {
                        Name = "Recargas30seg",
                        Columns = new List<Column>()
                                {
                                    new Column("Muestra", "Int64"),
                                    new Column("NoCarga", "Int64"),
                                    new Column("Movil", "String"),
                                    new Column("Fecha", "Datetime", "dd/mm/yy"),
                                    new Column("Hora", "Datetime","h:mm:ss"),
                                    new Column("FechaHora", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("SOC", "Int64"),
                                    new Column("Corriente", "Double"),
                                    new Column("Voltaje", "Int64")
                                }
                    };

                    var Alarmas = new Table()
                    {
                        Name = "Alarmas",
                        Columns = new List<Column>()
                                {
                                    new Column("Movil", "String"),
                                    new Column("Operador", "String"),
                                    new Column("Fecha", "Datetime", "dd/mm/yy"),
                                    new Column("Inicio", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("Fin", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("Descripcion", "String"),
                                    new Column("Duracion", "Datetime","h:mm:ss"),
                                    new Column("DuracionHora", "Double","0.#"),
                                    new Column("RutaCodigo", "String"),
                                    new Column("RutaNombre", "String")
                                }
                    };

                    var Viajes1Min = new Table()
                    {
                        Name = "Viajes1Min",
                        Columns = new List<Column>()
                                {
                                    new Column("Movil", "String"),
                                    new Column("Operador", "String"),
                                    new Column("Fecha", "Datetime", "dd/mm/yy"),
                                    new Column("Hora", "Datetime","h:mm:ss"),
                                    new Column("FechaHora", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("SOC", "Int64"),
                                    new Column("CargakWh", "Double"),
                                    new Column("DescargakWh", "Double"),
                                    new Column("Odometro", "Double"),
                                    new Column("Distancia", "Double"),
                                    new Column("RutaCodigo", "String"),
                                    new Column("RutaNombre", "String"),
                                    new Column("Latitud", "String"),
                                    new Column("Longitud", "String"),
                                    new Column("Altitud", "Int64"),
                                    new Column("DeltaSOC", "Int64"),
                                }
                    };

                    // Se crea DataSet con nombre, de este, y tablas a usar.
                    var dataset = await pbiClient.Datasets.PostDatasetAsync(workspaceId, new CreateDatasetRequest()
                    {
                        Name = "eSOMOS_F-DataSet_V1",
                        DefaultMode = "Push",
                        Tables = new List<Table>() { Eficiencia, ZP, Recargas30seg, Alarmas, Viajes1Min }
                    });
                }
                catch (Exception ex)
                {


                }
                result.Exitoso = true;
                return result;
            }
        }

        //Cargar Datos a PowerBi
        public static async Task<ResultObject> SetDataDataSet(PowerBIClient pbiClient, Guid workspaceId, string datasetid, List<object> data, string tableName)
        {

            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                //Se cambia la data recibida a Rows
                var postRows = new PostRowsRequest
                {
                    Rows = data,
                };

                //Se serializa la información 
                pbiClient.SerializationSettings.ContractResolver = new DefaultContractResolver();

                //Metodo para enviar la información a power BI
                await pbiClient.Datasets.PostRowsInGroupAsync(workspaceId, datasetid, tableName, postRows);
                 result.success();
            }
            catch (Exception ex)
            {
                result.error(ex.Message);

            }

           

            return result;

        }

        //Borrar datos de tablas en el DataSet
        public static async Task<ResultObject> DeleteDataDataSet(Guid workspaceId, string datasetid, string tableName)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                ResultObject result = new ResultObject() { Exitoso = false };
                try
                {
                    //Borrar Datos de tablas en el dataset, por nombre de tabla
                    pbiClient.Datasets.DeleteRowsInGroup(workspaceId, datasetid, tableName);
                }
                catch (Exception ex)
                {


                }

                result.Exitoso = true;

                return result;
            }
        }

        //Pruebas nuevo esquema
        public static async Task<ResultObject> SetNewSchema(Guid workspaceId, string datasetid)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                ResultObject result = new ResultObject() { Exitoso = false };

                try
                {
                    var table = new Table()
                    {
                        Name = "Viaje1Min",
                        Columns = new List<Column>()
                                {
                                    new Column("Movil", "String"),
                                    new Column("Operador", "String"),
                                    new Column("Fecha", "Datetime", "dd/mm/yy"),
                                    new Column("Hora", "Datetime","h:mm:ss"),
                                    new Column("FechaHora", "Datetime", "dd/MM/yy HH:mm:ss"),
                                    new Column("SOC", "Double"),
                                    new Column("CargakWh", "Double"),
                                    new Column("DescargakWh", "Double"),
                                    new Column("Odometro", "Double"),
                                    new Column("RutaCodigo", "Int64"),
                                    new Column("RutaNombre", "String"),
                                    new Column("Latitud", "String"),
                                    new Column("Longitud", "String"),
                                    new Column("Altitud", "String")
                                }
                    };
                }
                catch (Exception ex)
                {


                }
                result.Exitoso = true;
                return result;
            }
        }

        //Pruebas Descarga Reporte
        public static async Task<string> DecargarReporte(Guid workspaceId, Guid reportid, IList<string> pageNames = null, /* Get the page names from the GetPages REST API */
        string urlFilter = null)
        {
            using (var pbiClient = await GetPowerBiClient())
            {
                try
                {
                    var pages = pbiClient.Reports.GetPages(workspaceId, reportid).Value.ToList();

                    IList<string> pagesNames = pages.Select(s => s.Name).ToList();

                    var powerBIReportExportConfiguration = new PowerBIReportExportConfiguration
                    {
                        Settings = new ExportReportSettings
                        {
                            Locale = "en-us",
                        },
                        // Note that page names differ from the page display names
                        // To get the page names use the GetPages REST API
                        //Pages = (ExportReportPage)pbiClient.Reports.GetPages(workspaceId,reportid).ToList(),
                        Pages = pages?.Select(s => new ExportReportPage(pageName: s.Name, visualName: s.DisplayName)).ToList(),
                        // ReportLevelFilters collection needs to be instantiated explicitly
                        ReportLevelFilters = !string.IsNullOrEmpty(urlFilter) ? new List<ExportFilter>() { new ExportFilter(urlFilter) } : null,

                    };

                    var exportRequest = new ExportReportRequest
                    {
                        Format = FileFormat.PPTX,
                        PowerBIReportConfiguration = powerBIReportExportConfiguration,
                    };

                    var export = await pbiClient.Reports.ExportToFileInGroupAsync(workspaceId, reportid, exportRequest);

                    return export.Id;
                }

                catch (Exception ex)
                {


                }

                return null;
            }
        }
        #endregion
   

    #region newpbi



    //Cargar Datos a PowerBi
    public static async Task<ResultObject> SetDataDataSet(Guid workspaceId, string datasetid, List<object> data, string tableName)
    {
        using (var pbiClient = await GetPowerBiClient())
        {
            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                //Se cambia la data recibida a Rows
                var postRows = new PostRowsRequest
                {
                    Rows = data,
                };

                //Se serializa la información 
                pbiClient.SerializationSettings.ContractResolver = new DefaultContractResolver();

                //Metodo para enviar la información a power BI
                await pbiClient.Datasets.PostRowsInGroupAsync(workspaceId, datasetid, tableName, postRows);
                //Thread.Sleep(30000);
            }
            catch (Exception ex)
            {


            }

            result.Exitoso = true;

            return result;
        }
    }

    //traer tablas
    public static async Task<ResultObject> getTables(Guid workspaceId, string datasetid)
    {
        using (var pbiClient = await GetPowerBiClient())
        {
            ResultObject result = new ResultObject() { Exitoso = false };
            try
            {
                var a = pbiClient.Datasets.GetDataset(workspaceId, datasetid);
                var b = pbiClient.Datasets.GetDatasets(workspaceId);
                var c = pbiClient.Datasets.GetDatasetToDataflowsLinks(workspaceId);
                var d = pbiClient.Datasets.GetDatasources(workspaceId, datasetid);
                //var e = pbiClient.Datasets.GetDirectQueryRefreshSchedule(workspaceId, datasetid);
                //var f = pbiClient.Datasets.GetGatewayDatasources(workspaceId, datasetid);
                var g = pbiClient.Datasets.GetParameters(workspaceId, datasetid);
                var h = pbiClient.Datasets.GetRefreshHistory(workspaceId, datasetid);
                var i = pbiClient.Datasets.GetRefreshSchedule(workspaceId, datasetid);
                var j = pbiClient.Datasets.GetTables(datasetid).Value;
                var l = pbiClient.Datasets.GetTables(workspaceId, datasetid);
                var k = pbiClient.Datasets.GetTables(datasetid).Odatacontext;
                //Borrar Datos de tablas en el dataset, por nombre de tabla
                result.Data = pbiClient.Datasets.GetTables(datasetid);
            }
            catch (Exception ex)
            {


            }
            result.Exitoso = true;

            return result;
        }
    }

    //Pruebas nuevo esquema
    public static async Task<ResultObject> SetNewColumn(PowerBIClient pbiClient,  Guid workspaceId, string datasetid, string tableName, Table tableEdit)
    {
       
            ResultObject result = new ResultObject() { Exitoso = false };

            try
            {      

                await pbiClient.Datasets.PutTableInGroupAsync(workspaceId, datasetid, tableName, tableEdit);
                result.success();

            }
            catch (Exception ex)
            {

                result.error(ex.ToString());
            }
          
            return result;
        
    }

 
    #endregion
} 
}
