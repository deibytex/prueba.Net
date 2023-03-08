using Microsoft.Reporting.WebForms;
using System;

using System.Web;

using System.Collections.Generic;
using System.IO;

namespace Syscaf.Report.Helpers
{
    public /*abstract*/ class ReportBase
    {
        private string DirectorioReportesRelativo { get; set; }

        public string NombreReporte { get; set; }

        public string PathEnviroment { get; set; }


        private string FullPathReport { get; set; }

        private ReportViewer Reporte { get; set; }

        private ReportDataSource DataSource { get; set; }

        /// <summary>
        /// Dispara el evento CustomSubreportProcessingEventHandler
        /// </summary>
        public bool ContieneSubReporte { get; set; }

        public List<ReportParameter> Parametros { get; set; }

        public List<ReportDataSource> SubReporteDataSource { get; set; }


        public void AgregarParametro(string nombre, string valor)
        {
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(valor))
            {
                if (Parametros == null)
                    Parametros = new List<ReportParameter>();
                Parametros.Add(new ReportParameter(nombre, valor));
            }
            else
                throw new Exception("Parametros sin nombre o valor");
        }

        public void Inicializar(string DirectorioReportesRelativo)
        {
            try
            {


                DirectorioReportesRelativo = DirectorioReportesRelativo ?? "bin/Reportes/Sotramac/";
                //string rutaRecursos = ConfigurationManager.AppSettings["PathRecursos"];
                string tem = string.Format("{0}.{1}", NombreReporte, "rdlc");
                FullPathReport = PathEnviroment + DirectorioReportesRelativo + tem;
                if (!File.Exists(FullPathReport))
                {
                    throw new FileNotFoundException(string.Format("No existe reporte en la ruta : {0}", FullPathReport));
                }




            }
            catch (System.Exception ex)
            {
                throw new Exception("Error al inicializar: " + ex.Message);
            }
        }

        private void IncludeDataSetProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            if (SubReporteDataSource == null)
                SubReporteDataSource = new List<ReportDataSource>();
            foreach (ReportDataSource d in SubReporteDataSource)
                e.DataSources.Add(d);
        }
        public virtual void addEventSubReportes()
        {
            Reporte.LocalReport.SubreportProcessing += new
               SubreportProcessingEventHandler(IncludeDataSetProcessingEventHandler);
        }

        public void addDataSetSubreport(string nombreDataSet, object dataSet)
        {
            if (Reporte == null)
                Reporte = new ReportViewer();

            if (SubReporteDataSource == null)
                SubReporteDataSource = new List<ReportDataSource>();

            this.SubReporteDataSource.Add(new ReportDataSource(nombreDataSet, dataSet));
        }
        public virtual bool CargarDataset(string nombreDataSet, object dataSet)
        {
            bool result = true;
            if (Reporte == null)
                Reporte = new ReportViewer();
            DataSource = new ReportDataSource(nombreDataSet, dataSet);
            Reporte.LocalReport.DataSources.Add(DataSource);
            Reporte.LocalReport.Refresh();

            return result;
        }

        public virtual bool CargarDataset(string nombreDataSet, object dataSet, bool isSubreport)
        {
            bool result = true;
            if (Reporte == null)
                Reporte = new ReportViewer();
            DataSource = new ReportDataSource(nombreDataSet, dataSet);
            Reporte.LocalReport.DataSources.Add(DataSource);
            Reporte.LocalReport.Refresh();

            return result;
        }


        public virtual bool CargarDatos(string nombreDataSet, object dataSet, string DirectorioRelativo = null)
        {
            bool result = true;
            try
            {
                Inicializar(DirectorioRelativo);
                if (Reporte == null)
                    Reporte = new ReportViewer();

                Reporte.LocalReport.ReportPath = FullPathReport;
                DataSource = new ReportDataSource(nombreDataSet, dataSet);
                Reporte.LocalReport.DataSources.Add(DataSource);
                //Asigmaos parametro si se configuraron
                if (Parametros != null)
                    Reporte.LocalReport.SetParameters(Parametros);
                ////Personalizamos en enlace de datos de los reporte
                Reporte.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en método CargarDatos los datos de reporte: " + ex.Message);
                throw new Exception("Error en método CargarDatos los datos de reporte: " + ex.Message);
            }

            return result;
        }

        //public byte[] ObtenerReporte(int formato)
        //{
        //    byte[] file = null;
        //    file = Reporte.LocalReport.Render(formato.ToString());
        //    return file;
        //}


        public byte[] ObtenerReporte(string formato)//pdf//xls//word

        {
            byte[] file = null;

            file = Reporte.LocalReport.Render(formato);

            return file;
        }

        //  public abstract void CustomSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e);
    }
}

