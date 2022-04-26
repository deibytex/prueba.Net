
using Microsoft.Extensions.Options;
using Syscaf.Common.Utils;


namespace Syscaf.Common.Helpers
{
    public static  class Helpers
    { 
        
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars);
        }
    }


}
