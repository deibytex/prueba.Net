using System;
using System.Collections.Generic;
using System.Text;

namespace Syscaf.Data.Helpers.eBus.Gcp
{
    // trae los querys necesarios para la base de datos ebus 
    // modulo gcp
    public static class eBusGcpQuery
    {
        public static string _tableGetMessages = "INSERT INTO ITS.Messages_{0}( FechaHora,  Mensaje,ProfileData ) Values (@FechaHora, @Mensaje, @ProfileData)";

        public static string _Sp_CreateTableMessageByPeriod = "ITS.CreateTableMessageByPeriod ";
        public static string _Sp_InsertaMensaje = "ITS.InsertaMensaje ";

        public static string _table_ConfigurationPubSub = "SELECT  CPS.Sigla, CPS.Name, CPS.Value FROM ITS.ConfigurationPubSub AS CPS ";
    }
}
