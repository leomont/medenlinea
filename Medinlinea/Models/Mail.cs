using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mime;
using System.Drawing;

namespace Medinlinea.Models
{
    public class Mail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public StringBuilder Body { get; set; }

        public Mail() { }

        public Mail(string to, string subject, StringBuilder body)
        {
            this.From = "medenlinea@hotmail.com";
            this.To = to;
            this.Subject = subject;
            this.Body = body;
        }
        public Task<bool> sendMail()
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(this.To);
                mail.From = new MailAddress(this.From);
                mail.Subject = this.Subject;
                mail.Body = this.Body.ToString();
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.live.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("medenlinea@hotmail.com", "server_local1");
                smtp.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate(object s, X509Certificate certificate,
                             X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };
                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {
                SystemLog log = new SystemLog();
                log.ErrorLog(ex.Message);
            }
            return Task.FromResult<bool>(resultado);
        }

        public Task<bool> sendMailAttachments(List<ArchivoAdjunto> listado)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(this.To);
                mail.From = new MailAddress(this.From);
                mail.Subject = this.Subject;
                mail.Body = this.Body.ToString();
                mail.IsBodyHtml = true;
                foreach (ArchivoAdjunto  item in listado)
                {
                    item.archivo.Position = 0;
                    ContentType mContentType = new ContentType(item.tipo);
                    //mContentType.MediaType = item.tipo;
                    mContentType.Name = item.nombre;
                    Attachment inline = new Attachment(item.archivo,mContentType);
                    inline.ContentDisposition.Inline = true;
                    inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                    mail.Attachments.Add(inline);
                }
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.live.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential
                ("medenlinea@hotmail.com", "server_local1");
                smtp.EnableSsl = true;
                ServicePointManager.ServerCertificateValidationCallback =
                    delegate(object s, X509Certificate certificate,
                             X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    { return true; };
                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {
                SystemLog log = new SystemLog();
                log.ErrorLog(ex.Message);
            }
            return Task.FromResult<bool>(resultado);
        }

        /// <summary>
        /// Verifica que el correo este activo o exista.
        /// </summary>
        /// <returns></returns>
        public Task<List<Mensaje>> existeEmail()
        {
            List<Mensaje> lstMensajes = new List<Mensaje>();
            try
            {
                string[] host = (this.To.Split('@'));
                string hostname = host[1];
                IPHostEntry IPhst = Dns.Resolve(hostname);
                IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);
                Socket s = new Socket(endPt.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                SystemLog log = new SystemLog();
                log.ErrorLog(ex.Message);
                lstMensajes.Add(new Mensaje{tipo="Error",titulo= "Error", cuerpo="El correo electronico " + this.To + " no existe o se encuentra inactivo."});
            }
            return Task.FromResult<List<Mensaje>>(lstMensajes);
        }
    }
}