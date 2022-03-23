using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.DataTables
{
    public static class TransmisionDT
    {

        // arma el datatable para volcar la informacion a la base de datos
        public static DataTable GetDTPosition()
        {

            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "PositionId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "AgeOfReadingSeconds", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "AltitudeMetres", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "AssetId", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "DistanceSinceReadingKilometres", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "DriverId", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "FormattedAddress", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "Hdop", DataType = typeof(bool) },
                    new DataColumn(){ ColumnName = "Heading", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "IsAvl", DataType = typeof(bool) },
                    new DataColumn(){ ColumnName = "Latitude", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Longitude", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "NumberOfSatellites", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "OdometerKilometres", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Pdop", DataType = typeof(bool) },
                    new DataColumn(){ ColumnName = "SpeedKilometresPerHour", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "SpeedLimit", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "Timestamp", DataType = typeof(DateTime) },
                    new DataColumn(){ ColumnName = "Vdop", DataType = typeof(bool) },
                    new DataColumn(){ ColumnName = "assetPositionId", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "estadoBase", DataType = typeof(bool) },
                    new DataColumn(){ ColumnName = "fechaSistema", DataType = typeof(DateTime) }
                }
                );

            return dt;

        }




        public static DataTable GetDTDrivers()
        {

            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "driverId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "fmDriverId", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "siteIdS", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "name", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "employeeNumber", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "extendedDriverIdType", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "aditionalFields", DataType = typeof(string) }
                }
                );

            return dt;

        }

        public static DataTable GetDTAssets()
        {

            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "assetId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "groupId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "createdDate", DataType = typeof(DateTime) },
                    new DataColumn(){ ColumnName = "registrationNumber", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "assetsDescription", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "assetCodigo", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "siteId", DataType = typeof(int) }
                }
                );

            return dt;

        }

        public static DataTable GetDTTiposEventos()
        {

            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "eventTypeId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "clienteIdS", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "descriptionEvent", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "eventType", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "displayUnits", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "formatType", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "valueName", DataType = typeof(string) }
                }
                );

            return dt;

        }

        public static DataTable GetDataTableTrips()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Tripid", typeof(long)));
            dt.Columns.Add(new DataColumn("assetId", typeof(long)));
            dt.Columns.Add(new DataColumn("driverId", typeof(long)));
            dt.Columns.Add(new DataColumn("notes", typeof(string)));
            dt.Columns.Add(new DataColumn("distanceKilometers", typeof(decimal)));
            dt.Columns.Add(new DataColumn("StartOdometerKilometers", typeof(decimal)));
            dt.Columns.Add(new DataColumn("endOdometerKilometers", typeof(decimal)));
            dt.Columns.Add(new DataColumn("maxSpeedKilometersPerHour", typeof(decimal)));
            dt.Columns.Add(new DataColumn("maxAccelerationKilometersPerHourPerSecond", typeof(decimal)));
            dt.Columns.Add(new DataColumn("maxRpm", typeof(decimal)));
            dt.Columns.Add(new DataColumn("standingTime", typeof(decimal)));
            dt.Columns.Add(new DataColumn("fuelUsedLitres", typeof(decimal)));
            dt.Columns.Add(new DataColumn("startPositionId", typeof(string)));
            dt.Columns.Add(new DataColumn("endPositionId", typeof(string)));
            dt.Columns.Add(new DataColumn("startEngineSeconds", typeof(decimal)));
            dt.Columns.Add(new DataColumn("endEngineSeconds", typeof(decimal)));
            dt.Columns.Add(new DataColumn("tripEnd", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("tripStart", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("ClienteIds", typeof(int)));
            dt.Columns.Add(new DataColumn("CantSubtrips", typeof(int)));
            dt.Columns.Add(new DataColumn("FechaSistema", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("maxDecelerationKilometersPerHourPerSecond", typeof(decimal)));
            dt.Columns.Add(new DataColumn("Duracion", typeof(int)));

            return dt;


        }
        public static DataTable GetDataTableTripsMetrics()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Tripid", typeof(long)));
            dt.Columns.Add(new DataColumn("NIdleTime", typeof(int)));
            dt.Columns.Add(new DataColumn("NIdleOccurs", typeof(int)));
            dt.Columns.Add(new DataColumn("TripStart", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("ClienteIds", typeof(int)));
            dt.Columns.Add(new DataColumn("FechaSistema", typeof(DateTime)));

            return dt;


        }

        public static DataTable GetDataTableEventos()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("EventId", typeof(long)));
            dt.Columns.Add(new DataColumn("assetId", typeof(long)));
            dt.Columns.Add(new DataColumn("driverId", typeof(long)));
            dt.Columns.Add(new DataColumn("EventTypeId", typeof(long)));
            dt.Columns.Add(new DataColumn("TotalTimeSeconds", typeof(int)));
            dt.Columns.Add(new DataColumn("TotalOccurances", typeof(int)));
            dt.Columns.Add(new DataColumn("StartDateTime", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("EndDateTime", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("FuelUsedLitres", typeof(float)));
            dt.Columns.Add(new DataColumn("Value", typeof(double)));
            dt.Columns.Add(new DataColumn("Latitude", typeof(double)));
            dt.Columns.Add(new DataColumn("Longitude", typeof(double)));
            dt.Columns.Add(new DataColumn("StartOdometerKilometres", typeof(decimal)));
            dt.Columns.Add(new DataColumn("EndOdometerKilometres", typeof(decimal)));
            dt.Columns.Add(new DataColumn("AltitudMeters", typeof(int)));
            dt.Columns.Add(new DataColumn("ClienteIds", typeof(int)));
            dt.Columns.Add(new DataColumn("isebus", typeof(bool)));
            dt.Columns.Add(new DataColumn("FechaSistema", typeof(DateTime)));

            return dt;
        }

        public static DataTable GetDataTablePosiciones()
        {


            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "PositionId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "AssetId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "DriverId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "Timestamp", DataType = typeof(DateTime) },
                    new DataColumn(){ ColumnName = "Longitude", DataType = typeof(double) },
                    new DataColumn(){ ColumnName = "Latitude", DataType = typeof(double) },
                    new DataColumn(){ ColumnName = "FormattedAddress", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "AltitudeMetres", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "NumberOfSatellites", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "ClienteIds", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "fechaSistema", DataType = typeof(DateTime) }
                }
                );


            return dt;


        }
        public static DataTable GetDataTableLocalizaciones()
        {


            DataTable dt = new DataTable();

            dt.Columns.AddRange(
                new DataColumn[]
                {
                    new DataColumn(){ ColumnName = "LocationId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "OrganisationId", DataType = typeof(long) },
                    new DataColumn(){ ColumnName = "ClientIds", DataType = typeof(int) },
                    new DataColumn(){ ColumnName = "Name", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "Address", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "ShapeWkt", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "Radius", DataType = typeof(double) },
                    new DataColumn(){ ColumnName = "ColourOnMap", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "OpacityOnMap", DataType = typeof(decimal) },
                    new DataColumn(){ ColumnName = "LocationType", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "ShapeType", DataType = typeof(string) },
                    new DataColumn(){ ColumnName = "fechaSistema", DataType = typeof(DateTime) },
                    new DataColumn(){ ColumnName = "IsDeleted", DataType = typeof(string) }
                }
                );


            return dt;


        }

        public static DataTable GetDataTableSite()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("siteId", typeof(long)));
            dt.Columns.Add(new DataColumn("siteName", typeof(string)));
            dt.Columns.Add(new DataColumn("sitePadreId", typeof(long)));

            return dt;


        }


    }
}
