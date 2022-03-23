using Newtonsoft.Json;
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
        public string UrlToken
        {
            get
            {
                string data = ConfigurationManager.AppSettings["UrlToken"];
                return data ?? @"http://18.222.244.24/api/ext/auth";
            }
        }
        public string UrlRequest
        {
            get
            {
                string data = ConfigurationManager.AppSettings["UrlRequest"];
                return data ?? @"http://18.222.244.24/api/ext/location";
            }
        }
        public string PwsPeg
        {
            get
            {
                string data = ConfigurationManager.AppSettings["PwsPeg"];
                return data ?? "LAPTECHSA2021*";
            }
        }
        public string UserPeg
        {
            get
            {
                string data = ConfigurationManager.AppSettings["UserPeg"];
                return data ?? "jvillamil@equitel.com.co";
            }
        }

        public async Task<string> getToken()
        {

            var stringPayload = JsonConvert.SerializeObject(new
            {
                email = UserPeg,
                password = PwsPeg
            });
            using (var client = new HttpClient())
            {
                var res = client.PostAsync(new Uri(UrlToken),
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
                    var res = client.PostAsync(new Uri(UrlRequest), new StringContent(data, Encoding.UTF8, "application/json"));
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
