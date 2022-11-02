using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Syscaf.ApiCore.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabeceraAsync<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double cantidad =  await queryable.CountAsync();
           
            
            httpContext.Response.Headers.Add("TotalRegistros", cantidad.ToString());
        }
        public  static Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext httpContext,
            IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double cantidad =  queryable.Count();
            httpContext.Response.Headers.Add("TotalRegistros", cantidad.ToString());
            return Task.CompletedTask;
        }
    }
}
