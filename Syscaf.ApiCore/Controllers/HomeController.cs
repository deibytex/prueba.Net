
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Syscaf.ApiCore;
using Syscaf.ApiCore.ViewModels;
using Syscaf.Data.Helpers;
using Syscaf.Service.eBus.Gcp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Syscaf.ApiTx.Controllers
{
    public class HomeController : Controller
    {
        public string Periodo { get; set; }
        readonly PubsubOptions _options;
        readonly GlobalVariables _globalVariables;
        // Lock protects all the static members.
        static object s_lock = new object();
        // Keep the received messages in a list.
        static List<string> s_receivedMessages = new List<string>();

        public ConfiguracionPubSub _configuracionPubSub = null;

        readonly IeBusGcpService _ieBusGcpService;
        public HomeController(IOptions<PubsubOptions> options, IOptions<GlobalVariables> _globalVariables, IeBusGcpService _ieBusGcpService)
        {
            _options = options.Value;
            this._globalVariables = _globalVariables.Value;
            this._ieBusGcpService = _ieBusGcpService;


        }
        // [START gae_flex_pubsub_push]
        /// <summary>
        /// Handle a push request coming from pubsub.
        /// </summary>
        /// 

        private void setConfiguracionPubSubDataBase()
        {
            // verificamos que no exista la clase para que asigne los valores
            if (_configuracionPubSub == null)
            {
                // traemos la configuracion de la BD
                var configuracion = _ieBusGcpService.getConfigurationPubSub().Result;

                if (configuracion.Count > 0)
                {
                    _configuracionPubSub = new ConfiguracionPubSub(); // instanciamos la clase
                    var Type = typeof(ConfiguracionPubSub);
                    var propertys = Type.GetProperties(); // traemos las propiedades 

                    foreach (var property in propertys) // iteramos las propiedades para poder asignar el valor a la nueva instancia de la clase
                    {
                        string value = configuracion.Where(w => w.Sigla.Equals(property.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault()?.Value;
                        if (value != null)
                        {
                            object valor = value;
                            if (property.PropertyType.Name.Contains("Boolean")) // si es una propieda boleana se debera convert a ese typo de datos
                                valor = bool.Parse(value);
                            property.SetValue(_configuracionPubSub, valor);
                        }
                    }
                }
            }

        }

        [HttpPost]
        [Route("/Push")]
        public IActionResult Push([FromBody] PushBody body)
        {
            DateTime FechaActual = slnHelper.GetFechaServidor(TimeZoneInfo.FindSystemTimeZoneById(_globalVariables.NameZone));
   //         var resultadoEnc = _ieBusGcpService.InsertaMensaje(new Data.Models.eBus.gcp.GetMessages() { FechaHora = FechaActual, Mensaje = "InicioOperacion", ProfileData = null }, $"{FechaActual.Month}{FechaActual.Year}");

            // traemos la configuracion de la base de datos
            setConfiguracionPubSubDataBase();

           
            // desemcripta los datos a guradar 
            var messageBytes = Convert.FromBase64String(body.message.data);
            string message = System.Text.Encoding.UTF8.GetString(messageBytes);
            //


            string AuthorizationHeader = Request.Headers["Authorization"];
            string ProfileString = null;
            if (AuthorizationHeader != null || AuthorizationHeader.Length != 0
                    || AuthorizationHeader.Split(" ").Length == 2)
            {
                String authorization = AuthorizationHeader.Split(" ")[1];
                try
                {
                    GoogleJsonWebSignature.Payload payload = GoogleJsonWebSignature.ValidateAsync(authorization).Result;
                    var profileData = new { Audience = payload.Audience.ToString(), payload.Subject, payload.Email, payload.Issuer, payload.EmailVerified, payload.ExpirationTimeSeconds };                   
                    if (_configuracionPubSub.VERIFICA_AUTHORIZATION)
                    {                         
                        // verifica que los datos sean los correctos  sino devuelve un bad request
                        if (!profileData.Subject.Equals(_configuracionPubSub.AZP) ||
                             !profileData.EmailVerified ||
                             !profileData.Email.Equals(_configuracionPubSub.EMAIL))
                            return new BadRequestResult();
                    }
                }
                catch (Exception)
                {
                    return new BadRequestResult();
                }
            }

            var resultado = _ieBusGcpService.InsertaMensaje(new Data.Models.eBus.gcp.GetMessages() { FechaHora = FechaActual, Mensaje = message, ProfileData = ProfileString }, $"{FechaActual.Month}{FechaActual.Year}");

            if (!resultado.Result)
                return new BadRequestResult();

            return new OkResult();
        }
        // [END gae_flex_pubsub_push]

        public IActionResult Error()
        {
            return View();
        }
    }

    /// <summary>
    /// Pubsub messages will arrive in this format.
    /// </summary>
    public class PushBody
    {
        public PushMessages message { get; set; }
        public string subscription { get; set; }
    }

    public class PushMessages
    {
        public Dictionary<string, string> attributes { get; set; }
        public string data { get; set; }
        public string message_id { get; set; }
        public string publish_time { get; set; }
    }

}
