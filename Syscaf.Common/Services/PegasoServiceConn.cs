using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Syscaf.Common.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace Syscaf.Common.Services
{
    public class PegasoServiceConn
    {
        private readonly PegVariablesConn _UriVariables;
        public PegasoServiceConn(PegVariablesConn _UriVariables)
        {
            this._UriVariables = _UriVariables;

        }

        public async Task<string> getToken()
        {

            var stringPayload = JsonConvert.SerializeObject(new
            {
                email = _UriVariables.UserPeg,
                password = _UriVariables.PwsPeg
            });
            using (var client = new HttpClient())
            {
                var res = client.PostAsync(new Uri(_UriVariables.UrlToken),
                  new StringContent(stringPayload,
                    Encoding.UTF8, "application/json")
                );

                try
                {
                    res.Result.EnsureSuccessStatusCode();
                    string data = await res.Result.Content.ReadAsStringAsync();
                    dynamic jo = JsonConvert.DeserializeObject(data);
                    return (string)jo.token;

                }
                catch (Exception e)
                {
                }
            }


            return "";
        }

        public async Task<string> SendRequest(string token, string data)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var res = client.PostAsync(new Uri(_UriVariables.UrlRequest), new StringContent(data, Encoding.UTF8, "application/json"));
                    var status = res.Result.StatusCode;
                    string result = await res.Result.Content.ReadAsStringAsync();


                    HttpResponseHeaders h = res.Result.Headers;


                    return result;
                }
                catch (Exception e)
                {
                    return "500," + e.ToString();
                }
            }

        }
    }
}
