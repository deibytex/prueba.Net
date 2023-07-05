using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.DataTables
{
    public static class EbusDT
    {
        public static DataTable GetDTEventActiveRecarga()
        {

            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "EventId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "FechaHora", DataType = typeof(DateTime) },
                    new DataColumn(){ ColumnName = "EventTypeID", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "Consecutivo", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "Carga", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "AssetId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "DriverId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "Soc", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Corriente", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Voltaje", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Potencia", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Energia", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "ETA", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Odometer", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Latitud", DataType = typeof(float) },
                    new DataColumn(){ ColumnName = "Longitud", DataType = typeof(float) },
                    new DataColumn(){ ColumnName = "fechaSistema", DataType = typeof(DateTime) } }
                );

            return dt;

        }

        public static DataTable GetDTEventActiveViaje()
        {

            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "EventId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "FechaHora", DataType = typeof(DateTime) },
                    new DataColumn(){ ColumnName = "EventTypeID", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "AssetId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "DriverId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "Altitud", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "EnergiaRegenerada", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "EnergiaDescargada", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Soc", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Energia", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "PorRegeneracion", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Distancia", DataType = typeof(decimal) },   
                    new DataColumn(){ ColumnName = "Localizacion", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "Latitud", DataType = typeof(float) },
                    new DataColumn(){ ColumnName = "Longitud", DataType = typeof(float) },
                    new DataColumn(){ ColumnName = "Autonomia", DataType = typeof(decimal) },
                     new DataColumn(){ ColumnName = "VelocidadPromedio", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "fechaSistema", DataType = typeof(DateTime) }
                }
                );

            return dt;

        }
    }
}
