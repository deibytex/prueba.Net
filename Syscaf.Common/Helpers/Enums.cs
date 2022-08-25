namespace Syscaf.Common.Helpers
{
    public static class Enums
    {
        // enum para determinar el tipo de envio de los correos
        public enum TipoEnvio
        {
            TO,
            CC,
            CCO
        }
        public enum PlantillaCorreo
        {
            P_SISTEMA,
            TX_PROPIOS,
            TX_OPTIRENT,
            E_NEWUSER,
            E_MODPASS
        }
        public enum ListaDistribucion
        {
            LSSISTEMA = 1
        }
        public enum ListasParametros
        {
            NSENV,
            CORREO
        }
        public enum TipoNotificacion
        {
            Sistem = 3,

        }
        public enum EstadoProcesoGeneracionDatos
        {
            SW_NOEXEC = 19,
            SW_EXEC = 20,
            SW_ERROR = 21,
            SW_ONEXEC = 22
        }
        public enum Servicios
        {
            Eventos = 1,
            Viajes_y_Metricas = 2,
            Vehiculos_y_conductores = 3,
            Clientes_y_Bases = 4,
            Posiciones = 6,
            Reporte_Sotramac = 10,
            Envio_Correo_Tx = 11,
            PruebasSimCard = 13

        }

        public enum ReporteWebService
        {
            Errores_Viajes_Usos = 1,
            Configuracion = 2,
            Eventos = 3
        }


        public enum TipoDescargaWs
        {
            Clientes = 1,
            Usuario = 2,
            Seniales = 3,
            Sotramac = 4,
            Trace = 5,
            IMG = 3
        }


        public enum OpcionOperacion
        {
            ING,
            ADD,
            UPD,
            DEL,
            PRINT,
            DOWN,
            AUT,
            SEARCH,
            LINK
        }

        public enum TipoDescargaReporte
        {
            excel,
            pdf,
            word
        }

        public enum TipoId
        {
            viajes = 2,
            eventos = 1,
            metricas = 3
        }
        public enum PortalTipoValidacion
        {
            viajes = 1,
            eventos = 2,
            metricas = 3,
            posiciones = 4
        }

        public enum TipoReporteTrace
        {
            Rendimiento = 40,
            Odometro = 41,
            Zona_Operacion = 42,
            Rango_Velocidad = 43,
            Ralenti_Consumo = 44,
            Ralenti_Franja_Horaria = 45,
            Mechanical_Skill = 46,
            Pedales = 47,
            Puntuacion_Evento = 48,
            Alarmas = 49,
            Puntuacion_Evento_Total = 50,
            ConsolidadoDistancia = -1
        }

        public enum TipoGerencias
        {
            GERCOND,
            GERVEH
        }

        public enum TipoDiaExcepcion
        {
            Feriado = 38
        }

        public enum TiposEstadoAssets
        {
            Decommissioned,
            Available,
            Unavailable,
            Unverified
        }
    }
}
