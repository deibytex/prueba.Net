using Newtonsoft.Json;
using Syscaf.Common.Helpers;
using Syscaf.Common.Models.FRESH;
using Syscaf.Common.Utils;
using Syscaf.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Syscaf.Common.Services
{
    public class FreshDeskServiceConn
    {
        private readonly FreshdeskVariablesConn _UriVariables;
        public FreshDeskServiceConn(FreshdeskVariablesConn _UriVariables)
        {
            this._UriVariables = _UriVariables;

        }
        public async Task<ResultObject> GetTickets()
        {
            var r = new ResultObject();
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new  AuthenticationHeaderValue("Basic", Constants.Base64Encode($"{_UriVariables.Key}:{_UriVariables.Clave}"));
                    var res = client.GetAsync(new Uri($"{_UriVariables.Dominio}/tickets" ));
                    var status = res.Result.StatusCode;
                    var result = await res.Result.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(result, Encoding.UTF8);
                    var texto = readStream.ReadToEnd();
                    r.Data = JsonConvert.DeserializeObject<List<TicketsFreshDesk>>(texto) ;
                    r.Exitoso = true;
                    HttpResponseHeaders h = res.Result.Headers;
                    return r;
                }
                catch (Exception e)
                {
                    r.Mensaje = "500," + e.ToString();
                }
                return r;
            }

        }
        public async Task<ResultObject> GetTicketsFields()
        {

            var r = new ResultObject();
            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Constants.Base64Encode($"{_UriVariables.Key}:{_UriVariables.Clave}"));
                    var res = client.GetAsync(new Uri($"{_UriVariables.Dominio}/ticket_fields"));
                    var status = res.Result.StatusCode;
                    var result = await res.Result.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader(result, Encoding.UTF8);
                    var texto = readStream.ReadToEnd();
                    r.Data = JsonConvert.DeserializeObject<List<FreshDeskVM>>(texto);
                    r.Exitoso = true;
                    HttpResponseHeaders h = res.Result.Headers;
                    return r;
                }
                catch (Exception e)
                {
                    r.Mensaje = "500," + e.ToString();
                }
                return r;
            }

        }

    }
}
