using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Syscaf.Data.Helpers
{
    class Constants
    {
        public static DateTimeFormatInfo CultureDate_Es
        {
            get
            {
                return CultureInfo.CreateSpecificCulture("es-EN").DateTimeFormat;
            }
        }
    }
}
