using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Syscaf.Common.Helpers.EBUS;

namespace Syscaf.Service.DataTableSql
{
    public static class HelperDatatable
    {

        public static DataTable ToDataTable<T>(this List<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));

         
            
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];

            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static DataTable GetDataTableEventos()
        {
            var dt = new DataTable();
            dt.Columns.Add("EventId", typeof(long));
            dt.Columns.Add("DriverId", typeof(long));
            dt.Columns.Add("AssetId", typeof(long));
            dt.Columns.Add("EventDateTime", typeof(DateTime));
            dt.Columns.Add("ReceivedDateTime", typeof(DateTime));
            dt.Columns.Add("EventTypeId", typeof(long));
            dt.Columns.Add("OdometerKilometres", typeof(decimal));
            dt.Columns.Add("ValueUnits", typeof(string));
            dt.Columns.Add("ValueType", typeof(string));
            dt.Columns.Add("ValueEvent", typeof(decimal));
            dt.Columns.Add("SpeedLimit", typeof(decimal));
            dt.Columns.Add("PositionId", typeof(long));
            dt.Columns.Add("PriorityEvent", typeof(int));
            dt.Columns.Add("Armed", typeof(byte));
            dt.Columns.Add("assetIdS", typeof(int));
            dt.Columns.Add("driverIdS", typeof(int));
            dt.Columns.Add("eventTypeIdS", typeof(int));
            dt.Columns.Add("TotalOccurances", typeof(int));
            dt.Columns.Add("TotalTimeSeconds", typeof(int));

            return dt;
        }

        public static DataTable GetDataTableIdentity()
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(long));
            return dt;
        }

        public static DataTable GetDataTableMetricas()
        {




            var dt = new DataTable();
            dt.Columns.Add("TripId", typeof(long));
            dt.Columns.Add("MaxAccelerationKilometersPerHourPerSecond", typeof(decimal));
            dt.Columns.Add("MaxSpeedKilometersPerHour", typeof(decimal));
            dt.Columns.Add("ExcessiveIdleOccurs", typeof(int));
            dt.Columns.Add("HarshAccelerationOccurs", typeof(int));
            dt.Columns.Add("HarshBrakeOccurs", typeof(int));
            dt.Columns.Add("SpeedingOccurs", typeof(int));
            dt.Columns.Add("OutOfGreenBandTime", typeof(int));
            dt.Columns.Add("ExcessiveIdleTime", typeof(int));
            dt.Columns.Add("MaxDecelerationKilometersPerHourPerSecond", typeof(decimal));
            dt.Columns.Add("OverRevTime", typeof(int));
            dt.Columns.Add("HarshBrakeTime", typeof(int));
            dt.Columns.Add("SpeedingTime", typeof(int));
            dt.Columns.Add("DistanceKilometers", typeof(decimal));
            dt.Columns.Add("Duration", typeof(int));
            dt.Columns.Add("DrivingTime", typeof(int));
            dt.Columns.Add("TripStart", typeof(DateTime));
            dt.Columns.Add("DriverId", typeof(long));
            dt.Columns.Add("AssetId", typeof(long));
            dt.Columns.Add("HarshAccelerationTime", typeof(int));
            dt.Columns.Add("MaxRpm", typeof(int));
            dt.Columns.Add("FechaSistema", typeof(DateTime));
            dt.Columns.Add("IdleOccurs", typeof(int));
            dt.Columns.Add("IdleTime", typeof(int));


            return dt;
        }
    }
}
