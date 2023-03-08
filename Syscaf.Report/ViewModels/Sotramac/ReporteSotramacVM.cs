namespace Syscaf.Report.ViewModels.Sotramac
{
    public class ReporteSotramacVM
    {
        public int? Posicion { get; set; }
        public string Cedula { get; set; }
        public string Vehiculo { get; set; }
        public string Nombre { get; set; }
        public decimal? DistanciaRecorridaAcumulada { get; set; }
        public decimal? ConsumodeCombustibleAcumulado { get; set; }
        public decimal? DistanciaRecorridaUltimoDia { get; set; }
        public decimal? RendimientoCumbustibleAcumulado { get; set; }
        public int? UsoDelFreno { get; set; }
        public int? PorDeInercia { get; set; }
        public int? PorDeRalenti { get; set; }
        public decimal? Co2Equivalente { get; set; }
        public decimal? GalEquivalente { get; set; }
        public decimal? ConsumokWh { get; set; }
        public decimal? COmgkWh { get; set; }
        public decimal? NOxmgkWh { get; set; }
        public decimal? PMMasamgkWh { get; set; }
        public string TipoOperacion { get; set; }
        public decimal? VelPromedio { get; set; }
    }
}
