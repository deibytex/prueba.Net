using Syscaf.Service.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using static Syscaf.Common.Helpers.Enums;

namespace Syscaf.Service.Helpers
{
    public class MailNotification
    {
        
        private MailMessage _mail;
        private SmtpClient _cliente;
        public  MailNotification(string userCredentials, string passwordCredential, ICommonService CommonService)
        {
            // configuracion del cuerpo del correo
            _mail = new MailMessage
            {
                SubjectEncoding = Encoding.UTF32,
                BodyEncoding = Encoding.UTF32,
                IsBodyHtml = true
            };

            string Host = CommonService.GetDetalleListaBySigla("SMTP").Valor;

            _cliente = new SmtpClient
            {
                //Hay que crear las credenciales del correo emisor
                Credentials = new System.Net.NetworkCredential(userCredentials, passwordCredential),
                //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
                Port = 587,
                EnableSsl = true,
                //TRAERLO DESDE LOS PARAMETROS DEL SISTEMA
                Host = Host
            };
        }

        // retorna la instancia
        public MailMessage getInstanciaMail()
        {
            return this._mail;
        }
        public SmtpClient getInstanciaSmtp()
        {
            return this._cliente;
        }

        public void AddRemitente(List<ListaCorreoVM> lstcorreo)
        {
            foreach (var item in lstcorreo)
            {
                // convertimos el valor del tipo de envio en un enum 
                // para dinamizar el ingreso
                TipoEnvio tipoEnvio = (TipoEnvio)Enum.Parse(typeof(TipoEnvio), item.TipoEnvio);
                switch (tipoEnvio)
                {
                    case TipoEnvio.TO:
                        this._mail.To.Add(item.Correo);
                        break;
                    case TipoEnvio.CC:
                        this._mail.CC.Add(item.Correo);
                        break;
                    case TipoEnvio.CCO:
                        this._mail.Bcc.Add(item.Correo);
                        break;

                }
            }
        }


        private void AddCorreo(List<string> lstcorreo, TipoEnvio tipoEnvio)
        {

            foreach (string correo in lstcorreo)
                switch (tipoEnvio)
                {
                    case TipoEnvio.TO:
                        this._mail.To.Add(correo);
                        break;
                    case TipoEnvio.CC:
                        this._mail.CC.Add(correo);
                        break;
                    case TipoEnvio.CCO:
                        this._mail.Bcc.Add(correo);
                        break;

                }
        }
        // adiciona un correo  depentiendo del tipo antes de ser enviado el correo
        public void AddRemitente(string correo, TipoEnvio tipoEnvio)
        {
            AddCorreo(new List<string>() { correo }, tipoEnvio);
        }
        // adiciona a partir de una lista de string los correos 
        public void AddRemitente(List<string> lstcorreo, TipoEnvio tipoEnvio)
        {
            AddCorreo(lstcorreo, tipoEnvio);
        }


        public ResultObject SendEmail(string emailFrom, string emailSubject, string bodySubject, string archivo, ILogService LogService)
        {
            try
            {
                _mail.Subject = emailSubject;
                //Cuerpo del Mensaje
                _mail.Body = bodySubject;
                if (archivo.Length > 2) _mail.Attachments.Add(new Attachment(archivo));
                //Correo electronico desde la que enviamos el mensaje
                _mail.From = new MailAddress(emailFrom);

                _cliente.Send(_mail);
            }
            catch (SmtpException ex)
            {
                LogService.SetLog("Error Enviando Correo" + ex.ToString(), emailFrom, "MailNotification -sendEmail ");
            }

            return new ResultObject() { Exitoso = true };
        }


    }
}
