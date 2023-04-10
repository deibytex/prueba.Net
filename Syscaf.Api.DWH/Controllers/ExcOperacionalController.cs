

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using System.Text;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming.Values;
using Syscaf.Common.Helpers;
using Syscaf.Common.Models.EXC_OPERACIONAL;
using Syscaf.Data.Helpers.Portal;
using Syscaf.Service.Portal;

namespace Syscaf.Api.DWH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcOperacionalController : BaseController
    {
        private readonly IPortalMService _portalService;
        private readonly IAdmService _admService;
        public ExcOperacionalController(IPortalMService _portalService, IAdmService _admService) {
            this._portalService = _portalService;
            this._admService = _admService;
        }
        /// <summary>
        /// Controlador de transmisión
        /// </summary>
        /// <param name="id">aaa</param>
        [HttpPost("ReporteExcelencia")]
        public async Task<byte[]> ReporteExcelencia([FromBody] Dictionary<string, string>? parametros, [FromQuery] string Clase, [FromQuery] string NombreConsulta, [FromQuery] string TipoReporte)
        {
            try
            {
                // parametrizamos los indices a mostrar

                Dictionary<string, int> cabeceras = new Dictionary<string, int>()
                {
                    { "InformeConductor", 3 },
                    { "InformeVehiculos", 9 },
                    { "InformeConductorVehiculos", 4},
                };

                // guarnamos la configuracion de las columnas a mostrar y el orden en que se deben mostrar
                Dictionary<string, List<dynamic>> ConfiguracionReporte = new Dictionary<string, List<dynamic>>()
                {
                    { "InformeConductor", ConductorC },
                    { "InformeVehiculos", VehiculoC},
                    { "InformeConductorVehiculos", ConductorVehiculoC },
                };

                // guarnamos la configuracion de las formulas a mostrar y el orden en que se deben mostrar
                Dictionary<string, dynamic> ConfiguracionFormulas = new Dictionary<string, dynamic>()
                {
                    { "InformeConductor", ConductorFormulas },
                    { "InformeVehiculos", VehiculoFormulas},
                    { "InformeConductorVehiculos", ConductorVehiculoFormulas },
                };



                string assetTypeId = parametros["assetTypeId"];
                DateTime fechainicial =  Convert.ToDateTime(parametros["FechaInicial"]);
                var dynamic = new Dapper.DynamicParameters();
                if (parametros != null)
                    foreach (var kvp in parametros)
                    {
                        dynamic.Add(kvp.Key, kvp.Value);
                    }

                // obtenemos los datos a imprimir en los reportes
               var datos  =  await _portalService.getDynamicValueDWH(Clase, NombreConsulta, dynamic);

                // obtenemos el factor a implemetar
                 dynamic = new Dapper.DynamicParameters();
                 dynamic.Add("SiglaD", "Bus_Fact_M3");
                dynamic.Add("Sigla", null);
                var factor = await _admService.getDynamicValueCore("PortalQueryHelper", "GetDetallesListaBySisglas", dynamic);
               
                string _ValorfactorM3 = factor.First().Valor;
               

                var path = System.IO.Path.GetDirectoryName(
          System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                path = path.Substring(6);
                IWorkbook workbook;
                using( FileStream file = new FileStream($"bin/Debug/net6.0/Excelencia_Operacional/{TipoReporte}.xls", FileMode.Open, FileAccess.Read))
                {
                    workbook = new HSSFWorkbook(file);
                    var fs = new MemoryStream();               
                    

                    int rowTemplate = 7;
                    int rowTarget = rowTemplate +1;
                    int totalRows = datos.Count();

                        ISheet sheet1 = workbook.GetSheet(TipoReporte);
                    IRow row = sheet1.GetRow(rowTemplate);
                    // row cabeceerra

                    // 9   informe vehuculo
                    // 4  informe operador vehiculo
                    int celCabecera = cabeceras[TipoReporte];
                    dynamic Formulas = ConfiguracionFormulas[TipoReporte];
                    IRow rowcabecera = sheet1.GetRow(4);
                    rowcabecera.GetCell(celCabecera++).SetCellValue(Common.Helpers.Helpers.MonthName(fechainicial.Month)); // mes
                    rowcabecera.GetCell(celCabecera++).SetCellValue(fechainicial); // fecha
                    rowcabecera.GetCell(celCabecera++).SetCellFormula($"(SUM({Formulas.RendimientoD}{rowTemplate}:{Formulas.RendimientoD}{rowTemplate + totalRows })/SUM({Formulas.RendimientoC}{rowTemplate}:{Formulas.RendimientoC}{rowTemplate + totalRows }))/{_ValorfactorM3}");// rendimiento combustible

                    // formulas promedio
                    rowcabecera.GetCell(celCabecera++).SetCellFormula($"AVERAGE({Formulas.Freno}{rowTemplate}:{Formulas.Freno}{rowTemplate + totalRows})"); // uso del freno por aplicacion 
                    rowcabecera.GetCell(celCabecera++).SetCellFormula($"AVERAGE({Formulas.Inercia}{rowTemplate}:{Formulas.Inercia}{rowTemplate + totalRows })"); // % inercia
                    rowcabecera.GetCell(celCabecera++).SetCellFormula($"AVERAGE({Formulas.Ralenti}{rowTemplate}:{Formulas.Ralenti}{rowTemplate + totalRows })"); // %ralenti
                    rowcabecera.GetCell(celCabecera++).SetCellFormula($"AVERAGE({Formulas.Velocidad}{rowTemplate}:{Formulas.Velocidad}{rowTemplate + totalRows })"); // velocidad promedio

                    List<dynamic> columnas = ConfiguracionReporte[TipoReporte];
                    datos/*.Where( w=> w.Nombre.Contains("HUNDELSHA") ).ToList()*/.ForEach(elemento => {

                        var result = new RouteValueDictionary(elemento);
                        
                        sheet1.CopyRow(rowTemplate, rowTarget);
                        columnas.ForEach(columna => {
                            int colCell = columna.celda;
                            IRow row = sheet1.GetRow(rowTemplate);
                            if (result[columna.columna] == null)
                            {
                                row.GetCell(colCell).SetCellValue("");
                            }
                            else
                            {
                                switch (columna.type)
                                {
                                    case "decimal":
                                        row.GetCell(colCell).SetCellValue(Decimal.ToDouble(result[columna.columna] ?? 0));
                                        break;
                                    case "int":
                                        int valorElemento = result[columna.columna] ?? 0;
                                        row.GetCell(colCell).SetCellValue(valorElemento);
                                        break;
                                    default:
                                        string value = result[columna.columna];
                                        row.GetCell(colCell).SetCellValue(value);
                                        break;
                                }

                            }

                            
                        });                 

                            rowTemplate++;
                            rowTarget++;
                        });

                   // sheet1.RemoveRow(row);
                   
                    HSSFFormulaEvaluator.EvaluateAllFormulaCells(workbook);
                    workbook.Write(fs);
                   
                        fs.Position = 0;
                    
                    return fs.ToArray();
                    

                }

               
                
            }
            catch (Exception ex)
            {

            }
            
            return null;
        }


        private

            List<dynamic> ConductorVehiculoC = new List<dynamic>() {
                new {        celda = 0,     columna = "Cedula",        tdoble =false , type ="string"    },
                new {        celda = 1,     columna = "Nombre",        tdoble =false  , type ="string"   },
                new {        celda = 3,     columna = "Vehiculo",        tdoble =false , type ="string"    },
                new {        celda = 4,     columna = "DistanciaRecorridaAcumulada",        tdoble =true , type ="decimal"    },
                new {        celda = 5,     columna = "ConsumodeCombustibleAcumulado",        tdoble =true , type ="decimal"    },
                new {        celda = 6,     columna = "RendimientoCumbustibleAcumulado",        tdoble =true , type ="decimal"    },
                new {        celda = 7,     columna = "UsoDelFreno",        tdoble =false   , type ="int"  },
                new {        celda = 8,     columna = "PorDeInercia",        tdoble =true , type ="decimal"    },
                new {        celda = 9,     columna = "PorDeRalenti",        tdoble =true   , type ="decimal"  },
                new {        celda = 10,     columna = "VelPromedio",        tdoble =true   , type ="decimal"  }

            };
        private

           List<dynamic> ConductorC = new List<dynamic>() {
                new {        celda = 0,     columna = "Cedula",        tdoble =false  , type ="string"   },
                new {        celda = 1,     columna = "Nombre",        tdoble =false  , type ="string"   },
                new {        celda = 3,     columna = "DistanciaRecorridaAcumulada",        tdoble =true  , type ="decimal"    },
                new {        celda = 4,     columna = "ConsumodeCombustibleAcumulado",        tdoble =true   , type ="decimal"  },
                new {        celda = 5,     columna = "RendimientoCumbustibleAcumulado",        tdoble =true   , type ="decimal"  },
                new {        celda = 6,     columna = "UsoDelFreno",        tdoble =false    , type ="int"  },
                new {        celda = 7,     columna = "PorDeInercia",        tdoble =true  , type ="decimal"   },
                new {        celda = 8,     columna = "PorDeRalenti",        tdoble =true  , type ="decimal"   },
                new {        celda = 9,     columna = "VelPromedio",        tdoble =true    , type ="decimal" }

           };
        private

          List<dynamic> VehiculoC = new List<dynamic>() {
                new {        celda = 0,     columna = "Posicion",        tdoble =false   , type ="int"  },
                new {        celda = 1,     columna = "Vehiculo",        tdoble =false   , type ="string"  },
                new {        celda = 3,     columna = "DistanciaRecorridaAcumulada",        tdoble =true  , type ="decimal"   },
                new {        celda = 4,     columna = "ConsumodeCombustibleAcumulado",        tdoble =true  , type ="decimal"   },
                new {        celda = 5,     columna = "RendimientoCumbustibleAcumulado",        tdoble =true  , type ="decimal"   },
                new {        celda = 6,     columna = "UsoDelFreno",        tdoble =false  , type ="int"    },
                new {        celda = 7,     columna = "PorDeInercia",        tdoble =true   , type ="decimal"  },
                new {        celda = 8,     columna = "PorDeRalenti",        tdoble =true   , type ="decimal"  },

            

                new {        celda = 9,     columna = "Co2Equivalente",        tdoble =true  , type ="decimal"   },
                new {        celda = 10,     columna = "GalEquivalente",        tdoble =true  , type ="decimal"   },
                new {        celda = 11,     columna = "ConsumokWh",        tdoble =true  , type ="decimal"   },
                new {        celda = 12,     columna = "COmgkWh",        tdoble =true   , type ="decimal"  },
                new {        celda = 13,     columna = "NOxmgkWh",        tdoble =true   , type ="decimal"  },
                new {        celda = 14,     columna = "PMMasamgkWh",        tdoble =true  , type ="decimal"   },


                new {        celda = 15,     columna = "VelPromedio",        tdoble =true    , type ="decimal" }

          };

        // se condfiguran las columnas de las formulas
        private dynamic ConductorFormulas = new
        {
            RendimientoD = "D",
            RendimientoC = "E",
            Freno = "G",
            Inercia = "H",
            Ralenti = "I",
            Velocidad = "J"
            
        };

        private dynamic ConductorVehiculoFormulas = new
        {
            RendimientoD = "E",
            RendimientoC = "F",
            Freno = "H",
            Inercia = "I",
            Ralenti = "J",
            Velocidad = "k"

        };

        private dynamic VehiculoFormulas = new
        {
            RendimientoD = "D",
            RendimientoC = "E",
            Freno = "G",
            Inercia = "H",
            Ralenti = "I",
            Velocidad = "P"

        };
    }



  
}
