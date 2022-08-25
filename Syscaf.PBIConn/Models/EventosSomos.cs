// ----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
// ----------------------------------------------------------------------------

namespace Syscaf.PBIConn.Models
{
    using System;

    public class EventosSomos
    {        
        public long DriverId { get; set; }      
        public long AssetId { get; set; }
        public DateTime Fecha { get; set; }
        public double? SOC { get; set; }
        public double? ContadorCarga { get; set; }
        public double? ContadorDescarga { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public int? Altitud { get; set; }
    }
}